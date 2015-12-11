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

namespace MartinCostello.AdventOfCode.Day11
{
    using System;

    /// <summary>
    /// A console application that solves <c>http://adventofcode.com/day/11</c>. This class cannot be inherited.
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
                Console.Error.WriteLine("No input value specified.");
                return -1;
            }

            string current = args[0];
            string next = GenerateNextPassword(current);

            Console.WriteLine("Santa's new password should be '{0}'.", next);

            return 0;
        }

        /// <summary>
        /// Generates the next password that should be used based on a current password value.
        /// </summary>
        /// <param name="current">The current password.</param>
        /// <returns>The next password.</returns>
        internal static string GenerateNextPassword(string current)
        {
            return current;
        }
    }
}
