// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/22</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 22, "Sand Slabs", RequiresData = true)]
public sealed class Day22 : Puzzle
{
    /// <summary>
    /// Gets the number of bricks that could be safely chosen to be disintegrated.
    /// </summary>
    public int SafeBricks { get; private set; }

    /// <summary>
    /// Gets the sum of the number of bricks that would fall when removing each unsafe brick.
    /// </summary>
    public int MaximumChainReaction { get; private set; }

    /// <summary>
    /// Determines the number of bricks that could be safely chosen to be disintegrated.
    /// </summary>
    /// <param name="snapshot">The snapshot of bricks to analyze.</param>
    /// <returns>
    /// The number of bricks that could be safely chosen to be disintegrated
    /// and the sum of the number of bricks that would fall when removing each unsafe brick.
    /// </returns>
    public static (int Bricks, int ChainReaction) Disintegrate(IList<string> snapshot)
    {
        List<HashSet<Vector3>> bricks = [];

        foreach (string value in snapshot)
        {
            value.AsSpan().Bifurcate('~', out var first, out var second);

            var start = ParseVector(first);
            var end = ParseVector(second);

            var normal = Vector3.Normalize(end - start);

            HashSet<Vector3> brick = [start];

            while (start != end)
            {
                brick.Add(start += normal);
            }

            bricks.Add(brick);
        }

        (bricks, _) = Settle(bricks);

        int count = 0;
        int chainReaction = 0;

        foreach (var brick in bricks)
        {
            var remaining = bricks.Except([brick]).ToList();

            (_, int moved) = Settle(remaining);

            if (moved == 0)
            {
                count++;
            }
            else
            {
                chainReaction += moved;
            }
        }

        return (count, chainReaction);

        static Vector3 ParseVector(ReadOnlySpan<char> value)
        {
            (float x, float y, float z) = value.AsNumberTriple<float>();
            return new(x, y, z);
        }

        static (List<HashSet<Vector3>> Transformed, int Moved) Settle(List<HashSet<Vector3>> bricks)
        {
            const float Floor = 1;

            var gravity = -Vector3.UnitZ;

            var settled = new List<HashSet<Vector3>>(bricks.Count);
            var shape = new HashSet<Vector3>(bricks.Sum((p) => p.Count));

            int moved = 0;

            foreach (var brick in bricks.OrderBy((p) => p.Min((r) => r.Z)))
            {
                float height = brick.Min((p) => p.Z);
                var transform = Vector3.Zero;
                Vector3[]? cubes = null;

                for (float z = height; z > Floor; z--)
                {
                    cubes ??= [.. brick];

                    var next = transform + gravity;

                    bool intersects = false;

                    for (int i = 0; i < cubes.Length; i++)
                    {
                        if (shape.Contains(cubes[i] + next))
                        {
                            intersects = true;
                            break;
                        }
                    }

                    if (intersects)
                    {
                        break;
                    }

                    transform = next;
                }

                var transformed = brick;

                if (transform != Vector3.Zero)
                {
                    transformed = new HashSet<Vector3>(cubes!.Length);

                    for (int i = 0; i < cubes.Length; i++)
                    {
                        transformed.Add(cubes[i] + transform);
                    }

                    moved++;
                }

                shape.UnionWith(transformed);
                settled.Add(transformed);
            }

            return (settled, moved);
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var snapshot = await ReadResourceAsLinesAsync(cancellationToken);

        (SafeBricks, MaximumChainReaction) = Disintegrate(snapshot);

        if (Verbose)
        {
            Logger.WriteLine("{0} bricks could be safely chosen as the one to get disintegrated.", SafeBricks);
            Logger.WriteLine("The sum of the number of other bricks that would fall is {0}.", MaximumChainReaction);
        }

        return PuzzleResult.Create(SafeBricks, MaximumChainReaction);
    }
}
