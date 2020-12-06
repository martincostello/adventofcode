// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Api
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Sockets;
    using System.Reflection;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using MartinCostello.Logging.XUnit;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A test fixture representing an HTTP server hosting the application. This class cannot be inherited.
    /// </summary>
    public sealed class HttpServerFixture : WebApplicationFactory<Startup>, IAsyncLifetime, ITestOutputHelperAccessor
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
        public Uri ServerAddress => ClientOptions.BaseAddress;

        /// <inheritdoc />
        public override IServiceProvider? Services => _host?.Services;

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
        async Task IAsyncLifetime.InitializeAsync()
            => await EnsureHttpServerAsync();

        /// <inheritdoc />
        async Task IAsyncLifetime.DisposeAsync()
        {
            if (_host != null)
            {
                await _host.StopAsync();
                _host.Dispose();
                _host = null;
            }
        }

        /// <summary>
        /// Creates an <see cref="HttpClient"/> to communicate with the application.
        /// </summary>
        /// <returns>
        /// An <see cref="HttpClient"/> that can be to used to make application requests.
        /// </returns>
        public HttpClient CreateHttpClient()
        {
#pragma warning disable CA2000
            var handler = new HttpClientHandler()
#pragma warning restore CA2000
            {
                AllowAutoRedirect = ClientOptions.AllowAutoRedirect,
                MaxAutomaticRedirections = ClientOptions.MaxAutomaticRedirections,
                UseCookies = ClientOptions.HandleCookies,
            };

            try
            {
                if (ClientOptions.BaseAddress.IsLoopback &&
                    string.Equals(ClientOptions.BaseAddress.Scheme, "https", StringComparison.OrdinalIgnoreCase))
                {
                    handler.ServerCertificateCustomValidationCallback = (request, cert, chain, errors) => true;
                }

#pragma warning disable CA5400
                var client = new HttpClient(handler);
#pragma warning restore CA5400

                ConfigureClient(client);

                client.BaseAddress = ClientOptions.BaseAddress;

                return client;
            }
            catch (Exception)
            {
                handler.Dispose();
                throw;
            }
        }

        /// <inheritdoc />
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureLogging((loggingBuilder) => loggingBuilder.ClearProviders().AddXUnit(this))
                   .UseContentRoot(GetApplicationContentRootPath());

            builder.ConfigureKestrel(
                (p) => p.ConfigureHttpsDefaults(
                    (r) => r.ServerCertificate = new X509Certificate2("localhost-dev.pfx", "Pa55w0rd!")));

            builder.UseUrls(ServerAddress.ToString());
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

        private static Uri FindFreeServerAddress()
        {
            int port = GetFreePortNumber();

            return new UriBuilder()
            {
                Scheme = "https",
                Host = "localhost",
                Port = port,
            }.Uri;
        }

        private static int GetFreePortNumber()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();

            try
            {
                return ((IPEndPoint)listener.LocalEndpoint).Port;
            }
            finally
            {
                listener.Stop();
            }
        }

        private async Task EnsureHttpServerAsync()
        {
            if (_host == null)
            {
                await CreateHttpServer();
            }
        }

        private async Task CreateHttpServer()
        {
            // Configure the server address for the server to listen on for HTTP requests
            ClientOptions.BaseAddress = FindFreeServerAddress();

            var builder = CreateHostBuilder().ConfigureWebHost(ConfigureWebHost);

            _host = builder.Build();

            // Force creation of the Kestrel server and start it
            var hostedService = _host.Services.GetService<IHostedService>();
            await hostedService!.StartAsync(default);
        }

        private string GetApplicationContentRootPath()
        {
            var attribute = GetTestAssemblies()
                .SelectMany((p) => p.GetCustomAttributes<WebApplicationFactoryContentRootAttribute>())
                .Where((p) => string.Equals(p.Key, "AdventOfCode", StringComparison.OrdinalIgnoreCase))
                .OrderBy((p) => p.Priority)
                .First();

            return attribute.ContentRootPath;
        }
    }
}
