// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/10</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2020, 10, RequiresData = true)]
    public sealed class Day10 : Puzzle
    {
        /// <summary>
        /// Gets the product of the 1-jolt and 3-jolt differences of the specified adapters.
        /// </summary>
        public int JoltageProduct { get; private set; }

        /// <summary>
        /// Gets the product of the 1-jolt and 3-jolt differences when the adapters
        /// with the specified joltage ratings are linked together in series.
        /// </summary>
        /// <param name="joltageRatings">The joltage ratings of the adapters.</param>
        /// <returns>
        /// The product of the 1-jolt and 3-jolt differences.
        /// </returns>
        public static int GetJoltageProduct(IEnumerable<int> joltageRatings)
        {
            // Sort the ratings so the search space is efficient
            IList<int> sorted = joltageRatings
                .OrderBy((p) => p)
                .ToList();

            int maxRating = sorted.Max();
            int deviceRating = maxRating + 3;

            // Start with the charging outlet with a joltage of 0
            var chain = new Stack<int>();
            chain.Push(0);

            foreach (int adapter in JoltageCandidates(chain))
            {
                chain.Push(adapter);

                if (ContainsPathToTarget(chain))
                {
                    break;
                }

                chain.Pop();
            }

            var deltas = new Dictionary<int, int>()
            {
                [1] = 0,
                [2] = 0,
                [3] = 1, // For the built-in device's joltage
            };

            int last = chain.Pop();

            while (chain.TryPop(out int value))
            {
                deltas[last - value]++;
                last = value;
            }

            return deltas[1] * deltas[3];

            IList<int> JoltageCandidates(Stack<int> path)
            {
                int previous = path.Peek();

                return sorted
                    .Where((p) => !path.Contains(p))
                    .Where((p) => p >= previous + 1)
                    .Where((p) => p <= previous + 3)
                    .ToList();
            }

            bool ContainsPathToTarget(Stack<int> path)
            {
                int previous = path.Peek();

                if (previous == maxRating)
                {
                    return path.Count == sorted.Count + 1;
                }

                IList<int> candidates = JoltageCandidates(path);

                if (candidates.Count == 0)
                {
                    return false;
                }

                foreach (int candidate in candidates)
                {
                    path.Push(candidate);

                    if (ContainsPathToTarget(path))
                    {
                        return true;
                    }

                    path.Pop();
                }

                return false;
            }
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<int> joltages = await ReadResourceAsSequenceAsync<int>();

            JoltageProduct = GetJoltageProduct(joltages);

            if (Verbose)
            {
                Logger.WriteLine("The product of the 1-jolt differences and 3-jolt differences is {0}.", JoltageProduct);
            }

            return PuzzleResult.Create(JoltageProduct);
        }
    }
}
