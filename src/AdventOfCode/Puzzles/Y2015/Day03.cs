// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2015/day/3</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day03 : Puzzle2015
    {
        /// <summary>
        /// Gets the number of houses with presents delivered to by Santa.
        /// </summary>
        internal int HousesWithPresentsFromSanta { get; private set; }

        /// <summary>
        /// Gets the number of houses with presents delivered to by Santa and Robo-Santa.
        /// </summary>
        internal int HousesWithPresentsFromSantaAndRoboSanta { get; private set; }

        /// <summary>
        /// Gets the number of unique houses that Santa delivers at least one present to.
        /// </summary>
        /// <param name="instructions">The instructions Santa should follow.</param>
        /// <returns>The number of unique houses that receive a delivery of at least one present.</returns>
        internal static int GetUniqueHousesVisitedBySanta(string instructions)
        {
            ICollection<CardinalDirection> directions = GetDirections(instructions);

            var santa = new SantaGps();
            var coordinates = new List<Point>();

            foreach (var direction in directions)
            {
                if (!coordinates.Contains(santa.Location))
                {
                    coordinates.Add(santa.Location);
                }

                santa.Move(direction);
            }

            return coordinates.Count;
        }

        /// <summary>
        /// Gets the number of unique houses that Santa and Robo-Santa deliver at least one present to.
        /// </summary>
        /// <param name="instructions">The instructions that Santa and Robo-Santa should follow.</param>
        /// <returns>The number of unique houses that receive a delivery of at least one present.</returns>
        internal static int GetUniqueHousesVisitedBySantaAndRoboSanta(string instructions)
        {
            ICollection<CardinalDirection> directions = GetDirections(instructions);

            var santa = new SantaGps();
            var roboSanta = new SantaGps();
            var current = santa;

            var coordinates = new List<Point>();

            foreach (var direction in directions)
            {
                current.Move(direction);

                if (current.Location != Point.Empty && !coordinates.Contains(current.Location))
                {
                    coordinates.Add(current.Location);
                }

                current = current == santa ? roboSanta : santa;
            }

            return coordinates.Count + 1;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string instructions = ReadResourceAsString();

            HousesWithPresentsFromSanta = GetUniqueHousesVisitedBySanta(instructions);
            HousesWithPresentsFromSantaAndRoboSanta = GetUniqueHousesVisitedBySantaAndRoboSanta(instructions);

            Console.WriteLine(
                "In 2015, Santa delivered presents to {0:N0} houses.",
                HousesWithPresentsFromSanta);

            Console.WriteLine(
                "In 2016, Santa and Robo-Santa delivered presents to {0:N0} houses.",
                HousesWithPresentsFromSantaAndRoboSanta);

            return 0;
        }

        /// <summary>
        /// Reads the directions from the specified file.
        /// </summary>
        /// <param name="instructions">The path of the file containing the directions.</param>
        /// <returns>An <see cref="ICollection{T}"/> containing the directions from from the specified file.</returns>
        private static ICollection<CardinalDirection> GetDirections(string instructions)
        {
            var directions = new List<CardinalDirection>();

            for (int i = 0; i < instructions.Length; i++)
            {
                CardinalDirection direction;

                switch (instructions[i])
                {
                    case '^':
                        direction = CardinalDirection.North;
                        break;

                    case 'v':
                        direction = CardinalDirection.South;
                        break;

                    case '>':
                        direction = CardinalDirection.East;
                        break;

                    case '<':
                        direction = CardinalDirection.West;
                        break;

                    default:
                        Console.WriteLine("Invalid direction: '{0}'.", instructions[i]);
                        continue;
                }

                directions.Add(direction);
            }

            return directions;
        }

        /// <summary>
        /// A class representing a GPS locator for a Santa-type figure. This class cannot be inherited.
        /// </summary>
        private sealed class SantaGps
        {
            /// <summary>
            /// Gets or sets the location of the Santa-type figure.
            /// </summary>
            internal Point Location { get; set; }

            /// <summary>
            /// Moves the Santa-type figure in the specified direction.
            /// </summary>
            /// <param name="direction">The direction to move in.</param>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="direction"/> is invalid.</exception>
            internal void Move(CardinalDirection direction)
            {
                switch (direction)
                {
                    case CardinalDirection.East:
                        Location += new Size(1, 0);
                        break;

                    case CardinalDirection.North:
                        Location += new Size(0, 1);
                        break;

                    case CardinalDirection.South:
                        Location += new Size(0, -1);
                        break;

                    case CardinalDirection.West:
                        Location += new Size(-1, 0);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction, "The specified direction is invalid.");
                }
            }
        }
    }
}
