// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Channels;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2019/day/9</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day09 : Puzzle2019
    {
        /// <summary>
        /// Gets the key code output by the program.
        /// </summary>
        public long Keycode { get; private set; }

        /// <inheritdoc />
        protected override int MinimumArguments => 1;

        /// <summary>
        /// Runs the specified Intcode program.
        /// </summary>
        /// <param name="program">The Intcode program to run.</param>
        /// <param name="input">The input to the program.</param>
        /// <returns>
        /// The keycode output by the program.
        /// </returns>
        public static async Task<IReadOnlyList<long>> RunProgramAsync(string program, long? input = null)
        {
            long[] instructions = program
                .Split(',')
                .Select((p) => ParseInt64(p))
                .ToArray();

            var vm = new IntcodeVM(instructions, 2_000);

            var inputChannel = Channel.CreateBounded<long>(1);

            if (input.HasValue)
            {
                await inputChannel.Writer.WriteAsync(input.Value);
            }

            var outputChannel = Channel.CreateUnbounded<long>();

            await vm.RunAsync(inputChannel, outputChannel);

            var outputs = new List<long>();

            await foreach (long output in outputChannel.Reader.ReadAllAsync())
            {
                outputs.Add(output);
            }

            return outputs;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            long input = ParseInt64(args[0]);
            string program = ReadResourceAsString();

            Keycode = RunProgramAsync(program, input).Result[0];

            if (Verbose)
            {
                Logger.WriteLine("The program produces BOOST keycode {0}.", Keycode);
            }

            return 0;
        }
    }
}
