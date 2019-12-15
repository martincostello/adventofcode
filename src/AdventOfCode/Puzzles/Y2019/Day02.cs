// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Channels;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2019/day/2</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day02 : Puzzle2019
    {
        /// <summary>
        /// Gets the output of the program.
        /// </summary>
        public IReadOnlyList<long> Output { get; private set; } = Array.Empty<long>();

        /// <summary>
        /// Runs the specified Intcode program.
        /// </summary>
        /// <param name="program">The Intcode program to run.</param>
        /// <param name="adjust">Whether to adjust the state for <c>1202 program alarm</c> before running.</param>
        /// <returns>
        /// The memory values of the program once run.
        /// </returns>
        public static async Task<IReadOnlyList<long>> RunProgramAsync(string program, bool adjust = false)
        {
            long[] instructions = program
                .Split(',')
                .Select((p) => ParseInt64(p))
                .ToArray();

            if (adjust)
            {
                instructions[1] = 12;
                instructions[2] = 2;
            }

            var inputChannel = Channel.CreateBounded<long>(1);
            await inputChannel.Writer.WriteAsync(0L);

            return await IntcodeVM.RunAsync(instructions, inputChannel.Reader);
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string program = ReadResourceAsString();

            Output = RunProgramAsync(program, adjust: true).Result;

            if (Verbose)
            {
                Logger.WriteLine("The value at position 0 after the program halts is {0}.", Output[0]);
            }

            return 0;
        }
    }
}
