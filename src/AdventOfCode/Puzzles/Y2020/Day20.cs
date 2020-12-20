// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/20</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2020, 20, RequiresData = true)]
    public sealed class Day20 : Puzzle
    {
        /// <summary>
        /// Gets the product of Ids of the corner tiles.
        /// </summary>
        public long ProductOfCornerTiles { get; private set; }

        /// <summary>
        /// Gets the product of the four corner tiles of an image
        /// when they are assembled in the correct order.
        /// </summary>
        /// <param name="tiles">The tiles of the image.</param>
        /// <returns>
        /// The product of the Ids of the four corner tiles.
        /// </returns>
        public static long GetCornerTileIdProduct(IList<string> tiles)
        {
            IDictionary<long, Tile> image = ParseTiles(tiles);

            var corners = new List<Tile>();

            foreach (Tile tile in image.Values)
            {
                var candidates = new List<Tile>();

                foreach (Tile other in image.Values)
                {
                    if (tile == other)
                    {
                        continue;
                    }

                    if (other.EdgeCandidates.Any((p) => tile.EdgeCandidates.Contains(p)))
                    {
                        candidates.Add(other);
                    }
                }

                if (candidates.Count == 2)
                {
                    corners.Add(tile);
                }
            }

            return corners.Aggregate(1L, (x, y) => x * y.Id);

            static IDictionary<long, Tile> ParseTiles(IList<string> tiles)
            {
                var image = new Dictionary<long, Tile>();
                var current = new Tile();

                foreach (string line in tiles)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        image[current.Id] = current;
                        current = new Tile();
                    }
                    else if (line.StartsWith("Tile ", StringComparison.Ordinal))
                    {
                        current.Id = ParseInt64(line[5..^1].AsSpan());
                    }
                    else
                    {
                        current.Grid.Add(line);
                    }
                }

                foreach (Tile tile in image.Values)
                {
                    // Top edge
                    string edge = tile.Grid[0];

                    tile.EdgeCandidates.Add(edge);
                    tile.EdgeCandidates.Add(edge.Mirror());

                    // Right edge
                    edge = new string(tile.Grid.Select((p) => p[^1]).ToArray());

                    tile.EdgeCandidates.Add(edge);
                    tile.EdgeCandidates.Add(edge.Mirror());

                    // Bottom edge
                    edge = tile.Grid[^1];

                    tile.EdgeCandidates.Add(edge.Mirror());
                    tile.EdgeCandidates.Add(edge);

                    // Left edge
                    edge = new string(tile.Grid.Select((p) => p[0]).ToArray());

                    tile.EdgeCandidates.Add(edge.Mirror());
                    tile.EdgeCandidates.Add(edge);
                }

                return image;
            }
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> tiles = await ReadResourceAsLinesAsync();

            ProductOfCornerTiles = GetCornerTileIdProduct(tiles);

            if (Verbose)
            {
                Logger.WriteLine("The product of the Ids of the four corner tiles is {0}.", ProductOfCornerTiles);
            }

            return PuzzleResult.Create(ProductOfCornerTiles);
        }

        /// <summary>
        /// A class representing an image tile. This class cannot be inherited.
        /// </summary>
        private sealed class Tile
        {
            /// <summary>
            /// Gets or sets the Id of the tile.
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Gets the edges of the tile.
            /// </summary>
            public IList<string> EdgeCandidates { get; } = new List<string>();

            /// <summary>
            /// Gets the original grid of the image.
            /// </summary>
            public IList<string> Grid { get; } = new List<string>();
        }
    }
}
