// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2018/day/1</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day01 : Puzzle2018
    {
        /// <summary>
        /// Gets the frequency of the device calculated from the sequence.
        /// </summary>
        public int Frequency { get; private set; }

        /// <summary>
        /// Calculates the resulting frequency after applying the specified sequence.
        /// </summary>
        /// <param name="sequence">A sequence of frequency shifts to apply.</param>
        /// <returns>
        /// The resulting frequency from applying the sequence from <paramref name="sequence"/>.
        /// </returns>
        public static int CalculateFrequency(IEnumerable<string> sequence)
        {
            return sequence.Select((p) => ParseInt32(p)).Sum();
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> sequence = ReadResourceAsLines();

            Frequency = CalculateFrequency(sequence);

            if (Verbose)
            {
                Console.WriteLine($"The resulting frequency is {Frequency:N0}.");
            }

            return 0;
        }
    }
}
