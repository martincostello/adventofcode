// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   Program.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day11
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A console application that solves <c>http://adventofcode.com/day/11</c>. This class cannot be inherited.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry-point to the application.
        /// </summary>
        /// <param name="args">The arguments to the application.</param>
        /// <returns>The exit code from the application.</returns>
        internal static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("No input value specified.");
                return -1;
            }

            string current = args[0];
            string next = GenerateNextPassword(current);

            Console.WriteLine("Santa's new password should be '{0}'. But he really shouldn't print it out in plaintext.", next);

            return 0;
        }

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
        {
            return !value.Contains('i') && !value.Contains('o') && !value.Contains('l');
        }

        /// <summary>
        /// Tests whether an array contains a pair of any two letters that appear at least twice in the string without overlapping.
        /// </summary>
        /// <param name="value">The value to test against the rule.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> meets the rule; otherwise <see langword="false"/>.</returns>
        internal static bool HasMoreThanOneDistinctPairOfLetters(char[] value)
        {
            Dictionary<string, IList<int>> letterPairs = new Dictionary<string, IList<int>>();

            for (int i = 0; i < value.Length - 1; i++)
            {
                char first = value[i];
                char second = value[i + 1];

                if (first != second)
                {
                    continue;
                }

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

            return letterPairs.Count > 1;
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
