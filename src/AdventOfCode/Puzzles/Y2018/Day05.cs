// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018
{
    using System;

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
        /// Reduces the specified polymer.
        /// </summary>
        /// <param name="polymer">The polymer to reduce.</param>
        /// <returns>
        /// The polymer remaining after reducing the specified <paramref name="polymer"/>
        /// value until it can be reduced no further.
        /// </returns>
        public static ReadOnlySpan<char> Reduce(ReadOnlySpan<char> polymer)
        {
            while (true)
            {
                int before = polymer.Length;

                polymer = ReduceOnce(polymer);

                if (before == polymer.Length)
                {
                    break;
                }
            }

            return polymer;
        }

        /// <summary>
        /// Reduces the specified polymer once.
        /// </summary>
        /// <param name="polymer">The polymer to reduce.</param>
        /// <returns>
        /// The polymer remaining after reducing the specified <paramref name="polymer"/> value.
        /// </returns>
        public static ReadOnlySpan<char> ReduceOnce(ReadOnlySpan<char> polymer)
        {
            for (int i = 0; i < polymer.Length - 1; i++)
            {
                char x = polymer[i];
                char y = polymer[i + 1];

                if (Math.Abs(x - y) == ('a' - 'A'))
                {
                    if (i == 0)
                    {
                        polymer = polymer.Slice(2);
                    }
                    else
                    {
                        var prefix = polymer.Slice(0, i);

                        if (i == polymer.Length - 2)
                        {
                            polymer = prefix;
                        }
                        else
                        {
                            var suffix = polymer.Slice(i + 2);
                            polymer = new string(prefix) + new string(suffix);
                        }
                    }

                    break;
                }
            }

            return polymer;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string polymer = ReadResourceAsString().Trim('\r', '\n');

            RemainingUnits = Reduce(polymer).Length;

            if (Verbose)
            {
                Logger.WriteLine($"The number of units that remain after fully reacting the polymer is {RemainingUnits:N0}.");
            }

            return 0;
        }
    }
}
