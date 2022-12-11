// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/11</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2015, 11, "Corporate Policy", MinimumArguments = 1)]
public sealed class Day11 : Puzzle
{
    /// <summary>
    /// Gets the first next password.
    /// </summary>
    public string FirstPassword { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the second next password.
    /// </summary>
    public string SecondPassword { get; private set; } = string.Empty;

    /// <summary>
    /// Generates the next password that should be used based on a current password value.
    /// </summary>
    /// <param name="current">The current password.</param>
    /// <returns>The next password.</returns>
    public static string GenerateNextPassword(string current)
    {
        Span<char> next = current.ToCharArray();

        do
        {
            Rotate(next);
        }
        while (!(ContainsTriumvirateOfLetters(next) && ContainsNoForbiddenCharacters(next) && HasMoreThanOneDistinctPairOfLetters(next)));

        return new string(next);
    }

    /// <summary>
    /// Returns whether the specified span contains an increasing sequence of three characters at least once.
    /// </summary>
    /// <param name="value">The span to test.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> meets the rule; otherwise <see langword="false"/>.</returns>
    public static bool ContainsTriumvirateOfLetters(ReadOnlySpan<char> value)
    {
        bool result = false;

        for (int i = 0; !result && i < value.Length && (value.Length - i) > 2; i++)
        {
            result = value[i + 1] == value[i] + 1 && value[i + 2] == value[i] + 2;
        }

        return result;
    }

    /// <summary>
    /// Returns whether the specified span contains only valid characters.
    /// </summary>
    /// <param name="value">The span to test.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> meets the rule; otherwise <see langword="false"/>.</returns>
    public static bool ContainsNoForbiddenCharacters(ReadOnlySpan<char> value)
        => !value.Contains('i') && !value.Contains('o') && !value.Contains('l');

    /// <summary>
    /// Tests whether an span contains a pair of any two letters that appear at least twice in the string without overlapping.
    /// </summary>
    /// <param name="value">The value to test against the rule.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> meets the rule; otherwise <see langword="false"/>.</returns>
    public static bool HasMoreThanOneDistinctPairOfLetters(ReadOnlySpan<char> value)
    {
        var letterPairs = new Dictionary<string, List<int>>();

        for (int i = 0; i < value.Length - 1; i++)
        {
            char first = value[i];
            char second = value[i + 1];

            if (first != second)
            {
                continue;
            }

            string pair = new(value.Slice(i, 2));

            var indexes = letterPairs.GetOrAdd(pair);

            if (!indexes.Contains(i - 1))
            {
                indexes.Add(i);
            }
        }

        return letterPairs.Count > 1;
    }

    /// <inheritdoc />
    protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        string current = args[0];

        FirstPassword = GenerateNextPassword(current);
        SecondPassword = GenerateNextPassword(FirstPassword);

        if (Verbose)
        {
            Logger.WriteLine("Santa's first new password should be '{0}'.", FirstPassword);
            Logger.WriteLine("Santa's second new password should be '{0}'.", SecondPassword);
        }

        return PuzzleResult.Create(FirstPassword, SecondPassword);
    }

    /// <summary>
    /// Rotates the specified span of characters as if they were integers.
    /// </summary>
    /// <param name="value">The character span to rotate.</param>
    private static void Rotate(Span<char> value)
    {
        bool done = false;

        for (int i = value.Length - 1; !done && i > -1; i--)
        {
            if (value[i] == 'z')
            {
                value[i] = 'a';
            }
            else
            {
                value[i]++;
                done = true;
            }
        }
    }
}
