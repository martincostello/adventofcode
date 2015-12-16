// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

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

            string[] auntSueMetadata = File.ReadAllLines(args[0]);

            AuntSueNumber = WhichAuntSueSentTheGift(auntSueMetadata);

            Console.WriteLine("The number of the Aunt Sue that got me the gift is {0}.", AuntSueNumber);

            return 0;
        }

        /// <summary>
        /// Returns the number of the Aunt Sue that sent the gift from the specified Aunt Sue metadata.
        /// </summary>
        /// <param name="metadata">The metadata about all the Aunt Sues.</param>
        /// <returns>
        /// The number of the Aunt Sue which sent the gift based on forensic analysis of <paramref name="metadata"/>.
        /// </returns>
        internal static int WhichAuntSueSentTheGift(ICollection<string> metadata)
        {
            var parsed = metadata.Select(AuntSue.Parse);

            foreach (var item in ForensicAnalysis)
            {
                parsed = parsed.Where((p) => !p.Metadata.ContainsKey(item.Key) || p.Metadata[item.Key] == item.Value);
            }

            return parsed.Single().Number;
        }

        /// <summary>
        /// A class representing an aunt called Sue. This class cannot be inherited.
        /// </summary>
        private sealed class AuntSue
        {
            /// <summary>
            /// Gets the number of the Aunt Sue.
            /// </summary>
            internal int Number { get; private set; }

            /// <summary>
            /// Gets the metadata about this Aunt Sue.
            /// </summary>
            internal IDictionary<string, int> Metadata { get; private set; }

            /// <summary>
            /// Parses an instance of <see cref="AuntSue"/> from the specified <see cref="string"/>.
            /// </summary>
            /// <param name="value">The value to parse.</param>
            /// <returns>
            /// An instance of <see cref="AuntSue"/> that represents <paramref name="value"/>.
            /// </returns>
            internal static AuntSue Parse(string value)
            {
                AuntSue result = new AuntSue()
                {
                    Metadata = new Dictionary<string, int>(),
                };

                string[] split = value.Split(' ');

                result.Number = int.Parse(split[1].TrimEnd(':'), CultureInfo.InvariantCulture);

                split = string.Join(" ", split, 2, split.Length - 2).Split(',');

                foreach (string item in split)
                {
                    string[] itemSplit = item.Split(':');
                    result.Metadata[itemSplit[0].Trim()] = int.Parse(itemSplit[1].Trim(), CultureInfo.InvariantCulture);
                }

                return result;
            }
        }
    }
}
