// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2017/day/6</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day06 : Puzzle2017
    {
        /// <summary>
        /// Gets the count of redistribution cycles performed before a distribution of memory re-occurs.
        /// </summary>
        public int CycleCount { get; private set; }

        /// <summary>
        /// Debugs the specified memory to find the number of cycles performed before a distribution is repeated.
        /// </summary>
        /// <param name="memory">The memory to debug.</param>
        /// <returns>
        /// The number of redistribution cycles that must be completed before a configuration is repeated.
        /// </returns>
        public static int Debug(IList<int> memory)
        {
            var patterns = new List<string>();
            string pattern = string.Join(",", memory);

            int cycles = 0;

            do
            {
                patterns.Add(pattern);

                int blocks = memory.Max();
                int index = memory.IndexOf(blocks);

                memory[index] = 0;
                int next = index + 1;

                while (blocks > 0)
                {
                    if (next >= memory.Count)
                    {
                        next = 0;
                    }

                    memory[next++]++;
                    blocks--;
                }

                cycles++;
                pattern = string.Join(",", memory);
            }
            while (!patterns.Contains(pattern));

            return cycles;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<int> memory = ReadResourceAsString().Trim()
                .Split('\t')
                .Select((p) => ParseInt32(p))
                .ToList();

            CycleCount = Debug(memory);

            Console.WriteLine($"{CycleCount:N0} redistribution cycles must be completed before a configuration is produced that has been seen before.");

            return 0;
        }
    }
}
