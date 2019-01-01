// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2018/day/5</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day05 : Puzzle2018
    {
        /// <summary>
        /// Gets the number of remaining polymer units after the reduction.
        /// </summary>
        public int RemainingUnits { get; private set; }

        /// <summary>
        /// Gets the number of remaining polymer units after the reduction using optimization.
        /// </summary>
        public int RemainingUnitsOptimized { get; private set; }

        /// <summary>
        /// Reduces the specified polymer.
        /// </summary>
        /// <param name="polymer">The polymer to reduce.</param>
        /// <returns>
        /// The polymer remaining after reducing the specified <paramref name="polymer"/>
        /// value until it can be reduced no further.
        /// </returns>
        public static string Reduce(string polymer)
        {
            var builder = new StringBuilder(polymer, polymer.Length);

            while (true)
            {
                int before = builder.Length;

                builder = ReduceOnce(builder);

                if (before == builder.Length)
                {
                    break;
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Reduces the specified polymer optimally.
        /// </summary>
        /// <param name="polymer">The polymer to reduce optimally.</param>
        /// <returns>
        /// The polymer remaining after reducing the specified <paramref name="polymer"/>
        /// value until it can be reduced no further with optimizations in use.
        /// </returns>
        public static string ReduceWithOptimization(string polymer)
        {
            string[] units = polymer
                .Select((p) => char.ToLowerInvariant(p).ToString(CultureInfo.InvariantCulture))
                .Distinct()
                .OrderBy((p) => p)
                .ToArray();

            var optimized = units
                .Select((p) => polymer.Replace(p, string.Empty, StringComparison.OrdinalIgnoreCase))
                .Select((p) => Reduce(p))
                .OrderBy((p) => p.Length)
                .First();

            return optimized;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string polymer = ReadResourceAsString().Trim('\r', '\n');

            RemainingUnits = Reduce(polymer).Length;
            RemainingUnitsOptimized = ReduceWithOptimization(polymer).Length;

            if (Verbose)
            {
                Logger.WriteLine($"The number of units that remain after fully reacting the polymer is {RemainingUnits:N0}.");
                Logger.WriteLine($"The number of units that remain after fully reacting the polymer with optimization is {RemainingUnitsOptimized:N0}.");
            }

            return 0;
        }

        /// <summary>
        /// Reduces the specified polymer once.
        /// </summary>
        /// <param name="polymer">The polymer to reduce.</param>
        /// <returns>
        /// The polymer remaining after reducing the specified <paramref name="polymer"/> value.
        /// </returns>
        private static StringBuilder ReduceOnce(StringBuilder polymer)
        {
            const int Shift = 'a' - 'A';

            for (int i = 0; i < polymer.Length - 1; i++)
            {
                char x = polymer[i];
                char y = polymer[i + 1];

                if (Math.Abs(x - y) == Shift)
                {
                    polymer = polymer.Remove(i, 2);
                }
            }

            return polymer;
        }
    }
}
