// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System.Collections.Generic;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2016/day/19</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day19 : Puzzle2016
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
        /// <param name="version">The version of the rules to use.</param>
        /// <returns>
        /// The number of the elf that receives all the presents.
        /// </returns>
        internal static int FindElfThatGetsAllPresents(int count, int version)
        {
            return
                version == 2 ?
                FindElfThatGetsAllPresentsV2(count) :
                FindElfThatGetsAllPresentsV1(count);
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            int count = ParseInt32(args[0]);
            int version = args.Length > 1 ? ParseInt32(args[1]) : 1;

            ElfWithAllPresents = FindElfThatGetsAllPresents(count, version);

            if (Verbose)
            {
                Logger.WriteLine($"The elf that gets all the presents using version {version} of the rules is {ElfWithAllPresents:N0}.");
            }

            return 0;
        }

        /// <summary>
        /// Finds the elf that receives all of the presents using version 1 of the rules.
        /// </summary>
        /// <param name="count">The number of elves participating.</param>
        /// <returns>
        /// The number of the elf that receives all the presents.
        /// </returns>
        private static int FindElfThatGetsAllPresentsV1(int count)
        {
            LinkedList<int> circle = CreateCircle(count);
            var current = circle.First;

            while (circle.Count > 1)
            {
                circle.Remove(current.NextCircular());
                current = current.NextCircular();
            }

            return circle.First.Value;
        }

        /// <summary>
        /// Finds the elf that receives all of the presents using version 2 of the rules.
        /// </summary>
        /// <param name="count">The number of elves participating.</param>
        /// <returns>
        /// The number of the elf that receives all the presents.
        /// </returns>
        private static int FindElfThatGetsAllPresentsV2(int count)
        {
            LinkedList<int> circle = CreateCircle(count);
            var current = circle.First;
            var opposite = current;

            int steps = circle.Count / 2;

            for (int i = 0; i < steps; i++)
            {
                opposite = opposite.NextCircular();
            }

            while (circle.Count > 1)
            {
                var next = opposite.NextCircular();

                if (circle.Count % 2 == 1)
                {
                    next = next.NextCircular();
                }

                circle.Remove(opposite);

                opposite = next;

                current = current.NextCircular();
            }

            return circle.First.Value;
        }

        /// <summary>
        /// Creates the circle of elves.
        /// </summary>
        /// <param name="count">The number of elves in the circle.</param>
        /// <returns>
        /// The <see cref="LinkedList{T}"/> containing the number of elves specified by <paramref name="count"/>.
        /// </returns>
        private static LinkedList<int> CreateCircle(int count)
        {
            var circle = new LinkedList<int>();
            var current = circle.AddFirst(1);

            while (circle.Count < count)
            {
                current = circle.AddAfter(current, circle.Count + 1);
            }

            return circle;
        }
    }
}
