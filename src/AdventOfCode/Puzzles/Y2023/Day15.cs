// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/15</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 15, "Lens Library", RequiresData = true)]
public sealed class Day15 : Puzzle
{
    /// <summary>
    /// Gets the sum of the hash values of the initialization sequence.
    /// </summary>
    public int HashSum { get; private set; }

    /// <summary>
    /// Computes the hash of the specified initialization sequence.
    /// </summary>
    /// <param name="initializationSequence">The sequence to hash.</param>
    /// <returns>
    /// The sum of the hash values of <paramref name="initializationSequence"/>.
    /// </returns>
    public static int Hash(ReadOnlySpan<char> initializationSequence)
    {
        int sum = 0;
        int next;

        while ((next = initializationSequence.IndexOf(',')) != -1)
        {
            sum += Hash(initializationSequence[..next]);
            initializationSequence = initializationSequence[(next + 1)..];
        }

        sum += Hash(initializationSequence);

        return sum;

        static int Hash(ReadOnlySpan<char> value)
        {
            int hash = 0;

            for (int i = 0; i < value.Length; i++)
            {
                hash += value[i];
                hash *= 17;
                hash %= 256;
            }

            return hash;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        string initializationSequence = await ReadResourceAsStringAsync(cancellationToken);

        HashSum = Hash(initializationSequence.ReplaceLineEndings(string.Empty));

        if (Verbose)
        {
            Logger.WriteLine("The sum of the hash values of the initialization sequence is {0}", HashSum);
        }

        return PuzzleResult.Create(HashSum);
    }
}
