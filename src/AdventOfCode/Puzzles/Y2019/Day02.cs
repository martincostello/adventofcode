// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2019/day/2</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2019, 02, "1202 Program Alarm", RequiresData = true)]
public sealed class Day02 : Puzzle<long>
{
    private static readonly long[] Seed = [0];

    /// <summary>
    /// Runs the specified Intcode program.
    /// </summary>
    /// <param name="program">The Intcode program to run.</param>
    /// <param name="adjust">Whether to adjust the state for <c>1202 program alarm</c> before running.</param>
    /// <param name="cancellationToken">The optional cancellation token to use.</param>
    /// <returns>
    /// The memory values of the program once run.
    /// </returns>
    public static async Task<IReadOnlyList<long>> RunProgramAsync(
        string program,
        bool adjust = false,
        CancellationToken cancellationToken = default)
    {
        long[] instructions = IntcodeVM.ParseProgram(program);

        if (adjust)
        {
            instructions[1] = 12;
            instructions[2] = 2;
        }

        var vm = new IntcodeVM(instructions)
        {
            Input = await ChannelHelpers.CreateReaderAsync(Seed, cancellationToken),
        };

        if (!await vm.RunAsync(cancellationToken))
        {
            throw new PuzzleException("Failed to run program.");
        }

        return vm.Memory().ToArray();
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithStringAsync(
            static async (program, logger, cancellationToken) =>
            {
                var output = await RunProgramAsync(program, adjust: true, cancellationToken);

                if (logger is { })
                {
                    logger.WriteLine("The value at position 0 after the program halts is {0}.", output);
                }

                return output[0];
            },
            cancellationToken);
    }
}
