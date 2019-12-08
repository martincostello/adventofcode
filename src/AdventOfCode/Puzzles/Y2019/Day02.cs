﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2019/day/2</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day02 : Puzzle2019
    {
        /// <summary>
        /// Gets the output of the program.
        /// </summary>
        public IReadOnlyList<int> Output { get; private set; } = Array.Empty<int>();

        /// <summary>
        /// Runs the specified Intcode program.
        /// </summary>
        /// <param name="program">The Intcode program to run.</param>
        /// <param name="adjust">Whether to adjust the state for <c>1202 program alarm</c> before running.</param>
        /// <returns>
        /// The memory values of the program once run.
        /// </returns>
        public static IReadOnlyList<int> RunProgram(string program, bool adjust = false)
        {
            int[] instructions = program
                .Split(',')
                .Select((p) => ParseInt32(p))
                .ToArray();

            if (adjust)
            {
                instructions[1] = 12;
                instructions[2] = 2;
            }

            return IntcodeVM.Run(instructions, new[] { 0 }, out int _);
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string program = ReadResourceAsString();

            Output = RunProgram(program, adjust: true);

            if (Verbose)
            {
                Logger.WriteLine("The value at position 0 after the program halts is {0}.", Output[0]);
            }

            return 0;
        }
    }
}
