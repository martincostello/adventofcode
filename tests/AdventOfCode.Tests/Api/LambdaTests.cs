// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Net.Sockets;
using System.Reflection;
using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using MartinCostello.Testing.AwsLambdaTestServer;
using Microsoft.AspNetCore.Http;

namespace MartinCostello.AdventOfCode.Api;

/// <summary>
/// A class containing tests for the API when hosted in AWS Lambda.
/// </summary>
[Collection(nameof(LambdaTestsCollection))]
public class LambdaTests : IAsyncLifetime
{
    private readonly HttpLambdaTestServer _server;

    /// <summary>
    /// Initializes a new instance of the <see cref="LambdaTests"/> class.
    /// </summary>
    /// <param name="outputHelper">The test output helper to use.</param>
    public LambdaTests(ITestOutputHelper outputHelper)
    {
        _server = new() { OutputHelper = outputHelper };
    }

    public async Task DisposeAsync()
        => await _server.DisposeAsync();

    public async Task InitializeAsync()
        => await _server.InitializeAsync();

    [Fact(Timeout = 30_000)]
    public async Task Can_Get_Puzzle_Metadata()
    {
        // Arrange
        var request = new APIGatewayHttpApiV2ProxyRequest()
        {
            RequestContext = new()
            {
                DomainName = "adventofcode.local",
                Http = new()
                {
                    Method = HttpMethods.Get,
                    Path = "/api/puzzles",
                },
            },
        };

        // Act
        APIGatewayHttpApiV2ProxyResponse actual = await AssertApiGatewayHttpRequestIsHandledAsync(request);

        actual.ShouldNotBeNull();
        actual.StatusCode.ShouldBe(StatusCodes.Status200OK);
        actual.Headers.ShouldContainKeyAndValue("Content-Type", "application/json; charset=utf-8");

        using var puzzles = JsonDocument.Parse(actual.Body);

        puzzles.RootElement.GetArrayLength().ShouldBeGreaterThan(0);

        foreach (var puzzle in puzzles.RootElement.EnumerateArray())
        {
            puzzle.TryGetProperty("day", out _).ShouldBeTrue();
            puzzle.TryGetProperty("location", out _).ShouldBeTrue();
            puzzle.TryGetProperty("minimumArguments", out _).ShouldBeTrue();
            puzzle.TryGetProperty("name", out _).ShouldBeTrue();
            puzzle.TryGetProperty("requiresData", out _).ShouldBeTrue();
            puzzle.TryGetProperty("year", out _).ShouldBeTrue();
        }
    }

    [Theory(Timeout = 30_000)]
    [InlineData(2015, 11, new[] { "cqjxjnds" }, new[] { "cqjxxyzz" })]
    public async Task Can_Solve_Puzzle(int year, int day, string[] arguments, string[] expected)
    {
        // Arrange
        using var content = new MultipartFormDataContent("----puzzle----");

#pragma warning disable CA2000
        foreach (string value in arguments)
        {
            content.Add(new StringContent(value), "arguments");
        }
#pragma warning restore CA2000

        using var stream = new MemoryStream();
        await content.CopyToAsync(stream);

        stream.Seek(0, SeekOrigin.Begin);

        byte[] buffer = stream.ToArray();
        string body = Convert.ToBase64String(buffer);

        var request = new APIGatewayHttpApiV2ProxyRequest()
        {
            Body = body,
            IsBase64Encoded = true,
            Headers = new Dictionary<string, string>()
            {
                ["content-type"] = content.Headers.ContentType!.ToString(),
            },
            RequestContext = new()
            {
                DomainName = "adventofcode.local",
                Http = new()
                {
                    Method = HttpMethods.Post,
                    Path = $"/api/puzzles/{year}/{day}/solve",
                },
            },
        };

        // Act
        APIGatewayHttpApiV2ProxyResponse actual = await AssertApiGatewayHttpRequestIsHandledAsync(request);

        actual.ShouldNotBeNull();
        actual.StatusCode.ShouldBe(StatusCodes.Status200OK);
        actual.Headers.ShouldContainKeyAndValue("Content-Type", "application/json; charset=utf-8");

        using var solution = JsonDocument.Parse(actual.Body);

        solution.RootElement.GetProperty("year").GetInt32().ShouldBe(year);
        solution.RootElement.GetProperty("day").GetInt32().ShouldBe(day);
        solution.RootElement.GetProperty("timeToSolve").GetDouble().ShouldBeGreaterThan(0);
        solution.RootElement.GetProperty("visualizations").GetArrayLength().ShouldBe(0);

        solution.RootElement.TryGetProperty("solutions", out var solutions).ShouldBeTrue();
        solutions.GetArrayLength().ShouldBe(1);
        solutions.EnumerateArray().ToArray().Select((p) => p.GetString()).ToArray().ShouldBe(expected);
    }

    private static CancellationTokenSource GetCancellationTokenSourceForResponseAvailable(
        LambdaTestContext context,
        TimeSpan? timeout = null)
    {
        if (timeout == null)
        {
            timeout = System.Diagnostics.Debugger.IsAttached ? Timeout.InfiniteTimeSpan : TimeSpan.FromSeconds(3);
        }

        var cts = new CancellationTokenSource(timeout.Value);

        // Queue a task to stop the test server from listening as soon as the response is available
        _ = Task.Factory.StartNew(
            async () =>
            {
                await context.Response.WaitToReadAsync(cts.Token);

                if (!cts.IsCancellationRequested)
                {
                    cts.Cancel();
                }
            },
            cts.Token,
            TaskCreationOptions.None,
            TaskScheduler.Default);

        return cts;
    }

    private static bool LambdaServerWasShutDown(Exception exception)
    {
        if (exception is not TargetInvocationException targetException ||
            targetException.InnerException is not HttpRequestException httpException ||
            httpException.InnerException is not SocketException socketException)
        {
            return false;
        }

        return socketException.SocketErrorCode == SocketError.ConnectionRefused;
    }

    private async Task<APIGatewayHttpApiV2ProxyResponse> AssertApiGatewayHttpRequestIsHandledAsync(
        APIGatewayHttpApiV2ProxyRequest request)
    {
        // Arrange
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        string json = JsonSerializer.Serialize(request, options);

        LambdaTestContext context = await _server.EnqueueAsync(json);

        using var cts = GetCancellationTokenSourceForResponseAvailable(context);

        // Act
        _ = Task.Factory.StartNew(
            () =>
            {
                try
                {
                    typeof(Site.Program).Assembly.EntryPoint!.Invoke(null, new[] { Array.Empty<string>() });
                }
                catch (Exception ex) when (LambdaServerWasShutDown(ex))
                {
                    // The Lambda runtime server was shut down
                }
            },
            cts.Token,
            TaskCreationOptions.None,
            TaskScheduler.Default);

        // Assert
        await context.Response.WaitToReadAsync(cts.IsCancellationRequested ? default : cts.Token);

        context.Response.TryRead(out LambdaTestResponse? response).ShouldBeTrue();
        response.IsSuccessful.ShouldBeTrue($"Failed to process request: {await response.ReadAsStringAsync()}");
        response.Duration.ShouldBeInRange(TimeSpan.Zero, TimeSpan.FromSeconds(2));
        response.Content.ShouldNotBeEmpty();

        // Assert
        var actual = JsonSerializer.Deserialize<APIGatewayHttpApiV2ProxyResponse>(response.Content, options);

        actual.ShouldNotBeNull();

        return actual;
    }
}
