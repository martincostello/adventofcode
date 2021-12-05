// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/5</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 05, RequiresData = true)]
public sealed class Day05 : Puzzle
{
    /// <summary>
    /// Gets the number of positions with more than one vent.
    /// </summary>
    public int OverlappingVents { get; private set; }

    /// <summary>
    /// Gets the number of positions with more than one vent from
    /// navigating the field of vents specified by the line segments.
    /// </summary>
    /// <param name="lineSegments">The line segments that describe the location of the vents.</param>
    /// <returns>
    /// The number of positions with more than one vent.
    /// </returns>
    public static int NavigateVents(IEnumerable<string> lineSegments)
    {
        var field = new Dictionary<Point, int>();

        foreach (string segment in lineSegments)
        {
            string[] points = segment.Split(" -> ");
            int[] startCoordinates = points[0].AsNumbers<int>().ToArray();
            int[] endCoordinates = points[1].AsNumbers<int>().ToArray();

            var start = new Point(startCoordinates[0], startCoordinates[1]);
            var end = new Point(endCoordinates[0], endCoordinates[1]);

            if (start.X == end.X || start.Y == end.Y)
            {
                int startX = Math.Min(start.X, end.X);
                int endX = Math.Max(start.X, end.X);
                int startY = Math.Min(start.Y, end.Y);
                int endY = Math.Max(start.Y, end.Y);

                for (int x = startX; x <= endX; x++)
                {
                    for (int y = startY; y <= endY; y++)
                    {
                        var point = new Point(x, y);

                        if (field.ContainsKey(point))
                        {
                            field[point]++;
                        }
                        else
                        {
                            field[point] = 1;
                        }
                    }
                }
            }
        }

        return field.Count((p) => p.Value > 1);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> lineSegments = await ReadResourceAsLinesAsync();

        OverlappingVents = NavigateVents(lineSegments);

        if (Verbose)
        {
            Logger.WriteLine("There are overlapping vents at {0:N0} points.", OverlappingVents);
        }

        return PuzzleResult.Create(OverlappingVents);
    }
}
