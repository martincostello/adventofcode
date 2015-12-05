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

namespace MartinCostello.AdventOfCode.Day5
{
    using System;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// A console application that solves <c>http://adventofcode.com/day/5</c>. This class cannot be inherited.
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
                Console.Error.WriteLine("No input file path specified.");
                return -1;
            }

            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine("The input file path specified cannot be found.");
                return -1;
            }

            int niceStrings = 0;

            foreach (string value in File.ReadLines(args[0]))
            {
                if (IsStringNice(value))
                {
                    niceStrings++;
                }
            }

            Console.WriteLine("{0:N0} strings are nice.", niceStrings);

            return 0;
        }

        /// <summary>
        /// Returns whether the specified string is 'nice'.
        /// </summary>
        /// <param name="value">The string to test for niceness.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> is 'nice'; otherwise <see langword="false"/>.</returns>
        internal static bool IsStringNice(string value)
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
