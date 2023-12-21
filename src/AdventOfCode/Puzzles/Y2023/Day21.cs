// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/21</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 21, "Step Counter", RequiresData = true)]
public sealed class Day21 : Puzzle
{
    /// <summary>
    /// Gets the number of plots the elf can reach in exactly 64 steps.
    /// </summary>
    public int Plots64 { get; private set; }

    /// <summary>
    /// Finds the number of plots the elf can reach in the specified number of steps.
    /// </summary>
    /// <param name="map">The map of the garden.</param>
    /// <param name="steps">The number of steps to walk.</param>
    /// <returns>
    /// The number of plots the elf can reach in exactly <paramref name="steps"/> steps.
    /// </returns>
    public static int Walk(IList<string> map, int steps)
    {
        int height = map.Count;
        int width = map[0].Length;

        var origin = Point.Empty;
        var grid = new SquareGrid(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                char tile = map[y][x];

                if (tile is '#')
                {
                    grid.Borders.Add(new(x, y));
                }
                else if (tile is 'S')
                {
                    origin = new(x, y);
                }
            }
        }

        var garden = new Garden(grid, steps);

        var walk = PathFinding.BreadthFirst(garden, new(origin, 0), CancellationToken.None);
        return walk.Where((p) => p.Steps == steps).Select((p) => p.Location).Distinct().Count();
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var map = await ReadResourceAsLinesAsync(cancellationToken);

        Plots64 = Walk(map, steps: 64);

        if (Verbose)
        {
            Logger.WriteLine("The elf could reach {0} plots in exactly 64 steps.", Plots64);
        }

        return PuzzleResult.Create(Plots64);
    }

    private sealed record Step(Point Location, int Steps);

    private sealed class Garden(SquareGrid grid, int stepGoal) : IGraph<Step>
    {
        public IEnumerable<Step> Neighbors(Step id)
        {
            int steps = id.Steps + 1;

            if (steps > stepGoal)
            {
                yield break;
            }

            var vectors = Directions.All;

            for (int i = 0; i < vectors.Count; i++)
            {
                Point next = id.Location + vectors[i];

                if (grid.InBounds(next) && grid.IsPassable(next))
                {
                    yield return new(next, steps);
                }
            }
        }
    }
}
