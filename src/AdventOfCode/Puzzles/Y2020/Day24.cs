// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2020/day/24</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2020, 24, "Lobby Layout", RequiresData = true)]
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

            bool isBlack = floor.GetValueOrDefault(destination);
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
                int limit = Math.Min(n, -x + n);

                for (int y = Math.Max(-n, -x - n); y <= limit; y++)
                {
                    int z = -x - y;

                    var tile = new Tile(x, y, z);

                    bool isBlack = floor.GetValueOrDefault(tile);

                    int count = 0;

                    foreach (Tile neighbor in tile.Neighbors())
                    {
                        bool isNeighborBlack = floor.GetValueOrDefault(neighbor);

                        if (isNeighborBlack)
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
        var instructions = await ReadResourceAsLinesAsync(cancellationToken);

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
    private readonly struct Tile(int x, int y, int z) : IEquatable<Tile>
    {
        public static readonly Tile Zero = new(0, 0, 0);

        public readonly int X = x;

        public readonly int Y = y;

        public readonly int Z = z;

        public static Tile operator +(Tile point, Tile vector)
        {
            return new(
                point.X + vector.X,
                point.Y + vector.Y,
                point.Z + vector.Z);
        }

        public override readonly bool Equals(object? obj)
            => obj is Tile tile && Equals(tile);

        public readonly bool Equals(Tile other)
            => X == other.X && Y == other.Y && Z == other.Z;

        public override readonly int GetHashCode()
            => HashCode.Combine(X, Y, Z);

        public readonly Tile West() => this + new Tile(-1, 1, 0);

        public readonly Tile NorthWest() => this + new Tile(0, 1, -1);

        public readonly Tile NorthEast() => this + new Tile(1, 0, -1);

        public readonly Tile East() => this + new Tile(1, -1, 0);

        public readonly Tile SouthEast() => this + new Tile(0, -1, 1);

        public readonly Tile SouthWest() => this + new Tile(-1, 0, 1);

        public readonly IEnumerable<Tile> Neighbors()
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
