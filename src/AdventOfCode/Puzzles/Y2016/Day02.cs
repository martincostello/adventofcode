// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2016/day/2</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day02 : Puzzle2016
    {
        /// <summary>
        /// An enumeration of directions.
        /// </summary>
        private enum Direction
        {
            /// <summary>
            /// Up.
            /// </summary>
            Up,

            /// <summary>
            /// Down.
            /// </summary>
            Down,

            /// <summary>
            /// Left.
            /// </summary>
            Left,

            /// <summary>
            /// Right.
            /// </summary>
            Right,
        }

        /// <summary>
        /// Gets the code for the bathroom using an alphanumeric keypad.
        /// </summary>
        public string BathroomCodeAlphanumericKeypad { get; private set; }

        /// <summary>
        /// Gets the code for the bathroom using a keypad with only digits.
        /// </summary>
        public string BathroomCodeDigitKeypad { get; private set; }

        /// <summary>
        /// Gets the keypad grid containing digits and letters.
        /// </summary>
        internal static char[][] AlphanumericGrid => new[]
        {
            new[] { '\0', '\0', '1', '\0', '\0' },
            new[] { '\0', '2', '3', '4', '\0' },
            new[] { '5', '6', '7', '8', '9' },
            new[] { '\0', 'A', 'B', 'C', '\0' },
            new[] { '\0', '\0', 'D', '\0', '\0' },
        };

        /// <summary>
        /// Gets the keypad grid containing only digits.
        /// </summary>
        internal static char[][] DigitGrid => new[]
        {
            new[] { '1', '2', '3' },
            new[] { '4', '5', '6' },
            new[] { '7', '8', '9' },
        };

        /// <summary>
        /// Returns the bathroom code associated with the specified instructions and grid.
        /// </summary>
        /// <param name="instructions">The instructions to determine the bathroom code from.</param>
        /// <param name="grid">The grid representing the keypad in use.</param>
        /// <returns>
        /// The bathroom code to use given the instructions in <paramref name="instructions"/>.
        /// </returns>
        internal static string GetBathroomCode(ICollection<string> instructions, char[][] grid)
        {
            IList<IList<Direction>> directions = ParseInstructions(instructions);
            StringBuilder code = new StringBuilder(instructions.Count);

            var origin = Point.Empty;

            for (int y = 0; y < grid.Length && origin == Point.Empty; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    if (grid[y][x] == '5')
                    {
                        origin = new Point(x, y);
                        break;
                    }
                }
            }

            var position = origin;

            foreach (IList<Direction> sequence in directions)
            {
                foreach (Direction direction in sequence)
                {
                    Point next = position + SizeFromDirection(direction);

                    if (IsInBounds(grid, next))
                    {
                        position = next;
                    }
                }

                code.Append(grid[position.Y][position.X]);
            }

            return code.ToString();
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            ICollection<string> instructions = ReadResourceAsLines();

            BathroomCodeDigitKeypad = GetBathroomCode(instructions, DigitGrid);
            BathroomCodeAlphanumericKeypad = GetBathroomCode(instructions, AlphanumericGrid);

            if (Verbose)
            {
                Console.WriteLine(
                    "The code for the bathroom with a digit keypad is {0}.",
                    BathroomCodeDigitKeypad);

                Console.WriteLine(
                    "The code for the bathroom with an alphanumeric keypad is {0}.",
                    BathroomCodeAlphanumericKeypad);
            }

            return 0;
        }

        /// <summary>
        /// Returns a size representing the move in the specified direction.
        /// </summary>
        /// <param name="direction">The direction to move in.</param>
        /// <returns>
        /// A <see cref="Size"/> representing a move in that direction.
        /// </returns>
        private static Size SizeFromDirection(Direction direction)
        {
            var result = default(Size);

            switch (direction)
            {
                case Direction.Down:
                    result = new Size(0, 1);
                    break;

                case Direction.Left:
                    result = new Size(-1, 0);
                    break;

                case Direction.Right:
                    result = new Size(1, 0);
                    break;

                case Direction.Up:
                    result = new Size(0, -1);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Returns whether the specified position in the bounds of the specified array.
        /// </summary>
        /// <param name="grid">The grid to test the position's bounds for.</param>
        /// <param name="position">The position to test for being in the bounds of the grid.</param>
        /// <returns>
        /// <see langword="true"/> if the position is within the bounds of the grid; otherwise <see langword="false"/>.
        /// </returns>
        private static bool IsInBounds(char[][] grid, Point position)
        {
            return
                position.Y >= 0 &&
                position.Y < grid.Length &&
                position.X >= 0 &&
                position.X < grid[position.Y].Length &&
                grid[position.Y][position.X] != '\0';
        }

        /// <summary>
        /// Parses the instructions to unlock the bathroom from the specified string.
        /// </summary>
        /// <param name="instructions">The instructions to parse.</param>
        /// <returns>
        /// An <see cref="IList{T}"/> containing the instructions to open the bathroom door.
        /// </returns>
        private static IList<IList<Direction>> ParseInstructions(ICollection<string> instructions)
        {
            var result = new List<IList<Direction>>(instructions.Count);

            foreach (string instruction in instructions)
            {
                var instructionsForDigit = new List<Direction>(instruction.Length);

                foreach (char ch in instruction)
                {
                    Direction direction = default;

                    switch (ch)
                    {
                        case 'D':
                            direction = Direction.Down;
                            break;

                        case 'L':
                            direction = Direction.Left;
                            break;

                        case 'R':
                            direction = Direction.Right;
                            break;

                        case 'U':
                            direction = Direction.Up;
                            break;
                    }

                    instructionsForDigit.Add(direction);
                }

                result.Add(instructionsForDigit);
            }

            return result;
        }
    }
}
