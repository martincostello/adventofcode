// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/day/5</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day05 : Puzzle
    {
        /// <summary>
        /// Gets the number of 'nice' strings.
        /// </summary>
        internal int NiceStringCount { get; private set; }

        /// <inheritdoc />
        protected override bool IsFirstArgumentFilePath => true;

        /// <inheritdoc />
        protected override int MinimumArguments => 2;

        /// <summary>
        /// Returns whether the specified string is 'nice' using the first set of criteria.
        /// </summary>
        /// <param name="value">The string to test for niceness.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> is 'nice'; otherwise <see langword="false"/>.
        /// </returns>
        internal static bool IsNiceV1(string value)
        {
            // The string is not nice if it contain any of the following sequences
            if (new[] { "ab", "cd", "pq", "xy" }.Any((p) => value.Contains(p)))
            {
                return false;
            }

            int vowels = 0;
            bool hasAnyConsecutiveLetters = false;

            // The string is nice if it has three or more vowels and at least two consecutive letters
            Func<bool> isNice = () => hasAnyConsecutiveLetters && vowels > 2;

            for (int i = 0; i < value.Length; i++)
            {
                char current = value[i];

                if (IsVowel(current))
                {
                    vowels++;
                }

                if (i > 0 && !hasAnyConsecutiveLetters)
                {
                    hasAnyConsecutiveLetters = current == value[i - 1];
                }

                if (isNice())
                {
                    // Criteria all met, no further analysis required
                    return true;
                }
            }

            return isNice();
        }

        /// <summary>
        /// Returns whether the specified string is 'nice' using the second set of criteria.
        /// </summary>
        /// <param name="value">The string to test for niceness.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> is 'nice'; otherwise <see langword="false"/>.
        /// </returns>
        internal static bool IsNiceV2(string value)
        {
            return
                HasPairOfLettersWithMoreThanOneOccurence(value) &&
                HasLetterThatIsTheBreadOfALetterSandwich(value);
        }

        /// <summary>
        /// Tests whether a string contains a pair of any two letters that
        /// appear at least twice in the string without overlapping.
        /// </summary>
        /// <param name="value">The value to test against the rule.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> meets the rule; otherwise <see langword="false"/>.
        /// </returns>
        internal static bool HasPairOfLettersWithMoreThanOneOccurence(string value)
        {
            Dictionary<string, IList<int>> letterPairs = new Dictionary<string, IList<int>>();

            for (int i = 0; i < value.Length - 1; i++)
            {
                char first = value[i];
                char second = value[i + 1];

                string pair = new string(new[] { first, second });

                IList<int> indexes;

                if (!letterPairs.TryGetValue(pair, out indexes))
                {
                    indexes = letterPairs[pair] = new List<int>();
                }

                if (!indexes.Contains(i - 1))
                {
                    indexes.Add(i);
                }
            }

            return letterPairs.Any((p) => p.Value.Count > 1);
        }

        /// <summary>
        /// Tests whether a string contains at least one letter which repeats with exactly one letter between them.
        /// </summary>
        /// <param name="value">The value to test against the rule.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> meets the rule; otherwise <see langword="false"/>.
        /// </returns>
        internal static bool HasLetterThatIsTheBreadOfALetterSandwich(string value)
        {
            if (value.Length < 3)
            {
                // The value is not long enough
                return false;
            }

            for (int i = 1; i < value.Length - 1; i++)
            {
                if (value[i - 1] == value[i + 1])
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            int version = -1;

            switch (args[1])
            {
                case "1":
                    version = 1;
                    break;

                case "2":
                    version = 2;
                    break;

                default:
                    break;
            }

            if (version == -1)
            {
                Console.Error.WriteLine("The rules version specified is invalid.");
                return -1;
            }

            int count = 0;
            Func<string, bool> rule;

            if (version == 1)
            {
                rule = IsNiceV1;
            }
            else
            {
                rule = IsNiceV2;
            }

            foreach (string value in File.ReadLines(args[0]))
            {
                if (rule(value))
                {
                    count++;
                }
            }

            NiceStringCount = count;

            Console.WriteLine("{0:N0} strings are nice using version {1} of the rules.", NiceStringCount, version);

            return 0;
        }

        /// <summary>
        /// Returns whether the specified letter is a vowel.
        /// </summary>
        /// <param name="letter">The letter to test for being a vowel.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="letter"/> is a vowel; otherwise <see langword="false"/>.
        /// </returns>
        private static bool IsVowel(char letter)
        {
            switch (letter)
            {
                case 'a':
                case 'e':
                case 'i':
                case 'o':
                case 'u':
                    return true;

                default:
                    return false;
            }
        }
    }
}
