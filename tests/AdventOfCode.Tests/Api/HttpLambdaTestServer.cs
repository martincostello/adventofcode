// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using MartinCostello.Logging.XUnit;
using MartinCostello.Testing.AwsLambdaTestServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MartinCostello.AdventOfCode.Api;

internal sealed class HttpLambdaTestServer()
    : LambdaTestServer(new LambdaTestServerOptions() { FunctionMemorySize = FunctionMemorySize }), IAsyncLifetime, ITestOutputHelperAccessor
{
    private static readonly int FunctionMemorySize = GetFunctionMemorySize();
    private readonly CancellationTokenSource _cts = new();
    private bool _disposed;
    private IWebHost? _webHost;

    public ITestOutputHelper? OutputHelper { get; set; }

    public async Task DisposeAsync()
    {
        if (_webHost is not null)
        {
            await _webHost.StopAsync();
        }

        Dispose();
    }

    public async Task InitializeAsync()
        => await StartAsync(_cts.Token);

    protected override IServer CreateServer(WebHostBuilder builder)
    {
        _webHost = builder
            .UseKestrel()
            .ConfigureServices((services) => services.AddLogging((builder) => builder.AddXUnit(this)))
            .UseUrls("http://127.0.0.1:0")
            .Build();

        _webHost.Start();

        return _webHost.Services.GetRequiredService<IServer>();
    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _webHost?.Dispose();

                _cts.Cancel();
                _cts.Dispose();
            }

            _disposed = true;
        }

        base.Dispose(disposing);
    }

    private static int GetFunctionMemorySize()
    {
        // See https://github.com/aws/aws-lambda-dotnet/issues/1594 and
        // https://docs.github.com/en/actions/using-github-hosted-runners/about-github-hosted-runners/about-github-hosted-runners#supported-runners-and-hardware-resources.
        return Environment.GetEnvironmentVariable("GITHUB_ACTIONS") switch
        {
            "true" => (OperatingSystem.IsMacOS() ? 13 : 6) * 1024, // 1GB less than the runner
            _ => int.MaxValue,
        };
    }
}
