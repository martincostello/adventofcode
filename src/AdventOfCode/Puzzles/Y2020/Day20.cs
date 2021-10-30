// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

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
    /// <param name="logger">The logger to use.</param>
    /// <returns>
    /// The product of the Ids of the four corner tiles and the roughness of the water and the final image.
    /// </returns>
    public static (long CornerIdProduct, int Roughness, string Visualization) GetCornerTileIdProduct(
        IList<string> input,
        ILogger logger)
    {
        IDictionary<long, Tile> tiles = ParseTiles(input);

        (List<Tile> corners, List<Tile> edges, List<Tile> others) = Geometry(tiles.Values);

        Tile[,] image = BuildImage(corners, edges, others);

        int width = image.GetLength(0);
        int extent = width - 1;

        long cornerIdProduct =
            image[0, 0].Id *
            image[0, extent].Id *
            image[extent, 0].Id *
            image[extent, extent].Id;

        // Create the full image
        foreach (Tile tile in image)
        {
            tile.RemoveBorder();
        }

        int tileWidth = image[0, 0].Grid.Count;
        int finalWidth = tileWidth * width;

        char[,] finalImage = new char[finalWidth, finalWidth];

        for (int j = 0; j < width; j++)
        {
            for (int i = 0; i < width; i++)
            {
                Tile tile = image[i, j];

                int offsetX = i * tileWidth;
                int offsetY = j * tileWidth;

                for (int y = 0; y < tileWidth; y++)
                {
                    for (int x = 0; x < tileWidth; x++)
                    {
                        finalImage[x + offsetX, y + offsetY] = tile.Grid[y][x];
                    }
                }
            }
        }

        // Represent the sea monster
        ////| | | | | | | | | | | | | | | | | | |#| |
        ////|#| | | | |#|#| | | | |#|#| | | | |#|#|#|
        ////| |#| | |#| | |#| | |#| | |#| | |#| | | |
        const int SeaMonsterWidth = 20;
        const int SeaMonsterHeight = 3;

        var seaMonster = new[]
        {
            (0, 1),
            (1, 2),
            (4, 2),
            (5, 1),
            (6, 1),
            (7, 2),
            (10, 2),
            (11, 1),
            (12, 1),
            (13, 2),
            (16, 2),
            (17, 1),
            (18, 0),
            (18, 1),
            (19, 1),
        };

        int rotations = 0;
        int seaMonsters = 0;

        while (rotations < 8)
        {
            seaMonsters = FindMonsters(finalImage);

            if (seaMonsters > 0)
            {
                break;
            }

            finalImage = Rotate(finalImage);

            if (++rotations % 4 == 0)
            {
                finalImage = Flip(finalImage);
            }
        }

        int hashes = 0;

        for (int j = 0; j < finalWidth; j++)
        {
            for (int i = 0; i < finalWidth; i++)
            {
                if (finalImage[i, j] == '#')
                {
                    hashes++;
                }
            }
        }

        FindMonsters(finalImage, highlightMonsters: true);

        string visualization = logger.WriteGrid(finalImage);

        int roughness = hashes - (seaMonsters * seaMonster.Length);

        return (cornerIdProduct, roughness, visualization);

        int FindMonsters(char[,] image, bool highlightMonsters = false)
        {
            int width = image.GetLength(0);
            int monsters = 0;

            for (int y = 0; y < width - SeaMonsterHeight; y++)
            {
                for (int x = 0; x < width - SeaMonsterWidth; x++)
                {
                    int pixels = 0;

                    foreach ((int offsetX, int offsetY) in seaMonster)
                    {
                        if (image[x + offsetX, y + offsetY] == '#')
                        {
                            pixels++;
                        }
                    }

                    if (pixels == seaMonster.Length)
                    {
                        monsters++;

                        if (highlightMonsters)
                        {
                            foreach ((int offsetX, int offsetY) in seaMonster)
                            {
                                if (image[x + offsetX, y + offsetY] == '#')
                                {
                                    image[x + offsetX, y + offsetY] = 'O';
                                }
                            }
                        }
                    }
                }
            }

            return monsters;
        }

        static char[,] Flip(char[,] image)
        {
            int width = image.GetLength(0);

            char[,] rotated = new char[width, width];

            for (int x = 0; x < width; x++)
            {
                for (int y = width - 1; y > -1; y--)
                {
                    rotated[width - x - 1, y] = image[x, y];
                }
            }

            return rotated;
        }

        static char[,] Rotate(char[,] image)
        {
            int width = image.GetLength(0);

            char[,] rotated = new char[width, width];

            for (int x = 0; x < width; x++)
            {
                for (int y = width - 1; y > -1; y--)
                {
                    rotated[width - y - 1, x] = image[x, y];
                }
            }

            return rotated;
        }

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
            for (int i = 0; i < corners.Count; i++)
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

                        if (current.IsNeighbor(edge))
                        {
                            edges.RemoveAt(j--);
                            edgeRow.Add(edge);
                            current = edge;
                        }
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

                    if (corner != first && last.IsNeighbor(corner))
                    {
                        row.Add(corner);
                        corners.RemoveAt(i--);
                    }
                }
            }

            // By convention, the first row is considered the top
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

            // Align the grid's edge tiles with each other
            bool aligned;
            Tile topLeft = result[0, 0];

            do
            {
                aligned = true;

                for (int i = 1; i < topRow.Count; i++)
                {
                    aligned &= topRow[i - 1].TryAlignToEdge(topRow[i], (p) => p.Right(), (p) => p.Left());
                }

                for (int i = 1; aligned && i < rightRow.Count; i++)
                {
                    aligned &= rightRow[i - 1].TryAlignToEdge(rightRow[i], (p) => p.Bottom(), (p) => p.Top());
                }

                for (int i = 1; aligned && i < bottomRow.Count; i++)
                {
                    aligned &= bottomRow[i - 1].TryAlignToEdge(bottomRow[i], (p) => p.Left(), (p) => p.Right());
                }

                for (int i = 1; aligned && i < leftRow.Count - 1; i++)
                {
                    aligned &= leftRow[i - 1].TryAlignToEdge(leftRow[i], (p) => p.Top(), (p) => p.Bottom());
                }

                if (aligned)
                {
                    break;
                }

                topLeft.NextOrientation();
            }
            while (!aligned);

            // Fill and align the inner square(s)
            for (int y = 1; y < width - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    Tile above = result[x, y - 1];

                    for (int k = 0; k < others.Count; k++)
                    {
                        Tile thisTile = others[k];

                        if (above.TryAlignToEdge(thisTile, (p) => p.Bottom(), (p) => p.Top()))
                        {
                            others.RemoveAt(k);
                            result[x, y] = thisTile;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        static (List<Tile> Corners, List<Tile> Edges, List<Tile> Others) Geometry(ICollection<Tile> tiles)
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

        (long productOfCornerTiles, int waterRoughness, string image) = GetCornerTileIdProduct(tiles, Logger);

        ProductOfCornerTiles = productOfCornerTiles;
        WaterRoughness = waterRoughness;

        Logger.WriteLine(image);

        if (Verbose)
        {
            Logger.WriteLine("The product of the Ids of the four corner tiles is {0}.", ProductOfCornerTiles);
            Logger.WriteLine("The roughness of the water is {0}.", WaterRoughness);
        }

        var result = new PuzzleResult();

        result.Solutions.Add(ProductOfCornerTiles);
        result.Solutions.Add(WaterRoughness);
        result.Visualizations.Add(image);

        return result;
    }

    /// <summary>
    /// A class representing an image tile. This class cannot be inherited.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Id = {Id}")]
    private sealed class Tile
    {
        /// <summary>
        /// The number of times the tile has been flipped or rotated.
        /// </summary>
        private uint _iterations;

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
            int length = Grid.Count;

            var rotated = new List<string>(length);

            for (int x = 0; x < length; x++)
            {
                char[] rotation = new char[length];

                for (int y = length - 1; y > -1; y--)
                {
                    rotation[length - y - 1] = Grid[y][x];
                }

                rotated.Add(new(rotation));
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
        {
            char[] reversed = Grid.Select((p) => p[0]).ToArray();
            Array.Reverse(reversed);

            return new(reversed);
        }

        /// <summary>
        /// Returns the right edge.
        /// </summary>
        /// <returns>
        /// The right edge of the tile.
        /// </returns>
        public string Right()
            => new(Grid.Select((p) => p[^1]).ToArray());

        /// <summary>
        /// Tries to align the other tile to this tile's edge.
        /// </summary>
        /// <param name="other">The other tile to try to align.</param>
        /// <param name="thisEdge">A delegate to a method to get the edge to align to.</param>
        /// <param name="otherEdge">A delegate to a method to get the other edge to align with this tile.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="other"/> was aligned
        /// to the right edge of this tile; otherwise <see langword="false"/>.
        /// </returns>
        public bool TryAlignToEdge(Tile other, Func<Tile, string> thisEdge, Func<Tile, string> otherEdge)
        {
            if (!IsNeighbor(other))
            {
                return false;
            }

            string mirror = thisEdge(this).Mirror();

            // Flip and rotate the other tile until its left
            // edge aligns with the right edge of this tile
            for (int i = 0; i < 8; i++)
            {
                if (otherEdge(other) == mirror)
                {
                    return true;
                }

                other.NextOrientation();
            }

            return false;
        }

        /// <summary>
        /// Moves the tile to its next orientation.
        /// </summary>
        public void NextOrientation()
        {
            Rotate();

            if (++_iterations % 4 == 0)
            {
                Flip();
            }
        }
    }
}
