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

            ICollection<CardinalDirection> directions = GetDirections(args[0]);

            int housesWithPresents2015 = GetUniqueHousesVisitedBySanta(directions);
            int housesWithPresents2016 = GetUniqueHousesVisitedBySantaAndRoboSanta(directions);

            Console.WriteLine("In 2015, Santa delivered presents to {0:N0} houses.", housesWithPresents2015);
            Console.WriteLine("In 2016, Santa and Robo-Santa delivered presents to {0:N0} houses.", housesWithPresents2016);
            Console.WriteLine("Robo-Santa makes Santa {0:P2} more efficient.", ((double)housesWithPresents2016 / housesWithPresents2015) - 1);

            return 0;
        }

        /// <summary>
        /// Reads the directions from the specified file.
        /// </summary>
        /// <param name="path">The path of the file containing the directions.</param>
        /// <returns>An <see cref="ICollection{T}"/> containing the directions from from the specified file.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Usage",
            "CA2202:Do not dispose objects multiple times",
            Justification = "The stream is not disposed multiple times.")]
        private static ICollection<CardinalDirection> GetDirections(string path)
        {
            IList<CardinalDirection> directions = new List<CardinalDirection>();

            using (Stream stream = File.OpenRead(path))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    char ch;

                    while (!reader.EndOfStream)
                    {
                        ch = (char)reader.Read();
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
                }
            }

            return directions;
        }

        /// <summary>
        /// Gets the number of unique houses that Santa delivers at least one present to.
        /// </summary>
        /// <param name="directions">The directions Santa should follow.</param>
        /// <returns>The number of unique houses that receive a delivery of at least one present.</returns>
        private static int GetUniqueHousesVisitedBySanta(IEnumerable<CardinalDirection> directions)
        {
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
        /// <param name="directions">The directions that Santa and Robo-Santa should follow.</param>
        /// <returns>The number of unique houses that receive a delivery of at least one present.</returns>
        private static int GetUniqueHousesVisitedBySantaAndRoboSanta(IEnumerable<CardinalDirection> directions)
        {
            List<Point> coordinates = new List<Point>();
            SantaGps santa = new SantaGps();
            SantaGps roboSanta = new SantaGps();

            SantaGps current = santa;

            foreach (var direction in directions)
            {
                current.Move(direction);

                if (!coordinates.Contains(current.Location))
                {
                    coordinates.Add(current.Location);
                }

                current = current == santa ? roboSanta : santa;
            }

            return coordinates.Count;
        }
    }
}
