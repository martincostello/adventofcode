// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Range = (long Start, long End);

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
    public int AvailableFreshIngredientIds { get; private set; }

    /// <summary>
    /// Gets the total number of ingredient IDs that are fresh.
    /// </summary>
    public long FreshIngredientIds { get; private set; }

    /// <summary>
    /// Counts the number of fresh ingredients in the specified inventory database.
    /// </summary>
    /// <param name="database">The inventory management system database to use.</param>
    /// <returns>
    /// The number of available and total fresh ingredient IDs.
    /// </returns>
    public static (int Available, long Total) CountFreshIngredients(IReadOnlyList<string> database)
    {
        var ids = new List<long>();
        var ranges = new List<Range>();

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

        // Merge overlapping ranges
        ranges.Sort((x, y) =>
        {
            int comparison = x.Start.CompareTo(y.Start);

            if (comparison != 0)
            {
                return comparison;
            }

            return x.End.CompareTo(y.End);
        });

        var merged = new List<Range>() { ranges[0] };

        foreach (var range in ranges[1..])
        {
            var (start, end) = merged[^1];

            if (range.Start <= end + 1)
            {
                merged[^1] = (start, Math.Max(end, range.End));
            }
            else
            {
                merged.Add(range);
            }
        }

        int fresh = 0;

        foreach (long id in ids)
        {
            if (merged.Any((p) => id >= p.Start && id <= p.End))
            {
                fresh++;
            }
        }

        long possible = 0;

        foreach ((long start, long end) in merged)
        {
            possible += end - start + 1;
        }

        return (fresh, possible);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        (AvailableFreshIngredientIds, FreshIngredientIds) = CountFreshIngredients(values);

        if (Verbose)
        {
            Logger.WriteLine("{0} available ingredient IDs are fresh.", AvailableFreshIngredientIds);
            Logger.WriteLine("{0} ingredient IDs are fresh.", FreshIngredientIds);
        }

        return PuzzleResult.Create(AvailableFreshIngredientIds, FreshIngredientIds);
    }
}
