// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/12</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 12, "Christmas Tree Farm", RequiresData = true)]
public sealed class Day12 : Puzzle<int>
{
    /// <summary>
    /// Counts the number of regions that can fit all of the required presents.
    /// </summary>
    /// <param name="summary">The values to solve the puzzle from.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The count of regions that can fit all of the presents.
    /// </returns>
    public static int Arrange(IReadOnlyList<string> summary, CancellationToken cancellationToken)
    {
        (var shapes, var regions) = ParseSummary(summary);

        int count = 0;

        foreach (var region in regions)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (TryPack(region, shapes, cancellationToken))
            {
                count++;
            }
        }

        return count;

        static (List<Shape> Shapes, List<Region> Regions) ParseSummary(IReadOnlyList<string> summary)
        {
            var shapes = new List<Shape>();
            var regions = new List<Region>();

            for (int i = 0; i < summary.Count; i++)
            {
                string first = summary[i];

                if (TryParse(first.TrimEnd(':'), out int index))
                {
                    int y = 0;
                    var shape = new Shape(index);

                    while (!string.IsNullOrEmpty(summary[++i]))
                    {
                        string row = summary[i];

                        for (int x = 0; x < row.Length; x++)
                        {
                            if (row[x] == '#')
                            {
                                shape.Add(new(x, y));
                            }
                        }

                        y++;
                    }

                    shapes.Add(shape);
                }
                else
                {
                    var region = first.AsSpan();
                    index = region.IndexOf(' ');

                    var bounds = region[..(index - 1)];
                    var counts = region[(index + 1)..];

                    var quantities = new List<int>();

                    foreach (var range in counts.Split(' '))
                    {
                        quantities.Add(Parse<int>(counts[range]));
                    }

                    index = bounds.IndexOf('x');
                    int width = Parse<int>(bounds[..index]);
                    int height = Parse<int>(bounds[(index + 1)..]);

                    regions.Add(new(width, height, quantities));
                }
            }

            return (shapes, regions);
        }

        static bool TryPack(Region region, List<Shape> shapes, CancellationToken cancellationToken)
        {
            int requirement = 0;

            var presents = new List<Shape>();

            for (int i = 0; i < region.Quantities.Count; i++)
            {
                int quantity = region.Quantities[i];
                var shape = shapes[i];

                for (int j = 0; j < quantity; j++)
                {
                    presents.Add(shape);
                }

                requirement += quantity * shape.Count;
            }

            presents.Sort((a, b) => b.Count.CompareTo(a.Count));

            int area = region.Bounds.Area();

            if (requirement > area)
            {
                // Definitely cannot fit
                return false;
            }

            if ((float)requirement / area < 0.75f)
            {
                // If it is less than 75% full, we assume it can fit
                return true;
            }

            var cache = new Dictionary<int, bool>();
            var empty = region.Empty();
            var queue = new Queue<Shape>(presents);

            return Pack(area - requirement, region.Bounds, empty, queue, cache, cancellationToken);
        }

        static bool Pack(
            int target,
            Rectangle bounds,
            HashSet<Point> available,
            Queue<Shape> shapes,
            Dictionary<int, bool> cache,
            CancellationToken cancellationToken)
        {
            int hash = Region.HashCode(available, shapes);

            if (cache.TryGetValue(hash, out bool result))
            {
                return result;
            }

            bool canPack = false;

            if (shapes.Count == 0)
            {
                canPack = available.Count >= target;
            }
            else
            {
                if (available.Count < shapes.Peek().Count)
                {
                    return cache[hash] = false;
                }

                var shape = shapes.Dequeue();

                foreach (var transform in shape.Transformations())
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    for (int y = 0; y < bounds.Height && !cancellationToken.IsCancellationRequested; y++)
                    {
                        for (int x = 0; x < bounds.Width && !cancellationToken.IsCancellationRequested; x++)
                        {
                            if (!available.Contains(new(x, y)))
                            {
                                continue;
                            }

                            var offset = new Size(x, y);
                            HashSet<Point> transformation = [.. transform.Select((p) => p + offset)];

                            if (!transformation.IsSubsetOf(available))
                            {
                                continue;
                            }

                            available.Not(transformation);

                            if (Pack(target, bounds, available, shapes, cache, cancellationToken))
                            {
                                return cache[hash] = true;
                            }

                            available.Or(transformation);
                        }
                    }
                }

                shapes.Enqueue(shape);
            }

            cancellationToken.ThrowIfCancellationRequested();

            cache[hash] = canPack;

            return canPack;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (values, logger, cancellationToken) =>
            {
                int count = Arrange(values, cancellationToken);

                if (logger is { })
                {
                    logger.WriteLine("{0} regions can fit all of the presents.", count);
                }

                return count;
            },
            cancellationToken);
    }

    private sealed class Region(int width, int height, IReadOnlyList<int> quantities)
    {
        public Rectangle Bounds { get; } = new(0, 0, width, height);

        public IReadOnlyList<int> Quantities { get; } = quantities;

        public static int HashCode(HashSet<Point> region, IEnumerable<Shape> shapes)
        {
            HashCode hash = default;

            foreach (var point in region.OrderBy((p) => p.Y).ThenBy((p) => p.X))
            {
                hash.Add(point.GetHashCode());
            }

            foreach (var shape in shapes)
            {
                hash.Add(HashCode(shape, []));
            }

            return hash.ToHashCode();
        }

        public HashSet<Point> Empty()
        {
            var empty = new HashSet<Point>();

            for (int y = 0; y < Bounds.Height; y++)
            {
                for (int x = 0; x < Bounds.Width; x++)
                {
                    empty.Add(new(x, y));
                }
            }

            return empty;
        }
    }

    private sealed class Shape(int index) : HashSet<Point>
    {
        private readonly List<Shape> _transformations = new(8);

        public int Index { get; } = index;

        public IEnumerable<Shape> Transformations()
        {
            if (_transformations.Count == 8)
            {
                foreach (var transformation in _transformations)
                {
                    yield return transformation;
                }

                yield break;
            }

            Shape current = this;

            for (int i = 0; i < 4; i++)
            {
                _transformations.Add(current);
                yield return current;

                current = current.Flip();

                _transformations.Add(current);
                yield return current;

                current = current.Rotate();
            }
        }

        private Shape Flip()
        {
            var flipped = new Shape(Index);

            foreach (var point in this)
            {
                flipped.Add(new(-point.X, point.Y));
            }

            return flipped;
        }

        private Shape Rotate()
        {
            var rotated = new Shape(Index);

            foreach (var point in this)
            {
                rotated.Add(new(point.Y, -point.X));
            }

            return rotated;
        }
    }
}
