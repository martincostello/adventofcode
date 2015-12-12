// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.IO;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/day/1</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day01 : IPuzzle
    {
        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Usage",
            "CA2202:Do not dispose objects multiple times",
            Justification = "The stream is not disposed multiple times.")]
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

            int floor = 0;
            int instruction = 0;

            bool hasVisitedBasement = false;

            using (Stream stream = File.OpenRead(args[0]))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    char ch;

                    while (!reader.EndOfStream)
                    {
                        ch = (char)reader.Read();
                        instruction++;

                        switch (ch)
                        {
                            case '(':
                                floor++;
                                break;

                            case ')':
                                floor--;
                                break;

                            default:
                                break;
                        }

                        if (!hasVisitedBasement)
                        {
                            if (floor == -1)
                            {
                                hasVisitedBasement = true;
                                Console.WriteLine("Santa first enters the basement after following instruction {0:N0}.", instruction);
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Santa should go to floor {0}.", floor);

            return 0;
        }
    }
}
