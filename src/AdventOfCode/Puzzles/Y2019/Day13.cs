// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace MartinCostello.AdventOfCode.Puzzles.Y2019;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2019/day/13</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2019, 13, RequiresData = true)]
public sealed class Day13 : Puzzle
{
    /// <summary>
    /// Gets the number of block tiles on the screen.
    /// </summary>
    public long BlockTileCount { get; private set; }

    /// <summary>
    /// Gets the score when the game ends.
    /// </summary>
    public long Score { get; private set; }

    /// <summary>
    /// Gets the number of block tiles on the screen after the game is run.
    /// </summary>
    /// <param name="program">The Intcode program to run.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>
    /// The number of block tiles on the screen.
    /// </returns>
    public static async Task<(long BlockTileCount, long Score)> PlayGameAsync(
        string program,
        CancellationToken cancellationToken)
    {
        long[] instructions = IntcodeVM.ParseProgram(program);

        var vm = new IntcodeVM(instructions, 10_000)
        {
            Input = await ChannelHelpers.CreateReaderAsync(new[] { 2L }, cancellationToken),
        };

        if (!await vm.RunAsync(cancellationToken))
        {
            throw new PuzzleException("Failed to run program.");
        }

        var outputs = await vm.Output.ToListAsync(cancellationToken);

        var grid = new Dictionary<Point, int>(outputs.Count);

        long score = 0;

        for (int i = 0; i < outputs.Count; i += 3)
        {
            int x = (int)outputs[i];
            int y = (int)outputs[i + 1];
            int tileId = (int)outputs[i + 2];

            if (x == 1 && y == 0)
            {
                score = tileId;
            }
            else
            {
                grid[new Point(x, y)] = tileId;
            }
        }

        int initialBlockCount = grid.Values.Count((p) => p == 2);

        return (initialBlockCount, score);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        string program = await ReadResourceAsStringAsync();

        (BlockTileCount, Score) = await PlayGameAsync(program, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("There are {0} block tiles on the screen when the game exits.", BlockTileCount);
            Logger.WriteLine("The score after the last block is broken is {0}", Score);
        }

        return PuzzleResult.Create(BlockTileCount/*, Score*/);
    }
}
