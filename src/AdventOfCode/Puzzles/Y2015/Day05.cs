// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/5</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2015, 05, "Doesn't He Have Intern-Elves For This?", RequiresData = true)]
public sealed class Day05 : Puzzle
{
#pragma warning disable SA1010
    /// <summary>
    /// The sequences of characters that are not considered nice. This field is read-only.
    /// </summary>
    private static readonly ImmutableArray<string> NotNiceSequences = ["ab", "cd", "pq", "xy"];
#pragma warning restore SA1010

    /// <summary>
    /// Gets the number of 'nice' strings using version 1 of the rules.
    /// </summary>
    public int NiceStringCountV1 { get; private set; }

    /// <summary>
    /// Gets the number of 'nice' strings using version 2 of the rules.
    /// </summary>
    public int NiceStringCountV2 { get; private set; }

    /// <summary>
    /// Returns whether the specified string is 'nice' using the first set of criteria.
    /// </summary>
    /// <param name="value">The string to test for niceness.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is 'nice'; otherwise <see langword="false"/>.
    /// </returns>
    public static bool IsNiceV1(string value)
    {
        // The string is not nice if it contain any of the following sequences
        if (NotNiceSequences.Any((p) => value.Contains(p, StringComparison.Ordinal)))
        {
            return false;
        }

        int vowels = 0;
        bool hasAnyConsecutiveLetters = false;

        // The string is nice if it has three or more vowels and at least two consecutive letters
        bool IsNice() => hasAnyConsecutiveLetters && vowels > 2;

        for (int i = 0; i < value.Length; i++)
        {
            char current = value[i];

            if (IsVowel(current))
            {
                vowels++;
            }

            if (i > 0 && !hasAnyConsecutiveLetters)
            {
                hasAnyConsecutiveLetters = current == value[i - 1];
            }

            if (IsNice())
            {
                // Criteria all met, no further analysis required
                return true;
            }
        }

        return IsNice();

        static bool IsVowel(char letter)
        {
            return letter switch
            {
                'a' or 'e' or 'i' or 'o' or 'u' => true,
                _ => false,
            };
        }
    }

    /// <summary>
    /// Returns whether the specified string is 'nice' using the second set of criteria.
    /// </summary>
    /// <param name="value">The string to test for niceness.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is 'nice'; otherwise <see langword="false"/>.
    /// </returns>
    public static bool IsNiceV2(string value)
    {
        return
            HasLetterThatIsTheBreadOfALetterSandwich(value) &&
            HasPairOfLettersWithMoreThanOneOccurrence(value);
    }

    /// <summary>
    /// Tests whether a string contains a pair of any two letters that
    /// appear at least twice in the string without overlapping.
    /// </summary>
    /// <param name="value">The value to test against the rule.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> meets the rule; otherwise <see langword="false"/>.
    /// </returns>
    internal static bool HasPairOfLettersWithMoreThanOneOccurrence(ReadOnlySpan<char> value)
    {
        var letterPairs = new Dictionary<string, List<int>>();

        for (int i = 0; i < value.Length - 1; i++)
        {
            string pair = new(value.Slice(i, 2));

            var indexes = letterPairs.GetOrAdd(pair);

            if (!indexes.Contains(i - 1))
            {
                indexes.Add(i);
            }
        }

        return letterPairs.Any((p) => p.Value.Count > 1);
    }

    /// <summary>
    /// Tests whether a string contains at least one letter which repeats with exactly one letter between them.
    /// </summary>
    /// <param name="value">The value to test against the rule.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> meets the rule; otherwise <see langword="false"/>.
    /// </returns>
    internal static bool HasLetterThatIsTheBreadOfALetterSandwich(string value)
    {
        if (value.Length < 3)
        {
            // The value is not long enough
            return false;
        }

        for (int i = 1; i < value.Length - 1; i++)
        {
            if (value[i - 1] == value[i + 1])
            {
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var values = await ReadResourceAsLinesAsync(cancellationToken);

        NiceStringCountV1 = values.Count(IsNiceV1);
        NiceStringCountV2 = values.Count(IsNiceV2);

        if (Verbose)
        {
            Logger.WriteLine("{0:N0} strings are nice using version 1 of the rules.", NiceStringCountV1);
            Logger.WriteLine("{0:N0} strings are nice using version 2 of the rules.", NiceStringCountV2);
        }

        return PuzzleResult.Create(NiceStringCountV1, NiceStringCountV2);
    }
}
