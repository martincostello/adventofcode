// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MartinCostello.AdventOfCode
{
    /// <summary>
    /// A class representing the startup logic for the application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to use.</param>
        /// <param name="environment">The <see cref="IWebHostEnvironment"/> to use.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("/error.html?code={0}");

            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(
                (endpoints) =>
                {
                    endpoints.MapRazorPages();

                    endpoints.MapGet("/error", (context) =>
                    {
                        context.Response.Redirect("/error.html");
                        return Task.CompletedTask;
                    });

                    endpoints.MapGet("/api/puzzles", PuzzlesApi.GetPuzzlesAsync);
                    endpoints.MapPost("/api/puzzles/{year:int}/{day:int}/solve", PuzzlesApi.SolvePuzzleAsync);
                });
        }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to use.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILogger, WebLogger>();
            services.AddSingleton<PuzzleFactory>();

            var puzzles = GetType().Assembly
                .GetTypes()
                .Where((p) => p.IsAssignableTo(typeof(Puzzle)))
                .Select((p) => p.GetCustomAttribute<PuzzleAttribute>())
                .Where((p) => p is not null)
                .Where((p) => !p!.IsHidden)
                .ToList();

            foreach (var puzzle in puzzles)
            {
                services.AddSingleton(puzzle!);
            }

            services.Configure<JsonOptions>(options => options.SerializerOptions.WriteIndented = true);

            services.AddRazorPages();

            services.Configure<GzipCompressionProviderOptions>((p) => p.Level = CompressionLevel.Fastest);
            services.Configure<BrotliCompressionProviderOptions>((p) => p.Level = CompressionLevel.Fastest);

            services.AddResponseCompression((p) =>
            {
                p.EnableForHttps = true;
                p.Providers.Add<BrotliCompressionProvider>();
                p.Providers.Add<GzipCompressionProvider>();
            });
        }
    }
}
