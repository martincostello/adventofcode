// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Threading.Channels;

namespace MartinCostello.AdventOfCode.Puzzles.Y2019;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2019/day/7</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2019, 07, RequiresData = true)]
public sealed class Day07 : Puzzle
{
    /// <summary>
    /// Gets the highest signal that can be sent to the thrusters.
    /// </summary>
    public long HighestSignal { get; private set; }

    /// <summary>
    /// Gets the highest signal that can be sent to the thrusters using a feedback loop.
    /// </summary>
    public long HighestSignalUsingFeedback { get; private set; }

    /// <summary>
    /// Runs the specified Intcode program.
    /// </summary>
    /// <param name="program">The Intcode program to run.</param>
    /// <param name="useFeedback">Whether to arrange the amplifiers in a feedback loop.</param>
    /// <param name="cancellationToken">The optional cancellation to oken to use.</param>
    /// <returns>
    /// The diagnostic code output by the program.
    /// </returns>
    public static async Task<long> RunProgramAsync(
        string program,
        bool useFeedback = false,
        CancellationToken cancellationToken = default)
    {
        long[] instructions = IntcodeVM.ParseProgram(program);

        int[] seed = useFeedback ? new[] { 5, 6, 7, 8, 9 } : new[] { 0, 1, 2, 3, 4 };

        var signals = new List<long>();

        foreach (var permutation in Maths.GetPermutations(seed))
        {
            long signal = 0;
            long[] phases = permutation.Select((p) => (long)p).ToArray();

            if (!useFeedback)
            {
                for (int i = 0; i < phases.Length; i++)
                {
                    signal = (await IntcodeVM.RunAsync(instructions, new[] { phases[i], signal }, cancellationToken))[0];
                }
            }
            else
            {
                var amplifiers = new List<IntcodeVM>(phases.Length);
                var inputs = new List<Channel<long>>(phases.Length);

                for (int i = 0; i < phases.Length; i++)
                {
                    amplifiers.Add(new IntcodeVM(instructions));

                    var input = Channel.CreateUnbounded<long>();
                    await input.Writer.WriteAsync(phases[i], cancellationToken);

                    inputs.Add(input);
                }

                await inputs[0].Writer.WriteAsync(0, cancellationToken);

                for (int i = 0; i < phases.Length; i++)
                {
                    int nextAmp = i == phases.Length - 1 ? 0 : i + 1;

                    var thisAmp = amplifiers[i];
                    var nextInput = inputs[nextAmp];

                    thisAmp.Input = inputs[i].Reader;
                    thisAmp.OnOutput += async (_, value) =>
                    {
                        await nextInput.Writer.WriteAsync(value, cancellationToken);
                    };
                }

                bool completed = false;

                while (!completed)
                {
                    foreach (var amp in amplifiers)
                    {
                        completed = await amp.RunAsync(cancellationToken);
                    }
                }

                signal = (await amplifiers[^1].Output.ToListAsync(cancellationToken))[^1];
            }

            signals.Add(signal);
        }

        return signals.Max();
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        string program = await ReadResourceAsStringAsync();

        HighestSignal = await RunProgramAsync(program, useFeedback: false, cancellationToken);
        HighestSignalUsingFeedback = await RunProgramAsync(program, useFeedback: true, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("The highest signal that can be sent to the thrusters is {0}.", HighestSignal);
            Logger.WriteLine("The highest signal that can be sent to the thrusters using a feedback loop is {0}.", HighestSignalUsingFeedback);
        }

        return PuzzleResult.Create(HighestSignal, HighestSignalUsingFeedback);
    }
}
