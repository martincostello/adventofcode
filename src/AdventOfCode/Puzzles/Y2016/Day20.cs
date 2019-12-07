// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2016/day/20</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day20 : Puzzle2016
    {
        /// <summary>
        /// Gets the number of IP addresses that are not blocked.
        /// </summary>
        public uint AllowedIPCount { get; private set; }

        /// <summary>
        /// Gets the value of the lowest IP address that is not blocked.
        /// </summary>
        public uint LowestNonblockedIP { get; private set; }

        /// <summary>
        /// Returns the value of the lowest IP address that is not
        /// blocked by the specified IP address range blacklist.
        /// </summary>
        /// <param name="maxValue">The maximum possible IP address value.</param>
        /// <param name="blacklist">The IP address ranges that form the blacklist.</param>
        /// <param name="count">When the method returns contains the number of allowed IP addresses.</param>
        /// <returns>
        /// The lowest IP address that is not blocked, as a 32-bit integer.
        /// </returns>
        internal static uint GetLowestNonblockedIP(uint maxValue, IEnumerable<string> blacklist, out uint count)
        {
            count = 0;

            // Parse the IP ranges for the blacklist and sort
            var ranges = new List<(uint start, uint end)>();

            foreach (string range in blacklist)
            {
                string[] split = range.Split('-');

                uint low = ParseUInt32(split[0]);
                uint high = ParseUInt32(split[1]);

                ranges.Add((low, high));
            }

            ranges = ranges
                .OrderBy((p) => p.start)
                .ThenBy((p) => p.end)
                .ToList();

            for (int i = 0; i < ranges.Count - 1; i++)
            {
                var range1 = ranges[i];
                var range2 = ranges[i + 1];

                if (range1.end > range2.start ||
                    range1.end == range2.start - 1)
                {
                    if (range2.start < range1.end &&
                        range2.end < range1.end)
                    {
                        // Exclude the second range if entirely within the first
                        ranges.RemoveAt(i + 1);
                    }
                    else
                    {
                        // Create a new range that combines the existing ranges
                        var composite = (range1.start, range2.end);

                        // Remove the original ranges and replace with the new one
                        ranges.RemoveAt(i);
                        ranges.RemoveAt(i);
                        ranges.Insert(i, composite);

                        ranges = ranges
                            .OrderBy((p) => p.start)
                            .ThenBy((p) => p.end)
                            .ToList();
                    }

                    // Compare from the new range onwards
                    i--;
                }
            }

            uint result = 0;

            // Count the number of IPs not in the blacklist ranges
            for (int i = 0; i < ranges.Count - 1; i++)
            {
                var range1 = ranges[i];
                var range2 = ranges[i + 1];

                count += range2.start - range1.end - 1;

                if (i == 0)
                {
                    // As ranges are sorted and do not overlap,
                    // the lowest allowed IP is 1 more than the
                    // high value for the first blacklisted range.
                    result = range1.end + 1;
                }
            }

            // Add on the remaining IPs if the last blacklist
            // does not run to the maximum allowed IP address.
            var (start, end) = ranges.Last();

            if (end != maxValue)
            {
                count += maxValue - end;
            }

            return result;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> ranges = ReadResourceAsLines();

            LowestNonblockedIP = GetLowestNonblockedIP(uint.MaxValue, ranges, out uint count);
            AllowedIPCount = count;

            if (Verbose)
            {
                Logger.WriteLine($"The lowest-valued IP that is not blocked is {LowestNonblockedIP}.");
                Logger.WriteLine($"The number of IP addresses allowed is {AllowedIPCount:N0}.");
            }

            return 0;
        }
    }
}
