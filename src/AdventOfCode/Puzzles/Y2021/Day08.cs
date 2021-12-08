// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/8</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 08, RequiresData = true)]
public sealed class Day08 : Puzzle
{
    /// <summary>
    /// Gets the number of instances of digits that use a unique number of LED segments.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Returns the number of instances of digits that use a unique number of LED segments
    /// for the specified entries detailing how the LED segments are lit.
    /// </summary>
    /// <param name="entries">The entries to count the digits for.</param>
    /// <returns>
    /// The number of instances of digits that use a unique number of LED segments.
    /// </returns>
    public static int CountDigits(IList<string> entries)
    {
        int count = 0;

        foreach (string entry in entries)
        {
            string[] parts = entry.Split(" | ");
            string[] digits = parts[1].Split(' ');

            foreach (string value in digits)
            {
                switch (value.Length)
                {
                    case 2:
                    case 3:
                    case 4:
                    case 7:
                        count++;
                        break;

                    default:
                        break;
                }
            }
        }

        return count;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> entries = await ReadResourceAsLinesAsync();

        Count = CountDigits(entries);

        if (Verbose)
        {
            Logger.WriteLine(
                "There are {0:N0} instances of digits that use a unique number of LED segments.",
                Count);
        }

        return PuzzleResult.Create(Count);
    }
}
