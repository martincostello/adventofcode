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
    /// Digs out the lagoon specified in the plan and returns its volume.
    /// </summary>
    /// <param name="plan">The plan to dig the perimeter of the lagoon.</param>
    /// <returns>
    /// The volume of lava the lagoon can hold.
    /// </returns>
    public static long Dig(IList<string> plan)
    {
        var vertex = new Point(0, 0);
        var vertices = new List<Point>(plan.Count);

        int perimeter = 0;

        foreach (string instruction in plan)
        {
            instruction.AsSpan().Trifurcate(' ', out var direction, out var distance, out _);

            int steps = Parse<int>(distance);
            vertex += direction[0] switch
            {
                'U' => new(0, -steps),
                'D' => new(0, steps),
                'L' => new(-steps, 0),
                'R' => new(steps, 0),
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
                .Pairwise((i, j) => i.Cross(j))
                .Aggregate(1L, (x, y) => x + y);

            long area = Math.Abs(crossProductSum) / 2;
            area += (perimeter / 2) + 1;
            return area;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var plan = await ReadResourceAsLinesAsync(cancellationToken);

        Volume = Dig(plan);

        if (Verbose)
        {
            Logger.WriteLine("The lagoon can hold {0} cubic meters of lava.", Volume);
        }

        return PuzzleResult.Create(Volume);
    }
}
