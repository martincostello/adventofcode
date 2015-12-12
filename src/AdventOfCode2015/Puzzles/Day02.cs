// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Impl;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/day/2</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day02 : IPuzzle
    {
        /// <inheritdoc />
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
