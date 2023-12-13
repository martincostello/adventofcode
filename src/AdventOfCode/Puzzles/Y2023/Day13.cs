// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/13</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 13, "Point of Incidence", RequiresData = true)]
public sealed class Day13 : Puzzle
{
    private static readonly (int Columns, int Rows) None = (-1, -1);

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

            var (columns, rows) = cleanSmudges ? FindSymmetryWithSmudge(mirror, bounds) : FindSymmetry(mirror, bounds);

            sum += columns;
            sum += rows * 100;
        }

        return sum;

        static (int Columns, int Rows) FindSymmetryWithSmudge(ReadOnlySpan<string> mirror, Size bounds)
        {
            var original = FindSymmetry(mirror, bounds);
            var none = None;

            for (int y = 0; y < bounds.Height; y++)
            {
                for (int x = 0; x < bounds.Width; x++)
                {
                    var cleaned = Clean(mirror, new(x, y));
                    var symmetry = FindSymmetry(cleaned, bounds);

                    if (symmetry != none && symmetry != original)
                    {
                        if (original.Columns == symmetry.Columns)
                        {
                            return (0, symmetry.Rows);
                        }
                        else
                        {
                            return (symmetry.Columns, 0);
                        }
                    }
                }
            }

            throw new PuzzleException("Failed to find any symmetry for mirror.");

            static ReadOnlySpan<string> Clean(ReadOnlySpan<string> mirror, Point location)
            {
                const char Ash = '.';
                const char Rock = '#';

                string[] cleaned = new string[mirror.Length];

                if (location.Y > 0)
                {
                    mirror[..location.Y].CopyTo(cleaned);
                }

                int rest = location.Y + 1;

                if (rest < mirror.Length)
                {
                    mirror[rest..].CopyTo(cleaned.AsSpan(rest));
                }

                string dirty = mirror[location.Y];
                cleaned[location.Y] = string.Create(dirty.Length, (dirty, location), static (span, state) =>
                {
                    state.dirty.CopyTo(span);
                    span[state.location.X] = state.dirty[state.location.X] == Ash ? Rock : Ash;
                });

                return cleaned;
            }
        }

        static (int Columns, int Rows) FindSymmetry(ReadOnlySpan<string> mirrors, Size bounds)
        {
            int columns = 0;
            int rows = 0;

            bool found = false;

            for (int x = 1; x < bounds.Width; x++)
            {
                bool symmetric = true;

                for (int leftX = x - 1, rightX = x; symmetric && leftX > -1 && rightX < bounds.Width; leftX--, rightX++)
                {
                    symmetric &= HasVerticalSymmetry(mirrors, leftX, rightX, bounds);
                }

                if (symmetric)
                {
                    columns = x;
                    found = true;
                    break;
                }
            }

            for (int y = 1; y < bounds.Height; y++)
            {
                bool symmetric = true;

                for (int leftY = y - 1, rightY = y; symmetric && leftY > -1 && rightY < bounds.Height; leftY--, rightY++)
                {
                    symmetric &= HasHorizontalSymmetry(mirrors, leftY, rightY, bounds);
                }

                if (symmetric)
                {
                    rows = y;
                    found = true;
                    break;
                }
            }

            return found ? (columns, rows) : None;

            static bool HasHorizontalSymmetry(ReadOnlySpan<string> mirrors, int topY, int bottomY, Size bounds)
            {
                int count = 0;
                int x = 0;

                var first = mirrors[topY].AsSpan();
                var second = mirrors[bottomY].AsSpan();

                while (x < bounds.Width && first[x] == second[x++])
                {
                    count++;
                }

                return count == bounds.Width;
            }

            static bool HasVerticalSymmetry(ReadOnlySpan<string> mirrors, int leftX, int rightX, Size bounds)
            {
                int count = 0;
                int y = 0;

                while (y < bounds.Height && mirrors[y][leftX] == mirrors[y++][rightX])
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
