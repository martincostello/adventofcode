// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2016/day/13</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2016, 13, MinimumArguments = 1)]
    public sealed class Day13 : Puzzle
    {
        /// <summary>
        /// Gets the fewest steps that can be taken to reach coordinate x=13, y=39.
        /// </summary>
        public int FewestStepsToReach31X39Y { get; private set; }

        /// <summary>
        /// Gets the number of locations that are within 50 steps.
        /// </summary>
        public int LocationsWithin50 { get; private set; }

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
            SquareGrid maze = BuildMaze(x * 2, y * 2, favoriteNumber);

            var goal = new Point(x, y);

            return (int)GetMinimumStepsToGoal(maze, goal);
        }

        /// <inheritdoc />
        protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            int favoriteNumber = ParseInt32(args[0]);

            FewestStepsToReach31X39Y = GetMinimumStepsToReachCoordinate(favoriteNumber, 31, 39);
            LocationsWithin50 = CountLocationsWithin50Steps(favoriteNumber);

            if (Verbose)
            {
                Logger.WriteLine("The fewest number of steps required to reach 31,39 is {0}.", FewestStepsToReach31X39Y);
                Logger.WriteLine("The number of locations within 50 steps of the origin is {0}.", LocationsWithin50);
            }

            return PuzzleResult.Create(FewestStepsToReach31X39Y, LocationsWithin50);
        }

        /// <summary>
        /// Returns the number of locations that are no further than 50 steps away.
        /// </summary>
        /// <param name="favoriteNumber">The office designer's favorite number.</param>
        /// <returns>
        /// The number of locations within 50 steps of the origin.
        /// </returns>
        private static int CountLocationsWithin50Steps(int favoriteNumber)
        {
            int count = 0;
            int maximum = 50;
            int dimensions = (maximum / 2) + 1;

            SquareGrid maze = BuildMaze(dimensions, dimensions, favoriteNumber);

            for (int x = 0; x < maze.Width; x++)
            {
                for (int y = 0; y < maze.Height; y++)
                {
                    var goal = new Point(x, y);

                    double cost = GetMinimumStepsToGoal(maze, goal);

                    if (cost <= maximum)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Builds a maze with the specified dimensions.
        /// </summary>
        /// <param name="width">The width of the maze.</param>
        /// <param name="height">The height of the maze.</param>
        /// <param name="seed">The seed to use for wall placement.</param>
        /// <returns>
        /// A <see cref="SquareGrid"/> representing the maze.
        /// </returns>
        private static SquareGrid BuildMaze(int width, int height, int seed)
        {
            var maze = new SquareGrid(width, height);

            for (int i = 0; i < maze.Width; i++)
            {
                for (int j = 0; j < maze.Height; j++)
                {
                    if (IsWall(seed, i, j))
                    {
                        maze.Walls.Add(new Point(i, j));
                    }
                }
            }

            return maze;

            static bool IsWall(int seed, int x, int y)
            {
                int z = (x * x) + (3 * x) + (2 * x * y) + y + (y * y);

                z += seed;

                string binary = Convert.ToString(z, toBase: 2);

                return binary.Count((p) => p == '1') % 2 != 0;
            }
        }

        /// <summary>
        /// Returns the minimum number of steps required to reach the specified goal.
        /// </summary>
        /// <param name="maze">The maze to traverse.</param>
        /// <param name="goal">The destination to reach.</param>
        /// <returns>
        /// The minimum number of steps required to reach <paramref name="goal"/>.
        /// </returns>
        private static double GetMinimumStepsToGoal(SquareGrid maze, Point goal)
        {
            var start = new Point(1, 1);

            return PathFinding.AStar(
                maze,
                start,
                goal,
                ManhattanDistance);

            static double ManhattanDistance(Point a, Point b)
                => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }
    }
}
