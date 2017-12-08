// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using System;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2017/day/3</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day03 : Puzzle2017
    {
        /// <summary>
        /// Gets the checksum of the spreadsheet using the difference between the minimum and maximum.
        /// </summary>
        public int Steps { get; private set; }

        /// <inheritdoc />
        protected override int MinimumArguments => 1;

        /// <summary>
        /// Computes how many steps are required to carry the data from the specified square identified all the way to the access port.
        /// </summary>
        /// <param name="square">The number of the square to get the number of steps to retrieve data from.</param>
        /// <returns>
        /// The number of steps required to carry the data from the specified square specified by <paramref name="square"/> to the access port.
        /// </returns>
        public static int ComputeSteps(int square)
        {
            var position = Point.Empty;
            var right = new Size(width: 1, height: 0);
            var up = new Size(width: 0, height: 1);
            var left = new Size(width: -1, height: 0);
            var down = new Size(width: 0, height: -1);

            int LengthOfRing(int r) => 1 + (2 * (r - 1));
            int PerimiterOfRing(int r) => (2 * LengthOfRing(r)) + (2 * LengthOfRing(r - 1));

            int ring = 2;
            int currentSquare = 1;

            while (currentSquare < square)
            {
                int length = LengthOfRing(ring);
                int perimeter = PerimiterOfRing(ring);

                // Is the target square in the current ring?
                if (currentSquare + perimeter >= square)
                {
                    position += right;
                    currentSquare++;

                    int movesUp = length - 2;
                    int movesOther = movesUp + 1;

                    for (int i = 0; currentSquare != square && i < movesUp; i++)
                    {
                        position += up;
                        currentSquare++;
                    }

                    foreach (var direction in new[] { left, down, right })
                    {
                        for (int i = 0; currentSquare != square && i < movesOther; i++)
                        {
                            position += direction;
                            currentSquare++;
                        }

                        if (currentSquare == square)
                        {
                            break;
                        }
                    }

                    if (currentSquare == square)
                    {
                        break;
                    }
                }

                // Skip to the next ring
                currentSquare += PerimiterOfRing(ring++);
                position += right;
                position += down;
            }

            int manhattenDistance = Math.Abs(position.X) + Math.Abs(position.Y);

            return manhattenDistance;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            int input = ParseInt32(args[0]);

            Steps = ComputeSteps(input);

            Console.WriteLine($"The number of steps required to carry the data from square {input:N0} all the way to the access port is {Steps:N0}.");

            return 0;
        }
    }
}
