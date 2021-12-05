// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace MartinCostello.AdventOfCode.Puzzles.Y2018;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2018/day/6</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2018, 06, RequiresData = true)]
public sealed class Day06 : Puzzle
{
    /// <summary>
    /// Gets the largest non-infinite area between coordinates.
    /// </summary>
    public int LargestNonInfiniteArea { get; private set; }

    /// <summary>
    /// Returns the largest non-infinite area between a set of coordinates.
    /// </summary>
    /// <param name="coordinates">The coordinates to find the largest non-infinite area between.</param>
    /// <returns>
    /// The largest non-infinite area between the coordinates specified by <paramref name="coordinates"/>.
    /// </returns>
    public static int GetLargestArea(ICollection<string> coordinates)
    {
        var points = coordinates
            .Select((p) => p.AsNumbers<int>())
            .Select((p) => p.ToArray())
            .Select((p) => new Point(p[0], p[1]))
            .ToList();

        var ids = new Dictionary<Point, int>(points.Count);
        int id = 1;

        foreach (var point in points)
        {
            ids[point] = id++;
        }

        var map = new Dictionary<Point, int>(ids);

        // Get the maximum extent of the grid we need
        // to search within, which is one containing
        // all points with at least 1 space around each.
        int maxX = points.MaxBy((p) => p.X).X + 1;
        int maxY = points.MaxBy((p) => p.Y).Y + 1;

        for (int y = 0; y <= maxY; y++)
        {
            for (int x = 0; x <= maxX; x++)
            {
                var point = new Point(x, y);

                if (ids.ContainsKey(point))
                {
                    // Each point is the closest to itself
                    continue;
                }

                // Get the Manhattan distance each coordinate is from this point
                var distances = points
                    .Select((p) => new { Point = p, Distance = p - new Size(x, y) })
                    .Select((p) => new { Point = p.Point, Distance = Math.Abs(p.Distance.X) + Math.Abs(p.Distance.Y) })
                    .OrderBy((p) => p.Distance)
                    .ToList();

                var first = distances[0];
                var second = distances[1];

                if (first.Distance == second.Distance)
                {
                    // Two or more coordinates are equidistant from here
                    map[point] = 0;
                }
                else
                {
                    map[point] = ids[first.Point];
                }
            }
        }

        // Find the areas which touch the bounds and are therefore infinite
        var infiniteAreas = new HashSet<int>();

        for (int x = 0; x <= maxX; x++)
        {
            infiniteAreas.Add(map[new(x, 0)]);
            infiniteAreas.Add(map[new(x, maxY)]);
        }

        for (int y = 0; y <= maxY; y++)
        {
            infiniteAreas.Add(map[new(0, y)]);
            infiniteAreas.Add(map[new(maxX, y)]);
        }

        // Compute each non-infinite area and return the largest one
        return map
            .Where((p) => !infiniteAreas.Contains(p.Value))
            .GroupBy((p) => p.Value)
            .OrderByDescending((p) => p.Count())
            .Select((p) => p.Count())
            .First();
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var coordinates = await ReadResourceAsLinesAsync();

        LargestNonInfiniteArea = GetLargestArea(coordinates);

        if (Verbose)
        {
            Logger.WriteLine("The largest non-infinite area is {0:N0}.", LargestNonInfiniteArea);
        }

        return PuzzleResult.Create(LargestNonInfiniteArea);
    }
}
