﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
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
[Collection<LambdaTestsCollection>]
public class LambdaTests : IAsyncLifetime, IDisposable
{
    private const int LambdaTestTimeout = 10_000;

    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    private readonly HttpLambdaTestServer _server;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="LambdaTests"/> class.
    /// </summary>
    /// <param name="outputHelper">The test output helper to use.</param>
    public LambdaTests(ITestOutputHelper outputHelper)
    {
        _server = new() { OutputHelper = outputHelper };
    }

    ~LambdaTests()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await _server.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    public async ValueTask InitializeAsync()
        => await _server.InitializeAsync();

    [Fact(Timeout = LambdaTestTimeout)]
    public async Task Can_Get_Puzzle_Metadata()
    {
        // Arrange
        var request = new APIGatewayProxyRequest()
        {
            HttpMethod = HttpMethods.Get,
            Path = "/api/puzzles",
        };

        // Act
        APIGatewayProxyResponse actual = await AssertApiGatewayRequestIsHandledAsync(request);

        actual.ShouldNotBeNull();
        actual.StatusCode.ShouldBe(StatusCodes.Status200OK);
        actual.MultiValueHeaders.ShouldContainKey("Content-Type");
        actual.MultiValueHeaders["Content-Type"].ShouldBe(["application/json; charset=utf-8"]);

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

    [Theory(Timeout = LambdaTestTimeout)]
    [InlineData(2015, 11, new[] { "cqjxjnds" }, new[] { "cqjxxyzz", "cqkaabcc" })]
    public async Task Can_Solve_Puzzle_With_Input_Arguments(int year, int day, string[] arguments, string[] expected)
    {
        // Arrange
        var (body, contentType) = await GetFormContentAsync((content) =>
        {
            foreach (string value in arguments)
            {
                content.Add(new StringContent(value), "arguments");
            }
        });

        var request = new APIGatewayProxyRequest()
        {
            Body = body,
            IsBase64Encoded = true,
            Headers = new Dictionary<string, string>()
            {
                ["content-type"] = contentType,
            },
            HttpMethod = HttpMethods.Post,
            Path = $"/api/puzzles/{year}/{day}/solve",
        };

        // Act
        APIGatewayProxyResponse actual = await AssertApiGatewayRequestIsHandledAsync(request);

        actual.ShouldNotBeNull();
        actual.StatusCode.ShouldBe(StatusCodes.Status200OK);
        actual.MultiValueHeaders.ShouldContainKey("Content-Type");
        actual.MultiValueHeaders["Content-Type"].ShouldBe(["application/json; charset=utf-8"]);

        using var solution = JsonDocument.Parse(actual.Body);

        solution.RootElement.GetProperty("year").GetInt32().ShouldBe(year);
        solution.RootElement.GetProperty("day").GetInt32().ShouldBe(day);
        solution.RootElement.GetProperty("timeToSolve").GetDouble().ShouldBeGreaterThan(0);
        solution.RootElement.GetProperty("visualizations").GetArrayLength().ShouldBe(0);

        solution.RootElement.TryGetProperty("solutions", out var solutions).ShouldBeTrue();
        solutions.GetArrayLength().ShouldBe(expected.Length);
        solutions.EnumerateArray().ToArray().Select((p) => p.GetString()).ToArray().ShouldBe(expected);
    }

    [Theory(Timeout = LambdaTestTimeout)]
    [InlineData(2015, 1, new[] { 232, 1783 })]
    public async Task Can_Solve_Puzzle_With_Input_File(int year, int day, int[] expected)
    {
        // Arrange
        var (body, contentType) = await GetFormContentAsync(
            (content) => content.Add(new StringContent(GetPuzzleInput(year, day)), "resource", "input.txt"));

        var request = new APIGatewayProxyRequest()
        {
            Body = body,
            IsBase64Encoded = true,
            Headers = new Dictionary<string, string>()
            {
                ["content-type"] = contentType,
            },
            HttpMethod = HttpMethods.Post,
            Path = $"/api/puzzles/{year}/{day}/solve",
        };

        // Act
        APIGatewayProxyResponse actual = await AssertApiGatewayRequestIsHandledAsync(request);

        actual.ShouldNotBeNull();
        actual.StatusCode.ShouldBe(StatusCodes.Status200OK);
        actual.MultiValueHeaders.ShouldContainKey("Content-Type");
        actual.MultiValueHeaders["Content-Type"].ShouldBe(["application/json; charset=utf-8"]);

        using var solution = JsonDocument.Parse(actual.Body);

        solution.RootElement.GetProperty("year").GetInt32().ShouldBe(year);
        solution.RootElement.GetProperty("day").GetInt32().ShouldBe(day);
        solution.RootElement.GetProperty("timeToSolve").GetDouble().ShouldBeGreaterThan(0);
        solution.RootElement.GetProperty("visualizations").GetArrayLength().ShouldBe(0);

        solution.RootElement.TryGetProperty("solutions", out var solutions).ShouldBeTrue();
        solutions.GetArrayLength().ShouldBe(expected.Length);
        solutions.EnumerateArray().ToArray().Select((p) => p.GetInt32()).ToArray().ShouldBe(expected);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _server?.Dispose();
            }

            _disposed = true;
        }
    }

    private static async Task<(string Body, string ContentType)> GetFormContentAsync(Action<MultipartFormDataContent> configure)
    {
        using var content = new MultipartFormDataContent("----puzzle----");

        configure(content);

        using var stream = new MemoryStream();
        await content.CopyToAsync(stream);

        stream.Seek(0, SeekOrigin.Begin);

        byte[] buffer = stream.ToArray();
        return (Convert.ToBase64String(buffer), content.Headers.ContentType!.ToString());
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
                    await cts.CancelAsync();
                }
            },
            cts.Token,
            TaskCreationOptions.None,
            TaskScheduler.Default);

        return cts;
    }

    private static string GetPuzzleInput(int year, int day)
    {
        using var stream = InputProvider.Get(year, day)!;
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
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

    private async Task<APIGatewayProxyResponse> AssertApiGatewayRequestIsHandledAsync(
        APIGatewayProxyRequest request)
    {
        // Arrange
        string json = JsonSerializer.Serialize(request, SerializerOptions);

        LambdaTestContext context = await _server.EnqueueAsync(json);

        using var cts = GetCancellationTokenSourceForResponseAvailable(context);
        using var combined = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, _server.CancellationToken);

        combined.CancelAfter(LambdaTestTimeout);

        // Act
        _ = Task.Factory.StartNew(
            () =>
            {
                try
                {
                    typeof(Site.Program).Assembly.EntryPoint!.Invoke(null, [Array.Empty<string>()]);
                }
                catch (Exception ex) when (LambdaServerWasShutDown(ex))
                {
                    // The Lambda runtime server was shut down
                }
            },
            combined.Token,
            TaskCreationOptions.None,
            TaskScheduler.Default);

        // Assert
        await context.Response.WaitToReadAsync(combined.Token);

        context.Response.TryRead(out LambdaTestResponse? response).ShouldBeTrue();
        response.IsSuccessful.ShouldBeTrue($"Failed to process request: {await response.ReadAsStringAsync()}");
        response.Duration.ShouldBeInRange(TimeSpan.Zero, TimeSpan.FromSeconds(3));
        response.Content.ShouldNotBeEmpty();

        // Assert
        var actual = JsonSerializer.Deserialize<APIGatewayProxyResponse>(response.Content, SerializerOptions);

        actual.ShouldNotBeNull();

        return actual;
    }
}
