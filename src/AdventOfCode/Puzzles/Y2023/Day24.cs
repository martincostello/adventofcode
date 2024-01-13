// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/24</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 24, "Never Tell Me The Odds", RequiresData = true, Unsolved = true)]
public sealed class Day24 : Puzzle
{
    /// <summary>
    /// Gets the number of hailstone intersections that occur within the test area.
    /// </summary>
    public int Intersections { get; private set; }

    /// <summary>
    /// Predicts the number of hailstone intersections that occur within the test area.
    /// </summary>
    /// <param name="hailstones">The hailstones to test for intersecting paths.</param>
    /// <param name="minimumPosition">The minimum x and y positions of the test area.</param>
    /// <param name="maximumPosition">The maximum x and y positions of the test area.</param>
    /// <returns>
    /// The number of hailstone intersections that occur within the test area.
    /// </returns>
    public static int Predict(
        IList<string> hailstones,
        long minimumPosition,
        long maximumPosition)
    {
        var stones = hailstones.Select((p) => Parse(p)).ToList();

        var bounds = new RectangleF(
            new PointF(minimumPosition, minimumPosition),
            new SizeF(
                maximumPosition - minimumPosition,
                maximumPosition - minimumPosition));

        int count = 0;

        foreach (var first in stones)
        {
            foreach (var second in stones)
            {
                if (first == second)
                {
                    continue;
                }

                if (Hailstone.Intersects(first, second) is { } intersection &&
                    bounds.Contains(new PointF(intersection.X, intersection.Y)))
                {
                    var t1 = (new Vector3(intersection.X, intersection.Y, intersection.Z) - first.Position) / first.Velocity;
                    var t2 = (new Vector3(intersection.X, intersection.Y, intersection.Z) - second.Position) / second.Velocity;

                    if (t1.X > 0 && t2.X > 0)
                    {
                        count++;
                    }
                }
            }
        }

        return count / 2;

        static Hailstone Parse(ReadOnlySpan<char> hailstone)
        {
            hailstone.Bifurcate('@', out var first, out var second);

            (float x, float y, float z) = first.AsNumberTriple<float>(',', NumberStyles.Float);

            var position = new Vector3(x, y, z);

            (x, y, z) = second.AsNumberTriple<float>(',', NumberStyles.Float);
            var velocity = new Vector3(x, y, z);

            return new(position, velocity);
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var hailstones = await ReadResourceAsLinesAsync(cancellationToken);

        Intersections = Predict(hailstones, 200_000_000_000_000, 400_000_000_000_000);

        if (Verbose)
        {
            Logger.WriteLine("{0} intersections occur within the test area.", Intersections);
        }

        return PuzzleResult.Create(Intersections);
    }

    private record struct Hailstone(Vector3 Position, Vector3 Velocity)
    {
        public static Vector3? Intersects(Hailstone x, Hailstone y)
        {
            var p1 = x.Position;
            var p2 = x.Position + x.Velocity;
            var p3 = y.Position;
            var p4 = y.Position + y.Velocity;

            float dx12 = p2.X - p1.X;
            float dy12 = p2.Y - p1.Y;
            float dx34 = p4.X - p3.X;
            float dy34 = p4.Y - p3.Y;

            float denominator = (dy12 * dx34) - (dx12 * dy34);

            float t1 = (((p1.X - p3.X) * dy34) + ((p3.Y - p1.Y) * dx34)) / denominator;

            if (float.IsInfinity(t1) || float.IsNaN(t1))
            {
                return null;
            }

            return new(p1.X + (dx12 * t1), p1.Y + (dy12 * t1), 0);
        }
    }
}
