// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2020/day/22</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2020, 22, "Crab Combat", RequiresData = true)]
public sealed class Day22 : Puzzle
{
    /// <summary>
    /// Gets the score for the winning player.
    /// </summary>
    public int WinningScore { get; private set; }

    /// <summary>
    /// Gets the score for the winning player using recursive rules.
    /// </summary>
    public int WinningScoreRecursive { get; private set; }

    /// <summary>
    /// Plays a game of Combat for the specified starting deck.
    /// </summary>
    /// <param name="startingDeck">The starting deck of cards.</param>
    /// <param name="recursive">Whether to use the recursive rules.</param>
    /// <returns>
    /// The winning player's score.
    /// </returns>
    public static int PlayCombat(IList<string> startingDeck, bool recursive)
    {
        // With help from https://github.com/DanaL/AdventOfCode/blob/master/2020/Day22.cs for part 2
        int index = startingDeck.IndexOf(string.Empty);

        var deck1 = startingDeck
            .Skip(1)
            .Take(index - 1)
            .Select(Parse<int>)
            .ToList();

        var deck2 = startingDeck
            .Skip(index + 2)
            .TakeWhile((p) => !string.IsNullOrEmpty(p))
            .Select(Parse<int>)
            .ToList();

        return Play(new Queue<int>(deck1), new Queue<int>(deck2), recursive);

        static int Play(Queue<int> deck1, Queue<int> deck2, bool recursive)
        {
            var previousHands1 = new HashSet<string>();
            var previousHands2 = new HashSet<string>();

            while (deck1.Count > 0 && deck2.Count > 0)
            {
                if (recursive)
                {
                    string hand1 = string.Join('+', deck1);
                    string hand2 = string.Join('+', deck2);

                    if (previousHands1.Contains(hand1) || previousHands2.Contains(hand2))
                    {
                        return -1;
                    }
                    else
                    {
                        previousHands1.Add(hand1);
                        previousHands2.Add(hand2);
                    }
                }

                int card1 = deck1.Dequeue();
                int card2 = deck2.Dequeue();

                int winner;

                if (recursive && deck1.Count >= card1 && deck2.Count >= card2)
                {
                    var subDeck1 = new Queue<int>(deck1.Take(card1));
                    var subDeck2 = new Queue<int>(deck2.Take(card2));

                    int score = Play(subDeck1, subDeck2, recursive);

                    winner = score == -1 ? 1 : subDeck1.Count > subDeck2.Count ? 1 : 2;
                }
                else
                {
                    winner = card1 > card2 ? 1 : 2;
                }

                if (winner == 1)
                {
                    deck1.Enqueue(card1);
                    deck1.Enqueue(card2);
                }
                else
                {
                    deck2.Enqueue(card2);
                    deck2.Enqueue(card1);
                }
            }

            var winnersDeck = deck1.Count > 0 ? deck1 : deck2;

            return Score(winnersDeck);
        }

        static int Score(Queue<int> deck)
        {
            var copy = new Queue<int>(deck);
            int score = 0;

            while (copy.TryDequeue(out int card))
            {
                score += card * (copy.Count + 1);
            }

            return score;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var values = await ReadResourceAsLinesAsync(cancellationToken);

        WinningScore = PlayCombat(values, recursive: false);
        WinningScoreRecursive = PlayCombat(values, recursive: true);

        if (Verbose)
        {
            Logger.WriteLine("The winning player's score with normal rules is {0}.", WinningScore);
            Logger.WriteLine("The winning player's score with recursive rules is {0}.", WinningScoreRecursive);
        }

        return PuzzleResult.Create(WinningScore, WinningScoreRecursive);
    }
}
