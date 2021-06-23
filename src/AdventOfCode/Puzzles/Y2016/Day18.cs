// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2016/day/18</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2016, 18, MinimumArguments = 1, RequiresData = true)]
    public sealed class Day18 : Puzzle
    {
        /// <summary>
        /// Gets the number of safe tiles in the puzzle input.
        /// </summary>
        public int SafeTileCount { get; private set; }

        /// <summary>
        /// Finds the number of safe tiles from the specified map.
        /// </summary>
        /// <param name="firstRowTiles">The map of tiles in the first row.</param>
        /// <param name="rows">The number of rows.</param>
        /// <param name="logger">The logger to use.</param>
        /// <returns>
        /// The number of safe tiles in the map described by <paramref name="firstRowTiles"/>.
        /// </returns>
        internal static (int safeTileCount, string visualization) FindSafeTileCount(
            string firstRowTiles,
            int rows,
            ILogger logger)
        {
            int width = firstRowTiles.Length;

            bool[,] tiles = new bool[rows, width];

            for (int x = 0; x < width; x++)
            {
                if (firstRowTiles[x] == '^')
                {
                    tiles[0, x] = true;
                }
            }

            for (int y = 1; y < rows; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool left = x != 0 && tiles[y - 1, x - 1];
                    bool right = x != width - 1 && tiles[y - 1, x + 1];

                    bool isTrap = left ^ right;

                    if (isTrap)
                    {
                        tiles[y, x] = true;
                    }
                }
            }

            string visualization = logger.WriteGrid(tiles, '.', '^');

            return ((width * rows) - CountTrapTiles(tiles), visualization);
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            int rows = ParseInt32(args[0]);
            string firstRowTiles = args.Length > 1 ? args[1] : (await ReadResourceAsStringAsync()).TrimEnd();

            (int safeTileCount, string visualization) = FindSafeTileCount(firstRowTiles, rows, Logger);

            SafeTileCount = safeTileCount;

            if (Verbose)
            {
                Logger.WriteLine($"The number of safe tiles is {SafeTileCount:N0}.");
            }

            var result = new PuzzleResult();

            result.Solutions.Add(SafeTileCount);
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
        private static int CountTrapTiles(bool[,] grid) => grid.TrueCount();
    }
}
