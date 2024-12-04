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
    /// The target string to search for <c>XMAS</c>.
    /// </summary>
    private const string XmasTarget = "XMAS";

    /// <summary>
    /// The search space for the <see cref="XmasTarget"/> string. This field is read-only.
    /// </summary>
    private static readonly SearchValues<string> XmasNeedle = SearchValues.Create([XmasTarget], StringComparison.Ordinal);

    /// <summary>
    /// Gets the count of the number of occurences of <c>XMAS</c> in the grid.
    /// </summary>
    public int SimpleCount { get; private set; }

    /// <summary>
    /// Gets the count of the number of occurences of <c>MAS</c>-crossed in the grid.
    /// </summary>
    public int CrossCount { get; private set; }

    /// <summary>
    /// Searches for the number of occurences of a value in the specified grid.
    /// </summary>
    /// <param name="grid">The word grid to search.</param>
    /// <param name="crossCount"><see langword="false"/> to count <c>XMAS</c>; otherwise <see langword="true"/> to count <c>MAS</c>-crossed.</param>
    /// <returns>
    /// The number of occurences of <c>XMAS</c> or <c>MAS</c>-crossed in the grid.
    /// </returns>
    public static int Search(IList<string> grid, bool crossCount)
        => crossCount ? CountCrossMas(grid) : CountXmas(grid);

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        SimpleCount = Search(values, crossCount: false);
        CrossCount = Search(values, crossCount: true);

        if (Verbose)
        {
            Logger.WriteLine("XMAS appears {0} times.", SimpleCount);
            Logger.WriteLine("MAS appears crossed {0} times.", CrossCount);
        }

        return PuzzleResult.Create(SimpleCount, CrossCount);
    }

    private static int CountXmas(IList<string> grid)
    {
        int height = grid.Count;

        var builder = new StringBuilder(height);

        List<string> slices = [];

        // Build the rows and columns
        for (int y = 0; y < height; y++)
        {
            // Add row
            string row = grid[y];

            slices.Add(row);
            slices.Add(row.Reverse());

            // Add column
            for (int x = 0; x < row.Length; x++)
            {
                builder.Append(grid[x][y]);
            }

            AddSlices(builder);
        }

        AddDiagonals(Index.FromStart(0), 1);
        AddDiagonals(Index.FromEnd(1), -1);

        int count = 0;

        foreach (ReadOnlySpan<char> slice in slices)
        {
            ReadOnlySpan<char> span = slice;

            int index;

            while ((index = span.IndexOfAny(XmasNeedle)) != -1)
            {
                count++;
                span = span[(index + 1)..];
            }
        }

        return count;

        void AddSlices(StringBuilder builder)
        {
            if (builder.Length >= XmasTarget.Length)
            {
                string value = builder.ToString();

                slices.Add(value);
                slices.Add(value.Reverse());
            }

            builder.Clear();
        }

        void AddDiagonals(Index originX, int deltaX)
        {
            int width;

            // Diagonals below and including the midpoint
            for (int y = 0; y < height; y++)
            {
                string row = grid[y];
                width = row.Length;

                builder.Append(row[originX]);

                for (int x = originX.GetOffset(width) + deltaX, δy = y + 1; x > -1 && x < width && δy < height; x += deltaX, δy++)
                {
                    builder.Append(grid[δy][x]);
                }

                AddSlices(builder);
            }

            // Diagonals above the midpoint
            string firstRow = grid[0];
            width = firstRow.Length;

            for (int x = originX.GetOffset(width) + deltaX; x > -1 && x < width; x += deltaX)
            {
                builder.Append(firstRow[x]);

                for (int y = 1, δx = x + deltaX; y < height && δx > -1 && δx < width; y++, δx += deltaX)
                {
                    builder.Append(grid[y][δx]);
                }

                AddSlices(builder);
            }
        }
    }

    private static int CountCrossMas(IList<string> grid)
    {
        int height = grid.Count;
        int width = grid[0].Length;

        int count = 0;

        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                if (HasCrossMas(x, y, grid))
                {
                    count++;
                }
            }
        }

        return count;

        static bool HasCrossMas(int x, int y, IList<string> grid)
        {
            if (grid[y][x] is not 'A')
            {
                return false;
            }

            int up = y - 1;
            int down = y + 1;
            int left = x - 1;
            int right = x + 1;

            // M.?    S.?
            // .A. or .A.
            // ?.S    ?.M
            if (!((grid[up][left] is 'M' && grid[down][right] is 'S') ||
                  (grid[up][left] is 'S' && grid[down][right] is 'M')))
            {
                return false;
            }

            // ?.S    ?.M
            // .A. or .A.
            // M.?    S.?
            return (grid[down][left] is 'M' && grid[up][right] is 'S') ||
                   (grid[down][left] is 'S' && grid[up][right] is 'M');
        }
    }
}
