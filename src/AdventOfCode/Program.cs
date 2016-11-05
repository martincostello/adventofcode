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
                Console.Error.WriteLine("No day specified.");
                return -1;
            }

            int day;

            if (!int.TryParse(args[0], NumberStyles.Integer & ~NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out day) || day < 1 || day > 25)
            {
                Console.Error.WriteLine("The day specified is invalid.");
                return -1;
            }

            Console.WriteLine();
            Console.WriteLine("Advent of Code - Day {0}", day);
            Console.WriteLine();

            IPuzzle puzzle = GetPuzzle(day);
            args = args.Skip(1).ToArray();

            Stopwatch stopwatch = Stopwatch.StartNew();

            int result = puzzle.Solve(args);

            stopwatch.Stop();

            if (result == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Took {0:N2} seconds.", stopwatch.Elapsed.TotalSeconds);
            }

            return result;
        }

        /// <summary>
        /// Gets the puzzle to use for the specified day.
        /// </summary>
        /// <param name="day">The day to get the puzzle for.</param>
        /// <returns>The <see cref="IPuzzle"/> for the specified day.</returns>
        private static IPuzzle GetPuzzle(int day)
        {
            string typeName = string.Format(
                CultureInfo.InvariantCulture,
                "MartinCostello.AdventOfCode.Puzzles.Day{0:00}",
                day);

            return Activator.CreateInstance(Type.GetType(typeName), nonPublic: true) as IPuzzle;
        }
    }
}
