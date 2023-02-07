// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Threading.Channels;

namespace MartinCostello.AdventOfCode.Puzzles.Y2019;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2019/day/9</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2019, 09, "Sensor Boost", RequiresData = true)]
public sealed class Day09 : Puzzle
{
    /// <summary>
    /// Gets the key code output by the program for an input of 1.
    /// </summary>
    public long Keycode1 { get; private set; }

    /// <summary>
    /// Gets the key code output by the program for an input of 2.
    /// </summary>
    public long Keycode2 { get; private set; }

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
        string program = await ReadResourceAsStringAsync(cancellationToken);

        Keycode1 = (await RunProgramAsync(program, input: 1, cancellationToken))[0];
        Keycode2 = (await RunProgramAsync(program, input: 2, cancellationToken))[0];

        if (Verbose)
        {
            Logger.WriteLine("The program produces BOOST keycode {0} for an input of 1.", Keycode1);
            Logger.WriteLine("The program produces BOOST keycode {0} for an input of 2.", Keycode2);
        }

        return PuzzleResult.Create(Keycode1, Keycode2);
    }
}
