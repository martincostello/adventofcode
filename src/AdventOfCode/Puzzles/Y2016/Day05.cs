// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2016/day/5</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day05 : Puzzle2016
    {
        /// <summary>
        /// Gets the password for the door.
        /// </summary>
        public string? Password { get; private set; }

        /// <summary>
        /// Gets the password for the door when the hash indicates the position of password characters.
        /// </summary>
        public string? PasswordWhenPositionIsIndicated { get; private set; }

        /// <inheritdoc />
        protected override int MinimumArguments => 1;

        /// <summary>
        /// Generates the password for the door with the specified Id.
        /// </summary>
        /// <param name="doorId">The door Id to generate the password for.</param>
        /// <param name="isPositionSpecifiedByHash">Whether the hash specifies the position containing the password characters.</param>
        /// <returns>
        /// The password for the door specified by <paramref name="doorId"/>.
        /// </returns>
        internal static string GeneratePassword(string doorId, bool isPositionSpecifiedByHash)
        {
            const int PasswordLength = 8;

            var characters = new Dictionary<int, char>();
            int index = 0;

            byte[] doorIdAsBytes = Encoding.ASCII.GetBytes(doorId);

            using (HashAlgorithm algorithm = MD5.Create())
            {
                while (characters.Count < PasswordLength)
                {
                    byte[] buffer = GenerateBytesToHash(doorIdAsBytes, index);
                    byte[] hashBytes = algorithm.ComputeHash(buffer);

                    // Are the first four characters 0?
                    if (hashBytes[0] == 0 && hashBytes[1] == 0)
                    {
                        string hash = GetStringForHash(hashBytes);

                        if (hash[4] == '0')
                        {
                            if (isPositionSpecifiedByHash)
                            {
                                int position = hash[5] - '0';

                                if (position < PasswordLength && !characters.ContainsKey(position))
                                {
                                    characters[position] = hash[6];
                                }
                            }
                            else
                            {
                                characters[characters.Count] = hash[5];
                            }
                        }
                    }

                    index++;
                }
            }

            return string.Join(string.Empty, characters.OrderBy((p) => p.Key).Select((p) => p.Value));
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string doorId = args[0];

            Password = GeneratePassword(doorId, isPositionSpecifiedByHash: false);
            PasswordWhenPositionIsIndicated = GeneratePassword(doorId, isPositionSpecifiedByHash: true);

            if (Verbose)
            {
                Logger.WriteLine($"The password for door '{doorId}' is '{Password}'.");
                Logger.WriteLine($"The password for door '{doorId}' is '{PasswordWhenPositionIsIndicated}' when the position is specified in the hash.");
            }

            return 0;
        }

        /// <summary>
        /// Generates the hash bytes for the specified door Id and index.
        /// </summary>
        /// <param name="doorIdAsBytes">The bytes of the hash of the door Id.</param>
        /// <param name="index">The index to generate the hash with.</param>
        /// <returns>
        /// A <see cref="Array"/> of <see cref="byte"/> containing the hash for the specified door Id and index.
        /// </returns>
        private static byte[] GenerateBytesToHash(byte[] doorIdAsBytes, int index)
        {
            byte[] indexAsBytes = Encoding.ASCII.GetBytes(index.ToString(CultureInfo.InvariantCulture));

            byte[] result = new byte[indexAsBytes.Length + doorIdAsBytes.Length];

            Array.Copy(doorIdAsBytes, 0, result, 0, doorIdAsBytes.Length);
            Array.Copy(indexAsBytes, 0, result, doorIdAsBytes.Length, indexAsBytes.Length);

            return result;
        }

        /// <summary>
        /// Converts the specified <see cref="Array"/> of <see cref="byte"/> to a
        /// hexadecimal <see cref="string"/> representation of the hash.
        /// </summary>
        /// <param name="bytes">The hash bytes to generate the string representation of.</param>
        /// <returns>
        /// A <see cref="string"/> containing the hexadecimal representation of <paramref name="bytes"/>.
        /// </returns>
        private static string GetStringForHash(ReadOnlySpan<byte> bytes)
        {
            // TODO Use Convert.ToHexString() from 5.0 preview 8
            var hash = new StringBuilder("0000", bytes.Length + 4);

            // Skip the first 2 bytes as they should already have been checked to be zero
            for (int i = 2; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2", CultureInfo.InvariantCulture));
            }

            return hash.ToString();
        }
    }
}
