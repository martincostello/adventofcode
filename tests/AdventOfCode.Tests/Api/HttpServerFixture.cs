// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Security.Cryptography.X509Certificates;
using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MartinCostello.AdventOfCode.Api;

/// <summary>
/// A test fixture representing an HTTP server hosting the application. This class cannot be inherited.
/// </summary>
public sealed class HttpServerFixture : WebApplicationFactory<Puzzle>, ITestOutputHelperAccessor
{
    private IHost? _host;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpServerFixture"/> class.
    /// </summary>
    public HttpServerFixture()
        : base()
    {
        ClientOptions.AllowAutoRedirect = false;
        ClientOptions.BaseAddress = new Uri("https://localhost");
    }

    /// <inheritdoc />
    public ITestOutputHelper? OutputHelper { get; set; }

    /// <summary>
    /// Gets the server address of the application.
    /// </summary>
    public Uri ServerAddress
    {
        get
        {
            EnsureServer();
            return ClientOptions.BaseAddress;
        }
    }

    /// <inheritdoc />
    public override IServiceProvider Services
    {
        get
        {
            EnsureServer();
            return _host!.Services!;
        }
    }

    /// <summary>
    /// Clears the current <see cref="ITestOutputHelper"/>.
    /// </summary>
    public void ClearOutputHelper()
        => OutputHelper = null;

    /// <summary>
    /// Sets the <see cref="ITestOutputHelper"/> to use.
    /// </summary>
    /// <param name="value">The <see cref="ITestOutputHelper"/> to use.</param>
    public void SetOutputHelper(ITestOutputHelper value)
        => OutputHelper = value;

    /// <inheritdoc />
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging((loggingBuilder) => loggingBuilder.ClearProviders().AddXUnit(this))
               .UseSolutionRelativeContentRoot(Path.Combine("src", "AdventOfCode"));

        builder.ConfigureKestrel(
            (p) => p.ConfigureHttpsDefaults(
                (r) => r.ServerCertificate = new X509Certificate2("localhost-dev.pfx", "Pa55w0rd!")));

        // Configure the server address for the server to
        // listen on for HTTPS requests on a dynamic port.
        builder.UseUrls("https://127.0.0.1:0");
    }

    /// <inheritdoc />
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var testHost = builder.Build();

        builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());

        _host = builder.Build();
        _host.Start();

        var server = _host.Services.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>();

        ClientOptions.BaseAddress = addresses!.Addresses
            .Select((p) => new Uri(p))
            .Last();

        testHost.Start();

        return testHost;
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!_disposed)
        {
            if (disposing)
            {
                _host?.Dispose();
            }

            _disposed = true;
        }
    }

    private void EnsureServer()
    {
        if (_host is null)
        {
            using (CreateDefaultClient())
            {
            }
        }
    }
}
