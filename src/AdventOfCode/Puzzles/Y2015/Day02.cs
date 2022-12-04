// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/2</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2015, 02, "I Was Told There Would Be No Math", RequiresData = true)]
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
    internal static (int Area, int Length) GetTotalWrappingPaperAreaAndRibbonLength(IEnumerable<string> dimensions)
    {
        // Parse the dimensions of the presents
        var presents = dimensions
            .Select(Present.Parse)
            .ToList();

        // Determine the total area of wrapping paper required and the amount of ribbon
        int totalArea = presents.Sum((p) => p.GetWrappingPaperArea());
        int length = presents.Sum((p) => p.GetRibbonLength());

        return (totalArea, length);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var dimensions = await ReadResourceAsLinesAsync();

        (TotalAreaOfPaper, TotalLengthOfRibbon) = GetTotalWrappingPaperAreaAndRibbonLength(dimensions);

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
    /// Represents a Christmas present.
    /// </summary>
    private readonly struct Present
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Present"/> struct.
        /// </summary>
        /// <param name="length">The length of the present.</param>
        /// <param name="width">The width of the present.</param>
        /// <param name="height">The height of the present.</param>
        private Present(int length, int width, int height)
        {
            Length = length;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Gets the length of the present.
        /// </summary>
        public readonly int Length { get; }

        /// <summary>
        /// Gets the width of the present.
        /// </summary>
        public readonly int Width { get; }

        /// <summary>
        /// Gets the height of the present.
        /// </summary>
        public readonly int Height { get; }

        /// <summary>
        /// Gets the volume of the present.
        /// </summary>
        public int Volume => Height * Length * Width;

        /// <summary>
        /// Parses the specified <see cref="string"/> to an instance of <see cref="Present"/>.
        /// </summary>
        /// <param name="value">The present to parse.</param>
        /// <returns>The result of parsing <paramref name="value"/>.</returns>
        public static Present Parse(string value)
        {
            (int length, int width, int height) = value.AsNumberTriple<int>('x');
            return new Present(length, width, height);
        }

        /// <summary>
        /// Returns the length of ribbon required to wrap this present.
        /// </summary>
        /// <returns>The length of ribbon, in feet, required to wrap this present.</returns>
        public int GetRibbonLength()
        {
            int smallestPerimeter = Math.Min((Length + Width) * 2, (Width + Height) * 2);
            smallestPerimeter = Math.Min(smallestPerimeter, (Height + Length) * 2);

            return smallestPerimeter + Volume;
        }

        /// <summary>
        /// Returns the area of wrapping paper required to wrap this present.
        /// </summary>
        /// <returns>The area of wrapping paper, in square feet, required to wrap this present.</returns>
        public int GetWrappingPaperArea()
        {
            int surfaceArea =
                (2 * Length * Width) +
                (2 * Width * Height) +
                (2 * Height * Length);

            int extra = Math.Min(Length * Width, Width * Height);
            extra = Math.Min(extra, Height * Length);

            return surfaceArea + extra;
        }
    }
}
