// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/1</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day01 : Puzzle2020
    {
        /// <summary>
        /// Gets the product of the two input values that sum to a value of 2020.
        /// </summary>
        public int ProductOf2020Sum { get; private set; }

        /// <summary>
        /// Gets the product of two values from the specified set of values that
        /// which when added together equal 2020.
        /// </summary>
        /// <param name="values">The values to find the 2020 sum's product from.</param>
        /// <returns>
        /// The product of the two values that sum to a value of 2020.
        /// </returns>
        public static int Get2020Product(IEnumerable<string> values)
        {
            int[] expenses = values
                .Select((p) => ParseInt32(p))
                .ToArray();

            const int Limit = 2020;

            for (int i = 0; i < expenses.Length; i++)
            {
                int x = expenses[i];

                if (x > Limit)
                {
                    continue;
                }

                for (int j = 0; j < expenses.Length; j++)
                {
                    if (j == i)
                    {
                        continue;
                    }

                    int y = expenses[j];

                    if (x + y == Limit)
                    {
                        return x * y;
                    }
                }
            }

            return -1;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> values = ReadResourceAsLines();

            ProductOf2020Sum = Get2020Product(values);

            if (Verbose)
            {
                Logger.WriteLine("The product of the two entries that sum to 2020 is {0}.", ProductOf2020Sum);
            }

            return 0;
        }
    }
}
