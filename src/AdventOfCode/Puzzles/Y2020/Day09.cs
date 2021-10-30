// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2020/day/9</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2020, 09, RequiresData = true)]
public sealed class Day09 : Puzzle
{
    /// <summary>
    /// Gets the first weak number in the signal.
    /// </summary>
    public long WeakNumber { get; private set; }

    /// <summary>
    /// Gets the encryption weakness.
    /// </summary>
    public long Weakness { get; private set; }

    /// <summary>
    /// Gets the first weak number from the specified XMAS signal.
    /// </summary>
    /// <param name="values">The values of the signal.</param>
    /// <param name="preambleLength">The length of the preamble.</param>
    /// <returns>
    /// The first weak number in the XMAS signal or -1 if it is secure.
    /// </returns>
    public static long GetWeakNumber(ReadOnlySpan<long> values, int preambleLength)
    {
        for (int i = preambleLength; i < values.Length; i++)
        {
            long current = values[i];

            long[] preamble = values
                .Slice(i - preambleLength, preambleLength)
                .ToArray();

            bool isValid = Maths.GetPermutations(preamble, 2)
                .Where((p) => p.Sum() == current)
                .Where((p) => p.ElementAt(0) != p.ElementAt(1))
                .Any();

            if (!isValid)
            {
                return current;
            }
        }

        return -1;
    }

    /// <summary>
    /// Gets the encryption weakness from the specified XMAS signal.
    /// </summary>
    /// <param name="values">The values of the signal.</param>
    /// <param name="weakNumber">The weak number of the sequence.</param>
    /// <returns>
    /// The encryption weakness in the XMAS signal or -1 if it is secure.
    /// </returns>
    public static long GetWeakness(ReadOnlySpan<long> values, long weakNumber)
    {
        var sequence = new List<long>(values.Length);

        for (int i = 0; i < values.Length; i++)
        {
            long first = values[i];
            sequence.Add(first);

            long sum = first;

            for (int j = i + 1; j < values.Length && sum < weakNumber; j++)
            {
                long next = values[j];
                sum += next;
                sequence.Add(next);
            }

            if (sum == weakNumber && sequence.Count > 1)
            {
                return sequence.Min() + sequence.Max();
            }

            sequence.Clear();
        }

        return -1;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        long[] values = (await ReadResourceAsSequenceAsync<long>()).ToArray();

        WeakNumber = GetWeakNumber(values, 25);
        Weakness = GetWeakness(values, WeakNumber);

        if (Verbose)
        {
            Logger.WriteLine("The first weak number in the XMAS sequence is {0}.", WeakNumber);
            Logger.WriteLine("The encryption weakness of the XMAS sequence is {0}.", Weakness);
        }

        return PuzzleResult.Create(WeakNumber, Weakness);
    }
}
