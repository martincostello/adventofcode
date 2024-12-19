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
    /// Counts the number of possible towel designs that can be created from the specified values.
    /// </summary>
    /// <param name="values">The possible towels and desired patterns.</param>
    /// <returns>
    /// The number of possible towel designs that can be created.
    /// </returns>
    public static int CountPossibilities(IList<string> values)
    {
        string[] towels = [.. values[0].Split(", ").OrderByDescending((p) => p.Length)];

        var cache = new Dictionary<string, bool>();

        int count = 0;

        foreach (string pattern in values.Skip(2))
        {
            if (IsPossible(pattern, towels, cache))
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
    /// <param name="cache">The cache of possibilities.</param>
    /// <returns>
    /// <see langword="true"/> if the pattern is possible; otherwise <see langword="false"/>.
    /// </returns>
    internal static bool IsPossible(
        string pattern,
        ReadOnlySpan<string> towels,
        Dictionary<string, bool> cache)
    {
        if (cache.TryGetValue(pattern, out bool isPossible))
        {
            return isPossible;
        }

        if (pattern.Length < 1)
        {
            cache[pattern] = true;
            return true;
        }

        var patternSpan = pattern.AsSpan();

        foreach (ReadOnlySpan<char> towel in towels)
        {
            if (patternSpan.StartsWith(towel))
            {
                if (towel.Length == pattern.Length)
                {
                    cache[pattern] = true;
                    return true;
                }

                string next = pattern[towel.Length..];

                if (IsPossible(next, towels, cache))
                {
                    cache[pattern] = true;
                    return true;
                }
            }
        }

        cache[pattern] = false;
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
