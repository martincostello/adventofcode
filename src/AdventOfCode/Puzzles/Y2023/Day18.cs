// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/18</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 18, "Lavaduct Lagoon", RequiresData = true, IsHidden = true)]
public sealed class Day18 : Puzzle
{
    /// <summary>
    /// Gets the volume of lava the lagoon can hold.
    /// </summary>
    public long Volume { get; private set; }

    /// <summary>
    /// Gets the volume of lava the lagoon can hold using the fixed plan.
    /// </summary>
    public long VolumeWithFix { get; private set; }

    /// <summary>
    /// Digs out the lagoon specified in the plan and returns its volume.
    /// </summary>
    /// <param name="plan">The plan to dig the perimeter of the lagoon.</param>
    /// <param name="fix">Whether to fix the instructions before digging the lagoon.</param>
    /// <returns>
    /// The volume of lava the lagoon can hold.
    /// </returns>
    public static long Dig(IList<string> plan, bool fix)
    {
        var vertex = new Point(0, 0);
        var vertices = new List<Point>(plan.Count);

        int perimeter = 0;

        foreach (string instruction in plan)
        {
            instruction.AsSpan().Trifurcate(' ', out var direction, out var distance, out var color);

            int steps;

            if (fix)
            {
                steps = Parse<int>(color.Slice(2, 5), NumberStyles.HexNumber);
                direction = color[^2..];
            }
            else
            {
                steps = Parse<int>(distance);
            }

            vertex += direction[0] switch
            {
                'U' or '3' => new(0, -steps),
                'D' or '1' => new(0, steps),
                'L' or '2' => new(-steps, 0),
                'R' or '0' => new(steps, 0),
                _ => throw new PuzzleException($"Unknown direction '{direction}'."),
            };

            perimeter += steps;
            vertices.Add(vertex);
        }

        return Area(vertices, perimeter);

        static long Area(List<Point> vertices, int perimeter)
        {
            // https://en.wikipedia.org/wiki/Shoelace_formula#Generalization
            long crossProductSum = vertices
                .Pairwise(CrossProduct)
                .Aggregate(1L, (x, y) => x + y);

            long area = Math.Abs(crossProductSum) / 2;
            area += (perimeter / 2) + 1;
            return area;
        }

        static long CrossProduct(Point i, Point j)
        {
            long ix = i.X;
            long jx = j.X;

            return (ix * j.Y) - (jx * i.Y);
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var plan = await ReadResourceAsLinesAsync(cancellationToken);

        Volume = Dig(plan, fix: false);
        VolumeWithFix = Dig(plan, fix: true);

        if (Verbose)
        {
            Logger.WriteLine("The lagoon can hold {0} cubic meters of lava.", Volume);
            Logger.WriteLine("The lagoon can hold {0} cubic meters of lava using the fixed plan.", VolumeWithFix);
        }

        return PuzzleResult.Create(Volume, VolumeWithFix);
    }
}
