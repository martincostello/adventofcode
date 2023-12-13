// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/13</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 13, "Point of Incidence", RequiresData = true)]
public sealed class Day13 : Puzzle
{
    /// <summary>
    /// Gets the number after summarizing all of the notes.
    /// </summary>
    public int Summary { get; private set; }

    /// <summary>
    /// Finds the lines of symmetry for the specified mirrors.
    /// </summary>
    /// <param name="notes">The notes containing the mirrors.</param>
    /// <returns>
    /// The number after summarizing all of the notes.
    /// </returns>
    public static int Summarize(IList<string> notes)
    {
        var mirrors = notes.ToArray().AsSpan();
        int sum = 0;

        while (!mirrors.IsEmpty)
        {
            int length = mirrors.IndexOf(string.Empty);

            if (length == -1)
            {
                length = mirrors.Length;
            }

            var pattern = mirrors[..length];
            mirrors = mirrors[Math.Min(mirrors.Length, length + 1)..];

            var bounds = new Rectangle(0, 0, pattern[0].Length, pattern.Length);

            int symmetry = FindSymmetry(pattern, bounds);
            sum += symmetry;
        }

        return sum;

        static int FindSymmetry(ReadOnlySpan<string> pattern, Rectangle bounds)
        {
            for (int x = 1; x < bounds.Width; x++)
            {
                bool symmetric = true;

                for (int leftX = x - 1, rightX = x; symmetric && leftX > -1 && rightX < bounds.Width; leftX--, rightX++)
                {
                    symmetric &= HasVerticalSymmetry(pattern, leftX, rightX, bounds);
                }

                if (symmetric)
                {
                    return x;
                }
            }

            for (int y = 1; y < bounds.Height; y++)
            {
                bool symmetric = true;

                for (int leftY = y - 1, rightY = y; symmetric && leftY > -1 && rightY < bounds.Height; leftY--, rightY++)
                {
                    symmetric &= HasHorizontalSymmetry(pattern, leftY, rightY, bounds);
                }

                if (symmetric)
                {
                    return y * 100;
                }
            }

            throw new PuzzleException("Failed to find any symmetry.");

            static bool HasHorizontalSymmetry(ReadOnlySpan<string> pattern, int topY, int bottomY, Rectangle bounds)
            {
                int count = 0;
                int x = 0;

                while (x < bounds.Width && pattern[topY][x] == pattern[bottomY][x++])
                {
                    count++;
                }

                return count == bounds.Width;
            }

            static bool HasVerticalSymmetry(ReadOnlySpan<string> pattern, int leftX, int rightX, Rectangle bounds)
            {
                int count = 0;
                int y = 0;

                while (y < bounds.Height && pattern[y][leftX] == pattern[y++][rightX])
                {
                    count++;
                }

                return count == bounds.Height;
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        Summary = Summarize(values);

        if (Verbose)
        {
            Logger.WriteLine("The number after summarizing all of the notes is {0}.", Summary);
        }

        return PuzzleResult.Create(Summary);
    }
}
