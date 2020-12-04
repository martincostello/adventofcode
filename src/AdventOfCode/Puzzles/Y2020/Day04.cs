// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/4</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day04 : Puzzle2020
    {
        /// <summary>
        /// The required keys for passports. This field is read-only.
        /// </summary>
        private static readonly string[] RequiredKeys = { "byr", "ecl", "eyr", "hcl", "hgt", "iyr", "pid" };

        /// <summary>
        /// Gets the number of valid passports.
        /// </summary>
        public int ValidPassports { get; private set; }

        /// <summary>
        /// Gets the number of valid passports in the specified batch file.
        /// </summary>
        /// <param name="batch">The batch file to get the number of valid passports from.</param>
        /// <returns>
        /// The number of valid passports specified by <paramref name="batch"/>.
        /// </returns>
        public static int GetValidPassportCount(ICollection<string> batch)
        {
            var passports = new List<NameValueCollection>();
            var current = new NameValueCollection();

            foreach (string line in batch)
            {
                if (string.IsNullOrEmpty(line))
                {
                    passports.Add(current);
                    current = new NameValueCollection();
                    continue;
                }

                string[] split = line.Split(' ');

                foreach (string pair in split)
                {
                    string[] attribute = pair.Split(':');
                    current[attribute[0]] = attribute[1];
                }
            }

            if (current.Count > 0)
            {
                passports.Add(current);
            }

            int result = 0;

            foreach (var passport in passports)
            {
                bool isValid = passport.AllKeys.Intersect(RequiredKeys).Count() == RequiredKeys.Length;

                if (isValid)
                {
                    result++;
                }
            }

            return result;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> batch = ReadResourceAsLines();

            ValidPassports = GetValidPassportCount(batch);

            if (Verbose)
            {
                Logger.WriteLine("There are {0} valid passports.", ValidPassports);
            }

            return 0;
        }
    }
}
