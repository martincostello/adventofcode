﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/22</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 22, "Monkey Market", RequiresData = true)]
public sealed class Day22 : Puzzle
{
    /// <summary>
    /// Gets the sum of the 2,000th secret number for each buyer.
    /// </summary>
    public long SecretNumber2000Sum { get; private set; }

    /// <summary>
    /// Simulates the Monkey Market for the specified secret values.
    /// </summary>
    /// <param name="values">The initial secret number for each buyer.</param>
    /// <param name="rounds">The number of rounds to simulate for.</param>
    /// <returns>
    /// The sum of the <paramref name="rounds"/>-th secret number for each buyer.
    /// </returns>
    public static long Simulate(IList<string> values, int rounds)
    {
        long[] secrets = [.. values.Select(Parse<long>)];

        for (int i = 0; i < rounds; i++)
        {
            for (int j = 0; j < secrets.Length; j++)
            {
                secrets[j] = Next(secrets[j]);
            }
        }

        long sum = 0;

        for (int i = 0; i < secrets.Length; i++)
        {
            sum += secrets[i];
        }

        return sum;

        static long Next(long secret)
        {
            secret = Mix(secret * 64, secret);
            secret = Prune(secret);

            secret = Mix(secret / 32, secret);
            secret = Prune(secret);

            secret = Mix(secret * 2048, secret);
            secret = Prune(secret);

            return secret;
        }

        static long Mix(long value, long secret) => value ^ secret;

        static long Prune(long secret) => secret % 16777216;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        SecretNumber2000Sum = Simulate(values, rounds: 2000);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the 2000th secret number generated by each buyer is {0}.", SecretNumber2000Sum);
        }

        return PuzzleResult.Create(SecretNumber2000Sum);
    }
}
