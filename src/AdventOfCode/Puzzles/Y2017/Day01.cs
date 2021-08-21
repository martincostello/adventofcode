// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2017/day/1</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2017, 01, RequiresData = true)]
public sealed class Day01 : Puzzle
{
    /// <summary>
    /// Gets the solution to the captcha using the next number.
    /// </summary>
    public int CaptchaSolutionNext { get; private set; }

    /// <summary>
    /// Gets the solution to the captcha using the "opposite" number.
    /// </summary>
    public int CaptchaSolutionOpposite { get; private set; }

    /// <summary>
    /// Calculates the sum of all digits that match either the next or "opposite" digit in the specified string.
    /// </summary>
    /// <param name="digits">A <see cref="string"/> of digits to sum.</param>
    /// <param name="useOppositeDigit">Whether to calculate sums using the "opposite" digit.</param>
    /// <returns>
    /// The sum of all digits that match the relevant digit in <paramref name="digits"/>.
    /// </returns>
    public static int CalculateSum(string digits, bool useOppositeDigit)
    {
        int sum = 0;
        int offset = useOppositeDigit ? digits.Length / 2 : 1;

        for (int i = 0; i < digits.Length; i++)
        {
            int j = i + offset;

            if (j >= digits.Length)
            {
                j -= digits.Length;
            }

            char first = digits[i];
            char second = digits[j];

            if (first == second)
            {
                sum += first - '0';
            }
        }

        return sum;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        string digits = (await ReadResourceAsStringAsync()).TrimEnd();

        CaptchaSolutionNext = CalculateSum(digits, useOppositeDigit: false);
        CaptchaSolutionOpposite = CalculateSum(digits, useOppositeDigit: true);

        if (Verbose)
        {
            Logger.WriteLine($"The solution to the first captcha is {CaptchaSolutionNext:N0}.");
            Logger.WriteLine($"The solution to the second captcha is {CaptchaSolutionOpposite:N0}.");
        }

        return PuzzleResult.Create(CaptchaSolutionNext, CaptchaSolutionOpposite);
    }
}
