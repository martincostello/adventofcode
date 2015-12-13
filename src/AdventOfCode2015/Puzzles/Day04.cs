// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/day/4</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day04 : IPuzzle
    {
        /// <summary>
        /// Gets the lowest value that produces a hash that starts with the required number of zeroes.
        /// </summary>
        internal int LowestZeroHash { get; private set; }

        /// <inheritdoc />
        public int Solve(string[] args)
        {
            if (args.Length != 2)
            {
                Console.Error.WriteLine("No secret key and number of zeroes specified.");
                return -1;
            }

            string secretKey = args[0];
            int zeroes = int.Parse(args[1], CultureInfo.InvariantCulture);

            LowestZeroHash = GetLowestPositiveNumberWithStartingZeroes(secretKey, zeroes);

            Console.WriteLine(
                "The lowest positive number for a hash starting with {0} zeroes is {1:N0}.",
                zeroes,
                LowestZeroHash);

            return 0;
        }

        /// <summary>
        /// Gets the lowest positive integer which when combined with a secret key has an MD5 hash whose
        /// hexadecimal representation starts with the specified number of zeroes.
        /// </summary>
        /// <param name="secretKey">The secret key to use.</param>
        /// <param name="zeroes">The number of zeroes to get the value for.</param>
        /// <returns>The lowest positive integer that generates an MD5 hash with the number of zeroes specified.</returns>
        internal static int GetLowestPositiveNumberWithStartingZeroes(string secretKey, int zeroes)
        {
            var bag = new ConcurrentBag<int>();
            var source = Partitioner.Create(1, int.MaxValue - 1, 50000);

            Parallel.ForEach(
                source,
                (range, state) =>
                {
                    // Does this range start at a value greater than an already found value?
                    if (bag.Count > 0 && range.Item1 > bag.Min())
                    {
                        return;
                    }

                    for (int i = range.Item1; !state.ShouldExitCurrentIteration && i < range.Item2; i++)
                    {
                        string value = string.Format(CultureInfo.InvariantCulture, "{0}{1}", secretKey, i);
                        byte[] hash = ComputeMD5(value);

                        int wholeBytes = zeroes / 2;
                        bool hasHalfByte = zeroes % 2 == 1;

                        int sum = hash[0];

                        for (int j = 1; sum == 0 && j < wholeBytes; j++)
                        {
                            sum += hash[j];
                        }

                        if (sum == 0)
                        {
                            if (!hasHalfByte || hash[wholeBytes] < 0x10)
                            {
                                bag.Add(i);
                            }
                        }
                    }
                });

            if (bag.Count < 1)
            {
                throw new ArgumentException("No answer was found for the specified secret key.", nameof(secretKey));
            }

            return bag
                .ToArray()
                .Min();
        }

        /// <summary>
        /// Gets the MD5 hash of the specified <see cref="string"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to get the MD5 hash of.</param>
        /// <returns>The hash of <paramref name="value"/>.</returns>
        private static byte[] ComputeMD5(string value)
        {
            using (HashAlgorithm algorithm = HashAlgorithm.Create("MD5"))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(value);
                return algorithm.ComputeHash(buffer);
            }
        }
    }
}
