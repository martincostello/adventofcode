// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2017/day/5</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day05 : Puzzle2017
    {
        /// <summary>
        /// Gets the number of steps required to exit the input instructions.
        /// </summary>
        public int StepsToExit { get; private set; }

        /// <summary>
        /// Executes the specified program.
        /// </summary>
        /// <param name="program">The program to execute represented as CPU jump offsets.</param>
        /// <returns>
        /// The value of the program counter when the program terminates.
        /// </returns>
        public static int Execute(IList<int> program)
        {
            int counter = 0;
            int index = 0;

            while (index >= 0 && index < program.Count)
            {
                int offset = program[index];
                program[index]++;

                index += offset;
                counter++;
            }

            return counter;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<int> program = ReadResourceAsLines()
                .Select((p) => ParseInt32(p))
                .ToList();

            StepsToExit = Execute(program);

            Console.WriteLine($"It takes {StepsToExit:N0} to reach the exit.");

            return 0;
        }
    }
}
