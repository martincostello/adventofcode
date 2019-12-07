// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using System.Globalization;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2019/day/4</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day04 : Puzzle2019
    {
        /// <summary>
        /// Gets the number of valid passwords in the given range.
        /// </summary>
        public int Count { get; private set; }

        /// <inheritdoc />
        protected override int MinimumArguments => 1;

        /// <summary>
        /// Gets the number of valid passwords in the specified range.
        /// </summary>
        /// <param name="range">The range of numbers to get the number of passwords for.</param>
        /// <returns>
        /// The Manhattan distance from the central port to the closest intersection and the
        /// minimum number of combined steps to reach an intersection.
        /// </returns>
        public static int GetPasswordsInRange(string range)
        {
            string[] split = range.Split('-');

            int start = ParseInt32(split[0]);
            int end = ParseInt32(split[1]) + 1;

            int count = 0;

            for (int i = start; i < end; i++)
            {
                if (IsValid(i.ToString(CultureInfo.InvariantCulture)))
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Determines whether the specified password is valid.
        /// </summary>
        /// <param name="password">The password to test for validity.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="password"/> is valid; otherwise <see langword="false"/>.
        /// </returns>
        public static bool IsValid(string password)
        {
            bool foundAdjacent = false;

            for (int i = 0; i < password.Length - 1; i++)
            {
                int first = password[i] - '0';
                int second = password[i + 1] - '0';

                if (first == second)
                {
                    foundAdjacent = true;
                }

                if (first > second)
                {
                    return false;
                }
            }

            return foundAdjacent;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            Count = GetPasswordsInRange(args[0]);

            if (Verbose)
            {
                Logger.WriteLine("{0} different passwords within the range meet the criteria.", Count);
            }

            return 0;
        }
    }
}
