// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2016/day/5</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day05 : Puzzle2016
    {
        /// <summary>
        /// Gets the password for the door.
        /// </summary>
        public string Password { get; private set; }

        /// <inheritdoc />
        protected override int MinimumArguments => 1;

        /// <summary>
        /// Generates the password for the door with the specified Id.
        /// </summary>
        /// <param name="doorId">The door Id to generate the password for.</param>
        /// <returns>
        /// The password for the door specified by <paramref name="doorId"/>.
        /// </returns>
        internal static string GeneratePassword(string doorId)
        {
            var password = new StringBuilder();

            int index = 0;

            using (HashAlgorithm algorithm = MD5.Create())
            {
                while (password.Length < 8)
                {
                    byte[] buffer = GenerateBytesToHash(doorId, index);
                    byte[] hashBytes = algorithm.ComputeHash(buffer);
                    string hash = GetStringForHash(hashBytes);

                    if (hash.StartsWith("00000", StringComparison.Ordinal))
                    {
                        password.Append(hash[5]);
                    }

                    index++;
                }
            }

            return password.ToString();
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string doorId = args[0];

            Password = GeneratePassword(doorId);

            Console.WriteLine($"The password for door '{doorId}' is '{Password}'.");

            return 0;
        }

        /// <summary>
        /// Generates the hash bytes for the specified door Id and index.
        /// </summary>
        /// <param name="doorId">The door Id.</param>
        /// <param name="index">The index to generate the hash with.</param>
        /// <returns>
        /// A <see cref="Array"/> of <see cref="byte"/> containing the hash for the specified door Id and index.
        /// </returns>
        private static byte[] GenerateBytesToHash(string doorId, int index)
        {
            string key = string.Format(CultureInfo.InvariantCulture, "{0}{1}", doorId, index);
            return Encoding.UTF8.GetBytes(key);
        }

        /// <summary>
        /// Converts the specified <see cref="Array"/> of <see cref="byte"/> to a
        /// hexadecimal <see cref="string"/> representation of the hash.
        /// </summary>
        /// <param name="hashBytes">The hash bytes to generate the string representation of.</param>
        /// <returns>
        /// A <see cref="string"/> containing the hexadecimal representation of <paramref name="hashBytes"/>.
        /// </returns>
        private static string GetStringForHash(byte[] hashBytes)
        {
            var hash = new StringBuilder();

            foreach (byte b in hashBytes)
            {
                hash.AppendFormat(CultureInfo.InvariantCulture, "{0:x2}", b);
            }

            return hash.ToString();
        }
    }
}
