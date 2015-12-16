// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/day/16</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day16 : IPuzzle
    {
        /// <summary>
        /// The result of the forensic analysis of the gift from Aunt Sue X
        /// as provided by the My First Crime Scene Analysis Machine (MFCSAM).
        /// </summary>
        private static readonly IDictionary<string, int> ForensicAnalysis = new Dictionary<string, int>()
        {
            { "children", 3 },
            { "cats", 7 },
            { "samoyeds", 2 },
            { "pomeranians", 3 },
            { "akitas", 0 },
            { "vizslas", 0 },
            { "goldfish", 5 },
            { "trees", 3 },
            { "cars", 2 },
            { "perfumes", 1 },
        };

        /// <summary>
        /// Gets the number of the Aunt Sue that sent the gift.
        /// </summary>
        internal int AuntSueNumber { get; private set; }

        /// <inheritdoc />
        public int Solve(string[] args)
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

            string[] clues = File.ReadAllLines(args[0]);

            AuntSueNumber = WhichAuntSueSentTheGift(clues);

            return 0;
        }

        /// <summary>
        /// Returns the number of the Aunt Sue that sent the gift from the specified clues.
        /// </summary>
        /// <param name="clues">The clues.</param>
        /// <returns>
        /// The number of the Aunt Sue which sent the gift based on forensic analysis of <paramref name="clues"/>.
        /// </returns>
        internal static int WhichAuntSueSentTheGift(ICollection<string> clues)
        {
            foreach (var item in ForensicAnalysis)
            {
                // TODO Analyze the results
                if (clues != null && item.Value > 0)
                {
                }
            }

            // TODO Implement
            return 0;
        }
    }
}
