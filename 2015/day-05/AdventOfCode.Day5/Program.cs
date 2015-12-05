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

namespace MartinCostello.AdventOfCode.Day5
{
    using System;
    using System.IO;

    /// <summary>
    /// A console application that solves <c>http://adventofcode.com/day/5</c>. This class cannot be inherited.
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
                Console.Error.WriteLine("No input file path and rule version specified.");
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
                Console.Error.WriteLine("The rules version specified is invalid.");
                return -1;
            }

            int niceStrings = 0;
            INicenessRule rule;

            if (version == 1)
            {
                rule = new NicenessRuleV1();
            }
            else
            {
                rule = new NicenessRuleV2();
            }

            foreach (string value in File.ReadLines(args[0]))
            {
                if (rule.IsNice(value))
                {
                    niceStrings++;
                }
            }

            Console.WriteLine("{0:N0} strings are nice using version {1} of the rules.", niceStrings, version);

            return 0;
        }
    }
}
