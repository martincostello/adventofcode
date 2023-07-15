// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/9</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 09, "Smoke Basin", RequiresData = true)]
public sealed class Day09 : Puzzle
{
    /// <summary>
    /// Gets the sum of the risk levels of all the low points in the heightmap.
    /// </summary>
    public int SumOfRiskLevels { get; private set; }

    /// <summary>
    /// Gets the total area of the three largest basins in the heightmap.
    /// </summary>
    public int AreaOfThreeLargestBasins { get; private set; }

    /// <summary>
    /// Determines the level of risk of the low points in the specified heightmap.
    /// </summary>
    /// <param name="heightmap">The heightmap to analyze.</param>
    /// <param name="cancellationToken">The optional <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The sum of the risk levels of all the low points in the heightmap and
    /// the total area of the three largest basins in the heightmap.
    /// </returns>
    public static (int SumOfRiskLevels, int AreaOfThreeLargestBasins) AnalyzeRisk(
        IList<string> heightmap,
        CancellationToken cancellationToken = default)
    {
        int width = heightmap[0].Length;
        int height = heightmap.Count;

        var heights = new Dictionary<Point, int>(width * height);

        for (int y = 0; y < height; y++)
        {
            string row = heightmap[y];

            for (int x = 0; x < width; x++)
            {
                heights[new(x, y)] = row[x] - '0';
            }
        }

        var basins = new Heightmap(width, height);

        foreach ((var point, int value) in heights)
        {
            // The value of 9 does not count to a basin,
            // so is effectively the wall/frontier to it.
            if (value == 9)
            {
                basins.Borders.Add(point);
            }
            else
            {
                basins.Locations.Add(point);
            }
        }

        var graph = new Graph<Point>();
        var lowPoints = new Dictionary<Point, int>();

        foreach (Point point in basins.Locations)
        {
            int lowerPoints = 0;
            var neighbors = new List<Point>(4);

            int thisHeight = heights[point];

            foreach (Point neighbor in basins.Neighbors(point))
            {
                neighbors.Add(neighbor);

                if (thisHeight < heights[neighbor])
                {
                    lowerPoints++;
                }
            }

            graph.Edges[point] = neighbors;

            if (lowerPoints == neighbors.Count)
            {
                lowPoints[point] = thisHeight;
            }
        }

        var basinAreas = new List<int>(lowPoints.Count);

        foreach (var point in lowPoints.Keys)
        {
            var basin = PathFinding.BreadthFirst(graph, point, cancellationToken);
            basinAreas.Add(basin.Count);
        }

        int sumOfRiskLevels = lowPoints.Values
            .Select((p) => p + 1)
            .Sum();

        int areaOfThreeLargestBasins = basinAreas
            .OrderDescending()
            .Take(3)
            .Aggregate((x, y) => x *= y);

        return (sumOfRiskLevels, areaOfThreeLargestBasins);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var heightmap = await ReadResourceAsLinesAsync(cancellationToken);

        (SumOfRiskLevels, AreaOfThreeLargestBasins) = AnalyzeRisk(heightmap, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the risk levels of all low points on the heightmap is {0:N0}.", SumOfRiskLevels);
            Logger.WriteLine("The area of the three largest basins in the heightmap is {0:N0}.", AreaOfThreeLargestBasins);
        }

        return PuzzleResult.Create(SumOfRiskLevels, AreaOfThreeLargestBasins);
    }

    private sealed class Heightmap(int width, int height) : SquareGrid(width, height)
    {
        public override long Cost(Point a, Point b) => 1;
    }
}
