// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2017/day/13</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day13 : Puzzle2017
    {
        /// <summary>
        /// Gets the severity of the trip through the firewall.
        /// </summary>
        public int Severity { get; private set; }

        /// <summary>
        /// Gets the shortest delay for a trip through the firewall that has a severity of zero.
        /// </summary>
        public int ShortestDelay { get; private set; }

        /// <summary>
        /// Gets the severity of a trip through the firewall with the specified scanner depth and ranges.
        /// </summary>
        /// <param name="depthRanges">A collection of scanner depths and ranges.</param>
        /// <returns>
        /// The severity of the trip through the firewall with the scanners specified by <paramref name="depthRanges"/>.
        /// </returns>
        public static int GetSeverityOfTrip(ICollection<string> depthRanges)
        {
            var configuration = depthRanges
                .Select((p) => p.Split(Arrays.Colon))
                .Select((p) => new { layer = ParseInt32(p[0]), range = ParseInt32(p[1].Trim()) })
                .ToList();

            var depths = configuration.Select((p) => p.layer).ToArray();
            var ranges = configuration.Select((p) => p.range).ToArray();

            int severity = 0;

            for (int i = 0; i < depths.Length; i++)
            {
                int depth = depths[i];
                int range = ranges[i];

                int index = depth % ((2 * range) - 2);
                bool detected = index == 0;

                if (detected)
                {
                    severity += depth * range;
                }
            }

            return severity;
        }

        /// <summary>
        /// Gets the shortest delay for a trip through the firewall with the specified scanner depth and ranges that does not capture the packet.
        /// </summary>
        /// <param name="depthRanges">A collection of scanner depths and ranges.</param>
        /// <returns>
        /// The shortest delay to a trip with the scanners specified by <paramref name="depthRanges"/> that does not capture the packet.
        /// </returns>
        public static int GetShortestDelayForNeverCaught(ICollection<string> depthRanges)
        {
            var configuration = depthRanges
                .Select((p) => p.Split(Arrays.Colon))
                .Select((p) => new { layer = ParseInt32(p[0]), range = ParseInt32(p[1].Trim()) })
                .ToList();

            var depths = configuration.Select((p) => p.layer).ToArray();
            var ranges = configuration.Select((p) => p.range).ToArray();

            bool caught = false;
            int delay = 0;

            while (!caught)
            {
                caught = true;

                for (int i = 0; i < depths.Length; i++)
                {
                    int depth = depths[i];
                    int range = ranges[i];

                    int index = (depth + delay) % ((2 * range) - 2);
                    bool detected = index == 0;

                    if (detected)
                    {
                        delay++;
                        caught = false;
                        break;
                    }
                }
            }

            return delay;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> depthRanges = ReadResourceAsLines();

            Severity = GetSeverityOfTrip(depthRanges);
            ShortestDelay = GetShortestDelayForNeverCaught(depthRanges);

            if (Verbose)
            {
                Console.WriteLine($"The severity of the trip through the firewall is {Severity:N0}.");
                Console.WriteLine($"The fewest number of picoseconds that the packet needs to be delayed by to pass through the firewall without being caught is {ShortestDelay:N0}.");
            }

            return 0;
        }
    }
}
