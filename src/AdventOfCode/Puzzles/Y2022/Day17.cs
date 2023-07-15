// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/17</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 17, "Pyroclastic Flow", RequiresData = true, IsHidden = true)]
public sealed class Day17 : Puzzle
{
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
        var shapes = new[] { Rock.Horizontal, Rock.Plus, Rock.Boomerang, Rock.Vertical, Rock.Square };
        var down = new Size(0, -1);

        for (long i = 0, j = 0; i < count && !cancellationToken.IsCancellationRequested; i++)
        {
            var rock = Rock.Spawn(shapes[i % shapes.Length], tower.Height + 3);

            while (true)
            {
                var offset = GetOffset(jets[(int)(j++ % jets.Length)]);

                if (!tower.WillCollide(rock, offset))
                {
                    rock.Shift(offset);
                }

                if (tower.WillCollide(rock, down) || rock.Bottom == 0)
                {
                    break;
                }

                rock.Drop();
            }

            tower.Consume(rock);
        }

        cancellationToken.ThrowIfCancellationRequested();

        return tower.RealHeight;

        static Size GetOffset(char direction) => direction switch
        {
            '<' => new(-1, 0),
            '>' => new(1, 0),
            _ => throw new PuzzleException($"Invalid direction '{direction}'."),
        };
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        string jets = (await ReadResourceAsStringAsync(cancellationToken)).Trim();

        Height2022 = GetHeightOfTower(jets.Trim(), count: 2022, cancellationToken);
        HeightTrillion = GetHeightOfTower(jets.Trim(), count: 1_000_000_000_000, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("The tower of rocks is {0} units tall after 2022 rocks have stopped falling.", Height2022);
            Logger.WriteLine("The tower of rocks is {0} units tall after 1,000,000,000,000 rocks have stopped falling.", HeightTrillion);
        }

        return PuzzleResult.Create(Height2022, HeightTrillion);
    }

    private sealed class Rock
    {
        public static readonly ImmutableArray<Point> Horizontal = new Point[] { new(0, 0), new(1, 0), new(2, 0), new(3, 0) }.ToImmutableArray();
        public static readonly ImmutableArray<Point> Plus = new Point[] { new(1, 0), new(0, 1), new(1, 1), new(2, 1), new(1, 2) }.ToImmutableArray();
        public static readonly ImmutableArray<Point> Boomerang = new Point[] { new(0, 0), new(1, 0), new(2, 0), new(2, 1), new(2, 2) }.ToImmutableArray();
        public static readonly ImmutableArray<Point> Vertical = new Point[] { new(0, 0), new(0, 1), new(0, 2), new(0, 3) }.ToImmutableArray();
        public static readonly ImmutableArray<Point> Square = new Point[] { new(0, 0), new(1, 0), new(0, 1), new(1, 1) }.ToImmutableArray();

        private Rock(IReadOnlyList<Point> points)
        {
            Points = new List<Point>(points);
        }

        public int Left => Points.Min((p) => p.X);

        public int Right => Points.Max((p) => p.X);

        public int Top => Points.Max((p) => p.Y);

        public int Bottom => Points.Min((p) => p.Y);

        public List<Point> Points { get; }

        public static Rock Spawn(IReadOnlyList<Point> shape, int y)
        {
            var rock = new Rock(shape);
            var offset = new Size(2, y);

            for (int i = 0; i < rock.Points.Count; i++)
            {
                rock.Points[i] += offset;
            }

            return rock;
        }

        public void Drop()
        {
            for (int i = 0; i < Points.Count; i++)
            {
                Points[i] += new Size(0, -1);
            }
        }

        public void Raise()
        {
            for (int i = 0; i < Points.Count; i++)
            {
                Points[i] += new Size(0, 1);
            }
        }

        public void Shift(Size delta)
        {
            if (Right + delta.Width > 6 || Left + delta.Width < 0)
            {
                return;
            }

            for (int i = 0; i < Points.Count; i++)
            {
                Points[i] += delta;
            }
        }
    }

    private sealed class Tower
    {
        private const int Width = 7;

        private HashSet<Point> _rocks = new();
        private long _offset;

        public int Height => _rocks.Count == 0 ? 0 : _rocks.Max((p) => p.Y) + 1;

        public long RealHeight => _offset + Height;

        public int HeightAt(int x)
            => _rocks.Where((p) => p.X == x).Select((p) => p.Y).DefaultIfEmpty(0).Max();

        public bool WillCollide(Rock rock, Size offset)
        {
            foreach (Point point in rock.Points)
            {
                if (_rocks.Contains(point + offset))
                {
                    return true;
                }
            }

            return false;
        }

        public void Consume(Rock rock)
        {
            _rocks.UnionWith(rock.Points);
            TryCompact();
        }

        public override string ToString()
            => ToString(null);

        public string ToString(Rock? rock)
        {
            var builder = new StringBuilder();

            if (rock is not null || _rocks.Count > 0)
            {
                int maxY = Math.Max(rock?.Top ?? 0, _rocks.Count == 0 ? 0 : _rocks.MaxBy((p) => p.Y).Y);

                for (int y = maxY; y > -1; y--)
                {
                    builder.Append('|');

                    for (int x = 0; x < Width; x++)
                    {
                        builder.Append(rock?.Points.Contains(new(x, y)) == true ? '@' : _rocks.Contains(new(x, y)) ? '#' : '.');
                    }

                    builder.Append('|');
                    builder.Append('\n');
                }
            }

            builder.Append("+-------+");

            return builder.ToString();
        }

        private void TryCompact()
        {
            if (Height <= 100)
            {
                return;
            }

            int rowsToDelete = int.MaxValue;

            for (int i = 0; i < Width; i++)
            {
                rowsToDelete = Math.Min(HeightAt(i), rowsToDelete);
            }

            if (rowsToDelete > 0)
            {
                _offset += rowsToDelete;
                _rocks.RemoveWhere((p) => p.Y < rowsToDelete);

                var delta = new Size(0, rowsToDelete);
                _rocks = _rocks.Select((p) => p - delta).ToHashSet();
            }
        }
    }
}
