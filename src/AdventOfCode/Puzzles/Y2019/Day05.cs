// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2019/day/5</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day05 : Puzzle2019
    {
        /// <summary>
        /// Gets the diagnostic code output by the program.
        /// </summary>
        public int DiagnosticCode { get; private set; }

        /// <summary>
        /// Runs the specified Intcode program.
        /// </summary>
        /// <param name="program">The Intcode program to run.</param>
        /// <param name="input">The input to the program.</param>
        /// <returns>
        /// The diagnostic code output by the program.
        /// </returns>
        public static int RunProgram(string program, int input)
        {
            int[] instructions = program
                .Split(',')
                .Select((p) => ParseInt32(p))
                .ToArray();

            _ = IntcodeVM.Run(instructions, input, out int output);
            return output;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string program = ReadResourceAsString();

            DiagnosticCode = RunProgram(program, 1);

            if (Verbose)
            {
                Logger.WriteLine("The program produces diagnostic code {0}.", DiagnosticCode);
            }

            return 0;
        }
    }
}
