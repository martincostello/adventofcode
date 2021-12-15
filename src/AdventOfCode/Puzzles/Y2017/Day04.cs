// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2017/day/4</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2017, 04, "High-Entropy Passphrases", RequiresData = true)]
public sealed class Day04 : Puzzle
{
    /// <summary>
    /// Gets the number of valid passphrases in the input using version 1 of the passphrase policy.
    /// </summary>
    public int ValidPassphraseCountV1 { get; private set; }

    /// <summary>
    /// Gets the number of valid passphrases in the input using version 2 of the passphrase policy.
    /// </summary>
    public int ValidPassphraseCountV2 { get; private set; }

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
        bool isValid = words.Distinct().Count() == words.Length;

        if (isValid && version == 2)
        {
            string[] sorted = words
                .Select((p) => p.ToCharArray())
                .Select((p) => p.OrderBy((r) => r).ToArray())
                .Select((p) => new string(p))
                .ToArray();

            isValid = sorted.Distinct().Count() == words.Length;
        }

        return isValid;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ICollection<string> passphrases = await ReadResourceAsLinesAsync();

        ValidPassphraseCountV1 = passphrases.Where((p) => IsPassphraseValid(p, 1)).Count();
        ValidPassphraseCountV2 = passphrases.Where((p) => IsPassphraseValid(p, 2)).Count();

        if (Verbose)
        {
            Logger.WriteLine($"There are {ValidPassphraseCountV1:N0} valid passphrases using version 1 of the policy.");
            Logger.WriteLine($"There are {ValidPassphraseCountV2:N0} valid passphrases using version 2 of the policy.");
        }

        return PuzzleResult.Create(ValidPassphraseCountV1, ValidPassphraseCountV2);
    }
}
