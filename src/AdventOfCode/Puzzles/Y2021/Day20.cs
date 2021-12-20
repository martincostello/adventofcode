﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/20</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 20, "Trench Map", RequiresData = true)]
public sealed class Day20 : Puzzle
{
    /// <summary>
    /// Gets the number of lit pixels after the image is enhanced twice.
    /// </summary>
    public int LitPixelCount { get; private set; }

    /// <summary>
    /// Enhances an image the specified number of times.
    /// </summary>
    /// <param name="imageData">The image data to enhance.</param>
    /// <param name="enhancements">The number of times to enhance the image.</param>
    /// <param name="logger">The optional logger to use.</param>
    /// <returns>
    /// The number of lit pixels in the image after the specified number
    /// of enhancements and a visualization of the image.
    /// </returns>
    public static (int LitPixelCount, string Visualization) Enhance(
        IList<string> imageData,
        int enhancements,
        ILogger? logger = null)
    {
        const char LitCharacter = '#';
        const char UnlitCharacter = '.';

        string algorithm = imageData[0];
        var image = new Dictionary<Point, bool>((imageData.Count - 2) * imageData[2].Length);

        for (int y = 2; y < imageData.Count; y++)
        {
            string row = imageData[y];

            for (int x = 0; x < row.Length; x++)
            {
                image.Add(new(x, y - 2), row[x] == LitCharacter);
            }
        }

        bool[] indexBits = new bool[9];
        bool isFirstBitLit = algorithm[0] == LitCharacter;

        for (int i = 0; i < enhancements; i++)
        {
            int minX = image.Keys.Min((p) => p.X) - 1;
            int maxX = image.Keys.Max((p) => p.X) + 1;

            int minY = image.Keys.Min((p) => p.Y) - 1;
            int maxY = image.Keys.Max((p) => p.Y) + 1;

            // If bit 0 is lit in the algorithm, then every other iteration
            // of enhancement will set the pixels outside the bounds of the
            // known pixels to be lit because the value of `00000000` is zero,
            // meaning that all of the "infinite" bits will change to being lit.
            bool outOfBoundsValue = isFirstBitLit && i % 2 != 0;

            var next = new Dictionary<Point, bool>(image.Count);

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var pixel = new Point(x, y);

                    int j = 0;

                    foreach (Point neighbor in pixel.Neighbors(includeSelf: true))
                    {
                        indexBits[j++] = image.GetValueOrDefault(neighbor, outOfBoundsValue);
                    }

                    int index = ReadInteger(indexBits);
                    next[pixel] = algorithm[index] == LitCharacter;
                }
            }

            image = next;
        }

        string visualization = BuildVisualization(image);
        logger?.WriteLine(visualization);

        return (image.Values.Count((p) => p), visualization);

        static int ReadInteger(ReadOnlySpan<bool> bits)
        {
            int value = 0;

            for (int i = 0; i < bits.Length; i++)
            {
                SetBit(ref value, i, bits[^(i + 1)] ? 1 : 0);
            }

            return value;
        }

        static void SetBit(ref int reference, int bit, int value)
            => reference |= value << bit;

        static string BuildVisualization(Dictionary<Point, bool> image)
        {
            int minX = image.Keys.Min((p) => p.X) - 1;
            int maxX = image.Keys.Max((p) => p.X) + 1;

            int minY = image.Keys.Min((p) => p.Y) - 1;
            int maxY = image.Keys.Max((p) => p.Y) + 1;

            int deltaX = maxX - minX;
            int deltaY = maxY - minY;

            var builder = new StringBuilder((deltaX * deltaY) + (Environment.NewLine.Length * deltaY));

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    builder.Append(image.GetValueOrDefault(new(x, y)) ? LitCharacter : UnlitCharacter);
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> imageData = await ReadResourceAsLinesAsync();

        (LitPixelCount, string visualization) = Enhance(imageData, enhancements: 2, Logger);

        if (Verbose)
        {
            Logger.WriteLine("There are {0:N0} lit pixels after two enhancements of the image.", LitPixelCount);
        }

        var result = new PuzzleResult();

        result.Solutions.Add(LitPixelCount);
        result.Visualizations.Add(visualization);

        return result;
    }
}
