// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2017/day/15</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2017, 15, "Dueling Generators", RequiresData = true, IsSlow = true)]
public sealed class Day15 : Puzzle
{
    /// <summary>
    /// Gets the judge's final count using version 1.
    /// </summary>
    public int FinalCountV1 { get; private set; }

    /// <summary>
    /// Gets the judge's final count using version 2.
    /// </summary>
    public int FinalCountV2 { get; private set; }

    /// <summary>
    /// Gets the number of values whose lowest 16 bits match when a sequence is generated 40,000,000 times.
    /// </summary>
    /// <param name="seeds">The seed values for the generator.</param>
    /// <param name="version">The version of the generator logic to use.</param>
    /// <returns>
    /// The number of matching pairs from the generated sequences.
    /// </returns>
    public static int GetMatchingPairs(IList<string> seeds, int version)
    {
        long a = Parse<long>(seeds[0].Split(' ')[^1]);
        long b = Parse<long>(seeds[1].Split(' ')[^1]);

        var valuesA = new Queue<long>();
        var valuesB = new Queue<long>();

        int limit = version switch
        {
            1 => 40_000_000,
            _ => 5_000_000,
        };

        int generated = 0;
        int pairs = 0;

        while (generated < limit)
        {
            a = GenerateA(a);
            b = GenerateB(b);

            if (version == 1)
            {
                if (IsPair(a, b))
                {
                    pairs++;
                }

                generated++;
            }
            else
            {
                if (a % 4 == 0)
                {
                    valuesA.Enqueue(a);
                }

                if (b % 8 == 0)
                {
                    valuesB.Enqueue(b);
                }

                if (valuesA.Count > 0 && valuesB.Count > 0)
                {
                    long comparandA = valuesA.Dequeue();
                    long comparandB = valuesB.Dequeue();

                    if (IsPair(comparandA, comparandB))
                    {
                        pairs++;
                    }

                    generated++;
                }
            }
        }

        return pairs;

        static bool IsPair(long first, long second)
            => (first & 0xffff) == (second & 0xffff);

        static long GenerateA(long previous)
        {
            long product = previous * 16807;
            return product % int.MaxValue;
        }

        static long GenerateB(long previous)
        {
            long product = previous * 48271;
            return product % int.MaxValue;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> input = await ReadResourceAsLinesAsync(cancellationToken);

        FinalCountV1 = GetMatchingPairs(input, version: 1);
        FinalCountV2 = GetMatchingPairs(input, version: 2);

        if (Verbose)
        {
            Logger.WriteLine($"The judge's final count using version 1 is {FinalCountV1:N0}.");
            Logger.WriteLine($"The judge's final count using version 2 is {FinalCountV2:N0}.");
        }

        return PuzzleResult.Create(FinalCountV1, FinalCountV2);
    }
}
