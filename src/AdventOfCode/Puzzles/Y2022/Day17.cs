// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/17</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 17, "Pyroclastic Flow", RequiresData = true, IsHidden = true)]
public sealed class Day17 : Puzzle
{
    private const long OneTrillion = 1_000_000_000_000;

    private enum Direction
    {
        Left,
        Right,
        Down,
    }

    /// <summary>
    /// Gets how many units tall will the tower of rocks
    /// will be after 2022 rocks have stopped falling.
    /// </summary>
    public long Height2022 { get; private set; }

    /// <summary>
    /// Gets how many units tall will the tower of rocks
    /// will be after 1,000,000,000,000 rocks have stopped falling.
    /// </summary>
    public long HeightTrillion { get; private set; }

    /// <summary>
    /// Gets the height of the tower of rocks after
    /// the specified number of rocks have stopped falling.
    /// </summary>
    /// <param name="jets">The directions of the jets that move the rocks as they fall.</param>
    /// <param name="count">The number of rocks to simulate falling.</param>
    /// <param name="cancellationToken">The optional <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// Returns how many units tall will the tower of rocks will be after the number
    /// of rocks specified by <paramref name="count"/> have stopped falling.
    /// </returns>
    public static long GetHeightOfTower(string jets, long count, CancellationToken cancellationToken = default)
    {
        var tower = new Tower();
        uint[][] shapes = new[] { Rock.Horizontal, Rock.Plus, Rock.Boomerang, Rock.Vertical, Rock.Square };

        var history = new HashSet<int>();
        var heights = new Dictionary<long, (long Rock, long Height)>();

        for (long i = 0, j = 0; i < count && !cancellationToken.IsCancellationRequested; i++)
        {
            var rock = new Rock(shapes[i % shapes.Length], 2, tower.Height + 3);

            Dump(rock);

            while (true)
            {
                var direction = GetDirection(jets[(int)(j++ % jets.Length)]);

                if (!tower.WillCollide(rock, direction))
                {
                    rock.Shift(direction);
                    Dump(rock);
                }

                if (tower.WillCollide(rock, Direction.Down) || rock.Bottom == 0)
                {
                    break;
                }

                rock.Drop();
                Dump(rock);
            }

            Dump(rock);
            tower.Consume(rock);
            Dump();

            int hashCode = tower.GetHashCode();
            heights[hashCode] = (i, tower.Height);

            if (!history.Add(hashCode))
            {
                long cycleEnd = i;
                long cycleStart = heights[hashCode].Rock;
                long cycleSize = cycleEnd - cycleStart;

                long heightBeforeCycle = heights[hashCode].Height;
                long heightAddedByCycle = tower.Height - heightBeforeCycle;

                long rocksLeft = count - i;

                (long numberOfCycles, long leftover) = Math.DivRem(rocksLeft, cycleSize);

                long remainingHeight = heights[leftover].Height - heightBeforeCycle;

                return heightBeforeCycle + (heightAddedByCycle * numberOfCycles) + remainingHeight;
            }

            void Dump(Rock? rock = null)
            {
#if false
                Debug.WriteLine(string.Empty);
                Debug.WriteLine(tower.ToString(rock));
#endif
            }
        }

        cancellationToken.ThrowIfCancellationRequested();

        return tower.Height;

        static Direction GetDirection(char direction) => direction switch
        {
            '<' => Direction.Left,
            '>' => Direction.Right,
            _ => throw new PuzzleException($"Invalid direction '{direction}'."),
        };
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        string jets = (await ReadResourceAsStringAsync()).Trim();

        Height2022 = GetHeightOfTower(jets.Trim(), count: 2022, cancellationToken);
        HeightTrillion = GetHeightOfTower(jets.Trim(), count: OneTrillion, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("The tower of rocks is {0} units tall after 2022 rocks have stopped falling.", Height2022);
            Logger.WriteLine("The tower of rocks is {0} units tall after 1,000,000,000,000 rocks have stopped falling.", HeightTrillion);
        }

        return PuzzleResult.Create(Height2022, HeightTrillion);
    }

    private struct Rock
    {
        public static readonly uint[] Horizontal = { 0b_11110000_00000000_00000000_00000000 };
        public static readonly uint[] Plus = { 0b_01000000_00000000_00000000_00000000, 0b_11100000_00000000_00000000_00000000, 0b_01000000_00000000_00000000_00000000 };
        public static readonly uint[] Boomerang = { 0b_11100000_00000000_00000000_00000000, 0b_00100000_00000000_00000000_00000000, 0b_00100000_00000000_00000000_00000000 };
        public static readonly uint[] Vertical = { 0b_10000000_00000000_00000000_00000000, 0b_10000000_00000000_00000000_00000000, 0b_10000000_00000000_00000000_00000000, 0b_10000000_00000000_00000000_00000000 };
        public static readonly uint[] Square = { 0b_11000000_00000000_00000000_00000000, 0b_11000000_00000000_00000000_00000000 };

