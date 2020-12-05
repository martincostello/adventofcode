// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Json;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

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

            services.AddSingleton<ILogger, WebLogger>();
            services.AddSingleton<PuzzleFactory>();

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

            var factory = context.RequestServices.GetRequiredService<PuzzleFactory>();

            IPuzzle puzzle;

            try
            {
                puzzle = factory.Create(year, day);
            }
            catch (PuzzleException ex)
            {
                await WriteErrorAsync(context, StatusCodes.Status400BadRequest, "Invalid Request", ex.Message);
                return;
            }

            var stopwatch = Stopwatch.StartNew();

            try
            {
                puzzle.Solve(Array.Empty<string>());
            }
#pragma warning disable CA1031
            catch (Exception ex)
#pragma warning restore CA1031
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<Startup>>();
                logger.LogError(ex, "Failed to solve puzzle for year {Year} and day {Day}.", year, day);

                await WriteErrorAsync(
                    context,
                    StatusCodes.Status500InternalServerError,
                    "Internal Server Error",
                    "Failed to solve puzzle.");

                return;
            }

            stopwatch.Stop();

            var result = new
            {
                year,
                day,
                timeToSolve = stopwatch.Elapsed.ToString("g", CultureInfo.InvariantCulture),
            };

            await context.Response.WriteAsJsonAsync(result);
        }

        /// <summary>
        /// Writes an HTTP error response as an asynchronous operation.
        /// </summary>
        /// <param name="context">The HTTP context to write to.</param>
        /// <param name="statusCode">The HTTP status code to return.</param>
        /// <param name="title">The error title.</param>
        /// <param name="detail">The error detail.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation to write the error response.
        /// </returns>
        private static async Task WriteErrorAsync(
            HttpContext context,
            int statusCode,
            string title,
            string detail)
        {
            context.Response.StatusCode = statusCode;

            var error = new Microsoft.AspNetCore.Mvc.ProblemDetails()
            {
                Detail = detail,
                Status = statusCode,
                Title = title,
            };

            await context.Response.WriteAsJsonAsync(error);
        }
    }
}
