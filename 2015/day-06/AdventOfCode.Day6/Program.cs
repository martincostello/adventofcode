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

            var instructions = new List<Instruction>();

            foreach (string line in File.ReadLines(args[0]))
            {
                instructions.Add(Instruction.Parse(line));
            }

            Console.WriteLine("Processing instructions...");

            Stopwatch stopwatch = Stopwatch.StartNew();

            var grid = new LightGrid(1000, 1000);

            foreach (var instruction in instructions)
            {
                instruction.Act(grid);
            }

            stopwatch.Stop();

            Console.WriteLine("{0:N0} lights are illuminated. Took {1:N2} seconds.", grid.Count, stopwatch.Elapsed.TotalSeconds);

            return 0;
        }
    }
}
