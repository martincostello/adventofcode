// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/4</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 04, "Camp Cleanup", RequiresData = true)]
public sealed class Day04 : Puzzle
{
    /// <summary>
    /// Gets the number of assignment pairs where one range fully contains the other.
    /// </summary>
    public int FullyOverlappingAssignments { get; private set; }

    /// <summary>
    /// Gets the number of assignment pairs where one range partially contains the other.
    /// </summary>
    public int PartiallyOverlappingAssignments { get; private set; }

    /// <summary>
    /// Gets the number of assignment pairs where one range fully or partially contains the other.
    /// </summary>
    /// <param name="assignments">The section assignments.</param>
    /// <param name="partial">Whether to test that the pair only partially overlaps.</param>
    /// <returns>
    /// The number of assignment pairs where one range fully or partially contains the other.
    /// </returns>
    public static int GetOverlappingAssignments(IList<string> assignments, bool partial)
    {
        int count = 0;

        foreach (string assignment in assignments)
        {
            string[] split = assignment.Split(',');
            Range first = AsRange(split[0].AsNumberPair<int>('-'));
            Range second = AsRange(split[1].AsNumberPair<int>('-'));

            if (partial)
            {
                if (Overlaps(first, second) || Overlaps(second, first))
                {
                    count++;
                }
            }
            else
            {
                if (IsWithinRange(first, second) || IsWithinRange(second, first))
                {
                    count++;
                }
            }
        }

        return count;

        static Range AsRange((int Start, int End) pair)
            => new(pair.Start, pair.End);

        static bool IsWithinRange(Range range, Range other)
            => range.Start.Value >= other.Start.Value &&
               range.End.Value <= other.End.Value;

        static bool Overlaps(Range range, Range other)
        {
            return (range.Start.Value <= other.Start.Value && range.End.Value >= other.Start.Value) ||
                   (range.Start.Value <= other.End.Value && range.End.Value >= other.End.Value);
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var assignments = await ReadResourceAsLinesAsync();

        FullyOverlappingAssignments = GetOverlappingAssignments(assignments, false);
        PartiallyOverlappingAssignments = GetOverlappingAssignments(assignments, true);

        if (Verbose)
        {
            Logger.WriteLine(
                "There are {0:N0} assignment pairs where one range is entirely contained within the other.",
                FullyOverlappingAssignments);

            Logger.WriteLine(
                "There are {0:N0} assignment pairs where one range overlaps with the other.",
                PartiallyOverlappingAssignments);
        }

        return PuzzleResult.Create(FullyOverlappingAssignments, PartiallyOverlappingAssignments);
    }
}
