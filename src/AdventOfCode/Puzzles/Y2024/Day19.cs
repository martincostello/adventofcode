// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Buffers;

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
    /// Counts the number of possible towel designs that can be created from the specified values.
    /// </summary>
    /// <param name="values">The possible towels and desired patterns.</param>
    /// <returns>
    /// The number of possible towel designs that can be created.
    /// </returns>
    public static int CountPossibilities(IList<string> values)
    {
        string[] towels = [.. values[0].Split(", ").OrderByDescending((p) => p.Length)];

        var possibilities = SearchValues.Create(towels, StringComparison.Ordinal);

        int count = 0;

        foreach (string pattern in values.Skip(2))
        {
            if (IsPossible(pattern, towels, possibilities))
            {
                count++;
            }
        }

        return count;
    }

    /// <summary>
    /// Determines whether the specified pattern is possible with the specified values.
    /// </summary>
    /// <param name="pattern">The desired pattern.</param>
    /// <param name="towels">The possible towels.</param>
    /// <param name="searchValues">A value to use to search for towels.</param>
    /// <returns>
    /// <see langword="true"/> if the pattern is possible; otherwise <see langword="false"/>.
    /// </returns>
    internal static bool IsPossible(string pattern, ReadOnlySpan<string> towels, SearchValues<string> searchValues)
    {
        if (pattern.Length < 1)
        {
            return true;
        }

        var patternSpan = pattern.AsSpan();
        int offset = patternSpan.IndexOfAny(searchValues);

        if (offset is -1)
        {
            return false;
        }

        var suffix = patternSpan[offset..];

        foreach (ReadOnlySpan<char> towel in towels)
        {
            int index = suffix.IndexOf(towel);

            if (index > -1)
            {
                string next = pattern.Remove(offset + index, towel.Length);

                if (IsPossible(next, towels, searchValues))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        PossibleDesigns = CountPossibilities(values);

        if (Verbose)
        {
            Logger.WriteLine("{0} designs are possible.", PossibleDesigns);
        }

        return PuzzleResult.Create(PossibleDesigns);
    }
}
