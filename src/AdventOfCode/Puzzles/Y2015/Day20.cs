// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2015/day/20</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day20 : Puzzle2015
    {
        /// <summary>
        /// Gets the lowest house number that gets at least the specified number of presents.
        /// </summary>
        internal int LowestHouseNumber { get; private set; }

        /// <inheritdoc />
        protected override int MinimumArguments => 1;

        /// <summary>
        /// Returns the lowest house number that gets the specified number of presents.
        /// </summary>
        /// <param name="target">The target number.</param>
        /// <param name="maximumVisits">The optional maximum visits each elf makes to a house.</param>
        /// <returns>
        /// The lowest house number that receives at least the specified number of presents.
        /// </returns>
        internal static int GetLowestHouseNumber(int target, int? maximumVisits)
        {
            for (int i = 1; ; i++)
            {
                if (GetPresentsDelivered(i, maximumVisits) >= target)
                {
                    return i;
                }
            }
        }

        /// <summary>
        /// Returns the number of presents delivered to the specified house.
        /// </summary>
        /// <param name="house">The house number.</param>
        /// <param name="maximumVisits">The optional maximum visits each elf makes to a house.</param>
        /// <returns>
        /// The number of presents delivered to the specified house.
        /// </returns>
        internal static int GetPresentsDelivered(int house, int? maximumVisits)
        {
            int count = 0;

            int presents = maximumVisits.HasValue ? 11 : 10;

            for (int elf = 1; elf < house + 1; elf++)
            {
                bool isElfInRange = maximumVisits == null;

                if (!isElfInRange)
                {
                    isElfInRange = house <= elf * maximumVisits.Value;
                }

                if (isElfInRange && house % elf == 0)
                {
                    count += elf * presents;
                }
            }

            return count;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            if (args.Length != 1 && args.Length != 2)
            {
                Console.WriteLine("No target value or maximum number of visits specified.");
                return -1;
            }

            int target = ParseInt32(args[0]);
            int? maximumVisits = args.Length == 2 ? ParseInt32(args[1]) : default(int?);

            LowestHouseNumber = GetLowestHouseNumber(target, maximumVisits);

            Console.WriteLine(
                "The first house to receive at least {0:N0} presents is house number {1:N0}.",
                target,
                LowestHouseNumber);

            return 0;
        }
    }
}
