// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2016/day/1</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day01 : Puzzle2016
    {
        /// <summary>
        /// An enumeration of directions.
        /// </summary>
        internal enum Direction
        {
            /// <summary>
            /// Turn left.
            /// </summary>
            Left,

            /// <summary>
            /// Turn right.
            /// </summary>
            Right,
        }

        /// <summary>
        /// Gets the number of blocks to the Easter Bunny's headquarters.
        /// </summary>
        public int BlocksToEasterBunnyHQ { get; private set; }

        /// <summary>
        /// Gets the number of blocks to the Easter Bunny's headquarters ignoring duplicates.
        /// </summary>
        public int BlocksToEasterBunnyHQIgnoringDuplicates { get; private set; }

        /// <summary>
        /// Calculate the distance, in blocks, by following the specified instructions.
        /// </summary>
        /// <param name="input">The instructions to follow.</param>
        /// <param name="ignoreDuplicates">Whether to ignore duplicate locations.</param>
        /// <returns>
        /// The shortest distance, in blocks, away from the origin following the instructions would take you.
        /// </returns>
        internal static int CalculateDistance(string input, bool ignoreDuplicates)
        {
            var bearing = CardinalDirection.North;
            var position = Point.Empty;

            IList<Instruction> instructions = ParseDirections(input);
            IList<Point> positions = new List<Point>();

            foreach (Instruction instruction in instructions)
            {
                bearing = Turn(bearing, instruction.Direction);

                for (int i = 0; i < instruction.Distance; i++)
                {
                    if (!ignoreDuplicates)
                    {
                        if (positions.Contains(position))
                        {
                            break;
                        }

                        positions.Add(position);
                    }

                    switch (bearing)
                    {
                        case CardinalDirection.East:
                            position += new Size(1, 0);
                            break;

                        case CardinalDirection.North:
                            position += new Size(0, 1);
                            break;

                        case CardinalDirection.South:
                            position += new Size(0, -1);
                            break;

                        case CardinalDirection.West:
                            position += new Size(-1, 0);
                            break;
                    }
                }
            }

            return Math.Abs(position.X) + Math.Abs(position.Y);
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string instructions = ReadResourceAsString();

            BlocksToEasterBunnyHQIgnoringDuplicates = CalculateDistance(instructions, ignoreDuplicates: true);
            BlocksToEasterBunnyHQ = CalculateDistance(instructions, ignoreDuplicates: false);

            Console.WriteLine(
                "The Easter Bunny's headquarters are {0:N0} blocks away.",
                BlocksToEasterBunnyHQIgnoringDuplicates);

            Console.WriteLine(
                "The Easter Bunny's headquarters are {0:N0} blocks away if it is the first location visited twice.",
                BlocksToEasterBunnyHQ);

            return 0;
        }

        /// <summary>
        /// Parses the directions to the Easter Bunny headquarters from the specified string.
        /// </summary>
        /// <param name="input">The directions to parse.</param>
        /// <returns>
        /// An <see cref="IList{T}"/> containing the directions to Easter Bunny HQ.
        /// </returns>
        private static IList<Instruction> ParseDirections(string input)
        {
            string[] instructions = input.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var result = new List<Instruction>();

            foreach (string rawInstruction in instructions)
            {
                var instruction = new Instruction()
                {
                    Direction = rawInstruction[0] == 'L' ? Direction.Left : Direction.Right,
                    Distance = int.Parse(rawInstruction.Substring(1), CultureInfo.InvariantCulture),
                };

                result.Add(instruction);
            }

            return result;
        }

        /// <summary>
        /// Returns the new bearing after turning left or right from the specified bearing.
        /// </summary>
        /// <param name="bearing">The current bearing.</param>
        /// <param name="direction">The direction to turn in.</param>
        /// <returns>
        /// The new bearing.
        /// </returns>
        private static CardinalDirection Turn(CardinalDirection bearing, Direction direction)
        {
            CardinalDirection result = CardinalDirection.North;

            switch (bearing)
            {
                case CardinalDirection.East:
                    result = direction == Direction.Left ? CardinalDirection.North : CardinalDirection.South;
                    break;

                case CardinalDirection.North:
                    result = direction == Direction.Left ? CardinalDirection.West : CardinalDirection.East;
                    break;

                case CardinalDirection.South:
                    result = direction == Direction.Left ? CardinalDirection.East : CardinalDirection.West;
                    break;

                case CardinalDirection.West:
                    result = direction == Direction.Left ? CardinalDirection.South : CardinalDirection.North;
                    break;
            }

            return result;
        }

        /// <summary>
        /// A class representing an instruction for movement. This class cannot be inherited.
        /// </summary>
        internal sealed class Instruction
        {
            /// <summary>
            /// Gets or sets the direction to travel.
            /// </summary>
            internal Direction Direction { get; set; }

            /// <summary>
            /// Gets or sets the distance to travel.
            /// </summary>
            internal int Distance { get; set; }
        }
    }
}
