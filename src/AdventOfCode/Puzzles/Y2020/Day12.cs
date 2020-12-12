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
        /// Gets the Manhattan distance the ship has travelled when the waypoint is used.
        /// </summary>
        public int ManhattanDistanceWithWaypoint { get; private set; }

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
                [000] = (0, 1),
                [090] = (1, 0),
                [180] = (0, -1),
                [270] = (-1, 0),
            };

            int heading = 90;
            var ship = Point.Empty;

            foreach (string instruction in instructions)
            {
                int units = ParseInt32(instruction[1..]);

                switch (instruction[0..1])
                {
                    case "F":
                        (int x, int y) = vectors[heading];
                        ship += new Size(units * x, units * y);
                        break;

                    case "L":
                        heading = Math.Abs((heading - units + 360) % 360);
                        break;

                    case "R":
                        heading = Math.Abs((heading + units) % 360);
                        break;

                    case "N":
                        ship += new Size(0, units);
                        break;

                    case "S":
                        ship += new Size(0, -units);
                        break;

                    case "E":
                        ship += new Size(units, 0);
                        break;

                    case "W":
                        ship += new Size(-units, 0);
                        break;

                    default:
                        break;
                }
            }

            return Math.Abs(ship.X) + Math.Abs(ship.Y);
        }

        /// <summary>
        /// Gets the Manhattan distance travelled by the ship navigating the specified instructions as a waypoint.
        /// </summary>
        /// <param name="instructions">The navigation instructions for the ship.</param>
        /// <returns>
        /// The Manhattan distance the ship travels by following the instructions.
        /// </returns>
        public static int GetDistanceTravelledWithWaypoint(IEnumerable<string> instructions)
        {
            var ship = Point.Empty;
            var waypoint = new Point(10, 1);

            foreach (string instruction in instructions)
            {
                int units = ParseInt32(instruction[1..]);

                switch (instruction[0..1])
                {
                    case "F":
                        ship += new Size(units * waypoint.X, units * waypoint.Y);
                        break;

                    case "L":
                        waypoint = units switch
                        {
                            90 => new Point(-waypoint.Y, waypoint.X),
                            180 => new Point(-waypoint.X, -waypoint.Y),
                            270 => new Point(waypoint.Y, -waypoint.X),
                            _ => waypoint,
                        };
                        break;

                    case "R":
                        waypoint = units switch
                        {
                            90 => new Point(waypoint.Y, -waypoint.X),
                            180 => new Point(-waypoint.X, -waypoint.Y),
                            270 => new Point(-waypoint.Y, waypoint.X),
                            _ => waypoint,
                        };
                        break;

                    case "N":
                        waypoint += new Size(0, units);
                        break;

                    case "S":
                        waypoint += new Size(0, -units);
                        break;

                    case "E":
                        waypoint += new Size(units, 0);
                        break;

                    case "W":
                        waypoint += new Size(-units, 0);
                        break;

                    default:
                        break;
                }
            }

            return Math.Abs(ship.X) + Math.Abs(ship.Y);
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> instructions = await ReadResourceAsLinesAsync();

            ManhattanDistance = GetDistanceTravelled(instructions);
            ManhattanDistanceWithWaypoint = GetDistanceTravelledWithWaypoint(instructions);

            if (Verbose)
            {
                Logger.WriteLine("The Manhattan distance travelled by the ship is {0}.", ManhattanDistance);
                Logger.WriteLine("The Manhattan distance travelled by the ship when using the waypoint is {0}.", ManhattanDistanceWithWaypoint);
            }

            return PuzzleResult.Create(ManhattanDistance, ManhattanDistanceWithWaypoint);
        }
    }
}
