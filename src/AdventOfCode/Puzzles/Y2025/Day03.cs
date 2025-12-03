// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/3</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 03, "Lobby", RequiresData = true)]
public sealed class Day03 : Puzzle
{
    /// <summary>
    /// Gets the total output joltage.
    /// </summary>
    public int TotalOutputJoltage { get; private set; }

    /// <summary>
    /// Gets the sum of the maximum joltage produced by the specified battery banks.
    /// </summary>
    /// <param name="batteryBanks">The battery banks to get the joltage for.</param>
    /// <returns>
    /// The sum of the invalid product IDs.
    /// </returns>
    public static int GetJoltage(IReadOnlyList<string> batteryBanks)
    {
        int total = 0;

        foreach (ReadOnlySpan<char> bank in batteryBanks)
        {
            char max = Max(bank);
            int index = bank.LastIndexOf(max);

            var prefix = bank[..index];
            var suffix = bank[(index + 1)..];

            int maxPrefix = Max(prefix);
            int maxSuffix = Max(suffix);

            (int Tens, int Units) digits =
                maxSuffix is 0 || maxPrefix >= max ?
                (maxPrefix, max) :
                (max, maxSuffix);

            total += (10 * (digits.Tens - '0')) + (digits.Units - '0');
        }

        return total;

        static char Max(ReadOnlySpan<char> span)
        {
            int max = 0;

            foreach (char c in span)
            {
                max = Math.Max(c, max);
            }

            return (char)max;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var batteryBanks = await ReadResourceAsLinesAsync(cancellationToken);

        TotalOutputJoltage = GetJoltage(batteryBanks);

        if (Verbose)
        {
            Logger.WriteLine("The total output joltage is {0}", TotalOutputJoltage);
        }

        return PuzzleResult.Create(TotalOutputJoltage);
    }
}
