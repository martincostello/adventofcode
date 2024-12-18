// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/18</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 18, "RAM Run", RequiresData = true)]
public sealed class Day18 : Puzzle
{
    /// <summary>
    /// Gets the minimum number of steps needed to reach the exit.
    /// </summary>
    public int MinimumSteps { get; private set; }

    /// <summary>
    /// Simulates RAM falling into the specified region for the specified period of time.
    /// </summary>
    /// <param name="coordinates">The coordinates of the bytes that will fall into the region.</param>
    /// <param name="size">The height and width of the region in bytes.</param>
    /// <param name="ticks">The number of ticks to simulate for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The minimum number of steps needed to reach the exit.
    /// </returns>
    public static int Simulate(IList<string> coordinates, int size, int ticks, CancellationToken cancellationToken)
    {
        var grid = new SquareGrid(size, size);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                grid.Locations.Add(new(x, y));
            }
        }

        for (int i = 0; i < ticks; i++)
        {
            (int x, int y) = coordinates[i].AsNumberPair<int>();

            var corruption = new Point(x, y);

            grid.Borders.Add(corruption);
            grid.Locations.Add(corruption);
        }

        var start = new Point(0, 0);
        var goal = new Point(size - 1, size - 1);

        return (int)PathFinding.AStar(grid, start, goal, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        MinimumSteps = Simulate(values, size: 71, ticks: 1024, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("The minimum number of steps needed to reach the exit is {0}.", MinimumSteps);
        }

        return PuzzleResult.Create(MinimumSteps);
    }
}
