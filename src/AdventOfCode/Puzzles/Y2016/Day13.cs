// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2016/day/13</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2016, 13, MinimumArguments = 1)]
    public sealed class Day13 : Puzzle
    {
        /// <summary>
        /// Gets the value in register A after processing the instructions.
        /// </summary>
        public int FewestStepsToReach31X39Y { get; private set; }

        /// <summary>
        /// Returns the minimum number of steps required to reach the specified coordinates.
        /// </summary>
        /// <param name="favoriteNumber">The office designer's favorite number.</param>
        /// <param name="x">The x-coordinate of the destination.</param>
        /// <param name="y">The y-coordinate of the destination.</param>
        /// <returns>
        /// The minimum number of steps required to reach the coordinate in the maze specified
        /// by <paramref name="x"/> and <paramref name="y"/>.
        /// </returns>
        public static int GetMinimumStepsToReachCoordinate(
            int favoriteNumber,
            int x,
            int y)
        {
            var maze = new SquareGrid(x * 5, y * 5);

            for (int i = 0; i < maze.Width; i++)
            {
                for (int j = 0; j < maze.Height; j++)
                {
                    if (IsCoordinateWall(favoriteNumber, i, j))
                    {
                        maze.Walls.Add(new Point(i, j));
                    }
                }
            }

            return (int)PathFinding.AStar(
                maze,
                new Point(1, 1),
                new Point(x, y),
                (a, b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y));

            static bool IsCoordinateWall(int favoriteNumber, int x, int y)
            {
                int z = (x * x) + (3 * x) + (2 * x * y) + y + (y * y);

                z += favoriteNumber;

                string binary = Convert.ToString(z, toBase: 2);

                return binary.Count((p) => p == '1') % 2 != 0;
            }
        }

        /// <inheritdoc />
        protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            int favoriteNumber = ParseInt32(args[0]);

            FewestStepsToReach31X39Y = GetMinimumStepsToReachCoordinate(favoriteNumber, 31, 39);

            if (Verbose)
            {
                Logger.WriteLine("The fewest number of steps required to reach 31,39 is {0}.", FewestStepsToReach31X39Y);
            }

            return PuzzleResult.Create(FewestStepsToReach31X39Y);
        }
    }
}
