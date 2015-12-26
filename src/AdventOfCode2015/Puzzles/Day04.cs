// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/day/4</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day04 : Puzzle
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
            using (HashAlgorithm algorithm = HashAlgorithm.Create("MD5"))
            {
                for (int i = 1; ; i++)
                {
                    string value = Format("{0}{1}", secretKey, i);
                    byte[] buffer = Encoding.UTF8.GetBytes(value);
                    byte[] hash = algorithm.ComputeHash(buffer);

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
                            return i;
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string secretKey = args[0];
            int zeroes = ParseInt32(args[1]);

            LowestZeroHash = GetLowestPositiveNumberWithStartingZeroes(secretKey, zeroes);

            Console.WriteLine(
                "The lowest positive number for a hash starting with {0} zeroes is {1:N0}.",
                zeroes,
                LowestZeroHash);

            return 0;
        }
    }
}
