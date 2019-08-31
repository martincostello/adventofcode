// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// A console application that solves puzzles for <c>http://adventofcode.com</c>. This class cannot be inherited.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry-point to the application.
        /// </summary>
        /// <param name="args">The arguments to the application.</param>
        /// <returns>The exit code from the application.</returns>
        internal static int Main(string[] args)
            => Run(args, new ConsoleLogger());

        /// <summary>
        /// Runs the application.
        /// </summary>
        /// <param name="args">The arguments to the application.</param>
        /// <param name="logger">The logger to use.</param>
        /// <returns>The exit code from the application.</returns>
        internal static int Run(string[] args, ILogger logger)
        {
            if (args == null || args.Length < 1)
            {
                logger.WriteLine("No day specified.");
                return -1;
            }

            int year;

            if (args.Length == 2)
            {
                if (!int.TryParse(args[1], NumberStyles.Integer & ~NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out year))
                {
                    year = 0;
                }
            }
            else
            {
                year = DateTime.UtcNow.Year;
            }

            Type? type;

            if (!int.TryParse(args[0], NumberStyles.Integer & ~NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out int day) ||
                day < 1 ||
                year < 2015 ||
                year > DateTime.UtcNow.Year ||
                (type = GetPuzzleType(year, day)) == null)
            {
                logger.WriteLine("The year and/or puzzle number specified is invalid.");
                return -1;
            }

            args = args[1..];

            return SolvePuzzle(type, year, day, args, logger, verbose: true);
        }

        /// <summary>
        /// Solves the puzzle associated with the specified type.
        /// </summary>
        /// <param name="type">The type of the puzzle.</param>
        /// <param name="year">The year associated with the puzzle.</param>
        /// <param name="day">The day associated with the puzzle.</param>
        /// <param name="args">The arguments to pass to the puzzle.</param>
        /// <param name="logger">The logger to use.</param>
        /// <param name="verbose">Whether the puzzle should be run verbosely.</param>
        /// <returns>
        /// The value returned by <see cref="IPuzzle.Solve"/>.
        /// </returns>
        private static int SolvePuzzle(
            Type type,
            int year,
            int day,
            string[] args,
            ILogger logger,
            bool verbose = false)
        {
            var puzzle = Activator.CreateInstance(type) as Puzzle;

            puzzle!.Logger = logger;
            puzzle.Verbose = verbose;

            logger.WriteLine();
            logger.WriteLine($"Advent of Code {year} - Day {day}");
            logger.WriteLine();

            var stopwatch = Stopwatch.StartNew();

            int result = puzzle.Solve(args);

            if (result == 0)
            {
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
            }

            return result;
        }

        /// <summary>
        /// Gets the puzzle type to use for the specified number.
        /// </summary>
        /// <param name="year">The year to get the puzzle for.</param>
        /// <param name="day">The day to get the puzzle for.</param>
        /// <returns>
        /// The <see cref="Type"/> for the specified year and puzzle number, if found; otherwise <see langword="null"/>.
        /// </returns>
        private static Type? GetPuzzleType(int year, int day)
        {
            string typeName = string.Format(
                CultureInfo.InvariantCulture,
                "MartinCostello.AdventOfCode.Puzzles.Y{0}.Day{1:00}",
                year,
                day);

            return Type.GetType(typeName);
        }
    }
}
