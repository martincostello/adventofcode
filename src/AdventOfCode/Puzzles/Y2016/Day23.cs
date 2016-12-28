// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2016/day/23</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day23 : Puzzle2016
    {
        /// <summary>
        /// Gets the value to send to the safe.
        /// </summary>
        public int SafeValue { get; private set; }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> instructions = ReadResourceAsLines();

            IDictionary<char, int> registers = Day12.Process(instructions, initialValueOfA: 7);
            SafeValue = registers['a'];

            Console.WriteLine($"The value to send to the safe is '{SafeValue}'.");

            return 0;
        }
    }
}
