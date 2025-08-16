// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MartinCostello.AdventOfCode.Api;

/// <summary>
/// A test fixture representing an HTTP server hosting the application. This class cannot be inherited.
/// </summary>
public sealed class HttpServerFixture : WebApplicationFactory<Site.Program>, ITestOutputHelperAccessor
{
    public HttpServerFixture()
    {
        UseKestrel(
            (server) => server.Listen(
                IPAddress.Loopback, 0, (listener) => listener.UseHttps(
                    (https) => https.ServerCertificate = LoadDevelopmentCertificate())));
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
        builder.UseEnvironment(Environments.Production);
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
}
