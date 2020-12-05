// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2016/day/6</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2016, 06, RequiresData = true)]
    public sealed class Day06 : Puzzle
    {
        /// <summary>
        /// Gets the error corrected message.
        /// </summary>
        public string? ErrorCorrectedMessage { get; private set; }

        /// <summary>
        /// Gets the error corrected message when a modified repetition code is used.
        /// </summary>
        public string? ModifiedErrorCorrectedMessage { get; private set; }

        /// <summary>
        /// Decrypts the specified message using a repetition code.
        /// </summary>
        /// <param name="messages">The jammed messages.</param>
        /// <param name="leastLikely">Whether to use the least likely value instead of the most.</param>
        /// <returns>
        /// The decrypted message derived from <paramref name="messages"/>.
        /// </returns>
        internal static string DecryptMessage(ICollection<string> messages, bool leastLikely)
        {
            int length = messages.First().Length;

            var result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                var characters = messages
                    .Select((p) => p[i])
                    .GroupBy((p) => p)
                    .OrderBy((p) => p.Count())
                    .Select((p) => p.Key);

                char ch =
                    leastLikely ?
                    characters.First() :
                    characters.Last();

                result.Append(ch);
            }

            return result.ToString();
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> messages = ReadResourceAsLines();

            ErrorCorrectedMessage = DecryptMessage(messages, leastLikely: false);
            ModifiedErrorCorrectedMessage = DecryptMessage(messages, leastLikely: true);

            if (Verbose)
            {
                Logger.WriteLine($"The error-corrected message using the most likley letters is: {ErrorCorrectedMessage}.");
                Logger.WriteLine($"The error-corrected message using the least likely letters is: {ModifiedErrorCorrectedMessage}.");
            }

            return 0;
        }
    }
}
