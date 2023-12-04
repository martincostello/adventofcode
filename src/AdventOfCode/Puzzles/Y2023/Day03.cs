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
    /// Gets the sum of all of the gear ratios in the engine schematic.
    /// </summary>
    public int SumOfGearRatios { get; private set; }

    /// <summary>
    /// Gets the sum of all of the part numbers and gear ratios in the engine schematic.
    /// </summary>
    /// <param name="schematic">The lines of the engine schematic.</param>
    /// <returns>
    /// The sum of all of the part numbers and the gear ratios in the engine schematic.
    /// </returns>
    public static (int SumOfPartNumbers, int SumOfGearRatios) Solve(IList<string> schematic)
    {
        int partNumbersSum = 0;
        int gearRatiosSum = 0;
        int totalWidth = schematic[0].Length;

        var partsByRow = new Dictionary<int, List<(int PartNumber, HashSet<Point> Locations)>>();

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
                    int partNumber = Parse<int>(number);
                    partNumbersSum += partNumber;

                    var locations = new HashSet<Point>(number.Length);

                    for (int i = 0; i < number.Length; i++)
                    {
                        locations.Add(new(x + i, y));
                    }

                    if (!partsByRow.TryGetValue(y, out var rowParts))
                    {
                        partsByRow[y] = rowParts = [];
                    }

                    rowParts.Add((partNumber, locations));
                }

                row = row[number.Length..];
                index = row.IndexOfAny(Digits);
                x += index + number.Length;
            }
        }

        for (int y = 0; y < schematic.Count; y++)
        {
            var row = schematic[y].AsSpan();
            int index = row.IndexOf('*');
            int x = index;

            while (index != -1)
            {
                row = row[index..];

                var adjacentParts = new HashSet<int>(6);
                var location = new Point(x, y);

                foreach (var point in location.Neighbors())
                {
                    if (!partsByRow.TryGetValue(point.Y, out var items))
                    {
                        continue;
                    }

                    foreach (var (partNumber, locations) in items)
                    {
                        if (locations.Contains(point))
                        {
                            adjacentParts.Add(partNumber);
                        }
                    }
                }

                if (adjacentParts.Count == 2)
                {
                    gearRatiosSum += adjacentParts.Aggregate(1, (a, b) => a * b);
                }

                if (row.Length == 1)
                {
                    break;
                }

                row = row[1..];
                index = row.IndexOf('*');
                x += index + 1;
            }
        }

        return (partNumbersSum, gearRatiosSum);

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

        (SumOfPartNumbers, SumOfGearRatios) = Solve(values);

        if (Verbose)
        {
            Logger.WriteLine("The sum of all of the part numbers in the engine schematic is {0}.", SumOfPartNumbers);
            Logger.WriteLine("The sum of all of the gear ratios in the engine schematic is {0}.", SumOfGearRatios);
        }

        return PuzzleResult.Create(SumOfPartNumbers, SumOfGearRatios);
    }
}
