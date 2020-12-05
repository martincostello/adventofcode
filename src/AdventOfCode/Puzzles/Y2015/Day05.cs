// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2015/day/5</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2015, 05, MinimumArguments = 1, RequiresData = true)]
    public sealed class Day05 : Puzzle
    {
        /// <summary>
        /// The sequences of characters that are not considered nice. This field is read-only.
        /// </summary>
        private static readonly string[] NotNiceSequences = { "ab", "cd", "pq", "xy" };

        /// <summary>
        /// Gets the number of 'nice' strings.
        /// </summary>
        internal int NiceStringCount { get; private set; }

        /// <inheritdoc />
        protected override int MinimumArguments => 1;

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
            if (NotNiceSequences.Any((p) => value.Contains(p, StringComparison.Ordinal)))
            {
                return false;
            }

            int vowels = 0;
            bool hasAnyConsecutiveLetters = false;

            // The string is nice if it has three or more vowels and at least two consecutive letters
            bool IsNice() => hasAnyConsecutiveLetters && vowels > 2;

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

                if (IsNice())
                {
                    // Criteria all met, no further analysis required
                    return true;
                }
            }

            return IsNice();
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
            var letterPairs = new Dictionary<string, IList<int>>();

            for (int i = 0; i < value.Length - 1; i++)
            {
                char first = value[i];
                char second = value[i + 1];

                string pair = new string(new[] { first, second });

                if (!letterPairs.TryGetValue(pair, out IList<int>? indexes))
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
            int version = args[0] switch
            {
                "1" => 1,
                "2" => 2,
                _ => -1,
            };

            if (version == -1)
            {
                Logger.WriteLine("The rules version specified is invalid.");
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

            foreach (string value in ReadResourceAsLines())
            {
                if (rule(value))
                {
                    count++;
                }
            }

            NiceStringCount = count;

            if (Verbose)
            {
                Logger.WriteLine("{0:N0} strings are nice using version {1} of the rules.", NiceStringCount, version);
            }

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
            return letter switch
            {
                'a' => true,
                'e' => true,
                'i' => true,
                'o' => true,
                'u' => true,
                _ => false,
            };
        }
    }
}
