// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2016/day/19</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day19 : Puzzle2016
    {
        /// <summary>
        /// Gets the number of the elf who receives all the presents.
        /// </summary>
        public int ElfWithAllPresents { get; private set; }

        /// <inheritdoc />
        protected override int MinimumArguments => 1;

        /// <summary>
        /// Finds the elf that receives all of the presents.
        /// </summary>
        /// <param name="count">The number of elves participating.</param>
        /// <returns>
        /// The number of the elf that receives all the presents.
        /// </returns>
        internal static int FindElfThatGetsAllPresents(int count)
        {
            LinkedList<int> circle = new LinkedList<int>();

            var first = circle.AddFirst(1);
            var current = first;

            while (circle.Count < count)
            {
                current = circle.AddAfter(current, circle.Count + 1);
            }

            current = first;

            while (circle.Count > 1)
            {
                if (current == circle.Last)
                {
                    circle.Remove(circle.First);
                }
                else
                {
                    circle.Remove(current.Next);
                }

                if (current == circle.Last)
                {
                    current = circle.First;
                }
                else
                {
                    current = current.Next;
                }
            }

            return circle.First.Value;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            int count = ParseInt32(args[0]);

            ElfWithAllPresents = FindElfThatGetsAllPresents(count);

            Console.WriteLine($"The elf that gets all the presents is {ElfWithAllPresents:N0}.");

            return 0;
        }
    }
}
