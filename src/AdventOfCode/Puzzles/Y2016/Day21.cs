// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2016/day/21</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2016, 21, MinimumArguments = 1, RequiresData = true)]
    public sealed class Day21 : Puzzle
    {
        /// <summary>
        /// Gets the result of scrambling the puzzle input.
        /// </summary>
        public string? ScrambledResult { get; private set; }

        /// <summary>
        /// Scrambles the specified text.
        /// </summary>
        /// <param name="text">The text to scramble.</param>
        /// <param name="instructions">The instructions to use to scramble the string.</param>
        /// <param name="reverse">Whether to reverse the process.</param>
        /// <returns>
        /// The scrambled text after applying the instructions in <paramref name="instructions"/>
        /// to the text specified by <paramref name="text"/>.
        /// </returns>
        internal static string Scramble(string text, IEnumerable<string> instructions, bool reverse)
        {
            char[] values = text.ToCharArray();

            if (reverse)
            {
                instructions = instructions.Reverse();
            }

            foreach (string instruction in instructions)
            {
                Process(instruction, values, reverse);
            }

            return new string(values);
        }

        /// <summary>
        /// Reverses the span of letters at indexes X through Y (inclusive) should be reversed in order.
        /// </summary>
        /// <param name="values">The value to reverse characters for.</param>
        /// <param name="start">The index at which to start reversing the characters.</param>
        /// <param name="end">The index at which to end reversing the characters.</param>
        internal static void Reverse(char[] values, int start, int end)
        {
            char[] slice = values
                .Skip(start)
                .Take(end - start + 1)
                .Reverse()
                .ToArray();

            for (int i = start, j = 0; i <= end; i++, j++)
            {
                values[i] = slice[j];
            }
        }

        /// <summary>
        /// Rotates the characters in the specified array the specified
        /// number of steps in the specified direction.
        /// </summary>
        /// <param name="values">The value to rotate characters for.</param>
        /// <param name="right">Whether to rotate right (instead of left).</param>
        /// <param name="steps">The number of steps to rotate by.</param>
        /// <param name="reverse">Whether to reverse the process.</param>
        internal static void RotateDirection(char[] values, bool right, int steps, bool reverse)
        {
            if (reverse)
            {
                right = !right;
            }

            int rotations = steps % values.Length;

            for (int i = 0; i < rotations; i++)
            {
                if (right)
                {
                    char last = values[^1];

                    for (int j = values.Length - 2; j >= 0; j--)
                    {
                        values[j + 1] = values[j];
                    }

                    values[0] = last;
                }
                else
                {
                    char first = values[0];

                    for (int j = 1; j < values.Length; j++)
                    {
                        values[j - 1] = values[j];
                    }

                    values[^1] = first;
                }
            }
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            string text = args[0];
            bool reverse = args.Length > 1 && string.Equals(args[1], bool.TrueString, StringComparison.OrdinalIgnoreCase);

            IList<string> instructions = await ReadResourceAsLinesAsync();

            ScrambledResult = Scramble(text, instructions, reverse);

            if (Verbose)
            {
                Logger.WriteLine($"The result of {(reverse ? "un" : string.Empty)}scrambling '{text}' is '{ScrambledResult}'.");
            }

            return PuzzleResult.Create(ScrambledResult);
        }

        /// <summary>
        /// Processes the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction to process.</param>
        /// <param name="values">The characters to apply the instruction to when processes.</param>
        /// <param name="reverse">Whether to reverse the instruction.</param>
        private static void Process(string instruction, char[] values, bool reverse)
        {
            string[] split = instruction.Split(' ');

            switch (split[0])
            {
                case "move":
                    Move(values, ParseInt32(split[2]), ParseInt32(split[5]), reverse);
                    break;

                case "reverse":
                    Reverse(values, ParseInt32(split[2]), ParseInt32(split[4]));
                    break;

                case "rotate":

                    if (string.Equals(split[1], "based", StringComparison.Ordinal))
                    {
                        RotatePosition(values, split[6], reverse);
                    }
                    else
                    {
                        RotateDirection(values, string.Equals(split[1], "right", StringComparison.Ordinal), ParseInt32(split[2]), reverse);
                    }

                    break;

                case "swap":

                    if (string.Equals(split[1], "position", StringComparison.Ordinal))
                    {
                        SwapPosition(values, ParseInt32(split[2]), ParseInt32(split[5]));
                    }
                    else
                    {
                        SwapLetters(values, split[2], split[5]);
                    }

                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Moves the letter which is at index X should be removed from
        /// the string, then inserted such that it ends up at index Y.
        /// </summary>
        /// <param name="values">The value to move characters for.</param>
        /// <param name="x">The index to remove the letter from.</param>
        /// <param name="y">The index to insert the removed letter at.</param>
        /// <param name="reverse">Whether to reverse the process.</param>
        private static void Move(char[] values, int x, int y, bool reverse)
        {
            if (reverse)
            {
                int temp = x;
                x = y;
                y = temp;
            }

            string value = new string(values);
            string ch = values[x].ToString(CultureInfo.InvariantCulture);

            value = value.Remove(x, 1);
            value = value.Insert(y, ch);

            Array.Copy(value.ToCharArray(), values, values.Length);
        }

        /// <summary>
        /// Rotates the characters to the right based on the index of letter X.
        /// Once the index is determined, rotates the string to the right once,
        /// plus a number of times equal to that index, plus one additional time
        /// if the index was at least 4.
        /// </summary>
        /// <param name="values">The value to rotate characters for.</param>
        /// <param name="letter">The letter to use to perform the rotation.</param>
        /// <param name="reverse">Whether to reverse the process.</param>
        private static void RotatePosition(char[] values, string letter, bool reverse)
        {
            int index = Array.IndexOf(values, letter[0]);

            int steps;

            if (reverse)
            {
                steps = 1;

                if (index == 0 || index % 2 != 0)
                {
                    steps += index / 2;
                }
                else
                {
                    steps += index + Math.Abs((index / 2) - 4);
                }
            }
            else
            {
                steps = 1 + index + (index >= 4 ? 1 : 0);
            }

            RotateDirection(values, right: !reverse, steps: steps, reverse: false);
        }

        /// <summary>
        /// Swaps letter X with letter Y.
        /// </summary>
        /// <param name="values">The value to swap characters for.</param>
        /// <param name="x">The first character to swap.</param>
        /// <param name="y">The second character to swap.</param>
        private static void SwapLetters(char[] values, string x, string y)
        {
            char first = x[0];
            char second = y[0];

            var indexesOfX = new List<int>(values.Length / 2);
            var indexesOfY = new List<int>(indexesOfX.Capacity);

            for (int i = 0; i < values.Length; i++)
            {
                char ch = values[i];

                if (ch == first)
                {
                    indexesOfX.Add(i);
                }
                else if (ch == second)
                {
                    indexesOfY.Add(i);
                }
            }

            foreach (int i in indexesOfX)
            {
                values[i] = second;
            }

            foreach (int i in indexesOfY)
            {
                values[i] = first;
            }
        }

        /// <summary>
        /// Swap the letters at indexes X and Y.
        /// </summary>
        /// <param name="values">The value to swap characters for.</param>
        /// <param name="x">The index of the first location to swap.</param>
        /// <param name="y">The index of the second location to swap.</param>
        private static void SwapPosition(char[] values, int x, int y)
        {
            char first = values[x];
            char second = values[y];

            values[x] = second;
            values[y] = first;
        }
    }
}
