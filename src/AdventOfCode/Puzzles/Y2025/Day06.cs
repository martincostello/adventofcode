// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/6</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 06, "Trash Compactor", RequiresData = true)]
public sealed class Day06 : Puzzle<long, long>
{
    /// <summary>
    /// Solves the problems in the specified homework worksheet.
    /// </summary>
    /// <param name="worksheet">The worksheet to solve.</param>
    /// <param name="useCephalopodMaths">Whether or not to use cephalopod maths.</param>
    /// <returns>
    /// The grand total found by adding together all of the answers to the individual problems.
    /// </returns>
    public static long SolveWorksheet(IReadOnlyList<string> worksheet, bool useCephalopodMaths)
    {
        var groups = ParseGroups(worksheet);
        var operations = ParseOperations(worksheet);

        if (useCephalopodMaths)
        {
            groups = Transpose(worksheet, groups);
        }

        long result = 0;

        for (int i = 0; i < groups.Count; i++)
        {
            Func<long, long, long> aggregator = operations[i] switch
            {
                '+' => Sum,
                '*' => Product,
                _ => throw new System.Diagnostics.UnreachableException(),
            };

            result += groups[i].Aggregate(aggregator);
        }

        return result;

        static long Product(long a, long b) => a * b;
        static long Sum(long a, long b) => a + b;

        static List<int> GetColumnIndexes(IReadOnlyList<string> worksheet)
        {
            int width = worksheet[0].Length;

            var indexes = new List<int>();

            for (int i = 0; i < width; i++)
            {
                if (IsAllSpaces(worksheet, i))
                {
                    indexes.Add(i);
                }
            }

            // Add a virtual final column index
            indexes.Add(width);

            return indexes;
        }

        static List<bool> GetPadding(IReadOnlyList<string> worksheet)
        {
            var columns = GetColumnIndexes(worksheet);
            var padding = new List<bool>(columns.Count);

            foreach (int index in columns)
            {
                int column = index - 1;
                int rows = worksheet.Count - 1;

                bool rightAligned = true;

                for (int row = 0; rightAligned && row < rows; row++)
                {
                    rightAligned &= worksheet[row][column] != ' ';
                }

                padding.Add(rightAligned);
            }

            return padding;
        }

        static bool IsAllSpaces(IReadOnlyList<string> worksheet, int column)
        {
            int rows = worksheet.Count - 1;

            for (int row = 0; row < rows; row++)
            {
                if (worksheet[row][column] != ' ')
                {
                    return false;
                }
            }

            return true;
        }

        static List<List<long>> ParseGroups(IReadOnlyList<string> worksheet)
        {
            var groups = new List<List<long>>();

            foreach (string row in worksheet.Take(worksheet.Count - 1))
            {
                var numbers = row
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(Parse<long>)
                    .ToList();

                for (int i = 0; i < numbers.Count; i++)
                {
                    List<long> group;

                    if (groups.Count == i)
                    {
                        groups.Add(group = []);
                    }
                    else
                    {
                        group = groups[i];
                    }

                    group.Add(numbers[i]);
                }
            }

            return groups;
        }

        static List<char> ParseOperations(IReadOnlyList<string> worksheet)
        {
            var operations = new List<char>();

            foreach (string operation in worksheet[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                operations.Add(operation[0]);
            }

            return operations;
        }

        static List<List<long>> Transpose(IReadOnlyList<string> worksheet, List<List<long>> groups)
        {
            const byte Padding = byte.MaxValue;

            var result = new List<List<long>>();
            var padRight = GetPadding(worksheet);

            for (int i = 0; i < groups.Count; i++)
            {
                var groupDigits = new List<List<byte>>();

                foreach (long value in groups[i])
                {
                    var digits = Maths.Digits(value);
                    groupDigits.Add([.. digits]);
                }

                int length = groupDigits.Max((d) => d.Count);

                foreach (var digits in groupDigits.Where((p) => p.Count < length))
                {
                    bool alignRight = padRight[i];
                    int count = length - digits.Count;

                    for (int j = 0; j < count; j++)
                    {
                        digits.Insert(alignRight ? 0 : digits.Count, Padding);
                    }
                }

                var transposed = new List<long>(groupDigits.Count);

                for (int j = length - 1; j > -1; j--)
                {
                    var digits = new List<byte>();

                    foreach (var sequence in groupDigits)
                    {
                        byte digit = sequence[j];

                        if (digit is not Padding)
                        {
                            digits.Add(digit);
                        }
                    }

                    transposed.Add(Maths.FromDigits<long>(digits));
                }

                result.Add(transposed);
            }

            return result;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static async (values, logger, cancellationToken) =>
            {
                long grandTotalNormal = SolveWorksheet(values, useCephalopodMaths: false);
                long grandTotalCephalopod = SolveWorksheet(values, useCephalopodMaths: true);

                if (logger is { })
                {
                    logger.WriteLine("The grand total using normal maths is {0}.", grandTotalNormal);
                    logger.WriteLine("The grand total using cephalopod maths is {0}.", grandTotalCephalopod);
                }

                return (grandTotalNormal, grandTotalCephalopod);
            },
            cancellationToken);
    }
}
