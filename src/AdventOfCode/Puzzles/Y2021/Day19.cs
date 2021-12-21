// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/19</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 19, "Beacon Scanner", RequiresData = true)]
public sealed class Day19 : Puzzle
{
    /// <summary>
    /// Gets the number of beacons.
    /// </summary>
    public int BeaconCount { get; private set; }

    /// <summary>
    /// Gets the largest Manhattan distance between any two scanners.
    /// </summary>
    public int LargestScannerDistance { get; private set; }

    /// <summary>
    /// Finds the beacons from the specified scanner results.
    /// </summary>
    /// <param name="data">The scanner data to use to find the beacons.</param>
    /// <returns>
    /// The number of beacons found in the specified scanner data and
    /// the largest Manhattan distance between any two scanners.
    /// </returns>
    public static (int BeaconCount, int LargestScannerDistance) FindBeacons(IList<string> data)
    {
        List<Scanner> unoriented = Parse(data);

        var oriented = new List<Scanner>(unoriented.Count)
        {
            unoriented[0],
        };

        unoriented.Remove(unoriented[0]);

        while (unoriented.Count > 0)
        {
            for (int i = 0; i < unoriented.Count; i++)
            {
                Scanner unaligned = unoriented[i];

                for (int j = 0; j < oriented.Count; j++)
                {
                    Scanner baseline = oriented[j];
                    Scanner? aligned = TryAlign(baseline, unaligned);

                    if (aligned is not null)
                    {
                        oriented.Add(aligned);
                        unoriented.Remove(unaligned);
                        break;
                    }
                }
            }
        }

        Scanner final = oriented[0];

        foreach (var other in oriented.Skip(1))
        {
            final.UnionWith(other);
        }

        int maxDistance = 0;

        for (int i = 0; i < oriented.Count - 1; i++)
        {
            for (int j = i + 1; j < oriented.Count; j++)
            {
                Point3D x = oriented[i].Location;
                Point3D y = oriented[j].Location;

                int distance = Point3D.ManhattanDistance(x, y);

                if (distance > maxDistance)
                {
                    maxDistance = distance;
                }
            }
        }

        return (final.Count, maxDistance);

        static List<Scanner> Parse(IList<string> data)
        {
            var scanners = new List<Scanner>();
            var current = new Scanner() { Id = 0 };

            foreach (string value in data.Skip(1))
            {
                if (string.IsNullOrEmpty(value))
                {
                    scanners.Add(current);
                    continue;
                }

                if (value.StartsWith("---", StringComparison.Ordinal))
                {
                    current = new() { Id = current.Id + 1 };
                    continue;
                }

                int[] values = value.AsNumbers<int>().ToArray();

                current.Add(new(values[0], values[1], values[2]));
            }

            scanners.Add(current);

            return scanners;
        }

        static Scanner? TryAlign(Scanner aligned, Scanner unaligned)
        {
            // As we're looking for a commonality of at least 12 items
            // we can use the Pigeonhole Principle (hat-tip to @encse,
            // see https://en.wikipedia.org/wiki/Pigeonhole_principle)
            // to reduce the space of coordinates we need to search.
            const int CommonBeacons = 12;
            const int MaximumRange = 3000;

            foreach (Point3D first in aligned.Skip(CommonBeacons))
            {
                foreach (var transformation in Transforms())
                {
                    var rotated = unaligned.Select(transformation);

                    int withinReach = rotated
                        .Where((p) => Point3D.ManhattanDistance(aligned.Location, first - p) <= MaximumRange)
                        .Count();

                    if (withinReach < CommonBeacons)
                    {
                        break;
                    }

                    foreach (Point3D second in rotated.Skip(CommonBeacons))
                    {
                        Point3D delta = first - second;

                        if (Point3D.ManhattanDistance(aligned.Location, delta) > MaximumRange)
                        {
                            // This beacon is too far from the other scanner to be detected by it
                            break;
                        }

                        var transformed = rotated.Select((p) => p + delta);

                        int count = 0;

                        foreach (Point3D common in transformed.Intersect(aligned))
                        {
                            if (++count == CommonBeacons)
                            {
                                return new Scanner(transformed)
                                {
                                    Id = unaligned.Id,
                                    Location = transformation(unaligned.Location) + delta,
                                };
                            }
                        }
                    }
                }
            }

            return null;

            static IEnumerable<Func<Point3D, Point3D>> Transforms()
            {
                yield return (p) => new(p.X, -p.Y, -p.Z);
                yield return (p) => new(p.X, p.Z, -p.Y);
                yield return (p) => new(p.X, -p.Z, p.Y);
                yield return (p) => new(-p.X, -p.Y, p.Z);
                yield return (p) => new(-p.X, p.Y, -p.Z);
                yield return (p) => new(-p.X, p.Z, p.Y);
                yield return (p) => new(-p.X, -p.Z, -p.Y);
                yield return (p) => new(p.Y, p.Z, p.X);
                yield return (p) => new(p.Y, -p.Z, -p.X);
                yield return (p) => new(p.Y, p.X, -p.Z);
                yield return (p) => new(p.Y, -p.X, p.Z);
                yield return (p) => new(-p.Y, -p.Z, p.X);
                yield return (p) => new(-p.Y, p.Z, -p.X);
                yield return (p) => new(-p.Y, -p.X, -p.Z);
                yield return (p) => new(-p.Y, p.X, p.Z);
                yield return (p) => new(p.Z, p.X, p.Y);
                yield return (p) => new(p.Z, -p.X, -p.Y);
                yield return (p) => new(p.Z, p.Y, -p.X);
                yield return (p) => new(p.Z, -p.Y, p.X);
                yield return (p) => new(-p.Z, -p.X, p.Y);
                yield return (p) => new(-p.Z, p.X, -p.Y);
                yield return (p) => new(-p.Z, p.Y, p.X);
                yield return (p) => new(-p.Z, -p.Y, -p.X);
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> target = await ReadResourceAsLinesAsync();

        (BeaconCount, LargestScannerDistance) = FindBeacons(target);

        if (Verbose)
        {
            Logger.WriteLine("There are {0:N0} beacons.", BeaconCount);
            Logger.WriteLine("There largest Manhattan distance between two scanners is {0:N0}.", LargestScannerDistance);
        }

        return PuzzleResult.Create(BeaconCount, LargestScannerDistance);
    }

    [System.Diagnostics.DebuggerDisplay("({X}, {Y}, {Z})")]
    private readonly struct Point3D : IEquatable<Point3D>
    {
        public static readonly Point3D Zero = new(0, 0, 0);

        public readonly int X;

        public readonly int Y;

        public readonly int Z;

        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Point3D operator +(Point3D a, Point3D b)
            => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static Point3D operator -(Point3D a, Point3D b)
            => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static bool operator ==(Point3D a, Point3D b)
            => a.X == b.X && a.Y == b.Y && a.Z == b.Z;

        public static bool operator !=(Point3D a, Point3D b)
            => a.X != b.X || a.Y != b.Y || a.Z != b.Z;

        public static int ManhattanDistance(Point3D a, Point3D b)
        {
            Point3D delta = a - b;
            return Math.Abs(delta.X) + Math.Abs(delta.Y) + Math.Abs(delta.Z);
        }

        public override readonly bool Equals(object? obj)
            => obj is Point3D point && this == point;

        public readonly bool Equals(Point3D other)
            => this == other;

        public override readonly int GetHashCode()
            => HashCode.Combine(X, Y, Z);
    }

    private sealed class Scanner : HashSet<Point3D>
    {
        public Scanner()
            : base()
        {
        }

        public Scanner(IEnumerable<Point3D> collection)
            : base(collection)
        {
        }

        public int Id { get; init; }

        public Point3D Location { get; init; }
    }
}
