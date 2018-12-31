// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2018/day/2</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day02 : Puzzle2018
    {
        /// <summary>
        /// Gets the checksum of the box ids.
        /// </summary>
        public int Checksum { get; private set; }

        /// <summary>
        /// Calculates the checksum for the specified box Ids.
        /// </summary>
        /// <param name="boxIds">The box Ids to calculate the checksum for.</param>
        /// <returns>
        /// The checksum calculated from <paramref name="boxIds"/>.
        /// </returns>
        public static int CalculateChecksum(IEnumerable<string> boxIds)
        {
            int count2 = 0;
            int count3 = 0;

            foreach (string id in boxIds)
            {
                (int c2, int c3) = GetBoxScore(id);

                count2 += c2;
                count3 += c3;
            }

            return count2 * count3;
        }

        /// <summary>
        /// Gets the scores for the box with the specified Id
        /// </summary>
        /// <param name="id">The box Id to get the score for.</param>
        /// <returns>
        /// The scores of the specified Id.
        /// </returns>
        public static(int count2, int count3) GetBoxScore(string id)
        {
            var counts = new Dictionary<char, int>();

            foreach (char ch in id)
            {
                if (!counts.ContainsKey(ch))
                {
                    counts[ch] = 1;
                }
                else
                {
                    counts[ch]++;
                }
            }

            bool hasDoubles = counts.Values.Any((p) => p == 2);
            bool hasTriples = counts.Values.Any((p) => p == 3);

            return (hasDoubles ? 1 : 0, hasTriples ? 1 : 0);
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> ids = ReadResourceAsLines();

            Checksum = CalculateChecksum(ids);

            if (Verbose)
            {
                Logger.WriteLine($"The checksum is {Checksum:N0}.");
            }

            return 0;
        }
    }
}
