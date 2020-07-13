// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2015/day/4</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day04 : Puzzle2015
    {
        /// <summary>
        /// Gets the lowest value that produces a hash that starts with the required number of zeroes.
        /// </summary>
        internal int LowestZeroHash { get; private set; }

        /// <inheritdoc />
        protected override int MinimumArguments => 2;

        /// <summary>
        /// Gets the lowest positive integer which when combined with a secret key has an MD5 hash whose
        /// hexadecimal representation starts with the specified number of zeroes.
        /// </summary>
        /// <param name="secretKey">The secret key to use.</param>
        /// <param name="zeroes">The number of zeroes to get the value for.</param>
        /// <returns>The lowest positive integer that generates an MD5 hash with the number of zeroes specified.</returns>
        internal static int GetLowestPositiveNumberWithStartingZeroes(string secretKey, int zeroes)
        {
            var solutions = new ConcurrentBag<int>();
            var searchedRanges = new ConcurrentBag<int>();

            int fromInclusive = 1;
            int rangeSize = 50000;

            var source = Partitioner.Create(fromInclusive, int.MaxValue - 1, rangeSize);

            Parallel.ForEach(
                source,
                (range, state) =>
                {
                    try
                    {
                        // Does this range start at a value greater than an already found value?
                        if (!solutions.IsEmpty)
                        {
                            int bestSolution = solutions.Min();

                            if (range.Item1 > bestSolution)
                            {
                                var orderedRanges = searchedRanges.ToList();

                                if (orderedRanges.Count == 0)
                                {
                                    return;
                                }

                                orderedRanges.Sort();

                                // Have we searched the first possible range already?
                                if (orderedRanges[0] == fromInclusive)
                                {
                                    for (int i = 1; i < orderedRanges.Count; i++)
                                    {
                                        int lastRange = orderedRanges[i - 1];
                                        int thisRange = orderedRanges[i];

                                        // Is this range the next range?
                                        if (thisRange != lastRange + rangeSize)
                                        {
                                            // A range before the current best solution has not been searched yet
                                            break;
                                        }

                                        if (thisRange > bestSolution)
                                        {
                                            // We have found the best solution
                                            state.Stop();
                                        }
                                    }
                                }
                            }

                            return;
                        }

                        using HashAlgorithm algorithm = MD5.Create();

                        for (int i = range.Item1; !state.ShouldExitCurrentIteration && i < range.Item2; i++)
                        {
                            if (IsSolution(i, secretKey, zeroes, algorithm))
                            {
                                solutions.Add(i);
                                break;
                            }
                        }
                    }
                    finally
                    {
                        searchedRanges.Add(range.Item1);
                    }
                });

            if (solutions.IsEmpty)
            {
                throw new ArgumentException("No answer was found for the specified secret key.", nameof(secretKey));
            }

            return solutions.Min();
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string secretKey = args[0];
            int zeroes = ParseInt32(args[1]);

            LowestZeroHash = GetLowestPositiveNumberWithStartingZeroes(secretKey, zeroes);

            if (Verbose)
            {
                Logger.WriteLine(
                    "The lowest positive number for a hash starting with {0} zeroes is {1:N0}.",
                    zeroes,
                    LowestZeroHash);
            }

            return 0;
        }

        /// <summary>
        /// Returns whether the specified integer is a solution for the specified secret key and number of zeroes.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="secretKey">The secret key to use.</param>
        /// <param name="zeroes">The number of zeroes to get the value for.</param>
        /// <param name="algorithm">The <see cref="HashAlgorithm"/> to use.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> is a solution; otherwise <see langword="false"/>.
        /// </returns>
        private static bool IsSolution(int value, string secretKey, int zeroes, HashAlgorithm algorithm)
        {
            string formattted = Format("{0}{1}", secretKey, value);
            byte[] buffer = Encoding.UTF8.GetBytes(formattted);
            byte[] hash = algorithm.ComputeHash(buffer);

            int wholeBytes = zeroes / 2;
            bool hasHalfByte = zeroes % 2 == 1;

            int sum = hash[0];

            // Are the whole bytes all zero?
            for (int j = 1; sum == 0 && j < wholeBytes; j++)
            {
                sum += hash[j];
            }

            if (sum == 0)
            {
                // The current value is a solution if there is an even number
                // of zeroes of if the low bits of the odd byte are zero.
                if (!hasHalfByte || hash[wholeBytes] < 0x10)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
