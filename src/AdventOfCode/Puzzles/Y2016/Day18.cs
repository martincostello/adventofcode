// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/18</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2016, 18, "Like a Rogue", RequiresData = true, IsSlow = true)]
public sealed class Day18 : Puzzle
{
    /// <summary>
    /// Gets the number of safe tiles in the puzzle input for 40 rows.
    /// </summary>
    public int SafeTileCount40 { get; private set; }

    /// <summary>
    /// Gets the number of safe tiles in the puzzle input for 400,000 rows.
    /// </summary>
    public int SafeTileCount400000 { get; private set; }

    /// <summary>
    /// Finds the number of safe tiles from the specified map.
    /// </summary>
    /// <param name="firstRowTiles">The map of tiles in the first row.</param>
    /// <param name="rows">The number of rows.</param>
    /// <param name="logger">The logger to use.</param>
    /// <returns>
    /// The number of safe tiles in the map described by <paramref name="firstRowTiles"/>.
    /// </returns>
    internal static (int SafeTileCount, string Visualization) FindSafeTileCount(
        string firstRowTiles,
        int rows,
        ILogger logger)
    {
        int width = firstRowTiles.Length;

        bool[,] tiles = new bool[width, rows];

        for (int x = 0; x < width; x++)
        {
            if (firstRowTiles[x] == '^')
            {
                tiles[x, 0] = true;
            }
        }

        for (int y = 1; y < rows; y++)
        {
            for (int x = 0; x < width; x++)
            {
                bool left = x != 0 && tiles[x - 1, y - 1];
                bool right = x != width - 1 && tiles[x + 1, y - 1];

                bool isTrap = left ^ right;

                if (isTrap)
                {
                    tiles[x, y] = true;
                }
            }
        }

        string visualization = string.Empty;

        if (rows <= 75)
        {
            visualization = logger.WriteGrid(tiles, '.', '^');
        }

        return ((width * rows) - CountTrapTiles(tiles), visualization);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        string firstRowTiles = (await ReadResourceAsStringAsync()).TrimEnd();

        (SafeTileCount40, string visualization) = FindSafeTileCount(firstRowTiles, rows: 40, Logger);
        (SafeTileCount400000, _) = FindSafeTileCount(firstRowTiles, rows: 400000, Logger);

        if (Verbose)
        {
            Logger.WriteLine($"The number of safe tiles with 40 rows is {SafeTileCount40:N0}.");
            Logger.WriteLine($"The number of safe tiles with 400,000 rows is {SafeTileCount400000:N0}.");
        }

        var result = new PuzzleResult();

        result.Solutions.Add(SafeTileCount40);
        result.Solutions.Add(SafeTileCount400000);
        result.Visualizations.Add(visualization);

        return result;
    }

    /// <summary>
    /// Counts the number of safe tiles in the specified grid.
    /// </summary>
    /// <param name="grid">The grid to count the number of safe tiles in.</param>
    /// <returns>
    /// The number of safe tiles in <paramref name="grid"/>.
    /// </returns>
    private static int CountTrapTiles(bool[,] grid) => grid.Count(true);
}
