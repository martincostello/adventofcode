// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2017/day/1</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day01 : Puzzle2017
    {
        /// <summary>
        /// Gets the solution to the captcha.
        /// </summary>
        public int CaptchaSolution { get; private set; }

        /// <summary>
        /// Calculates the sum of all digits that match the next digit in the specified string.
        /// </summary>
        /// <param name="digits">A <see cref="string"/> of digits to sum.</param>
        /// <returns>
        /// The sum of all digits that match the next digit in <paramref name="digits"/>.
        /// </returns>
        public static int CalculateSum(string digits)
        {
            int sum = 0;

            for (int i = 0; i < digits.Length - 1; i++)
            {
                char first = digits[i];
                char second = digits[i + 1];

                if (first == second)
                {
                    sum += first - '0';
                }
            }

            if (digits[0] == digits[digits.Length - 1])
            {
                sum += digits[0] - '0';
            }

            return sum;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string digits = ReadResourceAsString().TrimEnd();

            CaptchaSolution = CalculateSum(digits);

            return 0;
        }
    }
}
