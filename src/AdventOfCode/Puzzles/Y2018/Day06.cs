// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2018/day/6</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2018, 06, "Chronal Coordinates", RequiresData = true)]
public sealed class Day06 : Puzzle
{
    /// <summary>
    /// Gets the largest non-infinite area between coordinates.
    /// </summary>
    public int LargestNonInfiniteArea { get; private set; }

    /// <summary>
    /// Gets the size of the area within the region defined by the distance limit.
    /// </summary>
    public int AreaOfRegion { get; private set; }

    /// <summary>
    /// Returns the largest non-infinite area between a set of coordinates.
    /// </summary>
    /// <param name="coordinates">The coordinates to find the largest non-infinite area between.</param>
    /// <param name="distanceLimit">The distance limit to use to determine the area of the region.</param>
    /// <returns>
    /// The largest non-infinite area between the coordinates specified by <paramref name="coordinates"/>
    /// and the area of the region which is less than <paramref name="distanceLimit"/> from all coordinates.
    /// </returns>
    public static (int LargestNonInfiniteArea, int AreaOfRegion) GetLargestArea(ICollection<string> coordinates, int distanceLimit)
    {
        var points = coordinates
            .Select((p) => p.AsNumberPair<int>())
            .Select((p) => new Point(p.First, p.Second))
            .ToList();

        var ids = new Dictionary<Point, int>(points.Count);
        int id = 1;

        foreach (var point in points)
        {
            ids[point] = id++;
        }

        var closestPoints = new Dictionary<Point, int>(ids);
        var closeRegion = new HashSet<Point>();

        // Get the maximum extent of the grid we need
        // to search within, which is one containing
        // all points with at least 1 space around each.
        int maxX = points.Max((p) => p.X) + 1;
        int maxY = points.Max((p) => p.Y) + 1;

        for (int y = 0; y <= maxY; y++)
        {
            for (int x = 0; x <= maxX; x++)
            {
                var point = new Point(x, y);

                // Get the Manhattan distance each coordinate is from this point
                var distances = points
                    .Select((p) => new { Point = p, Distance = p - new Size(x, y) })
                    .Select((p) => new { Point = p.Point, Distance = p.Distance.ManhattanDistance() })
                    .OrderBy((p) => p.Distance)
                    .ToList();

                var first = distances[0];
                var second = distances[1];

                if (first.Distance == second.Distance)
                {
                    // Two or more coordinates are equidistant from here
                    closestPoints[point] = 0;
                }
                else
                {
                    closestPoints[point] = ids[first.Point];
                }

                int totalDistance = distances.Sum((p) => p.Distance);

                if (totalDistance < distanceLimit)
                {
                    closeRegion.Add(point);
                }
            }
        }

        // Find the areas which touch the bounds and are therefore infinite
        var infiniteAreas = new HashSet<int>();

        for (int x = 0; x <= maxX; x++)
        {
            infiniteAreas.Add(closestPoints[new(x, 0)]);
            infiniteAreas.Add(closestPoints[new(x, maxY)]);
        }

        for (int y = 0; y <= maxY; y++)
        {
            infiniteAreas.Add(closestPoints[new(0, y)]);
            infiniteAreas.Add(closestPoints[new(maxX, y)]);
        }

        // Compute each non-infinite area and return the largest one
        int largestNonInfiniteArea = closestPoints
            .Where((p) => !infiniteAreas.Contains(p.Value))
            .GroupBy((p) => p.Value)
            .Select((p) => p.Count())
            .OrderByDescending((p) => p)
            .First();

        int areaOfRegion = closeRegion.Count;

        return (largestNonInfiniteArea, areaOfRegion);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var coordinates = await ReadResourceAsLinesAsync();

        const int DistanceLimit = 10_000;

        (LargestNonInfiniteArea, AreaOfRegion) = GetLargestArea(coordinates, DistanceLimit);

        if (Verbose)
        {
            Logger.WriteLine("The largest non-infinite area is {0:N0}.", LargestNonInfiniteArea);

            Logger.WriteLine(
                "The size of the region containing all locations which have a total distance to all given coordinates of less than {0:N0} is {1:N0}.",
                DistanceLimit,
                AreaOfRegion);
        }

        return PuzzleResult.Create(LargestNonInfiniteArea, AreaOfRegion);
    }
}
