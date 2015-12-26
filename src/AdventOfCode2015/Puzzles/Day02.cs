// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/day/2</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day02 : IPuzzle
    {
        /// <summary>
        /// Gets the total amount of wrapping paper required in square feet.
        /// </summary>
        internal int TotalAreaOfPaper { get; private set; }

        /// <summary>
        /// Gets the total length of ribbon required in feet.
        /// </summary>
        internal int TotalLengthOfRibbon { get; private set; }

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

            Tuple<int, int> result = GetTotalWrappingPaperAreaAndRibbonLength(File.ReadLines(args[0]));

            TotalAreaOfPaper = result.Item1;
            TotalLengthOfRibbon = result.Item2;

            Console.WriteLine(
                "The elves should order {0:N0} square feet of wrapping paper.{1}They also need {2:N0} feet of ribbon.",
                TotalAreaOfPaper,
                Environment.NewLine,
                TotalLengthOfRibbon);

            return 0;
        }

        /// <summary>
        /// Gets the total area of wrapping paper and length of ribbon required to
        /// wrap the presents of the specified dimensions.
        /// </summary>
        /// <param name="dimensions">The dimensions of the presents to wrap.</param>
        /// <returns>
        /// A <see cref="Tuple{T1, T2}"/> that returns the total area of wrapping paper
        /// required in square feet and the total length of ribbon required in feet.
        /// </returns>
        internal static Tuple<int, int> GetTotalWrappingPaperAreaAndRibbonLength(IEnumerable<string> dimensions)
        {
            // Read in the dimensions of the presents from the specified input file
            List<Present> presents = new List<Present>();

            foreach (string line in dimensions)
            {
                presents.Add(Present.Parse(line));
            }

            // Determine the total area of wrapping paper required and the amount of ribbon
            int totalArea = presents.Sum(GetWrappingPaperArea);
            int length = presents.Sum(GetRibbonLength);

            return Tuple.Create(totalArea, length);
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

        /// <summary>
        /// A class representing a Christmas present. This class cannot be inherited.
        /// </summary>
        internal sealed class Present
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Present"/> class.
            /// </summary>
            /// <param name="length">The length of the present.</param>
            /// <param name="width">The width of the present.</param>
            /// <param name="height">The height of the present.</param>
            internal Present(int length, int width, int height)
            {
                Length = length;
                Width = width;
                Height = height;
            }

            /// <summary>
            /// Gets the length of the present.
            /// </summary>
            internal int Length { get; }

            /// <summary>
            /// Gets the width of the present.
            /// </summary>
            internal int Width { get; }

            /// <summary>
            /// Gets the height of the present.
            /// </summary>
            internal int Height { get; }

            /// <summary>
            /// Parses the specified <see cref="string"/> to an instance of <see cref="Present"/>.
            /// </summary>
            /// <param name="value">The present to parse.</param>
            /// <returns>The result of parsing <paramref name="value"/>.</returns>
            internal static Present Parse(string value)
            {
                string[] split = value.Split('x');

                int length = int.Parse(split[0], NumberStyles.Integer, CultureInfo.InvariantCulture);
                int width = int.Parse(split[1], NumberStyles.Integer, CultureInfo.InvariantCulture);
                int height = int.Parse(split[2], NumberStyles.Integer, CultureInfo.InvariantCulture);

                return new Present(length, width, height);
            }
        }
    }
}
