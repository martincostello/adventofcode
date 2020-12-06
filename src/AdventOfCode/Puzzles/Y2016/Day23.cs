﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System.Collections.Generic;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2016/day/23</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2016, 23, MinimumArguments = 1, RequiresData = true, IsHidden = true)]
    public sealed class Day23 : Puzzle
    {
        /// <summary>
        /// Gets the value to send to the safe.
        /// </summary>
        public int SafeValue { get; private set; }

        /// <inheritdoc />
        protected override int MinimumArguments => 1;

        /// <inheritdoc />
        protected override object[] SolveCore(string[] args)
        {
            int input = ParseInt32(args[0]);

            IList<string> instructions = ReadResourceAsLines();

            IDictionary<char, int> registers = Day12.Process(instructions, initialValueOfA: input);
            SafeValue = registers['a'];

            if (Verbose)
            {
                Logger.WriteLine($"The value to send to the safe for an input of {input:N0} is '{SafeValue}'.");
            }

            return new object[] { SafeValue };
        }
    }
}
