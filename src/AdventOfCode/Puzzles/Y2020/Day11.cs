// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/11</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2020, 11, RequiresData = true)]
    public sealed class Day11 : Puzzle
    {
        /// <summary>
        /// An empty chair.
        /// </summary>
        private const char Empty = 'L';

        /// <summary>
        /// An occupied chair.
        /// </summary>
        private const char Occupied = '#';

        /// <summary>
        /// Gets the number of occupied seats using version 1 of the rules.
        /// </summary>
        public int OccupiedSeatsV1 { get; private set; }

        /// <summary>
        /// Gets the number of occupied seats using version 2 of the rules.
        /// </summary>
        public int OccupiedSeatsV2 { get; private set; }

        /// <summary>
        /// Gets the number of occupied seats for the specified layout.
        /// </summary>
        /// <param name="layout">The seat layout.</param>
        /// <param name="version">The version of the rules to use.</param>
        /// <param name="logger">The optional logger to use.</param>
        /// <returns>
        /// The number of occupied seats in the layout and a visualization of the final seat layout.
        /// </returns>
        public static (int occupiedSeats, string visualization) GetOccupiedSeats(
            IList<string> layout,
            int version,
            ILogger? logger = null)
        {
            var initial = layout
                .Select((p) => p.ToCharArray())
                .ToList();

            var previous = initial;
            var current = initial;

            string visualization = WriteGrid(current, logger);

            while ((current = Iterate(current, version)) != previous)
            {
                visualization = WriteGrid(current, logger);
                previous = current;
            }

            int occupiedSeats = current
                .SelectMany((p) => p)
                .Where((p) => p == Occupied)
                .Count();

            return (occupiedSeats, visualization);
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> layout = await ReadResourceAsLinesAsync();

            (int occupiedSeatsV1, string visualizationV1) = GetOccupiedSeats(layout, version: 1);
            (int occupiedSeatsV2, string visualizationV2) = GetOccupiedSeats(layout, version: 2);

            OccupiedSeatsV1 = occupiedSeatsV1;
            OccupiedSeatsV2 = occupiedSeatsV2;

            if (Verbose)
            {
                Logger.WriteLine("There are {0} occupied seats using the first set of rules.", OccupiedSeatsV1);
                Logger.WriteLine("There are {0} occupied seats using the second set of rules.", OccupiedSeatsV2);
            }

            var result = new PuzzleResult();

            result.Solutions.Add(OccupiedSeatsV1);
            result.Visualizations.Add(visualizationV1);

            result.Solutions.Add(OccupiedSeatsV2);
            result.Visualizations.Add(visualizationV2);

            return result;
        }

        /// <summary>
        /// Iterates the seat layout according to the rules.
        /// </summary>
        /// <param name="layout">The seat layout to iterate.</param>
        /// <param name="version">The version of the rules to use.</param>
        /// <returns>
        /// <paramref name="layout"/> if the seat layout did not change;
        /// otherwise a new value containing the new seat layout.
        /// </returns>
        private static List<char[]> Iterate(List<char[]> layout, int version)
        {
            var updated = new List<char[]>();

            foreach (char[] row in layout)
            {
                updated.Add((char[])row.Clone());
            }

            bool changed = false;

            int occupiedTolerance = version == 1 ? 4 : 5;

            int height = layout.Count;
            int width = layout[0].Length;

            for (int y = 0; y < height; y++)
            {
                char[] row = layout[y];

                for (int x = 0; x < width; x++)
                {
                    char seat = row[x];

                    if (seat == Empty && CountAdjacentSeats(x, y, version) == 0)
                    {
                        updated[y][x] = Occupied;
                        changed = true;
                    }
                    else if (seat == Occupied && CountAdjacentSeats(x, y, version) >= occupiedTolerance)
                    {
                        updated[y][x] = Empty;
                        changed = true;
                    }
                }
            }

            return changed ? updated : layout;

            int CountAdjacentSeats(int x, int y, int version)
            {
                int count = 0;

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0)
                        {
                            continue;
                        }

                        count += IsAdjacentSeatOccupied(x, y, version, (i, j));
                    }
                }

                return count;
            }

            int IsAdjacentSeatOccupied(int x, int y, int version, (int x, int y) vector)
            {
                x += vector.x;
                y += vector.y;

                if (x < 0 || y < 0 || x > width - 1 || y > height - 1)
                {
                    return 0;
                }

                char seat = layout[y][x];

                if (version == 1)
                {
                    return seat == Occupied ? 1 : 0;
                }

                return seat switch
                {
                    Empty => 0,
                    Occupied => 1,
                    _ => IsAdjacentSeatOccupied(x, y, version, vector),
                };
            }
        }

        /// <summary>
        /// Writes the specified message.
        /// </summary>
        /// <param name="grid">The message to write.</param>
        /// <param name="logger">The logger to write the message to.</param>
        /// <returns>
        /// The visualization of the data.
        /// </returns>
        private static string WriteGrid(IList<char[]> grid, ILogger? logger)
        {
            int width = grid.Count;
            int height = grid[0].Length;

            var builder = new StringBuilder((width + Environment.NewLine.Length) * height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    builder.Append(grid[x][y]);
                }

                builder.AppendLine();
            }

            string visualization = builder.ToString();

            logger?.WriteLine(visualization);

            return visualization;
        }
    }
}
