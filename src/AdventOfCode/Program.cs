// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;

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
        {
            if (args == null || args.Length < 1)
            {
                Console.WriteLine("No day specified.");
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

            Type type = null;

            if (!int.TryParse(args[0], NumberStyles.Integer & ~NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out int day) ||
                day < 1 ||
                year < 2015 ||
                year > DateTime.UtcNow.Year ||
                (type = GetPuzzleType(year, day)) == null)
            {
                Console.WriteLine("The year and/or puzzle number specified is invalid.");
                return -1;
            }

            args = args.Skip(1).ToArray();

            return SolvePuzzle(type, year, day, args, verbose: true);
        }

        /// <summary>
        /// Solves the puzzle associated with the specified type.
        /// </summary>
        /// <param name="type">The type of the puzzle.</param>
        /// <param name="year">The year associated with the puzzle.</param>
        /// <param name="day">The day associated with the puzzle.</param>
        /// <param name="args">The arguments to pass to the puzzle.</param>
        /// <param name="verbose">Whether the puzzle should be run verbosely.</param>
        /// <returns>
        /// The value returned by <see cref="IPuzzle.Solve"/>.
        /// </returns>
        internal static int SolvePuzzle(Type type, int year, int day, string[] args, bool verbose = false)
        {
            IPuzzle puzzle = Activator.CreateInstance(type) as IPuzzle;
            puzzle.Verbose = verbose;

            Console.WriteLine();
            Console.WriteLine("Advent of Code {0} - Day {1}", year, day);
            Console.WriteLine();

            Stopwatch stopwatch = Stopwatch.StartNew();

            int result = puzzle.Solve(args);

            if (result == 0)
            {
                stopwatch.Stop();

                if (stopwatch.Elapsed.TotalSeconds < 0.01f)
                {
                    Console.WriteLine("Took <0.01 seconds.");
                }
                else
                {
                    Console.WriteLine("Took {0:N2} seconds.", stopwatch.Elapsed.TotalSeconds);
                }

                Console.WriteLine();
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
        private static Type GetPuzzleType(int year, int day)
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
