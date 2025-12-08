// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/20</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2016, 20, "Firewall Rules", RequiresData = true)]
public sealed class Day20 : Puzzle<uint, uint>
{
    /// <summary>
    /// Returns the value of the lowest IP address that is not
    /// blocked by the specified IP address range deny-list.
    /// </summary>
    /// <param name="maxValue">The maximum possible IP address value.</param>
    /// <param name="denyList">The IP address ranges that form the deny-list.</param>
    /// <param name="count">When the method returns contains the number of allowed IP addresses.</param>
    /// <returns>
    /// The lowest IP address that is not blocked, as a 32-bit integer.
    /// </returns>
    internal static uint GetLowestNonblockedIP(uint maxValue, ICollection<string> denyList, out uint count)
    {
        count = 0;

        // Parse the IP ranges for the deny-list and sort
        var ranges = new List<(uint Start, uint End)>(denyList.Count);

        foreach (string range in denyList)
        {
            ranges.Add(range.AsNumberPair<uint>('-'));
        }

        ranges =
        [
            ..ranges.OrderBy((p) => p.Start)
                    .ThenBy((p) => p.End),
        ];

        for (int i = 0; i < ranges.Count - 1; i++)
        {
            var (start1, end1) = ranges[i];
            var (start2, end2) = ranges[i + 1];

            if (end1 > start2 || end1 == start2 - 1)
            {
                if (start2 < end1 && end2 < end1)
                {
                    // Exclude the second range if entirely within the first
                    ranges.RemoveAt(i + 1);
                }
                else
                {
                    // Create a new range that combines the existing ranges
                    var composite = (start1, end2);

                    // Remove the original ranges and replace with the new one
                    ranges.RemoveAt(i);
                    ranges.RemoveAt(i);
                    ranges.Insert(i, composite);

                    ranges =
                    [
                        ..ranges.OrderBy((p) => p.Start)
                                .ThenBy((p) => p.End),
                    ];
                }

                // Compare from the new range onwards
                i--;
            }
        }

        uint result = 0;

        // Count the number of IPs not in the deny-list ranges
        for (int i = 0; i < ranges.Count - 1; i++)
        {
            var (_, end1) = ranges[i];
            var (start2, _) = ranges[i + 1];

            count += start2 - end1 - 1;

            if (i == 0)
            {
                // As ranges are sorted and do not overlap,
                // the lowest allowed IP is 1 more than the
                // high value for the first deny-listed range.
                result = end1 + 1;
            }
        }

        // Add on the remaining IPs if the last deny-list
        // does not run to the maximum allowed IP address.
        var (_, end) = ranges[^1];

        if (end != maxValue)
        {
            count += maxValue - end;
        }

        return result;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (ranges, logger, _) =>
            {
                uint allowedIPCount = GetLowestNonblockedIP(uint.MaxValue, ranges, out uint count);
                uint lowestNonblockedIP = count;

                if (logger is { })
                {
                    logger.WriteLine($"The lowest-valued IP that is not blocked is {allowedIPCount}.");
                    logger.WriteLine($"The number of IP addresses allowed is {lowestNonblockedIP:N0}.");
                }

                return (allowedIPCount, lowestNonblockedIP);
            },
            cancellationToken);
    }
}
