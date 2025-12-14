// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/7</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 07, "Laboratories", RequiresData = true)]
public sealed class Day07 : Puzzle<int, long>
{
    /// <summary>
    /// Finds the number of times the beam splits and the number of timelines it exists in for the
    /// specified tachyon manifold by simulating the path of the beam from the origin in the diagram.
    /// </summary>
    /// <param name="diagram">The diagram of the tachyon manifold.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The number of times the beam splits and the number of timelines it exists on.
    /// </returns>
    public static (int Splits, long Timelines) Simulate(IReadOnlyList<string> diagram, CancellationToken cancellationToken)
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

        var splitters = new Dictionary<Point, long>(grid.Locations.Count);

        long timelines = Trace(grid, origin, splitters, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return (splitters.Count, timelines);

        static long Trace(SquareGrid grid, Point origin, Dictionary<Point, long> splitters, CancellationToken cancellationToken)
        {
            while (grid.InBounds(origin) && !cancellationToken.IsCancellationRequested)
            {
                origin -= Directions.Up;

                if (splitters.TryGetValue(origin, out long timelines))
                {
                    return timelines;
                }

                if (grid.Locations.Contains(origin))
                {
                    timelines = Trace(grid, origin + new Size(-1, 1), splitters, cancellationToken);
                    timelines += Trace(grid, origin + new Size(1, 1), splitters, cancellationToken);

                    splitters[origin] = timelines;

                    return timelines;
                }
            }

            return 1;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (diagram, logger, cancellationToken) =>
            {
                (int splits, long timelines) = Simulate(diagram, cancellationToken);

                if (logger is { })
                {
                    logger.WriteLine("The beam splits {0} times.", splits);
                    logger.WriteLine("A single tachyon particle would exist in {0} different timelines.", timelines);
                }

                return (splits, timelines);
            },
            cancellationToken);
    }
}
