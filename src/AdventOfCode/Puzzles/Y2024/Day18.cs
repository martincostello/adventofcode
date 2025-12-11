// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/18</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 18, "RAM Run", RequiresData = true)]
public sealed class Day18 : Puzzle<int, string>
{
    /// <summary>
    /// Simulates RAM falling into the specified region for the specified period of time.
    /// </summary>
    /// <param name="coordinates">The coordinates of the bytes that will fall into the region.</param>
    /// <param name="size">The height and width of the region in bytes.</param>
    /// <param name="ticks">The number of ticks to simulate for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The minimum number of steps needed to reach the exit at the time specified by
    /// <paramref name="ticks"/> and the coordinates of the byte that blocks the exit.
    /// </returns>
    public static (int MinimumSteps, string BlockingByte) Simulate(
        IList<string> coordinates,
        int size,
        int ticks,
        CancellationToken cancellationToken)
    {
        var bytes = new List<Point>();

        for (int i = 0; i < coordinates.Count; i++)
        {
            (int x, int y) = coordinates[i].AsNumberPair<int>();
            bytes.Add(new(x, y));
        }

        var grid = new SquareGrid(size, size);
        grid.VisitCells(static (grid, location) => grid.Locations.Add(location));

        for (int i = 0; i < ticks && i < bytes.Count; i++)
        {
            var location = bytes[i];
            grid.Borders.Add(location);
            grid.Locations.Add(location);
        }

        var start = new Point(0, 0);
        var goal = new Point(size - 1, size - 1);

        int minimum = (int)PathFinding.AStar(grid, start, goal, cancellationToken: cancellationToken);

        string blockingByte = string.Empty;

        for (int i = ticks; i < bytes.Count && !cancellationToken.IsCancellationRequested; i++)
        {
            var location = bytes[i];
            grid.Borders.Add(location);
            grid.Locations.Add(location);

            if (PathFinding.AStar(grid, start, goal, cancellationToken: cancellationToken) is long.MaxValue)
            {
                blockingByte = $"{location.X},{location.Y}";
                break;
            }
        }

        cancellationToken.ThrowIfCancellationRequested();

        return (minimum, blockingByte);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        (Solution1, Solution2) = Simulate(values, size: 71, ticks: 1024, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("The minimum number of steps needed to reach the exit is {0}.", Solution1);
            Logger.WriteLine("The coordinates of the first byte that will prevent the exit from being reachable is {0}.", Solution2);
        }

        return Result();
    }
}