        private const uint SignificantBits = 7;

        public Rock()
        {
            Shape = Array.Empty<uint>();
        }

        public Rock(uint[] shape, int x, int y)
        {
            Y = y;
            Shape = new uint[shape.Length];
            Array.Copy(shape, Shape, shape.Length);
            Shift(Direction.Right, x);
        }

        public uint[] Shape { get; set; }

        public int Y { get; set; }

        public int Left => Shape.Min(BitOperations.LeadingZeroCount);

        public int Right => Left + Width - 1;

        public int Top => Y + Shape.Length - 1;

        public int Bottom => Y;

        public int Width => Shape.Max(BitOperations.PopCount);

        public int Height => Shape.Length;

        public void Drop() => Y--;

        public void Shift(Direction direction, int units = 1)
        {
            if ((direction == Direction.Right && Right + units > SignificantBits - 1) ||
                (direction == Direction.Left && Left - units < 0))
            {
                return;
            }

            if (direction == Direction.Left)
            {
                for (int i = 0; i < Shape.Length; i++)
                {
                    Shape[i] = BitOperations.RotateLeft(Shape[i], units);
                }
            }
            else
            {
                for (int i = 0; i < Shape.Length; i++)
                {
                    Shape[i] = BitOperations.RotateRight(Shape[i], units);
                }
            }
        }

        public bool Overlaps(int x, int y)
        {
            if (y < Bottom || y > Bottom + Height || x < Left || x > Right)
            {
                return false;
            }

            int index = y - Y;

            if (index > Shape.Length - 1)
            {
                return false;
            }

            return (Shape[index] & (1 << (32 - x - 1))) != 0;
        }
    }

    private sealed class Tower
    {
        private const int Width = 7;

        private readonly List<uint> _rows = new();

        public int Height => _rows.Count;

        public bool WillCollide(Rock rock, Direction direction)
        {
            if (direction == Direction.Down)
            {
                if (rock.Bottom == 0)
                {
                    return true;
                }

                for (int i = 0; i < rock.Shape.Length; i++)
                {
                    int rowIndex = rock.Bottom + i - 1;
                    uint rowBelow = _rows.Count - 1 < rowIndex ? 0 : _rows[rowIndex];
                    uint rockNow = rock.Shape[i];

                    int rowsInRockBottom = BitOperations.PopCount(rockNow);
                    int rocksInRow = BitOperations.PopCount(rowBelow);
                    int rocksInRowAfter = BitOperations.PopCount(rowBelow | rockNow);

                    if (rocksInRow + rowsInRockBottom != rocksInRowAfter)
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                if (rock.Bottom > Height - 1)
                {
                    return false;
                }

                for (int y = 0; y < rock.Height; y++)
                {
                    int rowIndex = rock.Bottom + y;
                    uint row = _rows.Count - 1 < rowIndex ? 0 : _rows[rowIndex];

                    uint rockNow = rock.Shape[y];
                    uint rockAfter =
                        direction == Direction.Left ?
                        BitOperations.RotateLeft(rockNow, 1) :
                        BitOperations.RotateRight(rockNow, 1);

                    int rocksInRow = BitOperations.PopCount(row) + BitOperations.PopCount(rockNow);
                    int rocksInRowAfter = BitOperations.PopCount(row | rockAfter);

                    if (rocksInRow != rocksInRowAfter)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void Consume(Rock rock)
        {
            while (_rows.Count - 1 < rock.Top)
            {
                _rows.Add(0);
            }

            for (int y = 0; y < rock.Height; y++)
            {
                int rowY = rock.Bottom + y;
                _rows[rowY] = _rows[rowY] | rock.Shape[y];
            }
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            foreach (uint row in _rows.Take(10_000))
            {
                hash.Add(row);
            }

            return hash.ToHashCode();
        }

        public override string ToString()
            => ToString(null);

        public string ToString(Rock? rock)
        {
            var builder = new StringBuilder();

            if (rock is not null || _rows.Count > 0)
            {
                int maxY = Math.Max(rock?.Top ?? 0, Height);

                for (int y = maxY; y > -1; y--)
                {
                    builder.Append('|');

                    for (int x = 0; x < Width; x++)
                    {
                        char ch;

                        if (rock?.Overlaps(x, y) == true)
                        {
                            ch = '@';
                        }
                        else
                        {
                            ch = _rows.Count - 1 < y || (_rows[y] & (1 << (32 - x - 1))) == 0 ? '.' : '#';
                        }

                        builder.Append(ch);
                    }

                    builder.Append('|');
                    builder.Append('\n');
                }
            }

            builder.Append("+-------+");

            return builder.ToString();
        }
    }
}
