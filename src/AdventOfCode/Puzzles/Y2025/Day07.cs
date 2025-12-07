// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/7</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 07, "Laboratories", RequiresData = true)]
public sealed class Day07 : Puzzle<int, int>
{
    /// <summary>
    /// Finds the number of times the beam splits in the specified tachyon
    /// manifold by simulating the path of the beam from the origin in the diagram.
    /// </summary>
    /// <param name="diagram">The diagram of the tachyon manifold.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The number of times the beam splits.
    /// </returns>
    public static int Simulate(IReadOnlyList<string> diagram, CancellationToken cancellationToken)
    {
        int width = diagram[0].Length;
        int height = diagram.Count;

        var grid = new SquareGrid(width, height);

        var origin = grid.VisitCells(diagram, Point.Empty, (grid, point, cell, origin) =>
        {
            if (cell is '^')
            {
                grid.Locations.Add(point);
            }
            else if (cell is 'S')
            {
                origin = point;
            }

            return origin;
        });

        var splits = new HashSet<Point>();

        Trace(grid, origin, splits, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return splits.Count;

        static void Trace(SquareGrid grid, Point origin, HashSet<Point> splits, CancellationToken cancellationToken)
        {
            while (grid.InBounds(origin) && !cancellationToken.IsCancellationRequested)
            {
                origin -= Directions.Up;

                if (splits.Contains(origin))
                {
                    return;
                }

                if (grid.Locations.Contains(origin))
                {
                    splits.Add(origin);

                    Trace(grid, origin + new Size(-1, 1), splits, cancellationToken);
                    Trace(grid, origin + new Size(1, 1), splits, cancellationToken);

                    return;
                }
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var diagram = await ReadResourceAsLinesAsync(cancellationToken);

        Solution1 = Simulate(diagram, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("The beam splits {0} times.", Solution1);
        }

        return PuzzleResult.Create(Solution1);
    }
}
