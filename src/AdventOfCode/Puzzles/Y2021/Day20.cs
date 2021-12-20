// Copyright (c) Martin Costello, 2015. All rights reserved.
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
    /// <param name="image">The image data to enhance.</param>
    /// <param name="enhancements">The number of times to enhance the image.</param>
    /// <param name="logger">The optional logger to use.</param>
    /// <returns>
    /// The number of lit pixels in the image after the specified number
    /// of enhancements and a visualization of the image.
    /// </returns>
    public static (int LitPixelCount, string Visualization) Enhance(
        IList<string> image,
        int enhancements,
        ILogger? logger = null)
    {
        string algorithm = image[0];
        var current = new HashSet<Point>();

        for (int y = 2; y < image.Count; y++)
        {
            string row = image[y];

            for (int x = 0; x < row.Length; x++)
            {
                if (row[x] == '#')
                {
                    current.Add(new(x, y - 2));
                }
            }
        }

        bool[] bits = new bool[9];

        for (int i = 0; i < enhancements; i++)
        {
            int minX = current.MinBy((p) => p.X).X - 1;
            int maxX = current.MaxBy((p) => p.X).X + 1;

            int minY = current.MinBy((p) => p.Y).Y - 1;
            int maxY = current.MaxBy((p) => p.Y).Y + 1;

            var next = new HashSet<Point>();

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var pixel = new Point(x, y);

                    int j = 0;

                    foreach (Point neighbor in pixel.Neighbors(includeSelf: true))
                    {
                        bits[j++] = current.Contains(neighbor);
                    }

                    int index = ReadInteger(bits);
                    bool lit = algorithm[index] == '#';

                    if (lit)
                    {
                        next.Add(pixel);
                    }
                }
            }

            current = next;
        }

        string vizualization = string.Empty;

        if (current.Count > 1)
        {
            int minX = current.MinBy((p) => p.X).X - 1;
            int maxX = current.MaxBy((p) => p.X).X + 1;

            int minY = current.MinBy((p) => p.Y).Y - 1;
            int maxY = current.MaxBy((p) => p.Y).Y + 1;

            var builder = new StringBuilder((maxX - minX) * (maxY - minY));

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    builder.Append(current.Contains(new(x, y)) ? '#' : '.');
                }

                builder.AppendLine();
            }

            vizualization = builder.ToString();
            logger?.WriteLine(vizualization);
        }

        return (current.Count, vizualization);

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
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> image = await ReadResourceAsLinesAsync();

        (LitPixelCount, string visualization) = Enhance(image, enhancements: 2, Logger);

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
