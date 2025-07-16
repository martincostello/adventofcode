// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MartinCostello.AdventOfCode.Api;

/// <summary>
/// A test fixture representing an HTTP server hosting the application. This class cannot be inherited.
/// </summary>
public sealed class HttpServerFixture : WebApplicationFactory<Site.Program>, ITestOutputHelperAccessor
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
            StartServer();
            return ClientOptions.BaseAddress;
        }
    }

    /// <inheritdoc />
    public override IServiceProvider Services
    {
        get
        {
            StartServer();
            return _host!.Services!;
        }
    }

    /// <summary>
    /// Creates an <see cref="HttpClient"/> for use with the application.
    /// </summary>
    /// <returns>
    /// An <see cref="HttpClient"/> configured for use with the application.
    /// </returns>
    public HttpClient CreateHttpClientForApp()
    {
        var handler = new HttpClientHandler()
        {
            CheckCertificateRevocationList = true,
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };

        return new HttpClient(handler, disposeHandler: true)
        {
            BaseAddress = ServerAddress,
        };
    }

    /// <inheritdoc />
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging((loggingBuilder) => loggingBuilder.ClearProviders().AddXUnit(this));

        builder.ConfigureKestrel(
            (p) => p.ConfigureHttpsDefaults(
                (r) => r.ServerCertificate = LoadDevelopmentCertificate()));

        builder.UseEnvironment(Environments.Production);

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

    private static X509Certificate2 LoadDevelopmentCertificate()
    {
        var metadata = typeof(HttpServerFixture).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .ToArray();

        string fileName = metadata.First((p) => p.Key is "DevCertificateFileName").Value!;
        string? password = metadata.First((p) => p.Key is "DevCertificatePassword").Value;

        return X509CertificateLoader.LoadPkcs12(File.ReadAllBytes(fileName), password);
    }

    private void StartServer()
    {
        if (_host is null)
        {
            using (CreateDefaultClient())
            {
            }
        }
    }
}
