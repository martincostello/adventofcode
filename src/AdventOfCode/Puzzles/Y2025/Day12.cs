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
    /// Counts the number of regions whose arrangements that can fit all of the required presents.
    /// </summary>
    /// <param name="summary">The values to solve the puzzle from.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The count of regions can fit all of the presents.
    /// </returns>
    public static int Arrange(IReadOnlyList<string> summary, CancellationToken cancellationToken)
    {
        (var shapes, var regions) = ParseSummary(summary);

        shapes.Sort((a, b) => b.Count.CompareTo(a.Count));

        int count = 0;

        foreach (var region in regions)
        {
            if (TryPack(region, shapes, cancellationToken))
            {
                count++;
            }
        }

        cancellationToken.ThrowIfCancellationRequested();

        return count;

        static bool TryPack(Region region, List<Shape> shapes, CancellationToken cancellationToken)
        {
            var queue = new Queue<Shape>();

            foreach (var shape in shapes)
            {
                int quantity = region.Quantities[shape.Index];

                for (int i = 0; i < quantity; i++)
                {
                    queue.Enqueue(shape);
                }
            }

            var cache = new Dictionary<int, bool>();
            var occupied = new HashSet<Point>();

            return Pack(region.Bounds, region.Empty(), occupied, queue, cache, cancellationToken);
        }

        static bool Pack(
            Rectangle bounds,
            HashSet<Point> region,
            HashSet<Point> occupied,
            Queue<Shape> shapes,
            Dictionary<int, bool> cache,
            CancellationToken cancellationToken)
        {
            int hash = Region.HashCode(occupied);

            if (cache.TryGetValue(hash, out bool packed))
            {
                return packed;
            }

            if (shapes.Count is 0)
            {
                // All shapes placed
                return cache[hash] = true;
            }

            var shape = shapes.Dequeue();

            if (occupied.Count + shape.Count > region.Count)
            {
                // Not enough free space
                return cache[hash] = false;
            }

            foreach (var variant in shape.GetTransformations())
            {
                int height = variant.Max((p) => p.Y) + 1;
                int width = variant.Max((p) => p.X) + 1;

                int maxX = bounds.Width - width;
                int maxY = bounds.Height - height;

                for (int y = 0; y <= maxY; y++)
                {
                    for (int x = 0; x <= maxX; x++)
                    {
                        var transformed = variant.Transform(new(x, y));

                        if (!transformed.IsSubsetOf(region))
                        {
                            // Cannot fit in the region
                            continue;
                        }

                        if (transformed.Overlaps(occupied))
                        {
                            // Blocked by existing presents
                            continue;
                        }

                        occupied.UnionWith(transformed);

                        if (Pack(bounds, region, occupied, shapes, cache, cancellationToken))
                        {
                            return true;
                        }

                        occupied.ExceptWith(transformed);
                    }
                }
            }

            return cache[hash] = false;
        }

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

        public static int HashCode(HashSet<Point> region)
        {
            HashCode hash = default;

            foreach (var point in region.OrderBy((p) => p.Y).ThenBy((p) => p.X))
            {
                hash.Add(point.GetHashCode());
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
        public int Index { get; } = index;

        public HashSet<Point> Transform(Size transform)
        {
            if (transform == default)
            {
                return this;
            }

            var transformed = new HashSet<Point>(Count);

            foreach (var point in this)
            {
                transformed.Add(point + transform);
            }

            return transformed;
        }

        public Shape Rotate()
        {
            var rotated = new Shape(Index);

            foreach (var point in this)
            {
                rotated.Add(new(point.Y, -point.X));
            }

            return rotated;
        }

        public Shape Flip()
        {
            var flipped = new Shape(Index);

            foreach (var point in this)
            {
                flipped.Add(new(-point.X, point.Y));
            }

            return flipped;
        }

        public IEnumerable<Shape> GetTransformations()
        {
            Shape current = this;

            for (int i = 0; i < 4; i++)
            {
                yield return current;
                yield return current.Flip();

                current = current.Rotate();
            }
        }
    }
}
