// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2015/day/2</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2015, 02, RequiresData = true)]
    public sealed class Day02 : Puzzle
    {
        /// <summary>
        /// Gets the total amount of wrapping paper required in square feet.
        /// </summary>
        internal int TotalAreaOfPaper { get; private set; }

        /// <summary>
        /// Gets the total length of ribbon required in feet.
        /// </summary>
        internal int TotalLengthOfRibbon { get; private set; }

        /// <summary>
        /// Gets the total area of wrapping paper and length of ribbon required to
        /// wrap the presents of the specified dimensions.
        /// </summary>
        /// <param name="dimensions">The dimensions of the presents to wrap.</param>
        /// <returns>
        /// A named tuple that returns the total area of wrapping paper
        /// required in square feet and the total length of ribbon required in feet.
        /// </returns>
        internal static (int area, int length) GetTotalWrappingPaperAreaAndRibbonLength(IEnumerable<string> dimensions)
        {
            // Parse the dimensions of the presents
            var presents = dimensions
                .Select(Present.Parse)
                .ToList();

            // Determine the total area of wrapping paper required and the amount of ribbon
            int totalArea = presents.Sum(GetWrappingPaperArea);
            int length = presents.Sum(GetRibbonLength);

            return (totalArea, length);
        }

        /// <inheritdoc />
        protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            (TotalAreaOfPaper, TotalLengthOfRibbon) = GetTotalWrappingPaperAreaAndRibbonLength(ReadResourceAsLines());

            if (Verbose)
            {
                Logger.WriteLine(
                    "The elves should order {0:N0} square feet of wrapping paper.{1}They also need {2:N0} feet of ribbon.",
                    TotalAreaOfPaper,
                    Environment.NewLine,
                    TotalLengthOfRibbon);
            }

            return PuzzleResult.Create(TotalAreaOfPaper, TotalLengthOfRibbon);
        }

        /// <summary>
        /// Returns the length of ribbon required to wrap the specified present.
        /// </summary>
        /// <param name="present">The present to calculate the required length of ribbon for.</param>
        /// <returns>The length of ribbon, in feet, required to wrap the present.</returns>
        private static int GetRibbonLength(Present present)
        {
            int[] perimetersOfSides = new[]
            {
                (present.Length + present.Width) * 2,
                (present.Width + present.Height) * 2,
                (present.Height + present.Length) * 2,
            };

            int smallestPerimeter = perimetersOfSides.Min();

            int lengthForBow = present.Height * present.Length * present.Width;

            return smallestPerimeter + lengthForBow;
        }

        /// <summary>
        /// Returns the area of wrapping paper required to wrap the specified present.
        /// </summary>
        /// <param name="present">The present to calculate the required area of wrapping paper for.</param>
        /// <returns>The area of wrapping paper, in square feet, required to wrap the present.</returns>
        private static int GetWrappingPaperArea(Present present)
        {
            int surfaceArea =
                (2 * present.Length * present.Width) +
                (2 * present.Width * present.Height) +
                (2 * present.Height * present.Length);

            int[] areasOfSides = new[]
            {
                present.Length * present.Width,
                present.Width * present.Height,
                present.Height * present.Length,
            };

            int extra = areasOfSides.Min();

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

                int length = ParseInt32(split[0]);
                int width = ParseInt32(split[1]);
                int height = ParseInt32(split[2]);

                return new Present(length, width, height);
            }
        }
    }
}
