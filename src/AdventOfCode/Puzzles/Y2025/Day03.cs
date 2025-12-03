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
    /// Gets the total output joltage for two batteries.
    /// </summary>
    public long TotalOutputJoltageFor2 { get; private set; }

    /// <summary>
    /// Gets the total output joltage for twelve batteries.
    /// </summary>
    public long TotalOutputJoltageFor12 { get; private set; }

    /// <summary>
    /// Gets the sum of the maximum joltage produced by the specified battery banks.
    /// </summary>
    /// <param name="batteryBanks">The battery banks to get the joltage for.</param>
    /// <param name="batteries">The number of batteries to use from each bank.</param>
    /// <returns>
    /// The total output joltage.
    /// </returns>
    public static long GetJoltage(IReadOnlyList<string> batteryBanks, int batteries)
    {
        long total = 0;

        foreach (ReadOnlySpan<char> bank in batteryBanks)
        {
            total += FindMaximum(bank, batteries, start: 0);
        }

        return total;

        static long FindMaximum(ReadOnlySpan<char> digits, int batteries, int start)
        {
            if (batteries is 0)
            {
                return 0;
            }

            long tens = batteries - 1;
            long maximum = 0;

            for (int i = start; i < digits.Length - batteries + 1; i++)
            {
                long current = (long)Math.Pow(10, tens) * (digits[i] - '0');

                if (current >= maximum)
                {
                    current += FindMaximum(digits, batteries - 1, i + 1);
                    maximum = Math.Max(current, maximum);
                }
            }

            return maximum;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var batteryBanks = await ReadResourceAsLinesAsync(cancellationToken);

        TotalOutputJoltageFor2 = GetJoltage(batteryBanks, batteries: 2);
        TotalOutputJoltageFor12 = GetJoltage(batteryBanks, batteries: 12);

        if (Verbose)
        {
            Logger.WriteLine("The total output joltage for 2 batteries is {0}", TotalOutputJoltageFor2);
            Logger.WriteLine("The total output joltage for 12 batteries is {0}", TotalOutputJoltageFor12);
        }

        return PuzzleResult.Create(TotalOutputJoltageFor2, TotalOutputJoltageFor12);
    }
}
