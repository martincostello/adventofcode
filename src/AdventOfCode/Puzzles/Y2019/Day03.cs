// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2019/day/3</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day03 : Puzzle
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
        /// Gets the intersection that is reached in the minimum number of steps.
        /// </summary>
        public int MinimumSteps { get; private set; }

        /// <summary>
        /// Gets the Manhattan distance of the central port to the closest intersection for the specified wires.
        /// </summary>
        /// <param name="wires">The wires to compute the intersection distance for.</param>
        /// <returns>
        /// The Manhattan distance from the central port to the closest intersection and the
        /// minimum number of combined steps to reach an intersection.
        /// </returns>
        public static (int manhattanDistance, int minimumSteps) GetManhattanDistanceOfClosesIntersection(IList<string> wires)
        {
            var grid = new Dictionary<Point, Wires>();
            var stepsToIntersection = new Dictionary<Point, int[]>();
            var intersections = new List<Point>();

            void MarkWire(Point point, Wires wire, int steps)
            {
                Wires wiresSet = grid[point] = grid.GetValueOrDefault(point) | wire;

                if (wiresSet == (Wires.First | Wires.Second))
                {
                    intersections.Add(point);
                }

                int[] stepsForWires = Array.Empty<int>();

                if (!stepsToIntersection.ContainsKey(point))
                {
                    stepsForWires = stepsToIntersection[point] = new int[2];
                    Array.Fill(stepsForWires, int.MaxValue);
                }
                else
                {
                    stepsForWires = stepsToIntersection[point];
                }

                int index = (int)wire - 1;
                stepsForWires[index] = Math.Min(steps, stepsForWires[index]);
            }

            for (int i = 0; i < wires.Count; i++)
            {
                string wire = wires[i];
                string[] path = wire.Split(',');

                int x = 0;
                int y = 0;
                int steps = 0;

                foreach (string instruction in path)
                {
                    (int deltaX, int deltaY) = GetDelta(instruction);

                    int sign = Math.Sign(deltaX);

                    for (int j = 0; j < Math.Abs(deltaX); j++, steps++)
                    {
                        MarkWire(new Point(x + (j * sign), y), (Wires)(i + 1), steps);
                    }

                    sign = Math.Sign(deltaY);

                    for (int j = 0; j < Math.Abs(deltaY); j++, steps++)
                    {
                        MarkWire(new Point(x, y + (j * sign)), (Wires)(i + 1), steps);
                    }

                    x += deltaX;
                    y += deltaY;
                }
            }

            int manhattanDistance = intersections
                .Skip(1)
                .Min((p) => Math.Abs(p.X) + Math.Abs(p.Y));

            int minimumSteps = stepsToIntersection
                .Where((p) => p.Key != default && intersections.Contains(p.Key))
                .Min((p) => p.Value.Sum());

            return (manhattanDistance, minimumSteps);
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> wires = ReadResourceAsLines();

            (ManhattanDistance, MinimumSteps) = GetManhattanDistanceOfClosesIntersection(wires);

            if (Verbose)
            {
                Logger.WriteLine("The Manhattan distance from the central port to the closest intersection is {0}.", ManhattanDistance);
                Logger.WriteLine("The minimum number of combined steps to get to an intersection is {0}.", MinimumSteps);
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
            int distance = ParseInt32(instruction.AsSpan(1));

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
