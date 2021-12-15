// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/4</c>. This class cannot be inherited.
/// </summary>
[Puzzle("The Ideal Stocking Stuffer", 2015, 04, MinimumArguments = 2)]
public sealed class Day04 : Puzzle
{
    /// <summary>
    /// Gets the lowest value that produces a hash that starts with the required number of zeroes.
    /// </summary>
    internal int LowestZeroHash { get; private set; }

    /// <summary>
    /// Gets the lowest positive integer which when combined with a secret key has an MD5 hash whose
    /// hexadecimal representation starts with the specified number of zeroes.
    /// </summary>
    /// <param name="secretKey">The secret key to use.</param>
    /// <param name="zeroes">The number of zeroes to get the value for.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that represents the asynchronous operation to find the
    /// lowest positive integer that generates an MD5 hash with the number of zeroes specified.
    /// </returns>
    internal static async Task<int> GetLowestPositiveNumberWithStartingZeroesAsync(string secretKey, int zeroes)
    {
        var solutions = new ConcurrentBag<int>();
        var searchedRanges = new ConcurrentBag<int>();

        int fromInclusive = 1;
        int rangeSize = 50000;

        var chunks = Enumerable.Chunk(Enumerable.Range(fromInclusive, int.MaxValue - 1), rangeSize);
        using var cts = new CancellationTokenSource();

        try
        {
            await Parallel.ForEachAsync(
                chunks,
                cts.Token,
                (range, cancellationToken) =>
                {
                    try
                    {
                        // Does this range start at a value greater than an already found value?
                        if (!solutions.IsEmpty)
                        {
                            int bestSolution = solutions.Min();

                            if (range[0] > bestSolution)
                            {
                                var orderedRanges = searchedRanges.ToList();

                                if (orderedRanges.Count == 0)
                                {
                                    return ValueTask.CompletedTask;
                                }

                                orderedRanges.Sort();

                                // Have we searched the first possible range already?
                                if (orderedRanges[0] == fromInclusive)
                                {
                                    for (int i = 1; i < orderedRanges.Count; i++)
                                    {
                                        int lastRange = orderedRanges[i - 1];
                                        int thisRange = orderedRanges[i];

                                        // Is this range the next range?
                                        if (thisRange != lastRange + rangeSize)
                                        {
                                            // A range before the current best solution has not been searched yet
                                            break;
                                        }

                                        if (thisRange > bestSolution)
                                        {
                                            // We have found the best solution
                                            cts.Cancel();
                                        }
                                    }
                                }
                            }

                            return ValueTask.CompletedTask;
                        }

                        foreach (int i in range)
                        {
                            if (cancellationToken.IsCancellationRequested)
                            {
                                break;
                            }

                            if (IsSolution(i, secretKey, zeroes))
                            {
                                solutions.Add(i);
                                break;
                            }
                        }
                    }
                    finally
                    {
                        searchedRanges.Add(range[0]);
                    }

                    return ValueTask.CompletedTask;
                });
        }
        catch (TaskCanceledException)
        {
            // Solution found
        }

        if (solutions.IsEmpty)
        {
            throw new PuzzleException("No answer was found for the specified secret key.");
        }

        return solutions.Min();
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        string secretKey = args[0];
        int zeroes = Parse<int>(args[1]);

        LowestZeroHash = await GetLowestPositiveNumberWithStartingZeroesAsync(secretKey, zeroes);

        if (Verbose)
        {
            Logger.WriteLine(
                "The lowest positive number for a hash starting with {0} zeroes is {1:N0}.",
                zeroes,
                LowestZeroHash);
        }

        return PuzzleResult.Create(LowestZeroHash);
    }

    /// <summary>
    /// Returns whether the specified integer is a solution for the specified secret key and number of zeroes.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="secretKey">The secret key to use.</param>
    /// <param name="zeroes">The number of zeroes to get the value for.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is a solution; otherwise <see langword="false"/>.
    /// </returns>
    private static bool IsSolution(int value, string secretKey, int zeroes)
    {
        string formatted = secretKey + value.ToString(CultureInfo.InvariantCulture);
        byte[] buffer = Encoding.UTF8.GetBytes(formatted);
        byte[] hash = MD5.HashData(buffer);

        (int wholeBytes, int remainder) = Math.DivRem(zeroes, 2);
        bool hasHalfByte = remainder == 1;

        int sum = hash[0];

        // Are the whole bytes all zero?
        for (int j = 1; sum == 0 && j < wholeBytes; j++)
        {
            sum += hash[j];
        }

        if (sum == 0)
        {
            // The current value is a solution if there is an even number
            // of zeroes of if the low bits of the odd byte are zero.
            if (!hasHalfByte || hash[wholeBytes] < 0x10)
            {
                return true;
            }
        }

        return false;
    }
}
