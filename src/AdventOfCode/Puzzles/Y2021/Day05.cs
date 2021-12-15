// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/5</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 05, "Hydrothermal Venture", RequiresData = true)]
public sealed class Day05 : Puzzle
{
    /// <summary>
    /// Gets the number of positions with more than one vent.
    /// </summary>
    public int OverlappingVents { get; private set; }

    /// <summary>
    /// Gets the number of positions with more than one vent
    /// with line segments that are diagonal considered.
    /// </summary>
    public int OverlappingVentsWithDiagonals { get; private set; }

    /// <summary>
    /// Gets the number of positions with more than one vent from
    /// navigating the field of vents specified by the line segments.
    /// </summary>
    /// <param name="lineSegments">The line segments that describe the location of the vents.</param>
    /// <param name="useDiagonals">Whether to consider line segments that are diagonal.</param>
    /// <returns>
    /// The number of positions with more than one vent.
    /// </returns>
    public static int NavigateVents(ICollection<string> lineSegments, bool useDiagonals)
    {
        var field = new Dictionary<Point, int>(lineSegments.Count);

        foreach (string segment in lineSegments)
        {
            string[] points = segment.Split(" -> ");
            int[] startCoordinates = points[0].AsNumbers<int>().ToArray();
            int[] endCoordinates = points[1].AsNumbers<int>().ToArray();

            var start = new Point(startCoordinates[0], startCoordinates[1]);
            var end = new Point(endCoordinates[0], endCoordinates[1]);

            if (useDiagonals || start.X == end.X || start.Y == end.Y)
            {
                int deltaX = Math.Sign(end.X - start.X);
                int deltaY = Math.Sign(end.Y - start.Y);
                int length = Math.Max(Math.Abs(end.X - start.X), Math.Abs(end.Y - start.Y));

                for (int i = 0; i <= length; i++)
                {
                    int x = start.X + (i * deltaX);
                    int y = start.Y + (i * deltaY);

                    field.AddOrIncrement(new(x, y), 1);
                }
            }
        }

        return field.Count((p) => p.Value > 1);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> lineSegments = await ReadResourceAsLinesAsync();

        OverlappingVents = NavigateVents(lineSegments, useDiagonals: false);
        OverlappingVentsWithDiagonals = NavigateVents(lineSegments, useDiagonals: true);

        if (Verbose)
        {
            Logger.WriteLine(
                "There are overlapping vents at {0:N0} points without considering diagonals.",
                OverlappingVents);

            Logger.WriteLine(
                "There are overlapping vents at {0:N0} points considering diagonals.",
                OverlappingVentsWithDiagonals);
        }

        return PuzzleResult.Create(OverlappingVents, OverlappingVentsWithDiagonals);
    }
}
