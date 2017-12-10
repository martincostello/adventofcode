// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2017/day/10</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day10 : Puzzle2017
    {
        /// <summary>
        /// Gets the product of multiplying the first two elements after the hash is applied to the input.
        /// </summary>
        public int ProductOfFirstTwoElements { get; private set; }

        /// <summary>
        /// Finds the product of the first two elements in a string of the specified size when
        /// the hash specified by the specified list of lengths to use for knots is applied.
        /// </summary>
        /// <param name="size">The total size of the string to hash.</param>
        /// <param name="lengths">The lengths to use for the knots of the hash.</param>
        /// <returns>
        /// The product of the first two elements in the string once the hash of knots has been applied.
        /// </returns>
        public static int FindProductOfFirstTwoHashElements(int size, IEnumerable<int> lengths)
        {
            IList<int> sequence = Hash(size, lengths);
            return sequence[0] * sequence[1];
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string rawLengths = ReadResourceAsString().Trim();

            ICollection<int> lengths = rawLengths
                .Split(Arrays.Comma)
                .Select((p) => ParseInt32(p))
                .ToList();

            ProductOfFirstTwoElements = FindProductOfFirstTwoHashElements(256, lengths);

            Console.WriteLine($"The product of the first two elements of the hash is {ProductOfFirstTwoElements:N0}.");

            return 0;
        }

        /// <summary>
        /// Applies the hash to a string of the specified size using the specified lengths for knots.
        /// </summary>
        /// <param name="size">The total size of the string to hash.</param>
        /// <param name="lengths">The lengths to use for the knots of the hash.</param>
        /// <returns>
        /// The final hashed string's positions after applying the hash from the values in <paramref name="lengths"/>.
        /// </returns>
        private static IList<int> Hash(int size, IEnumerable<int> lengths)
        {
            var sequence = Enumerable.Range(0, size).ToArray();

            int index = 0;
            int skip = 0;

            foreach (int length in lengths)
            {
                Hash(sequence, index, length);

                index += (length + skip) % sequence.Length;
                skip++;
            }

            return sequence;
        }

        /// <summary>
        /// Applies the hash to the specified sequence from the specified position for the given length.
        /// </summary>
        /// <param name="sequence">The sequence to apply the hash to.</param>
        /// <param name="index">The index to apply the hash from.</param>
        /// <param name="length">The length to use to apply the hash.</param>
        private static void Hash(int[] sequence, int index, int length)
        {
            int CircularIndex(int i) => (index + i) % sequence.Length;
            int ReverseIndex(int i) => length - (i % length) - 1;

            int[] subset = new int[length];

            for (int i = 0; i < length; i++)
            {
                int target = CircularIndex(i);
                int destination = ReverseIndex(i);

                subset[destination] = sequence[target];
            }

            for (int i = 0; i < length; i++)
            {
                int target = CircularIndex(i);
                sequence[target] = subset[i];
            }
        }
    }
}
