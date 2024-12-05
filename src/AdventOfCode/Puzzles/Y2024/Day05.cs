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
    public int MiddlePageSumCorrect { get; private set; }

    /// <summary>
    /// Gets the sum of the middle page numbers for incorrectly-ordered updates once fixed.
    /// </summary>
    public int MiddlePageSumIncorrect { get; private set; }

    /// <summary>
    /// Sorts the rules and returns the sum of the middle page numbers for page updates.
    /// </summary>
    /// <param name="pageOrderingRulesAndUpdates">The page ordering rules to use and the updates.</param>
    /// <param name="fix">Whether to fix the incorrect updates and return their sum or not.</param>
    /// <returns>
    /// The sum of the middle page numbers for the updates.
    /// </returns>
    public static int Sort(IList<string> pageOrderingRulesAndUpdates, bool fix)
    {
        var rules = new List<(int First, int Second)>();
        var updates = new List<List<int>>();

        int midpoint = pageOrderingRulesAndUpdates.IndexOf(string.Empty);

        foreach (string rule in pageOrderingRulesAndUpdates.Take(midpoint))
        {
            rules.Add(rule.AsNumberPair<int>('|'));
        }

        foreach (string update in pageOrderingRulesAndUpdates.Skip(midpoint + 1))
        {
            updates.Add(update.AsNumbers<int>());
        }

        int sum = 0;

        foreach (var update in updates)
        {
            var updateRules = rules
                .Where((p) => update.Contains(p.First) && update.Contains(p.Second))
                .ToList();

            bool correct = updateRules.All((p) => update.IndexOf(p.First) < update.IndexOf(p.Second));

            if (!correct && fix)
            {
                var counts = update.ToDictionary((k) => k, (v) => updateRules.Count((p) => p.First == v));
                update.Sort((x, y) => counts[y] - counts[x]);
            }

            if (correct ^ fix)
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

        MiddlePageSumCorrect = Sort(values, fix: false);
        MiddlePageSumIncorrect = Sort(values, fix: true);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the middle page numbers from correctly-ordered updates is {0}.", MiddlePageSumCorrect);
            Logger.WriteLine("The sum of the middle page numbers after correctly ordering incorrectly-ordered updates is {0}.", MiddlePageSumIncorrect);
        }

        return PuzzleResult.Create(MiddlePageSumCorrect, MiddlePageSumIncorrect);
    }
}
