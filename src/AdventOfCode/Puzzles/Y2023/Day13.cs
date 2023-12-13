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
    /// Gets the number after summarizing all of the notes with all the smudges cleaned.
    /// </summary>
    public int SummaryWithSmudgesCleaned { get; private set; }

    /// <summary>
    /// Finds the lines of symmetry for the specified mirrors.
    /// </summary>
    /// <param name="notes">The notes containing the mirrors.</param>
    /// <param name="cleanSmudges">Whether to clean the smudges in the mirrors.</param>
    /// <returns>
    /// The number after summarizing all of the notes.
    /// </returns>
    public static int Summarize(IList<string> notes, bool cleanSmudges)
    {
        var mirrors = notes.ToArray().AsSpan();

        int desiredDifferences = cleanSmudges ? 1 : 0;
        int sum = 0;

        while (!mirrors.IsEmpty)
        {
            int length = mirrors.IndexOf(string.Empty);

            if (length == -1)
            {
                length = mirrors.Length;
            }

            var mirror = mirrors[..length];
            mirrors = mirrors[Math.Min(mirrors.Length, length + 1)..];

            var bounds = new Size(mirror[0].Length, mirror.Length);

            sum += FindSymmetry(mirror, bounds, desiredDifferences);
        }

        return sum;

        static int FindSymmetry(ReadOnlySpan<string> mirrors, Size bounds, int desiredDifferences)
        {
            for (int x = 1; x < bounds.Width; x++)
            {
                int differences = 0;

                for (int leftX = x - 1, rightX = x; leftX > -1 && rightX < bounds.Width; leftX--, rightX++)
                {
                    differences += CompareVertical(mirrors, leftX, rightX, bounds.Height);
                }

                if (differences == desiredDifferences)
                {
                    return x;
                }
            }

            for (int y = 1; y < bounds.Height; y++)
            {
                int differences = 0;

                for (int leftY = y - 1, rightY = y; leftY > -1 && rightY < bounds.Height; leftY--, rightY++)
                {
                    differences += CompareHorizontal(mirrors, leftY, rightY, bounds.Width);
                }

                if (differences == desiredDifferences)
                {
                    return y * 100;
                }
            }

            throw new PuzzleException("Failed to find any symmetry for mirror.");

            static int CompareHorizontal(ReadOnlySpan<string> mirrors, int topY, int bottomY, int width)
            {
                int differences = 0;
                int x = 0;

                var first = mirrors[topY].AsSpan();
                var second = mirrors[bottomY].AsSpan();

                while (x < width)
                {
                    if (first[x] != second[x++])
                    {
                        differences++;
                    }
                }

                return differences;
            }

            static int CompareVertical(ReadOnlySpan<string> mirrors, int leftX, int rightX, int height)
            {
                int differences = 0;
                int y = 0;

                while (y < height)
                {
                    if (mirrors[y][leftX] != mirrors[y++][rightX])
                    {
                        differences++;
                    }
                }

                return differences;
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        Summary = Summarize(values, cleanSmudges: false);
        SummaryWithSmudgesCleaned = Summarize(values, cleanSmudges: true);

        if (Verbose)
        {
            Logger.WriteLine("The number after summarizing all of the notes is {0}.", Summary);
            Logger.WriteLine("The number after summarizing all of the notes after cleaning the smudges is {0}.", SummaryWithSmudgesCleaned);
        }

        return PuzzleResult.Create(Summary, SummaryWithSmudgesCleaned);
    }
}
