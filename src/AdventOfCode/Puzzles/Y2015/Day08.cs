// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2015/day/8</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day08 : Puzzle2015
    {
        /// <summary>
        /// Gets the value for the first solution.
        /// </summary>
        internal int FirstSolution { get; private set; }

        /// <summary>
        /// Gets the value for the second solution.
        /// </summary>
        internal int SecondSolution { get; private set; }

        /// <summary>
        /// Returns the number of characters in the specified collection of <see cref="string"/> if literals are encoded.
        /// </summary>
        /// <param name="collection">The values to get the encoded number of characters from.</param>
        /// <returns>The number of characters in <paramref name="collection"/> when encoded.</returns>
        internal static int GetEncodedCharacterCount(IEnumerable<string> collection) => collection.Sum(GetEncodedCharacterCount);

        /// <summary>
        /// Returns the number of characters in the specified <see cref="string"/> if literals are encoded.
        /// </summary>
        /// <param name="value">The value to get the encoded number of characters from.</param>
        /// <returns>The number of characters in <paramref name="value"/> when encoded.</returns>
        internal static int GetEncodedCharacterCount(string value)
        {
            var builder = new StringBuilder("\"", value.Length + 2);

            for (int i = 0; i < value.Length; i++)
            {
                char current = value[i];

                switch (current)
                {
                    case '\"':
                    case '\\':
                    case '\'':
                        builder.Append('\\');
                        break;

                    default:
                        break;
                }

                builder.Append(current);
            }

            builder.Append('\"');

            return builder.Length;
        }

        /// <summary>
        /// Returns the number of literal characters in the specified collection of <see cref="string"/>.
        /// </summary>
        /// <param name="collection">The values to get the number of literal characters from.</param>
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
            bool removeFirstQuote = value.Length > 0 && value[0] == '\"';
            bool removeLastQuote = value.Length > 1 && value[^1] == '\"';

            if (removeFirstQuote)
            {
                value = value[1..];
            }

            if (removeLastQuote)
            {
                value = value[0..^1];
            }

            if (value.Length > 0)
            {
                var characters = new Queue<char>(value);

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

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> input = ReadResourceAsLines();

            int countForCode = input.Sum((p) => p.Length);
            int countInMemory = GetLiteralCharacterCount(input);
            int countEncoded = GetEncodedCharacterCount(input);

            FirstSolution = countForCode - countInMemory;
            SecondSolution = countEncoded - countForCode;

            if (Verbose)
            {
                Logger.WriteLine(
                    "The number of characters of code for string literals minus the number of characters in memory for the values of the strings is {0:N0}.",
                    FirstSolution);

                Logger.WriteLine(
                    "The total number of characters to represent the newly encoded strings minus the number of characters of code in each original string literal is {0:N0}.",
                    SecondSolution);
            }

            return 0;
        }
    }
}
