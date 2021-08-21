// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Threading.Channels;

namespace MartinCostello.AdventOfCode.Puzzles.Y2019;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2019/day/9</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2019, 09, MinimumArguments = 1, RequiresData = true)]
public sealed class Day09 : Puzzle
{
    /// <summary>
    /// Gets the key code output by the program.
    /// </summary>
    public long Keycode { get; private set; }

    /// <summary>
    /// Runs the specified Intcode program.
    /// </summary>
    /// <param name="program">The Intcode program to run.</param>
    /// <param name="input">The input to the program.</param>
    /// <param name="cancellationToken">The optional cancellation token to use.</param>
    /// <returns>
    /// The keycode output by the program.
    /// </returns>
    public static async Task<IReadOnlyList<long>> RunProgramAsync(
        string program,
        long? input = null,
        CancellationToken cancellationToken = default)
    {
        long[] instructions = IntcodeVM.ParseProgram(program);

        var channel = Channel.CreateUnbounded<long>();

        if (input.HasValue)
        {
            await channel.Writer.WriteAsync(input.Value, cancellationToken);
        }

        channel.Writer.Complete();

        var vm = new IntcodeVM(instructions, 2_000)
        {
            Input = channel.Reader,
        };

        if (!await vm.RunAsync(cancellationToken))
        {
            throw new PuzzleException("Failed to run program.");
        }

        return await vm.Output.ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        long input = ParseInt64(args[0]);
        string program = await ReadResourceAsStringAsync();

        Keycode = (await RunProgramAsync(program, input, cancellationToken))[0];

        if (Verbose)
        {
            Logger.WriteLine("The program produces BOOST keycode {0}.", Keycode);
        }

        return PuzzleResult.Create(Keycode);
    }
}
