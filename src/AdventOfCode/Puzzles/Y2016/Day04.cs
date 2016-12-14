// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2016/day/4</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day04 : Puzzle2016
    {
        /// <summary>
        /// Gets the sector Id of the room where North Pole objects are stored.
        /// </summary>
        public int SectorIdOfNorthPoleObjectsRoom { get; private set; }

        /// <summary>
        /// Gets the sum of the sector Ids of all real rooms.
        /// </summary>
        public int SumOfSectorIdsOfRealRooms { get; private set; }

        /// <summary>
        /// Decrypts the name of the specified room.
        /// </summary>
        /// <param name="name">The encrypted name of the room.</param>
        /// <returns>
        /// The decrypted name of the room.
        /// </returns>
        internal static string DecryptRoomName(string name)
        {
            string unused;
            return DecryptRoomName(name, out unused);
        }

        /// <summary>
        /// Returns whether the specified room is real.
        /// </summary>
        /// <param name="name">The encrypted name of the room.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="name"/> is a real room; otherwise <see langword="false"/>.
        /// </returns>
        internal static bool IsRoomReal(string name)
        {
            string unused1;
            string unused2;

            return IsRoomReal(name, out unused1, out unused2);
        }

        /// <summary>
        /// Returns the sum of the sector Ids of the specified room names which are real.
        /// </summary>
        /// <param name="names">The names of the rooms to compute the sum from.</param>
        /// <returns>
        /// The sum of the sector Ids for room names specified by <paramref name="names"/> which are real.
        /// </returns>
        internal static int GetSectorIdOfNorthPoleObjectsRoom(IEnumerable<string> names)
        {
            int sectorId = -1;

            foreach (string name in names)
            {
                string sectorIdString;
                string decryptedName = DecryptRoomName(name, out sectorIdString);

                if (string.Equals("northpole object storage", decryptedName, StringComparison.OrdinalIgnoreCase))
                {
                    sectorId = int.Parse(sectorIdString, CultureInfo.InvariantCulture);
                }
            }

            return sectorId;
        }

        /// <summary>
        /// Returns the sum of the sector Ids of the specified room names which are real.
        /// </summary>
        /// <param name="names">The names of the rooms to compute the sum from.</param>
        /// <returns>
        /// The sum of the sector Ids for room names specified by <paramref name="names"/> which are real.
        /// </returns>
        internal static int SumOfRealRoomSectorIds(IEnumerable<string> names)
        {
            int sum = 0;

            foreach (string name in names)
            {
                string encryptedName;
                string sectorIdString;

                if (IsRoomReal(name, out encryptedName, out sectorIdString))
                {
                    sum += int.Parse(sectorIdString, CultureInfo.InvariantCulture);
                }
            }

            return sum;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> names = ReadResourceAsLines();

            SumOfSectorIdsOfRealRooms = SumOfRealRoomSectorIds(names);
            SectorIdOfNorthPoleObjectsRoom = GetSectorIdOfNorthPoleObjectsRoom(names);

            Console.WriteLine("The sum of the sector Ids of the real rooms is {0:N0}.", SumOfSectorIdsOfRealRooms);
            Console.WriteLine("The sector ID of the room where North Pole objects are stored is {0:N0}.", SumOfSectorIdsOfRealRooms);

            return 0;
        }

        /// <summary>
        /// Decrypts the name of the specified room.
        /// </summary>
        /// <param name="name">The encrypted name of the room.</param>
        /// <param name="sectorId">When the method returns, contains the sector Id of the room.</param>
        /// <returns>
        /// The decrypted name of the room if the room is real; otherwise the empty string.
        /// </returns>
        private static string DecryptRoomName(string name, out string sectorId)
        {
            var builder = new StringBuilder();

            string encryptedName;

            if (IsRoomReal(name, out encryptedName, out sectorId))
            {
                int sectorIdValue = int.Parse(sectorId, CultureInfo.InvariantCulture);

                foreach (char ch in encryptedName)
                {
                    if (ch == '-')
                    {
                        builder.Append(' ');
                    }
                    else
                    {
                        int shift = sectorIdValue % 26;
                        int shifted = ch + shift;

                        if (shifted > 'z')
                        {
                            shifted -= 26;
                        }

                        builder.Append((char)shifted);
                    }
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Returns whether the specified room is real.
        /// </summary>
        /// <param name="name">The encrypted name of the room.</param>
        /// <param name="encryptedName">When the method returns, contains the encrypted name of the room.</param>
        /// <param name="sectorId">When the method returns, contains the sector Id of the room.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="name"/> is a real room; otherwise <see langword="false"/>.
        /// </returns>
        private static bool IsRoomReal(string name, out string encryptedName, out string sectorId)
        {
            int indexOfChecksum = name.IndexOf('[');

            string checksum = name.Substring(indexOfChecksum + 1, 5);

            int indexOfLastDash = name.LastIndexOf('-');

            sectorId = name.Substring(indexOfLastDash + 1, indexOfChecksum - indexOfLastDash - 1);

            encryptedName = name.Substring(0, indexOfLastDash);
            string encryptedNameLetters = encryptedName.Replace("-", string.Empty);

            var top5Letters = encryptedNameLetters
                .GroupBy((p) => p)
                .OrderByDescending((p) => p.Count())
                .ThenBy((p) => p.Key)
                .Take(5)
                .Select((p) => p.Key)
                .ToArray();

            string computedChecksum = string.Join(string.Empty, top5Letters);

            return string.Equals(computedChecksum, checksum, StringComparison.Ordinal);
        }
    }
}
