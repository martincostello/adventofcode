// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/3</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 03, "Lobby", RequiresData = true)]
public sealed class Day03 : Puzzle<long, long>
{
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
            total += FindMaximum(bank, batteries);
        }

        return total;

        static long FindMaximum(ReadOnlySpan<char> digits, int batteries)
        {
            if (batteries is 0)
            {
                return 0;
            }

            if (batteries >= digits.Length)
            {
                return Joltage(digits);
            }

            Span<char> span =
                digits.Length <= 32 ?
                stackalloc char[digits.Length] :
                new char[digits.Length];

            int remaining = digits.Length - batteries;
            int index = 0;

            for (int i = 0; i < digits.Length; i++)
            {
                char digit = digits[i];

                while (index > 0 && remaining > 0 && span[index - 1] < digit)
                {
                    index--;
                    remaining--;
                }

                span[index++] = digit;
            }

            return Joltage(span[..batteries]);

            static long Joltage(ReadOnlySpan<char> span)
            {
                long joltage = 0;

                for (int i = 0; i < span.Length; i++)
                {
                    joltage = (joltage * 10) + (span[i] - '0');
                }

                return joltage;
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (batteryBanks, logger, _) =>
            {
                long joltageFor2 = GetJoltage(batteryBanks, batteries: 2);
                long joltageFor12 = GetJoltage(batteryBanks, batteries: 12);

                if (logger is { })
                {
                    logger.WriteLine("The total output joltage for 2 batteries is {0}.", joltageFor2);
                    logger.WriteLine("The total output joltage for 12 batteries is {0}.", joltageFor12);
                }

                return (joltageFor2, joltageFor12);
            },
            cancellationToken);
    }
}
