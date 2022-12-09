// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Numerics;

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/19</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 19, "Beacon Scanner", RequiresData = true, IsSlow = true)]
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
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>
    /// The number of beacons found in the specified scanner data and
    /// the largest Manhattan distance between any two scanners.
    /// </returns>
    public static (int BeaconCount, int LargestScannerDistance) FindBeacons(
        IList<string> data,
        CancellationToken cancellationToken = default)
    {
        List<Scanner> unoriented = Parse(data);

        var oriented = new List<Scanner>(unoriented.Count)
        {
            unoriented[0],
        };

        unoriented.Remove(unoriented[0]);

        while (unoriented.Count > 0 && !cancellationToken.IsCancellationRequested)
        {
            for (int i = 0; i < unoriented.Count; i++)
            {
                Scanner unaligned = unoriented[i];

                for (int j = 0; j < oriented.Count; j++)
                {
                    Scanner baseline = oriented[j];
                    Scanner? aligned = TryAlign(baseline, unaligned, cancellationToken);

                    if (aligned is not null)
                    {
                        oriented.Add(aligned);
                        unoriented.Remove(unaligned);
                        break;
                    }
                }
            }
        }

        cancellationToken.ThrowIfCancellationRequested();

        Scanner final = oriented[0];

        foreach (var other in oriented.Skip(1))
        {
            final.UnionWith(other);
        }

        float maxDistance = 0;

        for (int i = 0; i < oriented.Count - 1; i++)
        {
            for (int j = i + 1; j < oriented.Count; j++)
            {
                Vector3 x = oriented[i].Location;
                Vector3 y = oriented[j].Location;

                float distance = x.ManhattanDistance(y);

                if (distance > maxDistance)
                {
                    maxDistance = distance;
                }
            }
        }

        return (final.Count, (int)maxDistance);

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

        static Scanner? TryAlign(Scanner aligned, Scanner unaligned, CancellationToken cancellationToken)
        {
            // As we're looking for a commonality of at least 12 items
            // we can use the Pigeonhole Principle (hat-tip to @encse,
            // see https://en.wikipedia.org/wiki/Pigeonhole_principle)
            // to reduce the space of coordinates we need to search.
            const int CommonBeacons = 12;
            const float MaximumRange = 3000;

            foreach (Vector3 first in aligned.Skip(CommonBeacons))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                foreach (var transformation in Transforms())
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    var rotated = unaligned.Select(transformation);

                    int withinReach = rotated
                        .Where((p) => aligned.Location.ManhattanDistance(first - p) <= MaximumRange)
                        .Count();

                    if (withinReach < CommonBeacons)
                    {
                        break;
                    }

                    foreach (Vector3 second in rotated.Skip(CommonBeacons))
                    {
                        Vector3 delta = first - second;

                        if (aligned.Location.ManhattanDistance(delta) > MaximumRange)
                        {
                            // This beacon is too far from the other scanner to be detected by it
                            break;
                        }

                        var transformed = rotated.Select((p) => p + delta);

                        int count = 0;

                        foreach (Vector3 common in transformed.Intersect(aligned))
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

            static IEnumerable<Func<Vector3, Vector3>> Transforms()
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

        (BeaconCount, LargestScannerDistance) = FindBeacons(target, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("There are {0:N0} beacons.", BeaconCount);
            Logger.WriteLine("There largest Manhattan distance between two scanners is {0:N0}.", LargestScannerDistance);
        }

        return PuzzleResult.Create(BeaconCount, LargestScannerDistance);
    }

    private sealed class Scanner : HashSet<Vector3>
    {
        public Scanner()
            : base()
        {
        }

        public Scanner(IEnumerable<Vector3> collection)
            : base(collection)
        {
        }

        public int Id { get; init; }

        public Vector3 Location { get; init; }
    }
}
