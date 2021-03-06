﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MartinCostello.AdventOfCode
{
    /// <summary>
    /// A class representing the puzzles API. This class cannot be inherited.
    /// </summary>
    internal static class PuzzlesApi
    {
        /// <summary>
        /// Gets the available puzzles as an asynchronous operation.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation to solve the puzzle.
        /// </returns>
        internal static async Task GetPuzzlesAsync(HttpContext context)
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
        internal static async Task SolvePuzzleAsync(HttpContext context)
        {
            if (!context.Request.HasFormContentType)
            {
                await WriteErrorAsync(context, StatusCodes.Status415UnsupportedMediaType, "Unsupported Media Type", "The specified media type is not supported.");
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
                await WriteErrorAsync(context, StatusCodes.Status404NotFound, "Not Found", ex.Message);
                return;
            }

            var metadata = puzzle.Metadata();

            if (metadata.IsHidden)
            {
                await WriteErrorAsync(context, StatusCodes.Status403Forbidden, "Forbidden", "This puzzle cannot be solved.");
                return;
            }

            string[] arguments = Array.Empty<string>();

            if (metadata.RequiresData || metadata.MinimumArguments > 0)
            {
                var form = await context.Request.ReadFormAsync(context.RequestAborted);

                if (metadata.RequiresData)
                {
                    if (!form.TryGetValue("resource", out var resource))
                    {
                        await WriteErrorAsync(context, StatusCodes.Status400BadRequest, "Bad Request", "No puzzle resource provided.");
                        return;
                    }

                    puzzle.Resource = new MemoryStream(Encoding.UTF8.GetBytes(resource));
                }

                if (form.TryGetValue("arguments", out var values))
                {
                    arguments = values.Select((p) => p).ToArray();
                }
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
                solutions = solution.Solutions,
                visualizations = solution.Visualizations,
                timeToSolve = stopwatch.Elapsed.TotalMilliseconds,
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
