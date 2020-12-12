// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/12</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2020, 12, RequiresData = true)]
    public sealed class Day12 : Puzzle
    {
        /// <summary>
        /// Gets the Manhattan distance the ship has travelled.
        /// </summary>
        public int ManhattanDistance { get; private set; }

        /// <summary>
        /// Gets the Manhattan distance travelled by the ship navigating the specified instructions.
        /// </summary>
        /// <param name="instructions">The navigation instructions for the ship.</param>
        /// <returns>
        /// The Manhattan distance the ship travels by following the instructions.
        /// </returns>
        public static int GetDistanceTravelled(IEnumerable<string> instructions)
        {
            var vectors = new Dictionary<int, (int x, int y)>()
            {
                [0] = (0, 1),
                [90] = (1, 0),
                [180] = (0, -1),
                [270] = (-1, 0),
            };

            int heading = 90;
            var position = Point.Empty;

            foreach (string instruction in instructions)
            {
                int? moveHeading = null;
                int units = ParseInt32(instruction[1..]);

                switch (instruction[0..1])
                {
                    case "F":
                        moveHeading = heading;
                        break;

                    case "L":
                        heading = Math.Abs((heading - units + 360) % 360);
                        break;

                    case "R":
                        heading = Math.Abs((heading + units) % 360);
                        break;

                    case "N":
                        moveHeading = 0;
                        break;

                    case "S":
                        moveHeading = 180;
                        break;

                    case "E":
                        moveHeading = 90;
                        break;

                    case "W":
                        moveHeading = 270;
                        break;

                    default:
                        break;
                }

                if (moveHeading.HasValue)
                {
                    (int x, int y) = vectors[moveHeading.Value];
                    position += new Size(units * x, units * y);
                }
            }

            return Math.Abs(position.X) + Math.Abs(position.Y);
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> instructions = await ReadResourceAsLinesAsync();

            ManhattanDistance = GetDistanceTravelled(instructions);

            if (Verbose)
            {
                Logger.WriteLine("The Manhattan distance travelled by the ship is {0}.", ManhattanDistance);
            }

            return PuzzleResult.Create(ManhattanDistance);
        }
    }
}
