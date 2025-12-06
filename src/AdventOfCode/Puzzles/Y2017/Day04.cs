// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2017/day/4</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2017, 04, "High-Entropy Passphrases", RequiresData = true)]
public sealed class Day04 : Puzzle<int, int>
{
    /// <summary>
    /// Returns whether the specified passphrase is valid.
    /// </summary>
    /// <param name="passphrase">The passphrase to test for validity.</param>
    /// <param name="version">The version of the passphrase policy to use.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="passphrase"/> is valid; otherwise <see langword="false"/>.
    /// </returns>
    internal static bool IsPassphraseValid(string passphrase, int version)
    {
        string[] words = passphrase.Split(' ');
        bool isValid = words.Distinct().ExactCount(words.Length);

        if (isValid && version == 2)
        {
            string[] sorted = [.. words
                .Select((p) =>
                {
                    Span<char> span = p.ToCharArray();
                    span.Sort();
                    return span.ToString();
                })];

            isValid = sorted.Distinct().ExactCount(words.Length);
        }

        return isValid;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var passphrases = await ReadResourceAsLinesAsync(cancellationToken);

        Solution1 = Count(passphrases, version: 1);
        Solution2 = Count(passphrases, version: 2);

        if (Verbose)
        {
            Logger.WriteLine($"There are {Solution1:N0} valid passphrases using version 1 of the policy.");
            Logger.WriteLine($"There are {Solution2:N0} valid passphrases using version 2 of the policy.");
        }

        return Result();

        static int Count(List<string> passphrases, int version)
            => passphrases.Count((p) => IsPassphraseValid(p, version));
    }
}
