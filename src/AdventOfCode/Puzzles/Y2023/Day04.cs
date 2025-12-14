// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/04</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 04, "Scratchcards", RequiresData = true)]
public sealed class Day04 : Puzzle<int, int>
{
    /// <summary>
    /// Gets the total number of points the specified scratchcards are worth.
    /// </summary>
    /// <param name="scratchcards">The scratchcards to add up the points for.</param>
    /// <returns>
    /// The total number of points the scratchcards are worth and the total
    /// number of scratchcards once the initial scratchcards are inspected.
    /// </returns>
    public static (int TotalPoints, int TotalScratchcards) Score(IList<string> scratchcards)
    {
        int[] counts = new int[scratchcards.Count];
        Array.Fill(counts, 1);

        int totalPoints = 0;

        foreach (string value in scratchcards)
        {
            var scratchcard = value.AsSpan();
            int index = scratchcard.IndexOf(':');
            int card = Parse<int>(scratchcard[5..index], NumberStyles.AllowLeadingWhite);

            scratchcard[(index + 1)..].Bifurcate('|', out var winningNumbers, out var numbersHave);

            var winning = new HashSet<int>();
            var have = new HashSet<int>();

            foreach (var range in winningNumbers.Split(' '))
            {
                var number = winningNumbers[range];

                if (!number.IsEmpty)
                {
                    winning.Add(Parse<int>(number));
                }
            }

            foreach (var range in numbersHave.Split(' '))
            {
                var number = numbersHave[range];

                if (!number.IsEmpty)
                {
                    have.Add(Parse<int>(number));
                }
            }

            have.IntersectWith(winning);

            if (have.Count > 0)
            {
                totalPoints += 1 << (have.Count - 1);

                int next = card;
                int last = Math.Min(card + have.Count, scratchcards.Count);
                int copies = counts[card - 1];

                for (int i = next; i < last; i++)
                {
                    counts[i] += copies;
                }
            }
        }

        return (totalPoints, counts.Sum());
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (scratchcards, logger, _) =>
            {
                (int totalPoints, int totalScratchcards) = Score(scratchcards);

                if (logger is { })
                {
                    logger.WriteLine("The scratchcards are worth {0} points in total.", totalPoints);
                    logger.WriteLine("The total number of scratchcards in the end is {0}.", totalScratchcards);
                }

                return (totalPoints, totalScratchcards);
            },
            cancellationToken);
    }
}
