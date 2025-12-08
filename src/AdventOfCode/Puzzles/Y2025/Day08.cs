// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/8</c>. This class cannot be inherited.
/// </summary>
public sealed class Day08 : Puzzle<int>
{
    /// <summary>
    /// Connects the specified function boxes together as distinct circuits.
    /// </summary>
    /// <param name="values">The values to solve the puzzle from.</param>
    /// <returns>
    /// The product of the sizes of the three largest circuits.
    /// </returns>
    public static int Connect(IReadOnlyList<string> values)
    {
        var all = new List<Vector3>();

        foreach (string item in values)
        {
            (string x, string y, string z) = item.Trifurcate(',');
            all.Add(new(Parse<float>(x), Parse<float>(y), Parse<float>(z)));
        }

        var circuits = new List<HashSet<Vector3>>();

        // TODO
        return circuits
            .OrderByDescending((p) => p.Count)
            .Take(3)
            .Aggregate(1, (x, y) => x * y.Count);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (values, logger, _) =>
            {
                int product = Connect(values);

                if (logger is { })
                {
                    logger.WriteLine("The product of the sizes of the three largest circuits is {0}.", product);
                }

                return product;
            },
            cancellationToken);
    }
}
