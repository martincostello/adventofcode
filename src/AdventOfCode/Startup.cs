// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;
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
                    endpoints.MapGet("/api/puzzles", GetPuzzlesAsync);
                    endpoints.MapPost("/api/puzzles/{year:int}/{day:int}/solve", SolvePuzzleAsync);
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
        /// Gets the available puzzles as an asynchronous operation.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation to solve the puzzle.
        /// </returns>
        private static async Task GetPuzzlesAsync(HttpContext context)
        {
            var puzzles = context.RequestServices
                .GetServices<PuzzleAttribute>()
                .OrderBy((p) => p.Year)
                .ThenBy((p) => p.Day)
                .Select((p) =>
                new
                {
                    p.Year,
                    p.Day,
                    p.MinimumArguments,
                    p.RequiresData,
                    Location = FormattableString.Invariant($"/api/puzzles/{p.Year}/{p.Day}/solve"),
                })
                .ToList();

            await context.Response.WriteAsJsonAsync(puzzles);
        }

        /// <summary>
        /// Solves the puzzle associated with the specified HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation to solve the puzzle.
        /// </returns>
        private static async Task SolvePuzzleAsync(HttpContext context)
        {
            if (!context.Request.HasFormContentType)
            {
                context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
                return;
            }

            int year = int.Parse((string)context.Request.RouteValues["year"] !, CultureInfo.InvariantCulture);
            int day = int.Parse((string)context.Request.RouteValues["day"] !, CultureInfo.InvariantCulture);

            var factory = context.RequestServices.GetRequiredService<PuzzleFactory>();

            Puzzle puzzle;

            try
            {
                puzzle = factory.Create(year, day);
            }
            catch (PuzzleException ex)
            {
                await WriteErrorAsync(context, StatusCodes.Status400BadRequest, "Bad Request", ex.Message);
                return;
            }

            var form = await context.Request.ReadFormAsync(context.RequestAborted);

            var metadata = puzzle.Metadata();

            if (metadata.RequiresData)
            {
                if (!form.TryGetValue("resource", out var resource))
                {
                    await WriteErrorAsync(context, StatusCodes.Status400BadRequest, "Bad Request", "No puzzle resource provided.");
                    return;
                }

                puzzle.Resource = new MemoryStream(Encoding.UTF8.GetBytes(resource));
            }

            string[] arguments = Array.Empty<string>();

            if (form.TryGetValue("arguments", out var values))
            {
                arguments = values.Select((p) => p).ToArray();
            }

            var timeout = TimeSpan.FromMinutes(1);

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(context.RequestAborted);
            cts.CancelAfter(timeout);

            var stopwatch = Stopwatch.StartNew();

            PuzzleResult solution;

            try
            {
                solution = await puzzle.SolveAsync(arguments, cts.Token);
            }
            catch (OperationCanceledException)
            {
                await WriteErrorAsync(context, StatusCodes.Status408RequestTimeout, "Request Timeout", $"The puzzle was not solved within {timeout}.");
                return;
            }
            catch (PuzzleException ex)
            {
                await WriteErrorAsync(context, StatusCodes.Status400BadRequest, "Bad Request", ex.Message);
                return;
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
                solution.Solutions,
                timeToSolve = stopwatch.Elapsed.TotalMilliseconds.ToString("g", CultureInfo.InvariantCulture),
            };

            await context.Response.WriteAsJsonAsync(solution);
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
