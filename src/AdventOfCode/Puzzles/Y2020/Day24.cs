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
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/24</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2020, 24, RequiresData = true)]
    public sealed class Day24 : Puzzle
    {
        /// <summary>
        /// Gets the number of black tiles after 1 day.
        /// </summary>
        public int BlackTilesDay0 { get; private set; }

        /// <summary>
        /// Gets the number of black tiles after 100 days.
        /// </summary>
        public int BlackTilesDay100 { get; private set; }

        /// <summary>
        /// Applies the tiling pattern to the floor using the specified instructions.
        /// </summary>
        /// <param name="instructions">The instructions of how to flip the tiles on the floor.</param>
        /// <param name="days">The number of days to iterate the floor for.</param>
        /// <returns>
        /// The number of black tiles on the floor once the instructions have been followed.
        /// </returns>
        public static int TileFloor(ICollection<string> instructions, int days)
        {
            var floor = new Dictionary<Tile, bool>();

            foreach (string instruction in instructions)
            {
                var destination = Tile.Zero;

                for (int i = 0; i < instruction.Length; i++)
                {
                    destination = instruction[i] switch
                    {
                        'e' => destination.East(),
                        'w' => destination.West(),
                        'n' => instruction[++i] switch
                        {
                            'e' => destination.NorthEast(),
                            'w' => destination.NorthWest(),
                            _ => throw new PuzzleException("Invalid direction."),
                        },
                        's' => instruction[++i] switch
                        {
                            'e' => destination.SouthEast(),
                            'w' => destination.SouthWest(),
                            _ => throw new PuzzleException("Invalid direction."),
                        },
                        _ => throw new PuzzleException("Invalid direction."),
                    };
                }

                if (!floor.TryGetValue(destination, out bool isBlack))
                {
                    isBlack = false;
                }

                floor[destination] = !isBlack;
            }

            for (int i = 0; i < days; i++)
            {
                floor = Iterate(floor);
            }

            return floor.Count((p) => p.Value);

            static Dictionary<Tile, bool> Iterate(Dictionary<Tile, bool> floor)
            {
                // See https://www.redblobgames.com/grids/hexagons/#range
                int maximumX = floor.Max((p) => Math.Abs(p.Key.X));
                int maximumY = floor.Max((p) => Math.Abs(p.Key.Y));
                int maximumZ = floor.Max((p) => Math.Abs(p.Key.Z));

                int n = Math.Max(maximumX, maximumY);
                n = Math.Max(n, maximumZ) + 1;

                var result = new Dictionary<Tile, bool>(floor);

                for (int x = -n; x <= n; x++)
                {
                    for (int y = Math.Max(-n, -x - n); y <= Math.Min(n, -x + n); y++)
                    {
                        int z = -x - y;

                        var tile = new Tile(x, y, z);

                        if (!floor.TryGetValue(tile, out bool isBlack))
                        {
                            isBlack = false;
                        }

                        int count = 0;

                        foreach (Tile neighbor in tile.Neighbours())
                        {
                            if (floor.TryGetValue(neighbor, out bool foo) && foo)
                            {
                                count++;
                            }
                        }

                        if (isBlack && (count == 0 || count > 2))
                        {
                            result[tile] = false;
                        }
                        else if (!isBlack && count == 2)
                        {
                            result[tile] = true;
                        }
                    }
                }

                return result;
            }
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> instructions = await ReadResourceAsLinesAsync();

            BlackTilesDay0 = TileFloor(instructions, days: 0);
            BlackTilesDay100 = TileFloor(instructions, days: 100);

            if (Verbose)
            {
                Logger.WriteLine("{0} tiles are left with the black side up initially.", BlackTilesDay0);
                Logger.WriteLine("{0} tiles are left with the black side up after 100 days.", BlackTilesDay100);
            }

            return PuzzleResult.Create(BlackTilesDay0, BlackTilesDay100);
        }

        /// <summary>
        /// A struct representing a hexagonal tile.
        /// </summary>
        [System.Diagnostics.DebuggerDisplay("({X}, {Y}, {Z})")]
        private struct Tile : IEquatable<Tile>
        {
            /// <summary>
            /// A tile with all coordinates having a value of zero. This field is read-only.
            /// </summary>
            public static readonly Tile Zero = new Tile(0, 0, 0);

            /// <summary>
            /// The X coordinate.
            /// </summary>
            public int X;

            /// <summary>
            /// The Y coordinate.
            /// </summary>
            public int Y;

            /// <summary>
            /// The Z coordinate.
            /// </summary>
            public int Z;

            /// <summary>
            /// Initializes a new instance of the <see cref="Tile"/> struct.
            /// </summary>
            /// <param name="x">The X coordinate.</param>
            /// <param name="y">The Y coordinate.</param>
            /// <param name="z">The Z coordinate.</param>
            public Tile(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public static Tile operator +(Tile point, Tile vector)
            {
                return new Tile(
                    point.X + vector.X,
                    point.Y + vector.Y,
                    point.Z + vector.Z);
            }

            /// <inheritdoc/>
            public override bool Equals(object? obj)
                => obj is Tile tile && Equals(tile);

            /// <inheritdoc/>
            public bool Equals(Tile other)
                => X == other.X && Y == other.Y && Z == other.Z;

            /// <inheritdoc/>
            public override int GetHashCode()
                => HashCode.Combine(X, Y, Z);

            /// <summary>
            /// Returns the tile west of this tile.
            /// </summary>
            /// <returns>
            /// The tile west of this tile.
            /// </returns>
            public Tile West() => this + new Tile(-1, 1, 0);

            /// <summary>
            /// Returns the tile north west of this tile.
            /// </summary>
            /// <returns>
            /// The tile north west of this tile.
            /// </returns>
            public Tile NorthWest() => this + new Tile(0, 1, -1);

            /// <summary>
            /// Returns the tile north east of this tile.
            /// </summary>
            /// <returns>
            /// The tile north east of this tile.
            /// </returns>
            public Tile NorthEast() => this + new Tile(1, 0, -1);

            /// <summary>
            /// Returns the tile east of this tile.
            /// </summary>
            /// <returns>
            /// The tile east of this tile.
            /// </returns>
            public Tile East() => this + new Tile(1, -1, 0);

            /// <summary>
            /// Returns the tile south east of this tile.
            /// </summary>
            /// <returns>
            /// The tile south east of this tile.
            /// </returns>
            public Tile SouthEast() => this + new Tile(0, -1, 1);

            /// <summary>
            /// Returns the tile south west of this tile.
            /// </summary>
            /// <returns>
            /// The tile south east of this tile.
            /// </returns>
            public Tile SouthWest() => this + new Tile(-1, 0, 1);

            /// <summary>
            /// Enumerates the tile's neighbours.
            /// </summary>
            /// <returns>
            /// The neighbours of this tile.
            /// </returns>
            public IEnumerable<Tile> Neighbours()
            {
                yield return West();
                yield return NorthWest();
                yield return NorthEast();
                yield return East();
                yield return SouthEast();
                yield return SouthWest();
            }
        }
    }
}
