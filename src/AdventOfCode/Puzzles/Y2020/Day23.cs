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
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/23</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2020, 23, MinimumArguments = 1)]
    public sealed class Day23 : Puzzle
    {
        /// <summary>
        /// Gets the labels of the cups after cup 1 after 100 moves.
        /// </summary>
        public string LabelsAfterCup1 { get; private set; } = string.Empty;

        /// <summary>
        /// Plays a game of cups using the specified arrangement of cups for a number of moves.
        /// </summary>
        /// <param name="arrangement">The starting arrangement of the cups.</param>
        /// <param name="moves">The number of moves to make.</param>
        /// <returns>
        /// The labels of the cups after cup 1 after the specified number of moves.
        /// </returns>
        public static string Play(string arrangement, int moves)
        {
            var circle = new LinkedList<int>(arrangement.Select((p) => p - '0'));
            var current = circle.First!;

            for (int i = 0; i < moves; i++)
            {
                const int ToRemove = 3;
                var selections = new List<int>(ToRemove);

                for (int j = 0; j < ToRemove; j++)
                {
                    var selection = NextCircular(current);
                    selections.Add(selection.Value);
                    circle.Remove(selection);
                }

                int destination = current.Value;

                do
                {
                    destination--;

                    if (destination < 1)
                    {
                        destination = 9;
                    }
                }
                while (selections.Contains(destination));

                var destinationNode = circle.Find(destination);

                while (selections.Count > 0)
                {
                    int selection = selections[0];
                    destinationNode = circle.AddAfter(destinationNode!, selection);
                    selections.Remove(selection);
                }

                current = NextCircular(current);
            }

            string final = string.Join(string.Empty, circle);
            int index = final.IndexOf('1', StringComparison.Ordinal);

            return final[(index + 1) ..] + final[..index];

            LinkedListNode<int> NextCircular(LinkedListNode<int> node)
            {
                return node.Next ?? circle!.First!;
            }
        }

        /// <inheritdoc />
        protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            string arrangement = args[0];
            int moves = 100;

            LabelsAfterCup1 = Play(arrangement, moves);

            if (Verbose)
            {
                Logger.WriteLine("The labels on the cups after {0} moves is {1}.", moves, LabelsAfterCup1);
            }

            return PuzzleResult.Create(LabelsAfterCup1);
        }
    }
}
