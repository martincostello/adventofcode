// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/11</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 11, RequiresData = true)]
public sealed class Day11 : Puzzle
{
    /// <summary>
    /// Gets the number of flashes after 100 steps.
    /// </summary>
    public int Flashes100 { get; private set; }

    /// <summary>
    /// Simulates the energy levels of the specified octopuses.
    /// </summary>
    /// <param name="grid">The grid of octopus energy levels.</param>
    /// <param name="iterations">The number of iterations to simulate the energy levels for.</param>
    /// <returns>
    /// The syntax error score and the middle auto-complete score for the navigation subsystem.
    /// </returns>
    public static int Simulate(IList<string> grid, int iterations)
    {
        const int FlashPoint = 10;

        var initial = new Dictionary<Point, int>(grid.Count * grid[0].Length);

        for (int y = 0; y < grid.Count; y++)
        {
            string line = grid[y];

            for (int x = 0; x < line.Length; x++)
            {
                initial[new(x, y)] = line[x] - '0';
            }
        }

        var previous = initial;
        Dictionary<Point, int> current;

        int flashes = 0;

        for (int i = 0; i < iterations; i++)
        {
            current = new Dictionary<Point, int>(previous.Count);

            var pointsToFlash = new List<Point>();

            foreach ((Point point, int value) in previous)
            {
                int nextValue = value + 1;

                if (nextValue >= FlashPoint)
                {
                    pointsToFlash.Add(point);
                }

                current[point] = nextValue;
            }

            var flashedPoints = new HashSet<Point>();

            while (pointsToFlash.Count > 0)
            {
                var neighborsToFlash = new List<Point>();

                foreach (Point point in pointsToFlash)
                {
                    flashedPoints.Add(point);

                    foreach (Point neighbor in Neighbors(point, current))
                    {
                        if (pointsToFlash.Contains(neighbor) ||
                            neighborsToFlash.Contains(neighbor) ||
                            flashedPoints.Contains(neighbor))
                        {
                            continue;
                        }

                        int newValue = ++current[neighbor];

                        if (newValue >= FlashPoint)
                        {
                            neighborsToFlash.Add(neighbor);
                        }
                    }
                }

                pointsToFlash = neighborsToFlash;
            }

            foreach (Point point in flashedPoints)
            {
                flashes++;
                current[point] = 0;
            }

            previous = current;
        }

        return flashes;

        static List<Point> Neighbors(Point point, Dictionary<Point, int> grid)
        {
            var neighbors = new List<Point>(8);

            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    var neighbor = point + new Size(x, y);

                    if (grid.TryGetValue(neighbor, out int value))
                    {
                        neighbors.Add(neighbor);
                    }
                }
            }

            return neighbors;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> lines = await ReadResourceAsLinesAsync();

        Flashes100 = Simulate(lines, iterations: 100);

        if (Verbose)
        {
            Logger.WriteLine("After 100 steps there are {0:N0} flashes.", Flashes100);
        }

        return PuzzleResult.Create(Flashes100);
    }
}
