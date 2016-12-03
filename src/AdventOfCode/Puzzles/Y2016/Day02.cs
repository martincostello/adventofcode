// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

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
        /// Gets the code for the bathroom.
        /// </summary>
        public int BathroomCode { get; private set; }

        /// <summary>
        /// Returns the bathroom code associated with the specified instructions.
        /// </summary>
        /// <param name="instructions">The instructions to determine the bathroom code from.</param>
        /// <returns>
        /// The bathroom code to use given the instructions in <paramref name="instructions"/>.
        /// </returns>
        internal static int GetBathroomCode(ICollection<string> instructions)
        {
            IList<IList<Direction>> directions = ParseInstructions(instructions);
            IList<int> digits = new List<int>(instructions.Count);

            var grid = new[]
            {
                new[] { 1, 2, 3 },
                new[] { 4, 5, 6 },
                new[] { 7, 8, 9 },
            };

            var position = new Point(1, 1);

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

                digits.Add(grid[position.Y][position.X]);
            }

            return Maths.FromDigits(digits);
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            ICollection<string> instructions = ReadResourceAsLines();

            BathroomCode = GetBathroomCode(instructions);

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
        private static bool IsInBounds(int[][] grid, Point position)
        {
            return
                position.Y >= 0 &&
                position.Y < grid.Length &&
                position.X >= 0 &&
                position.X < grid[position.Y].Length;
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
                    Direction direction = default(Direction);

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
