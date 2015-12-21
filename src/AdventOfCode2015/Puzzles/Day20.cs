// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.Globalization;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/day/20</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day20 : IPuzzle
    {
        /// <summary>
        /// Gets the lowest house number that gets at least the specified number of presents.
        /// </summary>
        internal int LowestHouseNumber { get; private set; }

        public int Solve(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("No target value specified.");
                return -1;
            }

            int target = int.Parse(args[0], CultureInfo.InvariantCulture);

            LowestHouseNumber = GetLowestHouseNumber(target);

            Console.WriteLine(
                "The first house to receive at least {0:N0} presents is house number {1:N0}.",
                target,
                LowestHouseNumber);

            return -1;
        }

        /// <summary>
        /// Returns the lowest house number that gets the specified number of presents.
        /// </summary>
        /// <param name="target">The target number.</param>
        /// <returns>
        /// The lowest house number that receives at least the specified number of presents.
        /// </returns>
        internal static int GetLowestHouseNumber(int target)
        {
            for (int i = 1; ; i++)
            {
                if (GetPresentsDelivered(i) >= target)
                {
                    return i;
                }
            }
        }

        /// <summary>
        /// Returns the number of presents delivered to the specified house.
        /// </summary>
        /// <param name="house">The house number.</param>
        /// <returns>
        /// The number of presents delivered to the specified house.
        /// </returns>
        internal static int GetPresentsDelivered(int house)
        {
            int count = 0;

            for (int elf = 1; elf < house + 1; elf++)
            {
                if (house % elf == 0)
                {
                    count += elf * 10;
                }
            }

            return count;
        }
    }
}
