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

namespace MartinCostello.AdventOfCode.Day8
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// A console application that solves <c>http://adventofcode.com/day/8</c>. This class cannot be inherited.
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

            string[] input = File.ReadAllLines(args[0]);

            int countForCode = input.Sum((p) => p.Length);
            int countInMemory = GetLiteralCharacterCount(input);

            Console.WriteLine(
                "The number of characters of code for string literals minus the number of characters in memory for the values of the strings is {0:N0}.",
                countForCode - countInMemory);

            return 0;
        }

        /// <summary>
        /// Returns the number of literal characters in the specified collection of <see cref="string"/>.
        /// </summary>
        /// <param name="value">The values to get the number of literal characters from.</param>
        /// <returns>The number of literal characters in <paramref name="collection"/>.</returns>
        internal static int GetLiteralCharacterCount(IEnumerable<string> collection) => collection.Sum(GetLiteralCharacterCount);

        /// <summary>
        /// Returns the number of literal characters in the specified <see cref="string"/>.
        /// </summary>
        /// <param name="value">The value to get the number of literal characters from.</param>
        /// <returns>The number of literal characters in <paramref name="value"/>.</returns>
        internal static int GetLiteralCharacterCount(string value)
        {
            int count = 0;

            // Remove quotes if present as first/last characters
            bool removeFirstQuote = value.Length > 0 && value.First() == '\"';
            bool removeLastQuote = value.Length > 1 && value.Last() == '\"';

            if (removeFirstQuote)
            {
                value = value.Substring(1);
            }

            if (removeLastQuote)
            {
                value = value.Substring(0, value.Length - 1);
            }

            if (value.Length > 0)
            {
                Queue<char> characters = new Queue<char>(value);

                while (characters.Count > 0)
                {
                    char current = characters.Dequeue();

                    if (characters.Count > 0)
                    {
                        switch (current)
                        {
                            case '\\':
                                char next = characters.Peek();

                                if (next == '\"' || next == '\'' || next == '\\')
                                {
                                    characters.Dequeue();
                                }
                                else if (next == 'x' && characters.Count > 2)
                                {
                                    characters.Dequeue();
                                    characters.Dequeue();
                                    characters.Dequeue();
                                }

                                break;

                            default:
                                break;
                        }
                    }

                    count++;
                }
            }

            return count;
        }
    }
}
