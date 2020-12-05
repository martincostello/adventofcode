// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    using System.Globalization;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Json;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

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
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(
                (endpoints) =>
                {
                    endpoints.MapGet("/error", ErrorAsync);
                    endpoints.MapGet("/api/puzzle/{year:int}/{day:int}/solve", PuzzleAsync);
                });
        }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to use.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(
                (p) =>
                {
                    p.AppendTrailingSlash = true;
                    p.LowercaseUrls = true;
                });

            services.Configure<JsonOptions>(options => options.SerializerOptions.WriteIndented = true);
        }

        /// <summary>
        /// Handles an error as an asynchronous operation.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation to handle the error.
        /// </returns>
        private static Task ErrorAsync(HttpContext context)
        {
            context.Response.Redirect("/error.html");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Solves the puzzle associated with the specified HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation to solve the puzzle.
        /// </returns>
        private static async Task PuzzleAsync(HttpContext context)
        {
            int year = int.Parse((string)context.Request.RouteValues["year"] !, CultureInfo.InvariantCulture);
            int day = int.Parse((string)context.Request.RouteValues["day"] !, CultureInfo.InvariantCulture);

            var result = new
            {
                year,
                day,
            };

            await context.Response.WriteAsJsonAsync(result);
        }
    }
}
