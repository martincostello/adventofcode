// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using System.Linq;

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
        /// Runs the specified Intcode program.
        /// </summary>
        /// <param name="program">The Intcode program to run.</param>
        /// <returns>
        /// The diagnostic code output by the program.
        /// </returns>
        public static int RunProgram(string program)
        {
            var permutations = Maths.GetPermutations(new[] { 0, 1, 2, 3, 4 });

            int[] instructions = program
                .Split(',')
                .Select((p) => ParseInt32(p))
                .ToArray();

            return permutations
                .Select((p) =>
                {
                    int signal = 0;
                    int[] phases = p.ToArray();

                    for (int i = 0; i < phases.Length; i++)
                    {
                        int phase = phases[i];
                        int[] amplifier = (int[])instructions.Clone();

                        _ = IntcodeVM.Run(amplifier, new[] { phase, signal }, out signal);
                    }

                    return signal;
                })
                .Max();
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string program = ReadResourceAsString();

            HighestSignal = RunProgram(program);

            if (Verbose)
            {
                Logger.WriteLine("The highest signal that can be sent to the thrusters is {0}.", HighestSignal);
            }

            return 0;
        }
    }
}
