// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Buffers;

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/4</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 04, "Ceres Search", RequiresData = true)]
public sealed class Day04 : Puzzle
{
    /// <summary>
    /// The target string to search for.
    /// </summary>
    private const string Target = "XMAS";

    /// <summary>
    /// The search space for the target string. This field is read-only.
    /// </summary>
    private static readonly SearchValues<string> Needle = SearchValues.Create([Target], StringComparison.Ordinal);

    /// <summary>
    /// Gets the count of the number of occurences of <c>XMAS</c> in the grid.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Searches for the number of occurences of <c>XMAS</c> in the specified grid.
    /// </summary>
    /// <param name="grid">The word grid to search.</param>
    /// <returns>
    /// The number of occurences of <c>XMAS</c> in the grid.
    /// </returns>
    public static int Search(IList<string> grid)
    {
        int height = grid.Count;

        var builder = new StringBuilder(height);

        List<string> slices = [];

        // Build the rows and columns
        for (int y = 0; y < height; y++)
        {
            string row = grid[y];

            slices.Add(row);
            slices.Add(row.Reverse());

            for (int x = 0; x < row.Length; x++)
            {
                builder.Append(grid[x][y]);
            }

            AddSlices(builder);
        }

        // Diagonals down and right
        for (int y = 0; y < height; y++)
        {
            int width = grid[y].Length;

            builder.Append(grid[y][0]);

            for (int x = 1, δy = y + 1; x < width && δy < height; x++, δy++)
            {
                builder.Append(grid[δy][x]);
            }

            AddSlices(builder);
        }

        string firstRow = grid[0];

        for (int x = 1; x < firstRow.Length; x++)
        {
            builder.Append(firstRow[x]);

            for (int y = 1, δx = x + 1; y < height && δx < firstRow.Length; y++, δx++)
            {
                builder.Append(grid[y][δx]);
            }

            AddSlices(builder);
        }

        // Diagonals down and left
        for (int y = 0; y < height; y++)
        {
            int width = grid[y].Length;

            builder.Append(grid[y][width - 1]);

            for (int x = width - 2, δy = y + 1; x > -1 && δy < height; x--, δy++)
            {
                builder.Append(grid[δy][x]);
            }

            AddSlices(builder);
        }

        for (int x = firstRow.Length - 2; x > -1; x--)
        {
            builder.Append(firstRow[x]);

            for (int y = 1, δx = x - 1; δx > -1 && y < height; y++, δx--)
            {
                builder.Append(grid[y][δx]);
            }

            AddSlices(builder);
        }

        int count = 0;

        foreach (ReadOnlySpan<char> slice in slices)
        {
            ReadOnlySpan<char> span = slice;

            int index;

            while ((index = span.IndexOfAny(Needle)) != -1)
            {
                count++;
                span = span[(index + 1)..];
            }
        }

        return count;

        void AddSlices(StringBuilder builder)
        {
            string value = builder.ToString();

            slices.Add(value);
            slices.Add(value.Reverse());

            builder.Clear();
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        Count = Search(values);

        if (Verbose)
        {
            Logger.WriteLine("XMAS appears {0} times.", Count);
        }

        return PuzzleResult.Create(Count);
    }
}
