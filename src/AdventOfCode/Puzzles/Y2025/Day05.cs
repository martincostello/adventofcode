// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/5</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 05, "Cafeteria", RequiresData = true, IsHidden = true, Unsolved = true)]
public sealed class Day05 : Puzzle
{
    /// <summary>
    /// Gets the number of available ingredient IDs that are fresh.
    /// </summary>
    public int FreshIngredientIds { get; private set; }

    /// <summary>
    /// Counts the number of fresh ingredients in the specified inventory database.
    /// </summary>
    /// <param name="database">The inventory management system database to use.</param>
    /// <returns>
    /// The solution.
    /// </returns>
    public static int CountFreshIngredients(IReadOnlyList<string> database)
    {
        var ranges = new List<(long Start, long End)>();
        var ids = new List<long>();

        bool isRange = true;

        for (int i = 0; i < database.Count; i++)
        {
            var value = database[i].AsSpan();

            if (value.IsEmpty)
            {
                isRange = false;
                continue;
            }

            if (isRange)
            {
                ranges.Add(value.AsNumberPair<long>('-'));
            }
            else
            {
                ids.Add(Parse<long>(value));
            }
        }

        int fresh = 0;

        foreach (long id in ids)
        {
            if (ranges.Any((p) => id >= p.Start && id <= p.End))
            {
                fresh++;
            }
        }

        return fresh;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        FreshIngredientIds = CountFreshIngredients(values);

        if (Verbose)
        {
            Logger.WriteLine("{0} available ingredient IDs are fresh.", FreshIngredientIds);
        }

        return PuzzleResult.Create(FreshIngredientIds);
    }
}
