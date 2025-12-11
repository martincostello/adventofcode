// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/8</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 08, "Playground", RequiresData = true, IsHidden = true, Unsolved = true)]
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

            for (int j = 0; j < boxes.Count; j++)
            {
                if (i == j)
                {
                    continue;
                }

                var right = boxes[j];

                float distance = Vector3.Distance(left, right);

                distances.Add((left, right, distance));
                distances.Add((right, left, distance));
            }
        }

        var sorted = distances.OrderBy((p) => p.Distance).ToList();
        var circuits = new List<HashSet<Vector3>>();

        // TODO Fix circuit connection processing
        for (int i = 0; i < sorted.Count; i++)
        {
            (var first, var second, _) = sorted[i];

            if (circuits.Any((p) => p.Contains(first) && p.Contains(second)))
            {
                continue;
            }

            var circuit =
                circuits.FirstOrDefault((p) => p.Contains(first) && !p.Contains(second)) ??
                circuits.FirstOrDefault((p) => !p.Contains(first) && p.Contains(second));

            if (circuit is null)
            {
                circuits.Add([first, second]);
            }
            else
            {
                circuit.Add(first);
                circuit.Add(second);
            }
        }

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
