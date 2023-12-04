// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/04</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 04, "Scratchcards", RequiresData = true)]
public sealed class Day04 : Puzzle
{
    /// <summary>
    /// Gets the total number of points the scratchcards are worth.
    /// </summary>
    public int TotalPoints { get; private set; }

    /// <summary>
    /// Gets the total number of points the specified scratchcards are worth.
    /// </summary>
    /// <param name="scratchcards">The scratchcards to add up the points for.</param>
    /// <returns>
    /// The total number of points the scratchcards are worth.
    /// </returns>
    public static int Score(IList<string> scratchcards)
    {
        int total = 0;

        foreach (string scratchcard in scratchcards)
        {
            int index = scratchcard.IndexOf(':', StringComparison.Ordinal);
            (string winningNumbers, string numbersHave) = scratchcard[(index + 1)..].Bifurcate('|');

            var winning = winningNumbers.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(Parse<int>).ToHashSet();
            var have = numbersHave.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(Parse<int>).ToHashSet();

            have.IntersectWith(winning);

            total += (int)Math.Pow(2, have.Count - 1);
        }

        return total;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var scratchcards = await ReadResourceAsLinesAsync(cancellationToken);

        TotalPoints = Score(scratchcards);

        if (Verbose)
        {
            Logger.WriteLine("The scratchcards are worth {0} points in total.", TotalPoints);
        }

        return PuzzleResult.Create(TotalPoints);
    }
}
