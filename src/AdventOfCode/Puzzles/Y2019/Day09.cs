// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
        public static (long output, IReadOnlyList<long> memory) RunProgram(string program, long? input)
        {
            long[] instructions = program
                .Split(',')
                .Select((p) => ParseInt64(p))
                .ToArray();

            var vm = new IntcodeVM(instructions, 2_000);
            long output = vm.Run(input == null ? Array.Empty<long>() : new[] { input.Value });

            return (output, vm.Memory());
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            long input = ParseInt64(args[0]);
            string program = ReadResourceAsString();

            (Keycode, _) = RunProgram(program, input);

            if (Verbose)
            {
                Logger.WriteLine("The program produces BOOST keycode {0}.", Keycode);
            }

            return 0;
        }
    }
}
