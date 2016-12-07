// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2016/day/6</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day06 : Puzzle2016
    {
        /// <summary>
        /// Gets the error corrected message.
        /// </summary>
        public string ErrorCorrectedMessage { get; private set; }

        /// <summary>
        /// Decrypts the specified message using a repetition code.
        /// </summary>
        /// <param name="messages">The jammed messages.</param>
        /// <returns>
        /// The decrypted message derived from <paramref name="messages"/>.
        /// </returns>
        internal static string DecryptMessage(ICollection<string> messages)
        {
            int length = messages.First().Length;

            var result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                char ch = messages
                    .Select((p) => p[i])
                    .GroupBy((p) => p)
                    .OrderByDescending((p) => p.Count())
                    .Select((p) => p.Key)
                    .First();

                result.Append(ch);
            }

            return result.ToString();
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> messages = ReadResourceAsLines();

            ErrorCorrectedMessage = DecryptMessage(messages);

            Console.WriteLine($"The error-corrected message is: {ErrorCorrectedMessage}.");

            return 0;
        }
    }
}
