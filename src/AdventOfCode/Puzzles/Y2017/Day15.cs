// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2017/day/15</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2017, 15, RequiresData = true)]
    public sealed class Day15 : Puzzle
    {
        /// <summary>
        /// Gets the judge's final count.
        /// </summary>
        public int FinalCount { get; private set; }

        /// <summary>
        /// Gets the number of values whose lowest 16 bits match when a sequence is generated 40,000,000 times.
        /// </summary>
        /// <param name="seeds">The seed values for the generator.</param>
        /// <returns>
        /// The number of matching pairs from the generated sequences.
        /// </returns>
        public static int GetMatchingPairs(IList<string> seeds)
        {
            long a = ParseInt64(seeds[0].Split(' ')[^1]);
            long b = ParseInt64(seeds[1].Split(' ')[^1]);

            int result = 0;

            for (int i = 0; i < 40_000_000; i++)
            {
                a = GenerateA(a);
                b = GenerateB(b);

                if ((a & 0xffff) == (b & 0xffff))
                {
                    result++;
                }
            }

            return result;

            static long GenerateA(long previous)
            {
                long product = previous * 16807;
                return product % int.MaxValue;
            }

            static long GenerateB(long previous)
            {
                long product = previous * 48271;
                return product % int.MaxValue;
            }
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> input = await ReadResourceAsLinesAsync();

            FinalCount = GetMatchingPairs(input);

            if (Verbose)
            {
                Logger.WriteLine($"The judge's final count is {FinalCount:N0}.");
            }

            return PuzzleResult.Create(FinalCount);
        }
    }
}
