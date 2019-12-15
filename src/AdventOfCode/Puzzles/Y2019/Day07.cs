// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Channels;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2019/day/7</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day07 : Puzzle2019
    {
        /// <summary>
        /// Gets the highest signal that can be sent to the thrusters.
        /// </summary>
        public int HighestSignal { get; private set; }

        /// <summary>
        /// Gets the highest signal that can be sent to the thrusters using a feedback loop.
        /// </summary>
        public int HighestSignalUsingFeedback { get; private set; }

        /// <summary>
        /// Runs the specified Intcode program.
        /// </summary>
        /// <param name="program">The Intcode program to run.</param>
        /// <param name="useFeedback">Whether to arrange the amplifiers in a feedback loop.</param>
        /// <returns>
        /// The diagnostic code output by the program.
        /// </returns>
        public static async Task<long> RunProgramAsync(string program, bool useFeedback = false)
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
                        signal = (await IntcodeVM.RunAsync(instructions, phases[i], signal))[0];
                    }
                }
                else
                {
                    var vms = new IntcodeVM[phases.Length];
                    var inputs = new Channel<long>[phases.Length];
                    var outputs = new Channel<long>[phases.Length];

                    for (int i = 0; i < phases.Length; i++)
                    {
                        vms[i] = new IntcodeVM(instructions);
                        inputs[i] = Channel.CreateUnbounded<long>();
                        outputs[i] = Channel.CreateUnbounded<long>();

                        await inputs[i].Writer.WriteAsync(phases[i]);

                        if (i == 0)
                        {
                            await inputs[i].Writer.WriteAsync(0);
                        }
                    }

                    for (int i = 0; i < phases.Length; i++)
                    {
                        int thisVM = i;
                        int nextVM = i == phases.Length - 1 ? 0 : i + 1;

                        vms[thisVM].Input = inputs[thisVM].Reader;
                        vms[thisVM].Output = outputs[thisVM].Writer;
                        vms[thisVM].OnOutput += async (_, value) =>
                        {
                            await inputs[nextVM].Writer.WriteAsync(value);
                        };
                    }

                    bool completed = false;

                    while (!completed)
                    {
                        for (int i = 0; i < phases.Length; i++)
                        {
                            completed = await vms[i].RunAsync();
                        }
                    }

                    signal = (await ChannelHelpers.ToListAsync(outputs.Last().Reader))[^1];
                }

                signals.Add(signal);
            }

            return signals.Max();
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string program = ReadResourceAsString();

            HighestSignal = (int)RunProgramAsync(program, useFeedback: false).Result;
            HighestSignalUsingFeedback = (int)RunProgramAsync(program, useFeedback: true).GetAwaiter().GetResult();

            if (Verbose)
            {
                Logger.WriteLine("The highest signal that can be sent to the thrusters is {0}.", HighestSignal);
                Logger.WriteLine("The highest signal that can be sent to the thrusters using a feedback loop is {0}.", HighestSignalUsingFeedback);
            }

            return 0;
        }
    }
}
