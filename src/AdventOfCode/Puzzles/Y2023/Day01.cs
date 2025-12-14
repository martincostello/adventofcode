// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Buffers;

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/01</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 01, "Trebuchet?!", RequiresData = true)]
public sealed class Day01 : Puzzle<int, int>
{
    private static readonly SearchValues<char> Digits = SearchValues.Create("123456789");

    private static ReadOnlySpan<string> Numbers => new[]
    {
        "one",
        "two",
        "three",
        "four",
        "five",
        "six",
        "seven",
        "eight",
        "nine",
    };

    /// <summary>
    /// Gets the sum of all of the specified calibration values.
    /// </summary>
    /// <param name="values">The calibration values to sum.</param>
    /// <param name="useWords">Whether to search for words of numbers.</param>
    /// <returns>
    /// The sum of all of the calibration values.
    /// </returns>
    public static int SumCalibrations(IList<string> values, bool useWords)
    {
        int result = 0;
        var firstIndexes = new Dictionary<int, int>(Numbers.Length);
        var lastIndexes = new Dictionary<int, int>(Numbers.Length);

        foreach (string value in values)
        {
            firstIndexes.Clear();
            lastIndexes.Clear();

            var window = value.AsSpan();
            int first = window.IndexOfAny(Digits);
            int last = window.LastIndexOfAny(Digits);

            if (first > -1)
            {
                int digit = window[first] - '0';
                firstIndexes[digit] = first;
            }

            if (last > -1)
            {
                int digit = window[last] - '0';
                lastIndexes[digit] = last;
            }

            if (useWords)
            {
                for (int i = 0; i < Numbers.Length; i++)
                {
                    string word = Numbers[i];
                    int digit = i + 1;

                    first = window.IndexOf(word, StringComparison.Ordinal);
                    last = window.LastIndexOf(word, StringComparison.Ordinal);

                    if (first > -1 && (!firstIndexes.TryGetValue(digit, out int other) || first < other))
                    {
                        firstIndexes[digit] = first;
                    }

                    if (last > -1 && (!lastIndexes.TryGetValue(digit, out other) || last > other))
                    {
                        lastIndexes[digit] = last;
                    }
                }
            }

            var digitOne = firstIndexes.MinBy((p) => p.Value);
            var digitTwo = lastIndexes.MaxBy((p) => p.Value);

            result += (digitOne.Key * 10) + digitTwo.Key;
        }

        return result;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (values, logger, _) =>
            {
                int sumOfCalibrationsDigits = SumCalibrations(values, useWords: false);
                int sumOfCalibrationsWordsAndDigits = SumCalibrations(values, useWords: true);

                if (logger is { })
                {
                    logger.WriteLine("The sum of all of the calibration values is {0} using only digits.", sumOfCalibrationsDigits);
                    logger.WriteLine("The sum of all of the calibration values is {0} using words and digits.", sumOfCalibrationsWordsAndDigits);
                }

                return (sumOfCalibrationsDigits, sumOfCalibrationsWordsAndDigits);
            },
            cancellationToken);
    }
}
