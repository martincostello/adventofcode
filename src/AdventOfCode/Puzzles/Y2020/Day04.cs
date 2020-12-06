// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/4</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2020, 04, RequiresData = true)]
    public sealed class Day04 : Puzzle
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
        /// Gets the number of verified passports.
        /// </summary>
        public int VerifiedPassports { get; private set; }

        /// <summary>
        /// Gets the number of valid passports in the specified batch file.
        /// </summary>
        /// <param name="batch">The batch file to get the number of valid passports from.</param>
        /// <returns>
        /// The number of valid passports specified by <paramref name="batch"/>.
        /// </returns>
        public static (int valid, int verified) VerifyPassports(ICollection<string> batch)
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

            int valid = 0;
            int verified = 0;

            foreach (var passport in passports)
            {
                bool isValid = passport.AllKeys.Intersect(RequiredKeys).Count() == RequiredKeys.Length;

                if (isValid)
                {
                    valid++;

                    if (IsPasswordVerified(passport))
                    {
                        verified++;
                    }
                }
            }

            return (valid, verified);
        }

        /// <inheritdoc />
        protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> batch = ReadResourceAsLines();

            (ValidPassports, VerifiedPassports) = VerifyPassports(batch);

            if (Verbose)
            {
                Logger.WriteLine("There are {0} valid passports.", ValidPassports);
                Logger.WriteLine("There are {0} verified passports.", VerifiedPassports);
            }

            return PuzzleResult.Create(ValidPassports, VerifiedPassports);
        }

        /// <summary>
        /// Returns whether the specified passport is verified.
        /// </summary>
        /// <param name="passport">The passport to verify.</param>
        /// <returns>
        /// <see langword="true"/> if the passport is verified; otherwise <see langword="false"/>.
        /// </returns>
        private static bool IsPasswordVerified(NameValueCollection passport)
        {
            static bool IsValidEyeColor(string? value)
            {
                return value switch
                {
                    "amb" => true,
                    "blu" => true,
                    "brn" => true,
                    "grn" => true,
                    "gry" => true,
                    "hzl" => true,
                    "oth" => true,
                    _ => false,
                };
            }

            static bool IsValidHairColor(string? value)
            {
                if (string.IsNullOrEmpty(value))
                {
                    return false;
                }

                return Regex.IsMatch(value, "#[0-9a-f]{6}", RegexOptions.IgnoreCase);
            }

            static bool IsValidHeight(string? value)
            {
                if (string.IsNullOrEmpty(value))
                {
                    return false;
                }

                if (value.EndsWith("cm", StringComparison.OrdinalIgnoreCase))
                {
                    return IsValidNumber(value[..^2], 150, 193);
                }
                else if (value.EndsWith("in", StringComparison.OrdinalIgnoreCase))
                {
                    return IsValidNumber(value[..^2], 59, 76);
                }

                return false;
            }

            static bool IsValidNumber(string? value, int minimum, int maximum)
            {
                if (!TryParseInt32(value, out int number))
                {
                    return false;
                }

                return number >= minimum && number <= maximum;
            }

            static bool IsValidYear(string? value, int minimum, int maximum)
                => IsValidNumber(value, minimum, maximum) && value!.Length == 4;

            foreach (string? key in passport.AllKeys)
            {
                string? value = passport[key];

                bool isValid = key switch
                {
                    "byr" => IsValidYear(value, 1920, 2002),
                    "ecl" => IsValidEyeColor(value),
                    "eyr" => IsValidYear(value, 2020, 2030),
                    "hcl" => IsValidHairColor(value),
                    "hgt" => IsValidHeight(value),
                    "iyr" => IsValidYear(value, 2010, 2020),
                    "pid" => TryParseInt32(value, out _) && value.Length == 9,
                    _ => true,
                };

                if (!isValid)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
