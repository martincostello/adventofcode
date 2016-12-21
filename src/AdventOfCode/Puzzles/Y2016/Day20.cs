// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2016/day/20</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day20 : Puzzle2016
    {
        /// <summary>
        /// Gets the value of the lowest IP address that is not blocked.
        /// </summary>
        public uint LowestNonblockedIP { get; private set; }

        /// <summary>
        /// Returns the value of the lowest IP address that is not
        /// blocked by the specified IP address range blacklist.
        /// </summary>
        /// <param name="blacklist">The IP address ranges that form the blacklist.</param>
        /// <returns>
        /// The lowest IP address that is not blocked, as a 32-bit integer.
        /// </returns>
        internal static uint GetLowestNonblockedIP(IEnumerable<string> blacklist)
        {
            IList<Tuple<uint, uint>> ranges = new List<Tuple<uint, uint>>();

            foreach (string range in blacklist)
            {
                string[] split = range.Split(Arrays.Dash);

                uint low = ParseUInt32(split[0]);
                uint high = ParseUInt32(split[1]);

                ranges.Add(Tuple.Create(low, high));
            }

            ranges = ranges
                .OrderBy((p) => p.Item1)
                .ThenBy((p) => p.Item2)
                .ToList();

            for (int i = 0; i < ranges.Count - 1; i++)
            {
                var range1 = ranges[i];
                var range2 = ranges[i + 1];

                if (range1.Item2 > range2.Item1 ||
                    range1.Item2 == range2.Item1 - 1)
                {
                    if (range2.Item1 < range1.Item2 &&
                        range2.Item2 < range1.Item2)
                    {
                        // Exclude the second range if entirely within the first
                        ranges.RemoveAt(i + 1);
                    }
                    else
                    {
                        var composite = Tuple.Create(range1.Item1, range2.Item2);

                        ranges.RemoveAt(i);
                        ranges.RemoveAt(i);
                        ranges.Insert(i, composite);

                        ranges = ranges
                            .OrderBy((p) => p.Item1)
                            .ThenBy((p) => p.Item2)
                            .ToList();
                    }

                    i--;
                }
            }

            uint result = 0;
            uint lastHigh = 0;

            foreach (var range in ranges)
            {
                uint low = range.Item1;
                uint high = range.Item2;

                if (result < low && result > lastHigh)
                {
                    break;
                }

                result = high + 1;
                lastHigh = high;
            }

            return result;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> ranges = ReadResourceAsLines();

            LowestNonblockedIP = GetLowestNonblockedIP(ranges);

            Console.WriteLine($"The lowest-valued IP that is not blocked is {LowestNonblockedIP}.");

            return 0;
        }
    }
}
