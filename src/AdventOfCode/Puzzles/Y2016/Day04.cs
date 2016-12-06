// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2016/day/4</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day04 : Puzzle2016
    {
        /// <summary>
        /// An array containing dashes. This field is read-only.
        /// </summary>
        private static readonly char[] Separator = new[] { '-' };

        /// <summary>
        /// Gets the sum of the sector Ids of all real rooms.
        /// </summary>
        public int SumOfSectorIdsOfRealRooms { get; private set; }

        /// <summary>
        /// Returns whether the specified room is real.
        /// </summary>
        /// <param name="name">The encrypted name of the room.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="name"/> is a real room; otherwise <see langword="false"/>.
        /// </returns>
        internal static bool IsRoomReal(string name)
        {
            string unused;
            return IsRoomReal(name, out unused);
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
                string sectorIdString;

                if (IsRoomReal(name, out sectorIdString))
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

            Console.WriteLine("The sum of the sector Ids of the real rooms is {0:N0}.", SumOfSectorIdsOfRealRooms);

            return 0;
        }

        /// <summary>
        /// Returns whether the specified room is real.
        /// </summary>
        /// <param name="name">The encrypted name of the room.</param>
        /// <param name="sectorId">When the method returns, contains the sector Id of the room.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="name"/> is a real room; otherwise <see langword="false"/>.
        /// </returns>
        private static bool IsRoomReal(string name, out string sectorId)
        {
            sectorId = null;

            string[] split = name.Split(Separator, StringSplitOptions.None);

            string encryptedName = string.Join(string.Empty, split.Take(split.Length - 1));
            string sectorIdAndChecksum = split[split.Length - 1];

            int index = sectorIdAndChecksum.IndexOf('[');

            sectorId = sectorIdAndChecksum.Substring(0, index);
            string checksum = sectorIdAndChecksum.Substring(index + 1, 5);

            var top5Letters = encryptedName
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
