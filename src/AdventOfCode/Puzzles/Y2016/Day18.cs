// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2016/day/18</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day18 : Puzzle2016
    {
        /// <summary>
        /// Gets the number of safe tiles in the puzzle input.
        /// </summary>
        public int SafeTileCount { get; private set; }

        /// <inheritdoc />
        protected override int MinimumArguments => 1;

        /// <summary>
        /// Finds the number of safe tiles from the specified map.
        /// </summary>
        /// <param name="firstRowTiles">The map of tiles in the first row.</param>
        /// <param name="rows">The number of rows.</param>
        /// <param name="logger">The logger to use.</param>
        /// <returns>
        /// The number of safe tiles in the map described by <paramref name="firstRowTiles"/>.
        /// </returns>
        internal static int FindSafeTileCount(
            string firstRowTiles,
            int rows,
            ILogger logger = null)
        {
            int width = firstRowTiles.Length;

            bool[,] tiles = new bool[rows, width];

            for (int x = 0; x < width; x++)
            {
                tiles[0, x] = firstRowTiles[x] == '^';
            }

            for (int y = 1; y < rows; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool left = x == 0 ? false : tiles[y - 1, x - 1];
                    bool right = x == width - 1 ? false : tiles[y - 1, x + 1];

                    bool isTrap = left ^ right;

                    tiles[y, x] = isTrap;
                }
            }

            logger?.WriteGrid(tiles, '.', '^');

            return (width * rows) - CountTrapTiles(tiles);
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            int rows = ParseInt32(args[0]);
            string firstRowTiles = args.Length > 1 ? args[1] : ReadResourceAsString().TrimEnd();

            SafeTileCount = FindSafeTileCount(firstRowTiles, rows, Logger);

            if (Verbose)
            {
                Logger.WriteLine($"The number of safe tiles is {SafeTileCount:N0}.");
            }

            return 0;
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
