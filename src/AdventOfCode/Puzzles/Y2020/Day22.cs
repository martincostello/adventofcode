// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/22</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2020, 22, RequiresData = true)]
    public sealed class Day22 : Puzzle
    {
        /// <summary>
        /// Gets the score for the winning player.
        /// </summary>
        public int WinningScore { get; private set; }

        /// <summary>
        /// Plays a game of Combat for the specified starting deck.
        /// </summary>
        /// <param name="startingDeck">The starting deck of cards.</param>
        /// <returns>
        /// The winning player's score.
        /// </returns>
        public static int PlayCombat(IList<string> startingDeck)
        {
            int index = startingDeck.IndexOf(string.Empty);

            var deck1 = startingDeck
                .Skip(1)
                .Take(index - 1)
                .Select(ParseInt32)
                .ToList();

            var deck2 = startingDeck
                .Skip(index + 2)
                .TakeWhile((p) => !string.IsNullOrEmpty(p))
                .Select(ParseInt32)
                .ToList();

            while (deck1.Count > 0 && deck2.Count > 0)
            {
                int card1 = deck1[0];
                int card2 = deck2[0];

                deck1.RemoveAt(0);
                deck2.RemoveAt(0);

                List<int> destination = card1 > card2 ? deck1 : deck2;

                destination.Insert(destination.Count, Math.Max(card1, card2));
                destination.Insert(destination.Count, Math.Min(card1, card2));
            }

            var winningDeck = deck1.Count > 0 ? deck1 : deck2;

            int score = 0;

            for (int i = 0; i < winningDeck.Count; i++)
            {
                score += winningDeck[winningDeck.Count - i - 1] * (i + 1);
            }

            return score;
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> values = await ReadResourceAsLinesAsync();

            WinningScore = PlayCombat(values);

            if (Verbose)
            {
                Logger.WriteLine("The winning player's score is {0}.", WinningScore);
            }

            return PuzzleResult.Create(WinningScore);
        }
    }
}
