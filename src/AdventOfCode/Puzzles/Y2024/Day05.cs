// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/5</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 05, "Print Queue", RequiresData = true)]
public sealed class Day05 : Puzzle
{
    /// <summary>
    /// Gets the sum of the middle page numbers for correctly-ordered updates.
    /// </summary>
    public int MiddlePageSum { get; private set; }

    /// <summary>
    /// Sorts the rules and returns the sum of the middle page numbers for correctly-ordered updates.
    /// </summary>
    /// <param name="rulesAndUpdates">The page ordering rules to use and the updates.</param>
    /// <returns>
    /// The sum of the middle page numbers for correctly-ordered updates.
    /// </returns>
    public static int Sort(IList<string> rulesAndUpdates)
    {
        var rules = new List<(int First, int Second)>();
        var updates = new List<List<int>>();

        int midpoint = rulesAndUpdates.IndexOf(string.Empty);

        foreach (string rule in rulesAndUpdates.Take(midpoint))
        {
            rules.Add(rule.AsNumberPair<int>('|'));
        }

        foreach (string update in rulesAndUpdates.Skip(midpoint + 1))
        {
            updates.Add(update.AsNumbers<int>());
        }

        int sum = 0;

        foreach (var update in updates)
        {
            bool correct = rules
                .Where((p) => update.Contains(p.First) && update.Contains(p.Second))
                .All((p) => update.IndexOf(p.First) < update.IndexOf(p.Second));

            if (correct)
            {
                sum += update[update.Count / 2];
            }
        }

        return sum;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        MiddlePageSum = Sort(values);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the middle page numbers from correctly-ordered updates is {0}.", MiddlePageSum);
        }

        return PuzzleResult.Create(MiddlePageSum);
    }
}
