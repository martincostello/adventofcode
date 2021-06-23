// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
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
        /// Gets the product of the labels of the two cups after cup 1 after 10,000,000 moves.
        /// </summary>
        public long ProductOfLabelsAfterCup1 { get; private set; }

        /// <summary>
        /// Plays a game of cups using the specified arrangement of cups for a number of moves.
        /// </summary>
        /// <param name="arrangement">The starting arrangement of the cups.</param>
        /// <param name="moves">The number of moves to make.</param>
        /// <returns>
        /// The circle of cups when the game ends.
        /// </returns>
        public static LinkedList<int> Play(IEnumerable<int> arrangement, int moves)
        {
            var circle = new LinkedList<int>(arrangement);

            int minimum = circle.Min();
            int maximum = circle.Max();

            var nodeMap = new Dictionary<int, LinkedListNode<int>>();
            var current = circle.First;

            while (current is not null)
            {
                nodeMap[current.Value] = current;
                current = current.Next;
            }

            current = circle.First!;

            for (int i = 0; i < moves; i++)
            {
                const int ToRemove = 3;
                var selections = new List<LinkedListNode<int>>(ToRemove);

                for (int j = 0; j < ToRemove; j++)
                {
                    var selection = circle.Clockwise(current);
                    selections.Add(selection);
                    circle.Remove(selection);
                }

                int destination = current.Value;

                do
                {
                    destination--;

                    if (destination < minimum)
                    {
                        destination = maximum;
                    }
                }
                while (selections.Exists((p) => p.Value == destination));

                var destinationNode = nodeMap[destination];

                while (selections.Count > 0)
                {
                    var selection = selections[0];
                    circle.AddAfter(destinationNode!, selection);
                    destinationNode = selection;
                    selections.Remove(selection);
                }

                current = circle.Clockwise(current);
            }

            return circle;
        }

        /// <inheritdoc />
        protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IEnumerable<int> arrangement = args[0].Select((p) => p - '0').ToArray();

            var circle = Play(arrangement, moves: 100);

            string final = string.Join(string.Empty, circle);
            int index = final.IndexOf('1', StringComparison.Ordinal);

            LabelsAfterCup1 = final[(index + 1) ..] + final[..index];

            arrangement = arrangement.Concat(Enumerable.Range(10, 999_991));

            circle = Play(arrangement, 10_000_000);

            var item1 = circle.Find(1);

            ProductOfLabelsAfterCup1 =
                1L *
                item1!.Next!.Value *
                item1.Next.Next!.Value;

            if (Verbose)
            {
                Logger.WriteLine("The labels on the cups after 100 moves is {0}.", LabelsAfterCup1);
                Logger.WriteLine("The product of the labels on the first two cups after cup 1 after 10,000,000 moves is {0}.", ProductOfLabelsAfterCup1);
            }

            return PuzzleResult.Create(LabelsAfterCup1, ProductOfLabelsAfterCup1);
        }
    }
}
