// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/8</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 08, "Playground", RequiresData = true)]
public sealed class Day08 : Puzzle<int, int>
{
    /// <summary>
    /// Connects the specified function boxes together as distinct circuits.
    /// </summary>
    /// <param name="values">The values to solve the puzzle from.</param>
    /// <param name="pairs">The maximum number of junction boxes to pair.</param>
    /// <returns>
    /// The product of the sizes of the three largest circuits and the product
    /// of the X coordinates of the last two junction boxes to be connected.
    /// </returns>
    public static (int Top3Product, int LastPairXProduct) Connect(IReadOnlyList<string> values, int pairs)
    {
        var boxes = new List<Vector3>();

        foreach (string item in values)
        {
            (string x, string y, string z) = item.Trifurcate(',');
            boxes.Add(new(Parse<float>(x), Parse<float>(y), Parse<float>(z)));
        }

        var distances = new HashSet<(Vector3 Left, Vector3 Right, float Distance)>();

        for (int i = 0; i < boxes.Count; i++)
        {
            var left = boxes[i];

            for (int j = i + 1; j < boxes.Count; j++)
            {
                var right = boxes[j];

                float distance = Vector3.Distance(left, right);

                distances.Add((left, right, distance));
            }
        }

        var sorted = distances.OrderBy((p) => p.Distance).ToList();
        var circuits = new List<HashSet<Vector3>>();

        int top3Product = Unsolved;
        int lastPairXProduct = Unsolved;

        for (int i = 0; i < sorted.Count; i++)
        {
            if (i == pairs)
            {
                top3Product = circuits
                    .OrderByDescending((p) => p.Count)
                    .Take(3)
                    .Aggregate(1, (x, y) => x * y.Count);
            }

            (var first, var second, _) = sorted[i];

            if (circuits.Any((p) => p.Contains(first) && p.Contains(second)))
            {
                continue;
            }

            var leftJunction = circuits.FirstOrDefault((p) => p.Contains(first));
            var rightJunction = circuits.FirstOrDefault((p) => p.Contains(second));

            if (leftJunction is null && rightJunction is null)
            {
                circuits.Add([first, second]);
            }
            else if (leftJunction is { } && rightJunction is { })
            {
                leftJunction.UnionWith(rightJunction);
                circuits.Remove(rightJunction);
            }
            else if (leftJunction is { })
            {
                leftJunction.Add(second);
            }
            else if (rightJunction is { })
            {
                rightJunction.Add(first);
            }

            if (circuits.Count == 1)
            {
                lastPairXProduct = (int)first.X * (int)second.X;
            }
        }

        return (top3Product, lastPairXProduct);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (values, logger, _) =>
            {
                (int top3Product, int lastPairXProduct) = Connect(values, pairs: 1_000);

                if (logger is { })
                {
                    logger.WriteLine("The product of the sizes of the three largest circuits is {0}.", top3Product);
                    logger.WriteLine("The product of the X coordinates of the last two junction boxes to connect is {0}.", lastPairXProduct);
                }

                return (top3Product, lastPairXProduct);
            },
            cancellationToken);
    }
}
