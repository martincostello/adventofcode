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

            List<Point> houseCoordinates = new List<Point>();
            Point current = new Point(0, 0);

            foreach (var direction in directions)
            {
                if (!houseCoordinates.Contains(current))
                {
                    houseCoordinates.Add(current);
                }

                switch (direction)
                {
                    case CardinalDirection.East:
                        current += Moves.East;
                        break;

                    case CardinalDirection.North:
                        current += Moves.North;
                        break;

                    case CardinalDirection.South:
                        current += Moves.South;
                        break;

                    case CardinalDirection.West:
                        current += Moves.West;
                        break;

                    default:
                        Console.WriteLine("Invalid direction: {0}.", direction);
                        return -1;
                }
            }

            Console.Write("Santa has delivered presents to {0:N0} houses.", houseCoordinates.Count);
            Console.Read();

            return 0;
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
