// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Buffers;

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/01</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 01, "Trebuchet?!", RequiresData = true)]
public sealed class Day01 : Puzzle
{
    private static readonly SearchValues<char> Digits = SearchValues.Create("123456789");

    /// <summary>
    /// Gets the sum of all of the calibration values using only digits.
    /// </summary>
    public int SumOfCalibrationsDigits { get; private set; }

    /// <summary>
    /// Gets the sum of all of the calibration values using words and digits.
    /// </summary>
    public int SumOfCalibrationsWordsAndDigits { get; private set; }

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

            int first;
            int last;

            if (useWords)
            {
                for (int i = 0; i < Numbers.Length; i++)
                {
                    string number = Numbers[i];
                    first = value.IndexOf(number, StringComparison.Ordinal);
                    last = value.LastIndexOf(number, StringComparison.Ordinal);

                    if (first > -1)
                    {
                        firstIndexes[i + 1] = first;
                    }

                    if (last > -1)
                    {
                        lastIndexes[i + 1] = last;
                    }
                }
            }

            first = System.MemoryExtensions.IndexOfAny(value, Digits);
            last = System.MemoryExtensions.LastIndexOfAny(value, Digits);

            if (first > -1)
            {
                int digit = value[first] - '0';

                if (!firstIndexes.TryGetValue(digit, out int other) || first < other)
                {
                    firstIndexes[digit] = first;
                }
            }

            if (last > -1)
            {
                int digit = value[last] - '0';

                if (!lastIndexes.TryGetValue(digit, out int other) || last > other)
                {
                    lastIndexes[digit] = last;
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
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        SumOfCalibrationsDigits = SumCalibrations(values, useWords: false);
        SumOfCalibrationsWordsAndDigits = SumCalibrations(values, useWords: true);

        if (Verbose)
        {
            Logger.WriteLine("The sum of all of the calibration values is {0} using only digits.", SumOfCalibrationsDigits);
            Logger.WriteLine("The sum of all of the calibration values is {0} using words and digits.", SumOfCalibrationsWordsAndDigits);
        }

        return PuzzleResult.Create(SumOfCalibrationsDigits, SumOfCalibrationsWordsAndDigits);
    }
}
