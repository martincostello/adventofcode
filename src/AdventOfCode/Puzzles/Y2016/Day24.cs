// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2016/day/24</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2016, 24, RequiresData = true)]
    public sealed class Day24 : Puzzle
    {
        /// <summary>
        /// Gets the fewest steps that can be taken to visit all of the locations.
        /// </summary>
        public int FewestStepsToVisitAllLocations { get; private set; }

        /// <summary>
        /// Returns the minimum number of steps required to visit all of the locations in the maze.
        /// </summary>
        /// <param name="layout">The layout of the maze.</param>
        /// <returns>
        /// The minimum number of steps required to visit all locations.
        /// </returns>
        public static int GetMinimumStepsToVisitLocations(IList<string> layout)
        {
            (SquareGrid maze, Point origin, IList<Point> waypoints) = BuildMaze(layout);

            double minimumCost = double.MaxValue;

            foreach (IEnumerable<Point> goals in Maths.GetPermutations(waypoints))
            {
                double cost = 0;
                Point current = origin;

                foreach (Point goal in goals)
                {
                    cost += PathFinding.AStar(maze, current, goal, ManhattanDistance);

                    if (cost > minimumCost)
                    {
                        break;
                    }

                    current = goal;
                }

                if (cost < minimumCost)
                {
                    minimumCost = cost;
                }
            }

            return (int)minimumCost;

            static double ManhattanDistance(Point a, Point b)
                => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> layout = await ReadResourceAsLinesAsync();

            FewestStepsToVisitAllLocations = GetMinimumStepsToVisitLocations(layout);

            if (Verbose)
            {
                Logger.WriteLine("The fewest number of steps required to visit every location is {0}.", FewestStepsToVisitAllLocations);
            }

            return PuzzleResult.Create(FewestStepsToVisitAllLocations);
        }

        /// <summary>
        /// Builds a maze with the specified layout.
        /// </summary>
        /// <param name="layout">The layout of the grid.</param>
        /// <returns>
        /// A <see cref="SquareGrid"/> representing the maze, the origin point and the goal locations in the maze.
        /// </returns>
        private static (SquareGrid maze, Point origin, IList<Point> goals) BuildMaze(IList<string> layout)
        {
            var maze = new SquareGrid(layout[0].Length, layout.Count);
            var origin = Point.Empty;
            var waypoints = new List<Point>();

            for (int y = 0; y < maze.Height; y++)
            {
                for (int x = 0; x < maze.Width; x++)
                {
                    char content = layout[y][x];

                    if (content == '#')
                    {
                        maze.Walls.Add(new Point(x, y));
                    }
                    else if (content == '0')
                    {
                        origin = new Point(x, y);
                    }
                    else if (content != '.')
                    {
                        waypoints.Add(new Point(x, y));
                    }
                }
            }

            return (maze, origin, waypoints);
        }
    }
}
