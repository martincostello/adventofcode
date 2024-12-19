// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/19</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 19, "Linen Layout", RequiresData = true)]
public sealed class Day19 : Puzzle
{
    /// <summary>
    /// Gets the number of possible towel designs that can be created.
    /// </summary>
    public int PossibleDesigns { get; private set; }

    /// <summary>
    /// Gets the number of unique towel designs that can be created.
    /// </summary>
    public long UniqueDesigns { get; private set; }

    /// <summary>
    /// Counts the number of possible towel designs that can be created from the specified values.
    /// </summary>
    /// <param name="values">The possible towels and desired patterns.</param>
    /// <returns>
    /// The number of possible towel designs that can be created and the number of unique designs.
    /// </returns>
    public static (int Possibilities, long Designs) CountPossibilities(IList<string> values)
    {
        string[] towels = [.. values[0].Split(", ").OrderByDescending((p) => p.Length)];

        var cache = new Dictionary<string, long>();
        var lookup = cache.GetAlternateLookup<ReadOnlySpan<char>>();

        int possible = 0;
        long designs = 0;

        for (int i = 2; i < values.Count; i++)
        {
            long count = CountDesigns(values[i], towels, lookup);

            if (count > 0)
            {
                possible++;
                designs += count;
            }
        }

        return (possible, designs);
    }

    /// <summary>
    /// Determines how many possible designs can be created for
    /// the specified pattern with the specified towels.
    /// </summary>
    /// <param name="pattern">The desired pattern.</param>
    /// <param name="towels">The possible towels.</param>
    /// <param name="cache">The cache of possibilities.</param>
    /// <returns>
    /// The number of possible designs that can create the pattern.
    /// </returns>
    internal static long CountDesigns(
        ReadOnlySpan<char> pattern,
        ReadOnlySpan<string> towels,
        Dictionary<string, long>.AlternateLookup<ReadOnlySpan<char>> cache)
    {
        if (cache.TryGetValue(pattern, out long count))
        {
            return count;
        }

        count = 0;

        foreach (ReadOnlySpan<char> towel in towels)
        {
            if (!pattern.StartsWith(towel))
            {
                continue;
            }

            if (towel.Length == pattern.Length)
            {
                count++;
                continue;
            }

            count += CountDesigns(pattern[towel.Length..], towels, cache);
        }

        cache[pattern] = count;
        return count;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        (PossibleDesigns, UniqueDesigns) = CountPossibilities(values);

        if (Verbose)
        {
            Logger.WriteLine("{0} designs are possible.", PossibleDesigns);
            Logger.WriteLine("There are {0} different ways to make each design.", UniqueDesigns);
        }

        return PuzzleResult.Create(PossibleDesigns, UniqueDesigns);
    }
}
