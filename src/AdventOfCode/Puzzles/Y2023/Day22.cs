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
    public int DisintegratedBricks { get; private set; }

    /// <summary>
    /// Determines the number of bricks that could be safely chosen to be disintegrated.
    /// </summary>
    /// <param name="snapshot">The snapshot of bricks to analyze.</param>
    /// <returns>
    /// The number of bricks that could be safely chosen to be disintegrated.
    /// </returns>
    public static int Disintegrate(IList<string> snapshot)
    {
        List<HashSet<Vector3>> bricks = [];

        foreach (string value in snapshot)
        {
            value.AsSpan().Bifurcate('~', out var first, out var last);

            first.Trifurcate(',', out var x, out var y, out var z);
            var start = new Vector3(Parse<float>(x), Parse<float>(y), Parse<float>(z));

            last.Trifurcate(',', out x, out y, out z);
            var end = new Vector3(Parse<float>(x), Parse<float>(y), Parse<float>(z));

            var normal = Vector3.Normalize(end - start);

            HashSet<Vector3> brick = [start];

            while (start != end)
            {
                start += normal;
                brick.Add(start);
            }

            bricks.Add(brick);
        }

        bricks = Settle(bricks);

        var settled = new HashSet<Vector3>(bricks.SelectMany((p) => p));

        int count = 0;

        foreach (var brick in bricks)
        {
            var disintegrated = new HashSet<Vector3>(settled);
            disintegrated.ExceptWith(brick);

            var arranged = Settle(bricks.Except([brick]));

            if (disintegrated.SetEquals(arranged.SelectMany((p) => p)))
            {
                count++;
            }
        }

        return count;

        static List<HashSet<Vector3>> Settle(IEnumerable<HashSet<Vector3>> bricks)
        {
            const float Floor = 1;

            var gravity = -Vector3.UnitZ;

            var settled = new List<HashSet<Vector3>>();
            var shape = new HashSet<Vector3>(bricks.Sum((p) => p.Count));

            foreach (var brick in bricks.OrderBy((p) => p.Min((r) => r.Z)))
            {
                float height = brick.Min((p) => p.Z);
                var transform = Vector3.Zero;

                for (float z = height; z > Floor; z--)
                {
                    var next = transform + gravity;

                    if (shape.Overlaps(brick.Select((p) => p + next)))
                    {
                        break;
                    }

                    transform = next;
                }

                if (transform != Vector3.Zero)
                {
                    var transformed = new HashSet<Vector3>(brick.Select((p) => p + transform));
                    shape.UnionWith(transformed);
                    settled.Add(transformed);
                }
                else
                {
                    shape.UnionWith(brick);
                    settled.Add(brick);
                }
            }

            return settled;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var snapshot = await ReadResourceAsLinesAsync(cancellationToken);

        DisintegratedBricks = Disintegrate(snapshot);

        if (Verbose)
        {
            Logger.WriteLine("{0} bricks could be safely chosen as the one to get disintegrated.", DisintegratedBricks);
        }

        return PuzzleResult.Create(DisintegratedBricks);
    }
}
