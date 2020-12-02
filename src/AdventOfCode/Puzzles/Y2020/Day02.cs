// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/2</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day02 : Puzzle2020
    {
        /// <summary>
        /// Gets the number of valid passwords.
        /// </summary>
        public int ValidPasswords { get; private set; }

        /// <summary>
        /// Gets whether the specified password is valid as determined by its criteria.
        /// </summary>
        /// <param name="value">The value to test for validity.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> is valid; otherwise <see langword="false"/>.
        /// </returns>
        public static bool IsPasswordValid(string value)
        {
            string[] parts = value.Split(' ');
            string[] range = parts[0].Split('-');

            int minimumCount = ParseInt32(range[0]);
            int maximumCount = ParseInt32(range[1]);
            char requiredCharacter = parts[1][0];
            string password = parts[2];

            return IsPasswordValid(password, requiredCharacter, minimumCount, maximumCount);
        }

        /// <summary>
        /// Gets the number of valid passwords in the specified list.
        /// </summary>
        /// <param name="values">The values to validate passwords against.</param>
        /// <returns>
        /// The number of valid passwords in the specified set.
        /// </returns>
        public static int GetValidPasswordCount(IEnumerable<string> values)
        {
            int result = 0;

            foreach (string value in values)
            {
                string[] parts = value.Split(' ');
                string[] range = parts[0].Split('-');

                int minimumCount = ParseInt32(range[0]);
                int maximumCount = ParseInt32(range[1]);
                char requiredCharacter = parts[1][0];
                string password = parts[2];

                if (IsPasswordValid(password, requiredCharacter, minimumCount, maximumCount))
                {
                    result++;
                }
            }

            return result;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> values = ReadResourceAsLines();

            ValidPasswords = GetValidPasswordCount(values);

            if (Verbose)
            {
                Logger.WriteLine("There are {0} valid passwords.", ValidPasswords);
            }

            return 0;
        }

        /// <summary>
        /// Gets whether the specified password is valid.
        /// </summary>
        /// <param name="password">The password to test for validity.</param>
        /// <param name="requiredCharacter">The character required to be in the password.</param>
        /// <param name="minimumCount">The minimum number of occurences of the character required.</param>
        /// <param name="maximumCount">The maximum number of occurences of the character required.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="password"/> is valid; otherwise <see langword="false"/>.
        /// </returns>
        private static bool IsPasswordValid(
            string password,
            char requiredCharacter,
            int minimumCount,
            int maximumCount)
        {
            int count = password.Count((p) => p == requiredCharacter);
            return count >= minimumCount && count <= maximumCount;
        }
    }
}
