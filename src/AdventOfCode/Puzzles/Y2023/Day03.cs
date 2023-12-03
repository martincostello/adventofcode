// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Buffers;

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/03</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 03, "Gear Ratios", RequiresData = true)]
public sealed class Day03 : Puzzle
{
    private static readonly SearchValues<char> Digits = SearchValues.Create("0123456789");
    private static readonly SearchValues<char> NotParts = SearchValues.Create("0123456789.");

    /// <summary>
    /// Gets the sum of all of the part numbers in the engine schematic.
    /// </summary>
    public int SumOfPartNumbers { get; private set; }

    /// <summary>
    /// Gets the sum of all of the part numbers in the engine schematic.
    /// </summary>
    /// <param name="schematic">The lines of the engine schematic.</param>
    /// <returns>
    /// The sum of all of the part numbers in the engine schematic.
    /// </returns>
    public static int Solve(IList<string> schematic)
    {
        int sum = 0;
        int totalWidth = schematic[0].Length;

        for (int y = 0; y < schematic.Count; y++)
        {
            var row = schematic[y].AsSpan();
            int index = row.IndexOfAny(Digits);
            int x = index;

            while (index != -1)
            {
                row = row[index..];
                var number = row;

                if (row.Length > 1)
                {
                    index = number.IndexOfAnyExcept(Digits);

                    if (index > -1)
                    {
                        number = number[..index];
                    }
                }

                if (IsAdjacentToPart(schematic, x, y, number.Length, totalWidth))
                {
                    sum += Parse<int>(number);
                }

                row = row[number.Length..];
                index = row.IndexOfAny(Digits);
                x += index + number.Length;
            }
        }

        return sum;

        static bool IsAdjacentToPart(IList<string> schematic, int x, int y, int length, int totalWidth)
        {
            if (x + length < totalWidth)
            {
                length++;
            }

            if (x > 0)
            {
                length++;
                x--;
            }

            int rows = 1;

            if (y < schematic.Count - 1)
            {
                rows++;
            }

            if (y > 0)
            {
                rows++;
                y--;
            }

            int maxY = y + rows;

            for (; y < maxY; y++)
            {
                var window = schematic[y].AsSpan(x, length);

                if (window.IndexOfAnyExcept(NotParts) > -1)
                {
                    return true;
                }
            }

            return false;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        SumOfPartNumbers = Solve(values);

        if (Verbose)
        {
            Logger.WriteLine("The sum of all of the part numbers in the engine schematic is {0}.", SumOfPartNumbers);
        }

        return PuzzleResult.Create(SumOfPartNumbers);
    }
}
