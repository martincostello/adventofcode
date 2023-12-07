// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/07</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 07, "Camel Cards", RequiresData = true)]
public sealed class Day07 : Puzzle
{
    private static readonly Dictionary<char, int> ScoreMap = new()
    {
        ['2'] = 2,
        ['3'] = 3,
        ['4'] = 4,
        ['5'] = 5,
        ['6'] = 6,
        ['7'] = 7,
        ['8'] = 8,
        ['9'] = 9,
        ['T'] = 10,
        ['J'] = 11,
        ['Q'] = 12,
        ['K'] = 13,
        ['A'] = 14,
    };

    private enum HandType
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind,
    }

    /// <summary>
    /// Gets the total winnings.
    /// </summary>
    public int TotalWinnings { get; private set; }

    /// <summary>
    /// Plays the specified hands of Camel Cards.
    /// </summary>
    /// <param name="handsAndBids">The hands to play.</param>
    /// <returns>
    /// The total winnings for the game.
    /// </returns>
    public static int Play(IList<string> handsAndBids)
    {
        var hands = new List<(HandType Type, int[] Cards, int Bid)>(handsAndBids.Count);

        foreach (string value in handsAndBids)
        {
            (string hand, string bid) = value.Bifurcate(' ');

            int[] cards = hand.Select((p) => ScoreMap[p]).ToArray();

            var counts = cards
                .GroupBy((p) => p)
                .Select((p) => new { Card = p.Key, Count = p.Count() })
                .OrderByDescending((p) => p.Count)
                .ToArray();

            HandType type = counts[0].Count switch
            {
                5 => HandType.FiveOfAKind,
                4 => HandType.FourOfAKind,
                3 => counts.Length == 2 ? HandType.FullHouse : HandType.ThreeOfAKind,
                2 => counts.Length == 3 ? HandType.TwoPair : HandType.OnePair,
                _ => HandType.HighCard,
            };

            hands.Add((type, cards, Parse<int>(bid)));
        }

        hands.Sort(static (x, y) =>
        {
            int comparison = x.Type.CompareTo(y.Type);

            if (comparison is not 0)
            {
                return comparison;
            }

            for (int i = 0; i < x.Cards.Length; i++)
            {
                comparison = x.Cards[i].CompareTo(y.Cards[i]);

                if (comparison is not 0)
                {
                    return comparison;
                }
            }

            return 0;
        });

        return hands.Select((p, i) => p.Bid * (i + 1)).Sum();
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var hands = await ReadResourceAsLinesAsync(cancellationToken);

        TotalWinnings = Play(hands);

        if (Verbose)
        {
            Logger.WriteLine("The total winnings are {0}.", TotalWinnings);
        }

        return PuzzleResult.Create(TotalWinnings);
    }
}
