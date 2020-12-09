// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/9</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2020, 09, RequiresData = true)]
    public sealed class Day09 : Puzzle
    {
        /// <summary>
        /// Gets the first weak number in the signal.
        /// </summary>
        public long WeakNumber { get; private set; }

        /// <summary>
        /// Gets the first weak number from the specified XMAS signal.
        /// </summary>
        /// <param name="values">The values of the signal.</param>
        /// <param name="preambleLength">The length of the preamble.</param>
        /// <returns>
        /// The first weak number in the XMAS signal or -1 if it is secure.
        /// </returns>
        public static long GetWeakNumber(ReadOnlySpan<long> values, int preambleLength)
        {
            for (int i = preambleLength; i < values.Length; i++)
            {
                long current = values[i];
                long[] preamble = values.Slice(i - preambleLength, preambleLength).ToArray();

                bool isValid = Maths.GetPermutations(preamble, 2)
                    .Where((p) => p.Sum() == current)
                    .Where((p) => p.ElementAt(0) != p.ElementAt(1))
                    .Any();

                if (!isValid)
                {
                    return values[i];
                }
            }

            return -1;
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> signal = await ReadResourceAsLinesAsync();

            long[] values = signal
                .Select((p) => ParseInt64(p))
                .ToArray();

            WeakNumber = GetWeakNumber(values, 25);

            if (Verbose)
            {
                Logger.WriteLine("The first weak number in the XMAS sequence is {0}.", WeakNumber);
            }

            return PuzzleResult.Create(WeakNumber);
        }
    }
}
