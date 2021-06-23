// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MartinCostello.AdventOfCode
{
    /// <summary>
    /// An application that solves puzzles for <c>https://adventofcode.com</c>. This class cannot be inherited.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry-point to the application.
        /// </summary>
        /// <param name="args">The arguments to the application.</param>
        /// <returns>The exit code from the application.</returns>
        public static async Task<int> Main(string[] args)
            => await RunAsync(args, new ConsoleLogger());

        /// <summary>
        /// Creates the host builder to use for the application.
        /// </summary>
        /// <param name="args">The arguments to the application.</param>
        /// <returns>
        /// A <see cref="IHostBuilder"/> to use.
        /// </returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .ConfigureWebHostDefaults((webBuilder) =>
                       {
                           webBuilder.CaptureStartupErrors(true)
                                     .ConfigureKestrel((p) => p.AddServerHeader = false)
                                     .UseStartup<Startup>();
                       });
        }

        /// <summary>
        /// Runs the application.
        /// </summary>
        /// <param name="args">The arguments to the application.</param>
        /// <param name="logger">The logger to use.</param>
        /// <returns>The exit code from the application.</returns>
        internal static async Task<int> RunAsync(string[] args, ILogger logger)
        {
            if (args == null || args.Length < 1)
            {
                try
                {
                    await CreateHostBuilder(Array.Empty<string>()).Build().RunAsync();
                    return 0;
                }
#pragma warning disable CA1031
                catch (Exception ex)
#pragma warning restore CA1031
                {
                    logger.WriteLine($"Unhandled exception: {ex}");
                    return -1;
                }
            }

            if (!int.TryParse(args[0], NumberStyles.Integer & ~NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out int day))
            {
                day = 0;
            }

            int year = 0;

            if (args.Length > 1)
            {
                if (!int.TryParse(args[1], NumberStyles.Integer & ~NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out year))
                {
                    year = 0;
                }

                args = args[2..];
            }
            else
            {
                year = DateTime.UtcNow.Year;
                args = args[1..];
            }

            using var cts = new CancellationTokenSource();

            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            return await SolvePuzzleAsync(year, day, args, logger, cts.Token);
        }

        /// <summary>
        /// Solves the puzzle associated with the specified year and day as an asychronous operation.
        /// </summary>
        /// <param name="year">The year associated with the puzzle.</param>
        /// <param name="day">The day associated with the puzzle.</param>
        /// <param name="args">The arguments to pass to the puzzle.</param>
        /// <param name="logger">The logger to use.</param>
        /// <param name="cancellationToken">The cancellation token to use.</param>
        /// <returns>
        /// The solution to the puzzle.
        /// </returns>
        private static async Task<int> SolvePuzzleAsync(
            int year,
            int day,
            string[] args,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            var factory = new PuzzleFactory(logger);

            Puzzle puzzle;

            try
            {
                puzzle = factory.Create(year, day);
            }
            catch (PuzzleException ex)
            {
                logger.WriteLine(ex.Message);
                return -1;
            }

            logger.WriteLine();
            logger.WriteLine($"Advent of Code {year} - Day {day}");
            logger.WriteLine();

            var stopwatch = Stopwatch.StartNew();

            try
            {
                _ = await puzzle.SolveAsync(args, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                logger.WriteLine("Solution canceled.");
                return -1;
            }
            catch (PuzzleException ex)
            {
                logger.WriteLine(ex.Message);
                return -1;
            }

            stopwatch.Stop();

            logger.WriteLine();

            if (stopwatch.Elapsed.TotalSeconds < 0.01f)
            {
                logger.WriteLine("Took <0.01 seconds.");
            }
            else
            {
                logger.WriteLine($"Took {stopwatch.Elapsed.TotalSeconds:N2} seconds.");
            }

            logger.WriteLine();

            return 0;
        }
    }
}
