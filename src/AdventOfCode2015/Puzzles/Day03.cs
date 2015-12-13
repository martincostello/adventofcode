// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using Impl;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/day/3</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day03 : IPuzzle
    {
        /// <summary>
        /// Gets the number of houses with presents delivered to by Santa.
        /// </summary>
        internal int HousesWithPresentsFromSanta { get; private set; }

        /// <summary>
        /// Gets the number of houses with presents delivered to by Santa and Robo-Santa.
        /// </summary>
        internal int HousesWithPresentsFromSantaAndRoboSanta { get; private set; }

        /// <inheritdoc />
        public int Solve(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("No input file path specified.");
                return -1;
            }

            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine("The input file path specified cannot be found.");
                return -1;
            }

            string instructions = File.ReadAllText(args[0]);

            HousesWithPresentsFromSanta = GetUniqueHousesVisitedBySanta(instructions);
            HousesWithPresentsFromSantaAndRoboSanta = GetUniqueHousesVisitedBySantaAndRoboSanta(instructions);

            Console.WriteLine("In 2015, Santa delivered presents to {0:N0} houses.", HousesWithPresentsFromSanta);
            Console.WriteLine("In 2016, Santa and Robo-Santa delivered presents to {0:N0} houses.", HousesWithPresentsFromSantaAndRoboSanta);
            Console.WriteLine("Robo-Santa makes Santa {0:P2} more efficient.", ((double)HousesWithPresentsFromSantaAndRoboSanta / HousesWithPresentsFromSanta) - 1);

            return 0;
        }

        /// <summary>
        /// Gets the number of unique houses that Santa delivers at least one present to.
        /// </summary>
        /// <param name="instructions">The instructions Santa should follow.</param>
        /// <returns>The number of unique houses that receive a delivery of at least one present.</returns>
        internal static int GetUniqueHousesVisitedBySanta(string instructions)
        {
            ICollection<CardinalDirection> directions = GetDirections(instructions);

            List<Point> coordinates = new List<Point>();
            SantaGps santa = new SantaGps();

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

            SantaGps santa = new SantaGps();
            SantaGps roboSanta = new SantaGps();

            List<Point> coordinates = new List<Point>();

            SantaGps current = santa;

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

        /// <summary>
        /// Reads the directions from the specified file.
        /// </summary>
        /// <param name="instructions">The path of the file containing the directions.</param>
        /// <returns>An <see cref="ICollection{T}"/> containing the directions from from the specified file.</returns>
        private static ICollection<CardinalDirection> GetDirections(string instructions)
        {
            IList<CardinalDirection> directions = new List<CardinalDirection>();

            for (int i = 0; i < instructions.Length; i++)
            {
                char ch = (char)instructions[i];
                CardinalDirection direction;

                switch (ch)
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
                        Console.WriteLine("Invalid direction: '{0}'.", ch);
                        continue;
                }

                directions.Add(direction);
            }

            return directions;
        }
    }
}
