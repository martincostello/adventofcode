// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2016/day/8</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day08 : Puzzle2016
    {
        /// <summary>
        /// Gets the number of pixels that are be lit.
        /// </summary>
        public int PixelsLit { get; private set; }

        /// <summary>
        /// Gets the number of pixels lit in the specified grid after following the specified instructions.
        /// </summary>
        /// <param name="instructions">The instructions to use to manipulate the grid of lights.</param>
        /// <param name="width">The width of the grid.</param>
        /// <param name="height">The height of the grid.</param>
        /// <param name="logger">The logger to use.</param>
        /// <returns>
        /// The number of pixels lit in the grid once the instructions are processed.
        /// </returns>
        internal static int GetPixelsLit(
            IEnumerable<string> instructions,
            int width,
            int height,
            ILogger logger = null)
        {
            IList<Instruction> operations = instructions.Select(ParseInstruction).ToArray();

            bool[,] grid = new bool[height, width];

            foreach (Instruction instruction in operations)
            {
                if (instruction.IsRotation)
                {
                    if (instruction.IsColumn)
                    {
                        RotateColumn(grid, column: instruction.A, pixels: instruction.B);
                    }
                    else
                    {
                        RotateRow(grid, row: instruction.A, pixels: instruction.B);
                    }
                }
                else
                {
                    LightRectangle(grid, width: instruction.A, height: instruction.B);
                }
            }

            logger?.WriteGrid(grid, ' ', 'X');

            return CountLitPixels(grid);
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> instructions = ReadResourceAsLines();

            PixelsLit = GetPixelsLit(instructions, width: 50, height: 6, Logger);

            if (Verbose)
            {
                Logger.WriteLine($"There are {PixelsLit:N0} pixels lit.");
            }

            return 0;
        }

        /// <summary>
        /// Counts the number of pixels lit in the specified grid.
        /// </summary>
        /// <param name="grid">The grid to count the number of lit pixels in.</param>
        /// <returns>
        /// The number of pixels lit in <paramref name="grid"/>.
        /// </returns>
        private static int CountLitPixels(bool[,] grid) => grid.TrueCount();

        /// <summary>
        /// Lights a rectangle of the specified size in the top-left of the specified grid.
        /// </summary>
        /// <param name="grid">The grid to light a rectangle in.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        private static void LightRectangle(bool[,] grid, int width, int height)
        {
            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < height; x++)
                {
                    grid[x, y] = true;
                }
            }
        }

        /// <summary>
        /// Rotates the specified column the specified number of pixels.
        /// </summary>
        /// <param name="grid">The grid to rotate the column for.</param>
        /// <param name="column">The index of the column to rotate.</param>
        /// <param name="pixels">The number of pixels to rotate the column by.</param>
        private static void RotateColumn(bool[,] grid, int column, int pixels)
        {
            for (int i = 0; i < pixels; i++)
            {
                bool[] next = new bool[grid.GetLength(0)];

                for (int row = 0; row < next.Length - 1; row++)
                {
                    next[row + 1] = grid[row, column];
                }

                next[0] = grid[next.Length - 1, column];

                for (int row = 0; row < next.Length; row++)
                {
                    grid[row, column] = next[row];
                }
            }
        }

        /// <summary>
        /// Rotates the specified row the specified number of pixels.
        /// </summary>
        /// <param name="grid">The grid to rotate the row for.</param>
        /// <param name="row">The index of the row to rotate.</param>
        /// <param name="pixels">The number of pixels to rotate the row by.</param>
        private static void RotateRow(bool[,] grid, int row, int pixels)
        {
            for (int i = 0; i < pixels; i++)
            {
                bool[] next = new bool[grid.GetLength(1)];

                for (int column = 0; column < next.Length - 1; column++)
                {
                    next[column + 1] = grid[row, column];
                }

                next[0] = grid[row, next.Length - 1];

                for (int column = 0; column < next.Length; column++)
                {
                    grid[row, column] = next[column];
                }
            }
        }

        /// <summary>
        /// Parses the specified instruction to change the grid.
        /// </summary>
        /// <param name="instruction">The instruction to parse.</param>
        /// <returns>
        /// An instance of <see cref="Instruction"/> representing the parsed representation of <paramref name="instruction"/>.
        /// </returns>
        private static Instruction ParseInstruction(string instruction)
        {
            var result = new Instruction();

            string[] split = instruction.Split(' ', StringSplitOptions.None);

            switch (split[0])
            {
                case "rect":
                    split = split[1].Split('x', StringSplitOptions.None);
                    result.A = ParseInt32(split[0]);
                    result.B = ParseInt32(split[1]);
                    break;

                case "rotate":
                    result.IsRotation = true;
                    result.IsColumn = string.Equals(split[1], "column", StringComparison.Ordinal);
                    result.A = ParseInt32(split[2].Split('=', StringSplitOptions.None)[1]);
                    result.B = ParseInt32(split[4]);
                    break;

                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// A class representing an instruction to perform to a light grid.
        /// </summary>
        private sealed class Instruction
        {
            /// <summary>
            /// Gets or sets a value indicating whether to perform a rotation.
            /// </summary>
            internal bool IsRotation { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether a column is being acted on if a rotation is performed.
            /// </summary>
            internal bool IsColumn { get; set; }

            /// <summary>
            /// Gets or sets the value of A.
            /// </summary>
            internal int A { get; set; }

            /// <summary>
            /// Gets or sets the value of B.
            /// </summary>
            internal int B { get; set; }
        }
    }
}
