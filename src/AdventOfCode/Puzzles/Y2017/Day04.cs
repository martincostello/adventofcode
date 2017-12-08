// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2017/day/4</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day04 : Puzzle2017
    {
        /// <summary>
        /// Gets the number of valid passwords in the input.
        /// </summary>
        public int ValidPasswordCount { get; private set; }

        /// <summary>
        /// Returns whether the specified passphrase is valid.
        /// </summary>
        /// <param name="passphrase">The passphrase to test for validity.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="passphrase"/> is valid; otherwise <see langword="false"/>.
        /// </returns>
        internal static bool IsPassphraseValid(string passphrase)
        {
            string[] words = passphrase.Split(Arrays.Space);

            return words.Distinct().Count() == words.Length;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            ICollection<string> passphrases = ReadResourceAsLines();

            ValidPasswordCount = passphrases.Where(IsPassphraseValid).Count();

            Console.WriteLine($"There are {ValidPasswordCount:N0} valid passphrases.");

            return 0;
        }
    }
}
