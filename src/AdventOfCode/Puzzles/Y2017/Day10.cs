// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2017/day/10</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day10 : Puzzle
    {
        /// <summary>
        /// The default sequence length to use.
        /// </summary>
        private const int SequenceLength = 256;

        /// <summary>
        /// Gets the product of multiplying the first two elements after the hash is applied to the input.
        /// </summary>
        public int ProductOfFirstTwoElements { get; private set; }

        /// <summary>
        /// Gets the hexadecimal representation of the dense hash of the input.
        /// </summary>
        public string? DenseHash { get; private set; }

        /// <summary>
        /// Computes the hash of the specified sequence of ASCII-encoded bytes.
        /// </summary>
        /// <param name="asciiBytes">The ASCII-encoded bytes to hash.</param>
        /// <returns>
        /// The hexadecimal dense hash of <paramref name="asciiBytes"/>.
        /// </returns>
        public static string ComputeHash(string asciiBytes)
        {
            int[] lengths = Encoding.ASCII.GetBytes(asciiBytes)
                .Select((p) => (int)p)
                .Concat(new[] { 17, 31, 73, 47, 23 })
                .ToArray();

            int index = 0;
            int skip = 0;

            int[] sequence = CreateSequence(SequenceLength);

            const int Rounds = 64;

            for (int i = 0; i < Rounds; i++)
            {
                Hash(sequence, lengths, ref index, ref skip);
            }

            return ComputeDenseHash(sequence);
        }

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
            int index = 0;
            int skip = 0;

            int[] sequence = CreateSequence(size);

            Hash(sequence, lengths, ref index, ref skip);

            return sequence[0] * sequence[1];
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string rawLengths = ReadResourceAsString().Trim();

            ICollection<int> lengths = rawLengths
                .Split(',')
                .Select((p) => ParseInt32(p))
                .ToList();

            ProductOfFirstTwoElements = FindProductOfFirstTwoHashElements(SequenceLength, lengths);
            DenseHash = ComputeHash(rawLengths);

            if (Verbose)
            {
                Logger.WriteLine($"The product of the first two elements of the hash is {ProductOfFirstTwoElements:N0}.");
                Logger.WriteLine($"The hexadecimal dense hash of the input is {DenseHash}.");
            }

            return 0;
        }

        /// <summary>
        /// Applies the hash to the specified sequence using the specified lengths for knots.
        /// </summary>
        /// <param name="sequence">The sequence to apply the hash to.</param>
        /// <param name="lengths">The lengths to use for the knots of the hash.</param>
        /// <param name="index">The index to start the hash from.</param>
        /// <param name="skip">The skip value to use.</param>
        private static void Hash(int[] sequence, IEnumerable<int> lengths, ref int index, ref int skip)
        {
            foreach (int length in lengths)
            {
                Hash(sequence, index, length);

                index += (length + skip) % sequence.Length;
                skip++;
            }
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

        /// <summary>
        /// Computes the dense hash for the specified sparse hash.
        /// </summary>
        /// <param name="sparseHash">The input to use to generate the dense hash.</param>
        /// <returns>
        /// The hexadecimal dense hash of <paramref name="sparseHash"/>.
        /// </returns>
        private static string ComputeDenseHash(int[] sparseHash)
        {
            const int BlockSize = 16;

            var denseHash = new List<int>(sparseHash.Length / BlockSize);

            for (int i = 0; i < sparseHash.Length; i += BlockSize)
            {
                int part = 0;

                for (int j = 0; j < BlockSize; j++)
                {
                    part ^= sparseHash[i + j];
                }

                denseHash.Add(part);
            }

            var builder = new StringBuilder(denseHash.Count);

            foreach (int value in denseHash)
            {
                builder.Append(value.ToString("x2", CultureInfo.InvariantCulture));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Creates an ascending sequence of numbers of the specified length.
        /// </summary>
        /// <param name="length">The length of the sequence to generate.</param>
        /// <returns>
        /// An array containing an ascending sequence of numbers of the length specified by <paramref name="length"/>.
        /// </returns>
        private static int[] CreateSequence(int length) => Enumerable.Range(0, length).ToArray();
    }
}
