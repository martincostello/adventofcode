// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2020/day/10</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2020, 10, "Adapter Array", RequiresData = true)]
public sealed class Day10 : Puzzle
{
    /// <summary>
    /// Gets the product of the 1-jolt and 3-jolt differences of the specified adapters.
    /// </summary>
    public int JoltageProduct { get; private set; }

    /// <summary>
    /// Gets the number of distinct valid arrangements of adapters.
    /// </summary>
    public long ValidArrangements { get; private set; }

    /// <summary>
    /// Gets the product of the 1-jolt and 3-jolt differences when the adapters
    /// with the specified joltage ratings are linked together in series.
    /// </summary>
    /// <param name="joltageRatings">The joltage ratings of the adapters.</param>
    /// <returns>
    /// The product of the 1-jolt and 3-jolt differences.
    /// </returns>
    public static int GetJoltageProduct(IEnumerable<int> joltageRatings)
    {
        // Sort the ratings so the search space is efficient
        var sorted = joltageRatings.ToList();
        sorted.Sort();

        int maxRating = sorted[^1];

        // Start with the charging outlet with a joltage of 0
        var chain = new Stack<int>();
        chain.Push(0);

        foreach (int adapter in JoltageCandidates(chain))
        {
            chain.Push(adapter);

            if (ContainsPathToTarget(chain))
            {
                break;
            }

            chain.Pop();
        }

        var deltas = new Dictionary<int, int>(3)
        {
            [1] = 0,
            [2] = 0,
            [3] = 1, // For the built-in device's joltage
        };

        int last = chain.Pop();

        while (chain.TryPop(out int value))
        {
            deltas[last - value]++;
            last = value;
        }

        return deltas[1] * deltas[3];

        IEnumerable<int> JoltageCandidates(Stack<int> path)
        {
            int previous = path.Peek();

            return sorted
                .Where((p) => !path.Contains(p))
                .Where((p) => p >= previous + 1)
                .Where((p) => p <= previous + 3);
        }

        bool ContainsPathToTarget(Stack<int> path)
        {
            int previous = path.Peek();

            if (previous == maxRating)
            {
                return path.Count == sorted.Count + 1;
            }

            foreach (int candidate in JoltageCandidates(path))
            {
                path.Push(candidate);

                if (ContainsPathToTarget(path))
                {
                    return true;
                }

                path.Pop();
            }

            return false;
        }
    }

    /// <summary>
    /// Gets the total number of distinct valid arrangements of adapters.
    /// </summary>
    /// <param name="joltageRatings">The joltage ratings of the adapters.</param>
    /// <returns>
    /// The number of distinct valid arrangements of the adapters.
    /// </returns>
    public static long GetValidArrangements(IEnumerable<int> joltageRatings)
    {
        var sorted = joltageRatings.ToList();
        sorted.Sort();

        // Adapted from https://github.com/thatsumoguy/Advent-of-Code-2020.
        // My original solution was brute force and worked out the valid
        // paths and just took to long to work it all out. It worked fine
        // with the examples and solved them, but never completed the file.
        // See https://www.reddit.com/r/adventofcode/comments/kadp4g/why_does_this_code_work_and_what_does_it_do/gf9pixm/
        sorted.Insert(0, 0);

        long[] steps = new long[sorted.Count];
        steps[0] = 1;

        for (int i = 1; i < sorted.Count; i++)
        {
            for (int j = 0; j < i; j++)
            {
                if (sorted[i] - sorted[j] <= 3)
                {
                    steps[i] += steps[j];
                }
            }
        }

        return steps[^1];
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var joltages = await ReadResourceAsNumbersAsync<int>(cancellationToken);

        JoltageProduct = GetJoltageProduct(joltages);
        ValidArrangements = GetValidArrangements(joltages);

        if (Verbose)
        {
            Logger.WriteLine("The product of the 1-jolt differences and 3-jolt differences is {0}.", JoltageProduct);
            Logger.WriteLine("The total number of distinct ways to arrange the adapters is {0}.", ValidArrangements);
        }

        return PuzzleResult.Create(JoltageProduct, ValidArrangements);
    }
}
