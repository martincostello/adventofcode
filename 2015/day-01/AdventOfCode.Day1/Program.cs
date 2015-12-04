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

    /// <summary>
    /// A console application that solves <c>http://adventofcode.com/day/1</c>. This class cannot be inherited.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry-point to the application.
        /// </summary>
        /// <param name="args">The arguments to the application.</param>
        /// <returns>The exit code from the application.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "The stream is not disposed multiple times.")]
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
