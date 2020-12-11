// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2018/day/1</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2018, 01, RequiresData = true)]
    public sealed class Day01 : Puzzle
    {
        /// <summary>
        /// Gets the frequency of the device calculated from the sequence.
        /// </summary>
        public int Frequency { get; private set; }

        /// <summary>
        /// Gets the frequency of the device calculated from the sequence that is repeated first.
        /// </summary>
        public int FirstRepeatedFrequency { get; private set; }

        /// <summary>
        /// Calculates the resulting frequency after applying the specified sequence.
        /// </summary>
        /// <param name="sequence">A sequence of frequency shifts to apply.</param>
        /// <returns>
        /// The resulting frequency from applying the sequence from <paramref name="sequence"/>.
        /// </returns>
        public static int CalculateFrequency(IList<int> sequence)
            => CalculateFrequencyWithRepetition(sequence).frequency;

        /// <summary>
        /// Calculates the resulting frequency after applying the specified sequence.
        /// </summary>
        /// <param name="sequence">A sequence of frequency shifts to apply.</param>
        /// <returns>
        /// The resulting frequency from applying the sequence from <paramref name="sequence"/>.
        /// </returns>
        public static (int frequency, int firstRepeat) CalculateFrequencyWithRepetition(IList<int> sequence)
        {
            int current = 0;
            int? frequency = null;
            int? firstRepeat = null;

            var history = new List<int>(sequence.Count)
            {
                current,
            };

            bool isInfinite = TendsToInfinity(sequence);

            do
            {
                foreach (int shift in sequence)
                {
                    int previous = current;
                    current += shift;

                    if (!firstRepeat.HasValue && history.Contains(current) && history[^2] != previous)
                    {
                        firstRepeat = current;
                    }

                    history.Add(current);
                }

                if (!frequency.HasValue)
                {
                    frequency = current;
                }
            }
            while (!firstRepeat.HasValue && !isInfinite);

            return (frequency.Value, firstRepeat ?? frequency.Value);
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<int> sequence = await ReadResourceAsSequenceAsync<int>();

            (Frequency, FirstRepeatedFrequency) = CalculateFrequencyWithRepetition(sequence);

            if (Verbose)
            {
                Logger.WriteLine($"The resulting frequency is {Frequency:N0}.");
                Logger.WriteLine($"The first repeated frequency is {FirstRepeatedFrequency:N0}.");
            }

            return PuzzleResult.Create(Frequency, FirstRepeatedFrequency);
        }

        /// <summary>
        /// Returns whether the specified sequences tends towards infinity.
        /// </summary>
        /// <param name="sequence">The sequence of integers.</param>
        /// <returns>
        /// <see langword="true"/> if the sequence tends towards positive or negative infinity.
        /// </returns>
        private static bool TendsToInfinity(IList<int> sequence)
            => Math.Abs(sequence.Sum(Math.Sign)) == sequence.Count((p) => p != 0);
    }
}
