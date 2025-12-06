// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/6</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 06, "Trash Compactor", RequiresData = true)]
public sealed class Day06 : Puzzle
{
    /// <summary>
    /// Gets the grand total found by adding together all of the answers to the individual problems.
    /// </summary>
    public long GrandTotal { get; private set; }

    /// <summary>
    /// Solves the problems in the specified homework worksheet.
    /// </summary>
    /// <param name="worksheet">The worksheet to solve.</param>
    /// <returns>
    /// The grand total found by adding together all of the answers to the individual problems.
    /// </returns>
    public static long SolveWorksheet(IReadOnlyList<string> worksheet)
    {
        var groups = new List<List<long>>();
        var operations = new List<char>();

        foreach (string operation in worksheet[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            operations.Add(operation[0]);
        }

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
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        GrandTotal = SolveWorksheet(values);

        if (Verbose)
        {
            Logger.WriteLine("The solution is {0}.", GrandTotal);
        }

        return PuzzleResult.Create(GrandTotal);
    }
}
