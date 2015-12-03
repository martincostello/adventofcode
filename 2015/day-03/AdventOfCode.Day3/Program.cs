// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   Program.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day3
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;

    internal static class Program
    {
        internal static int Main(string[] args)
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

            IList<CardinalDirection> directions = new List<CardinalDirection>();

            using (Stream stream = File.OpenRead(args[0]))
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
                                return -1;
                        }

                        directions.Add(direction);
                    }
                }
            }

            List<Point> houseCoordinates2015 = new List<Point>();
            SantaGps santa2015 = new SantaGps();

            foreach (var direction in directions)
            {
                if (!houseCoordinates2015.Contains(santa2015.Location))
                {
                    houseCoordinates2015.Add(santa2015.Location);
                }

                switch (direction)
                {
                    case CardinalDirection.East:
                        santa2015.Location += Moves.East;
                        break;

                    case CardinalDirection.North:
                        santa2015.Location += Moves.North;
                        break;

                    case CardinalDirection.South:
                        santa2015.Location += Moves.South;
                        break;

                    case CardinalDirection.West:
                        santa2015.Location += Moves.West;
                        break;

                    default:
                        Console.WriteLine("Invalid direction: {0}.", direction);
                        return -1;
                }
            }

            List<Point> houseCoordinates2016 = new List<Point>();
            SantaGps santa2016 = new SantaGps();
            SantaGps roboSanta2016 = new SantaGps();

            bool roboSantasMove = false;
            SantaGps currentSanta = santa2016;

            foreach (var direction in directions)
            {
                switch (direction)
                {
                    case CardinalDirection.East:
                        currentSanta.Location += Moves.East;
                        break;

                    case CardinalDirection.North:
                        currentSanta.Location += Moves.North;
                        break;

                    case CardinalDirection.South:
                        currentSanta.Location += Moves.South;
                        break;

                    case CardinalDirection.West:
                        currentSanta.Location += Moves.West;
                        break;

                    default:
                        Console.WriteLine("Invalid direction: {0}.", direction);
                        return -1;
                }

                if (!houseCoordinates2016.Contains(currentSanta.Location))
                {
                    houseCoordinates2016.Add(currentSanta.Location);
                }

                roboSantasMove = !roboSantasMove;
                currentSanta = roboSantasMove ? roboSanta2016 : santa2016;
            }

            Console.WriteLine("In 2015, Santa delivered presents to {0:N0} houses.", houseCoordinates2015.Count);
            Console.WriteLine("In 2016, Santa and Robo-Santa delivered presents to {0:N0} houses.", houseCoordinates2016.Count);
            Console.Write("Robo-Santa makes Santa {0:P2} more efficient.", ((double)houseCoordinates2016.Count / houseCoordinates2015.Count) - 1);
            Console.Read();

            return 0;
        }

        private sealed class SantaGps
        {
            internal Point Location { get; set; }
        }

        private sealed class Moves
        {
            internal static readonly Size North = new Size(0, 1);

            internal static readonly Size East = new Size(1, 0);

            internal static readonly Size South = new Size(0, -1);

            internal static readonly Size West = new Size(-1, 0);
        }
    }
}
