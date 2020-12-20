﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
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
        /// Gets the roughness of the waters.
        /// </summary>
        public int WaterRoughness { get; private set; }

        /// <summary>
        /// Gets the product of the four corner tiles of an image
        /// when they are assembled in the correct order.
        /// </summary>
        /// <param name="input">The input data containing the description of the tiles of the image.</param>
        /// <returns>
        /// The product of the Ids of the four corner tiles and the roughness of the water.
        /// </returns>
        public static (long cornerIdProduct, int roughness) GetCornerTileIdProduct(IList<string> input)
        {
            IDictionary<long, Tile> tiles = ParseTiles(input);

            (List<Tile> corners, List<Tile> edges, List<Tile> others) = Geometry(tiles.Values);

            Tile[,] image = BuildImage(corners, edges, others);

            int extent = image.GetLength(0) - 1;

            long cornerIdProduct =
                image[0, 0].Id *
                image[0, extent].Id *
                image[extent, 0].Id *
                image[extent, extent].Id;

            return (cornerIdProduct, 0);

            static Tile[,] BuildImage(
                List<Tile> corners,
                List<Tile> edges,
                List<Tile> others)
            {
                int width = (int)Math.Sqrt(corners.Count + edges.Count + others.Count);

                var result = new Tile[width, width];

                var edgeRows = new List<IList<Tile>>();

                // Iterate through the corners and find the tiles that adjoin it to the right:
                // [Corner] -> [Edge] -> [Edge] -> [Edge]
                for (int i = 0; i < 4; i++)
                {
                    Tile corner = corners[i];
                    Tile current = corner;

                    var edgeRow = new List<Tile>(width)
                    {
                        corner,
                    };

                    while (edgeRow.Count < width - 1)
                    {
                        for (int j = 0; j < edges.Count; j++)
                        {
                            Tile edge = edges[j];

                            if (!current.TryAlignToRightEdge(edge))
                            {
                                continue;
                            }

                            edges.RemoveAt(j--);
                            edgeRow.Add(edge);
                            current = edge;
                        }
                    }

                    edgeRows.Add(edgeRow);
                }

                foreach (IList<Tile> row in edgeRows)
                {
                    Tile first = row[0];
                    Tile last = row[^1];

                    for (int i = 0; i < corners.Count; i++)
                    {
                        Tile corner = corners[i];

                        if (corner == first)
                        {
                            continue;
                        }

                        if (!last.TryAlignToRightEdge(corner))
                        {
                            continue;
                        }

                        row.Add(corner);
                        corners.RemoveAt(i--);
                    }
                }

                // By convention, the first row is considered the top, aligned left-to-right
                IList<Tile> topRow = edgeRows[0];
                IList<Tile> rightRow = edgeRows.Single((p) => p[0] == topRow[^1]);
                IList<Tile> bottomRow = edgeRows.Single((p) => p[0] == rightRow[^1]);
                IList<Tile> leftRow = edgeRows.Single((p) => p[0] == bottomRow[^1]);

                // Place the edges onto the grid clockwise
                for (int i = 0; i < topRow.Count; i++)
                {
                    result[i, 0] = topRow[i];
                }

                for (int i = 0; i < rightRow.Count; i++)
                {
                    Tile current = rightRow[i];
                    result[width - 1, i] = current;
                }

                for (int i = 0; i < bottomRow.Count; i++)
                {
                    Tile current = bottomRow[i];
                    result[width - i - 1, width - 1] = current;
                }

                for (int i = 0; i < leftRow.Count; i++)
                {
                    Tile current = leftRow[i];
                    result[0, width - i - 1] = current;
                }

                //// TODO Correctly orient the tiles

                if (others.Count == 1)
                {
                    result[width / 2, width / 2] = others[0];
                }
                else if (others.Count > 1)
                {
                    // TODO Recurse to fill in the next ring
                }

                return result;
            }

            static (List<Tile> corners, List<Tile> edges, List<Tile> others) Geometry(ICollection<Tile> tiles)
            {
                var corners = new List<Tile>();
                var edges = new List<Tile>();
                var others = new List<Tile>();

                foreach (Tile tile in tiles)
                {
                    var candidates = new List<Tile>();

                    foreach (Tile other in tiles)
                    {
                        if (other.IsNeighbor(tile))
                        {
                            candidates.Add(other);
                        }
                    }

                    if (candidates.Count == 2)
                    {
                        corners.Add(tile);
                    }
                    else if (candidates.Count == 3)
                    {
                        edges.Add(tile);
                    }
                    else
                    {
                        others.Add(tile);
                    }
                }

                return (corners, edges, others);
            }

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
                    foreach (string edge in new[] { tile.Top(), tile.Bottom(), tile.Left(), tile.Right() })
                    {
                        tile.EdgeCandidates.Add(edge);
                        tile.EdgeCandidates.Add(edge.Mirror());
                    }
                }

                return image;
            }
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> tiles = await ReadResourceAsLinesAsync();

            (ProductOfCornerTiles, WaterRoughness) = GetCornerTileIdProduct(tiles);

            if (Verbose)
            {
                Logger.WriteLine("The product of the Ids of the four corner tiles is {0}.", ProductOfCornerTiles);
                Logger.WriteLine("The roughness of the water is {0}.", WaterRoughness);
            }

            return PuzzleResult.Create(ProductOfCornerTiles, WaterRoughness);
        }

        /// <summary>
        /// A class representing an image tile. This class cannot be inherited.
        /// </summary>
        [System.Diagnostics.DebuggerDisplay("Id = {Id}")]
        private sealed class Tile
        {
            /// <summary>
            /// Gets or sets the Id of the tile.
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Gets the candidates for the edges of the tile.
            /// </summary>
            public HashSet<string> EdgeCandidates { get; } = new HashSet<string>();

            /// <summary>
            /// Gets the original grid of the image.
            /// </summary>
            public IList<string> Grid { get; private set; } = new List<string>();

            /// <summary>
            /// Flips the tile's grid around the Y axis.
            /// </summary>
            public void Flip()
                => Grid = Grid.Select((p) => p.Mirror()).ToList();

            /// <summary>
            /// Returns whether the specified tile is a neighbor of this tile.
            /// </summary>
            /// <param name="other">The other tile.</param>
            /// <returns>
            /// <see langword="true"/> if <paramref name="other"/> is a
            /// neighbor of this tile; otherwise <see langword="false"/>.
            /// </returns>
            public bool IsNeighbor(Tile other)
                => this != other && other.EdgeCandidates.Any((p) => EdgeCandidates.Contains(p));

            /// <summary>
            /// Removes the border from the tile's grid.
            /// </summary>
            public void RemoveBorder()
            {
                Grid = Grid
                    .Skip(1)
                    .Take(Grid.Count - 2)
                    .Select((p) => p[1..^1])
                    .ToList();
            }

            /// <summary>
            /// Rotates the tile's grid clockwise by 90 degrees.
            /// </summary>
            public void Rotate()
            {
                var rotated = new List<string>();

                int length = Grid.Count;

                for (int x = 0; x < length; x++)
                {
                    char[] rotation = new char[length];

                    for (int y = length - 1; y > -1; y--)
                    {
                        rotation[length - y - 1] = Grid[y][x];
                    }

                    rotated.Add(new string(rotation));
                }

                Grid = rotated;
            }

            /// <summary>
            /// Returns the top edge.
            /// </summary>
            /// <returns>
            /// The top edge of the tile.
            /// </returns>
            public string Top()
                => Grid[0];

            /// <summary>
            /// Returns the bottom edge.
            /// </summary>
            /// <returns>
            /// The bottom edge of the tile.
            /// </returns>
            public string Bottom()
                => Grid[^1].Mirror();

            /// <summary>
            /// Returns the left edge.
            /// </summary>
            /// <returns>
            /// The left edge of the tile.
            /// </returns>
            public string Left()
                => new string(Grid.Select((p) => p[0]).Reverse().ToArray());

            /// <summary>
            /// Returns the right edge.
            /// </summary>
            /// <returns>
            /// The right edge of the tile.
            /// </returns>
            public string Right()
                => new string(Grid.Select((p) => p[^1]).ToArray());

            /// <summary>
            /// Tries to align the other tile to this tile's right edge.
            /// </summary>
            /// <param name="other">The other tile to try to align.</param>
            /// <returns>
            /// <see langword="true"/> if <paramref name="other"/> was aligned
            /// to the right edge of this tile; otherwise <see langword="false"/>.
            /// </returns>
            public bool TryAlignToRightEdge(Tile other)
            {
                if (!IsNeighbor(other))
                {
                    return false;
                }

                string right = Right();

                // Flip and rotate the other tile until its left
                // edge aligns with the right edge of this tile
                for (int i = 0; i < 4; i++)
                {
                    if (other.Left() == right)
                    {
                        break;
                    }

                    other.Flip();

                    if (other.Left() == right)
                    {
                        break;
                    }

                    other.Rotate();
                }

                return true;
            }

            /// <summary>
            /// Tries to align the other tile to this tile's bottom edge.
            /// </summary>
            /// <param name="other">The other tile to try to align.</param>
            /// <returns>
            /// <see langword="true"/> if <paramref name="other"/> was aligned
            /// to the bottom edge of this tile; otherwise <see langword="false"/>.
            /// </returns>
            public bool TryAlignToBottomEdge(Tile other)
            {
                if (!IsNeighbor(other))
                {
                    return false;
                }

                string bottom = Bottom();

                // Flip and rotate the other tile until its top
                // edge aligns with the bottom edge of this tile
                for (int i = 0; i < 4; i++)
                {
                    if (other.Top() == bottom)
                    {
                        break;
                    }

                    other.Flip();

                    if (other.Top() == bottom)
                    {
                        break;
                    }

                    other.Rotate();
                }

                return true;
            }
        }
    }
}
