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

namespace MartinCostello.AdventOfCode.Day1
{
    using System;
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

            int floor = 0;

            using (Stream stream = File.OpenRead(args[0]))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    char ch;

                    while (!reader.EndOfStream)
                    {
                        ch = (char)reader.Read();

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
                    }
                }
            }

            Console.Write("Santa should go to floor {0}. Press any key to exit...", floor);
            Console.Read();

            return 0;
        }
    }
}
