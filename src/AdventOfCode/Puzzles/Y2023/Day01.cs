// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/01</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 01, "Trebuchet?!", RequiresData = true)]
public sealed class Day01 : Puzzle
{
    /// <summary>
    /// Gets the sum of all of the calibration values.
    /// </summary>
    public int SumOfCalibrations { get; private set; }

    /// <summary>
    /// Gets the sum of all of the specified calibration values.
    /// </summary>
    /// <param name="values">The calibration values to sum.</param>
    /// <returns>
    /// The sum of all of the calibration values.
    /// </returns>
    public static int SumCalibrations(IList<string> values)
    {
        int result = 0;

        foreach (string value in values)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsAsciiDigit(value[i]))
                {
                    result += (value[i] - '0') * 10;
                    break;
                }
            }

            for (int i = value.Length - 1; i > -1; i--)
            {
                if (char.IsAsciiDigit(value[i]))
                {
                    result += value[i] - '0';
                    break;
                }
            }
        }

        return result;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        SumOfCalibrations = SumCalibrations(values);

        if (Verbose)
        {
            Logger.WriteLine("The sum of all of the calibration values is {0}.", SumOfCalibrations);
        }

        return PuzzleResult.Create(SumOfCalibrations);
    }
}
