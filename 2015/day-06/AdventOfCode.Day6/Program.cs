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

namespace MartinCostello.AdventOfCode.Day6
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// A console application that solves <c>http://adventofcode.com/day/6</c>. This class cannot be inherited.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry-point to the application.
        /// </summary>
        /// <param name="args">The arguments to the application.</param>
        /// <returns>The exit code from the application.</returns>
        internal static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.Error.WriteLine("No input file path and instruction set specified.");
                return -1;
            }

            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine("The input file path specified cannot be found.");
                return -1;
            }

            int version = -1;

            switch (args[1])
            {
                case "1":
                    version = 1;
                    break;

                case "2":
                    version = 2;
                    break;

                default:
                    break;
            }

            if (version == -1)
            {
                Console.Error.WriteLine("The instruction set specified is invalid.");
                return -1;
            }

            var instructions = new List<IInstruction>();

            foreach (string line in File.ReadLines(args[0]))
            {
                if (version == 1)
                {
                    instructions.Add(InstructionV1.Parse(line));
                }
                else
                {
                    instructions.Add(InstructionV2.Parse(line));
                }
            }

            Console.WriteLine("Processing instructions using set {0}...", version);

            Stopwatch stopwatch = Stopwatch.StartNew();

            var grid = new LightGrid(1000, 1000);

            foreach (var instruction in instructions)
            {
                instruction.Act(grid);
            }

            stopwatch.Stop();

            if (version == 1)
            {
                Console.WriteLine("{0:N0} lights are illuminated. Took {1:N2} seconds.", grid.Count, stopwatch.Elapsed.TotalSeconds);
            }
            else
            {
                Console.WriteLine("The total brightness of the grid is {0:N0}. Took {1:N2} seconds.", grid.Brightness, stopwatch.Elapsed.TotalSeconds);
            }

            return 0;
        }
    }
}
