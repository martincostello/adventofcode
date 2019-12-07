// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2019/day/3</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day03 : Puzzle2019
    {
        /// <summary>
        /// Represents the wires passing through a grid square.
        /// </summary>
        [Flags]
        private enum Wires
        {
            /// <summary>
            /// None.
            /// </summary>
            None,

            /// <summary>
            /// The first wire.
            /// </summary>
            First = 1,

            /// <summary>
            /// The second wire.
            /// </summary>
            Second = 2,
        }

        /// <summary>
        /// Gets the Manhattan distance of the central port to the closest intersection.
        /// </summary>
        public int ManhattanDistance { get; private set; }

        /// <summary>
        /// Gets the Manhattan distance of the central port to the closest intersection for the specified wires.
        /// </summary>
        /// <param name="wires">The wires to compute the intersection distance for.</param>
        /// <returns>
        /// The Manhattan distance from the central port to the closest intersection.
        /// </returns>
        public static int GetManhattanDistanceOfClosesIntersection(IList<string> wires)
        {
            var grid = new Dictionary<Point, Wires>();
            var intersections = new List<Point>();

            void MarkWire(Point point, Wires wire)
            {
                Wires wiresSet = grid[point] = grid.GetValueOrDefault(point) | wire;

                if (wiresSet == (Wires.First | Wires.Second))
                {
                    intersections.Add(point);
                }
            }

            for (int i = 0; i < wires.Count; i++)
            {
                string wire = wires[i];
                string[] path = wire.Split(',');

                int x = 0;
                int y = 0;

                foreach (string instruction in path)
                {
                    (int deltaX, int deltaY) = GetDelta(instruction);

                    int sign = Math.Sign(deltaX);

                    for (int j = 0; j < Math.Abs(deltaX); j++)
                    {
                        MarkWire(new Point(x + (j * sign), y), (Wires)(i + 1));
                    }

                    sign = Math.Sign(deltaY);

                    for (int j = 0; j < Math.Abs(deltaY); j++)
                    {
                        MarkWire(new Point(x, y + (j * sign)), (Wires)(i + 1));
                    }

                    x += deltaX;
                    y += deltaY;
                }
            }

            return intersections.Skip(1).Min((p) => Math.Abs(p.X) + Math.Abs(p.Y));
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> wires = ReadResourceAsLines();

            ManhattanDistance = GetManhattanDistanceOfClosesIntersection(wires);

            if (Verbose)
            {
                Logger.WriteLine("The Manhattan distance from the central port to the closest intersection is {0}.", ManhattanDistance);
            }

            return 0;
        }

        /// <summary>
        /// Gets the x and y coordinates to change by for the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction to get the coordinate delta for.</param>
        /// <returns>
        /// The x and y coordinates to move the grid by.
        /// </returns>
        private static (int x, int y) GetDelta(string instruction)
        {
            char direction = instruction[0];
            int distance = ParseInt32(instruction[1..]);

            return direction switch
            {
                'D' => (0, -distance),
                'L' => (-distance, 0),
                'R' => (distance, 0),
                'U' => (0, distance),
                _ => throw new NotSupportedException($"The direction '{direction}' is invalid."),
            };
        }
    }
}
