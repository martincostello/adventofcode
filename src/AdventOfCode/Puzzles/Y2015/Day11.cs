// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2015/day/11</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2015, 11, MinimumArguments = 1)]
    public sealed class Day11 : Puzzle
    {
        /// <summary>
        /// Gets the next password.
        /// </summary>
        internal string? NextPassword { get; private set; }

        /// <inheritdoc />
        protected override int MinimumArguments => 1;

        /// <summary>
        /// Generates the next password that should be used based on a current password value.
        /// </summary>
        /// <param name="current">The current password.</param>
        /// <returns>The next password.</returns>
        internal static string GenerateNextPassword(string current)
        {
            char[] next = current.ToCharArray();

            do
            {
                Rotate(next);
            }
            while (!(ContainsTriumvirateOfLetters(next) && ContainsNoForbiddenCharacters(next) && HasMoreThanOneDistinctPairOfLetters(next)));

            return new string(next);
        }

        /// <summary>
        /// Returns whether the specified array contains an increasing sequence of three characters at least once.
        /// </summary>
        /// <param name="value">The array to test.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> meets the rule; otherwise <see langword="false"/>.</returns>
        internal static bool ContainsTriumvirateOfLetters(char[] value)
        {
            bool result = false;

            for (int i = 0; !result && i < value.Length && (value.Length - i) > 2; i++)
            {
                result = value[i + 1] == value[i] + 1 && value[i + 2] == value[i] + 2;
            }

            return result;
        }

        /// <summary>
        /// Returns whether the specified array contains only valid characters.
        /// </summary>
        /// <param name="value">The array to test.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> meets the rule; otherwise <see langword="false"/>.</returns>
        internal static bool ContainsNoForbiddenCharacters(char[] value)
            => !value.Contains('i') && !value.Contains('o') && !value.Contains('l');

        /// <summary>
        /// Tests whether an array contains a pair of any two letters that appear at least twice in the string without overlapping.
        /// </summary>
        /// <param name="value">The value to test against the rule.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> meets the rule; otherwise <see langword="false"/>.</returns>
        internal static bool HasMoreThanOneDistinctPairOfLetters(char[] value)
        {
            var letterPairs = new Dictionary<string, IList<int>>();

            for (int i = 0; i < value.Length - 1; i++)
            {
                char first = value[i];
                char second = value[i + 1];

                if (first != second)
                {
                    continue;
                }

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

            return letterPairs.Count > 1;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string current = args[0];
            NextPassword = GenerateNextPassword(current);

            if (Verbose)
            {
                Logger.WriteLine("Santa's new password should be '{0}'.", NextPassword);
            }

            return 0;
        }

        /// <summary>
        /// Rotates the specified array of characters as if they were integers.
        /// </summary>
        /// <param name="value">The character array to rotate.</param>
        private static void Rotate(char[] value)
        {
            bool done = false;

            for (int i = value.Length - 1; !done && i > -1; i--)
            {
                if (value[i] == 'z')
                {
                    value[i] = 'a';
                }
                else
                {
                    value[i]++;
                    done = true;
                }
            }
        }
    }
}
