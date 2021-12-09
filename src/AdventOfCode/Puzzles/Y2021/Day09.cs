// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/9</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 09, RequiresData = true)]
public sealed class Day09 : Puzzle
{
    /// <summary>
    /// Gets the sum of the risk levels of all the low points in the heightmap.
    /// </summary>
    public int SumOfRiskLevels { get; private set; }

    /// <summary>
    /// Determines the level of risk of the low points in the specified heightmap.
    /// </summary>
    /// <param name="heightmap">The heightmap to analyze.</param>
    /// <returns>
    /// The sum of the risk levels of all the low points in the heightmap.
    /// </returns>
    public static int AnalyzeRisk(IList<string> heightmap)
    {
        var grid = new Dictionary<Point, int>(heightmap.Count * heightmap[0].Length);

        for (int y = 0; y < heightmap.Count; y++)
        {
            string row = heightmap[y];

            for (int x = 0; x < row.Length; x++)
            {
                grid[new(x, y)] = row[x] - '0';
            }
        }

        var lowPoints = new List<int>();

        var directions = new Size[]
        {
            new(0, -1),
            new(0, 1),
            new(-1, 0),
            new(1, 0),
        };

        foreach ((Point point, int value) in grid)
        {
            int lowCount = 0;
            int adjacentPoints = 0;

            foreach (Size direction in directions)
            {
                if (grid.TryGetValue(point + direction, out int other))
                {
                    adjacentPoints++;

                    if (value < other)
                    {
                        lowCount++;
                    }
                }
            }

            if (lowCount == adjacentPoints)
            {
                lowPoints.Add(value);
            }
        }

        return lowPoints.Select((p) => p + 1).Sum();
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> heightmap = await ReadResourceAsLinesAsync();

        SumOfRiskLevels = AnalyzeRisk(heightmap);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the risk levels of all low points on the heightmap is {0:N0}.", SumOfRiskLevels);
        }

        return PuzzleResult.Create(SumOfRiskLevels);
    }
}
