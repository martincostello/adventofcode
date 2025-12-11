// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/9</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 09, "Movie Theater", RequiresData = true, IsHidden = true, Unsolved = true)]
public sealed class Day09 : Puzzle<long>
{
    /// <summary>
    /// Solves the puzzle.
    /// </summary>
    /// <param name="values">The values to solve the puzzle from.</param>
    /// <returns>
    /// The solution.
    /// </returns>
    public static long FindLargestRectangle(IReadOnlyList<string> values)
    {
        var points = new List<Point>();

        foreach (string value in values)
        {
            (string x, string y) = value.Bifurcate(',');
            points.Add(new(Parse<int>(x), Parse<int>(y)));
        }

        var pairs = new List<(Point A, Point B)>();

        for (int i = 0; i < points.Count; i++)
        {
            var left = points[i];

            for (int j = 0; j < points.Count; j++)
            {
                if (i != j)
                {
                    pairs.Add((left, points[j]));
                }
            }
        }

        long area = 0;

        foreach ((var a, var b) in pairs)
        {
            var bounds = new Rectangle(
                Math.Min(a.X, b.X),
                Math.Min(a.Y, b.Y),
                Math.Abs(a.X - b.X) + 1,
                Math.Abs(a.Y - b.Y) + 1);

            area = Math.Max((long)bounds.Width * bounds.Height, area);
        }

        return area;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (values, logger, _) =>
            {
                long solution = FindLargestRectangle(values);

                if (logger is { })
                {
                    logger.WriteLine("The largest area of any rectangle is {0}.", solution);
                }

                return solution;
            },
            cancellationToken);
    }
}
