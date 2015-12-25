// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.Drawing;
    using System.Globalization;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/day/25</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day25 : IPuzzle
    {
        /// <summary>
        /// Gets the code for the weather machine.
        /// </summary>
        internal ulong Code { get; private set; }

        /// <inheritdoc />
        public int Solve(string[] args)
        {
            if (args.Length != 2)
            {
                Console.Error.WriteLine("No row number and column number specified.");
                return -1;
            }

            int row = int.Parse(args[0], CultureInfo.InvariantCulture);
            int column = int.Parse(args[1], CultureInfo.InvariantCulture);

            Code = GetCodeForWeatherMachine(row, column);

            Console.WriteLine(
                "The code for row {0:N0} and column {1:N0} is {2:N0}.",
                row,
                column,
                Code);

            return 0;
        }

        /// <summary>
        /// Gets the code for the weather machine at the specified row and column.
        /// </summary>
        /// <param name="row">The row number.</param>
        /// <param name="column">The column number.</param>
        /// <returns>
        /// The weather machine code for the specified row and column.
        /// </returns>
        internal static ulong GetCodeForWeatherMachine(int row, int column)
        {
            // Zero-index the row and column
            row--;
            column--;

            ulong result = 20151125;

            Point current = new Point(0, 0);
            Point target = new Point(column, row);

            int currentRow = 0;

            while (current != target)
            {
                currentRow++;

                for (int i = 0; i <= currentRow && current != target; i++)
                {
                    current = new Point(i, currentRow - i);
                    result = GenerateCode(result);
                }
            }

            return result;
        }

        /// <summary>
        /// Generates the next code from the specified value.
        /// </summary>
        /// <param name="value">The value to generate the code from.</param>
        /// <returns>
        /// The code generated from the specified value.
        /// </returns>
        internal static ulong GenerateCode(ulong value)
        {
            ulong result = value * 252533;
            result = result % 33554393;

            return result;
        }
    }
}
