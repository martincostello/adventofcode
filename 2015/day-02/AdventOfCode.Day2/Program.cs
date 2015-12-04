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

namespace MartinCostello.AdventOfCode.Day2
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// A console application that solves <c>http://adventofcode.com/day/2</c>. This class cannot be inherited.
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

            // Read in the dimensions of the presents from the specified input file
            List<Present> presents = new List<Present>();

            foreach (string line in File.ReadLines(args[0]))
            {
                presents.Add(Present.Parse(line));
            }

            // Determine the total area of wrapping paper required and the amount of ribbon
            int totalArea = presents.Sum(GetWrappingPaperArea);
            int length = presents.Sum(GetRibbonLength);

            Console.WriteLine(
                "The elves should order {0:N0} square feet of wrapping paper.{1}They also need {2:N0} feet of ribbon.",
                totalArea,
                Environment.NewLine,
                length);

            return 0;
        }

        /// <summary>
        /// Returns the length of ribbon required to wrap the specified present.
        /// </summary>
        /// <param name="present">The present to calculate the required length of ribbon for.</param>
        /// <returns>The length of ribbon, in feet, required to wrap the present.</returns>
        private static int GetRibbonLength(Present present)
        {
            int smallestPerimiter = new[] { (present.Length + present.Width) * 2, (present.Width + present.Height) * 2, (present.Height + present.Length) * 2 }.Min();
            int lengthForBow = present.Height * present.Length * present.Width;

            return smallestPerimiter + lengthForBow;
        }

        /// <summary>
        /// Returns the area of wrapping paper required to wrap the specified present.
        /// </summary>
        /// <param name="present">The present to calculate the required area of wrapping paper for.</param>
        /// <returns>The area of wrapping paper, in square feet, required to wrap the present.</returns>
        private static int GetWrappingPaperArea(Present present)
        {
            int surfaceArea = (2 * present.Length * present.Width) + (2 * present.Width * present.Height) + (2 * present.Height * present.Length);
            int extra = new[] { present.Length * present.Width, present.Width * present.Height, present.Height * present.Length }.Min();

            return surfaceArea + extra;
        }
    }
}
