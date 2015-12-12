// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles.Impl
{
    using System;
    using System.Linq;

    /// <summary>
    /// A class defining version 1 of the niceness rule. This class cannot be inherited.
    /// </summary>
    internal sealed class NicenessRuleV1 : INicenessRule
    {
        /// <inheritdoc />
        public bool IsNice(string value)
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
        /// Returns whether the specified letter is a vowel.
        /// </summary>
        /// <param name="letter">The letter to test for being a vowel.</param>
        /// <returns><see langword="true"/> if <paramref name="letter"/> is a vowel; otherwise <see langword="false"/>.</returns>
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
