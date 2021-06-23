// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2015/day/16</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2015, 16, RequiresData = true)]
    public sealed class Day16 : Puzzle
    {
        /// <summary>
        /// The result of the forensic analysis of the gift from Aunt Sue X
        /// as provided by the My First Crime Scene Analysis Machine (MFCSAM).
        /// </summary>
        private static readonly IDictionary<string, Tuple<int, int>> ForensicAnalysis = new Dictionary<string, Tuple<int, int>>()
        {
            ["children"] = Tuple.Create(3, 0),
            ["cats"] = Tuple.Create(7, 1),
            ["samoyeds"] = Tuple.Create(2, 0),
            ["pomeranians"] = Tuple.Create(3, -1),
            ["akitas"] = Tuple.Create(0, 0),
            ["vizslas"] = Tuple.Create(0, 0),
            ["goldfish"] = Tuple.Create(5, -1),
            ["trees"] = Tuple.Create(3, 1),
            ["cars"] = Tuple.Create(2, 0),
            ["perfumes"] = Tuple.Create(1, 0),
        };

        /// <summary>
        /// Gets the number of the Aunt Sue that sent the gift.
        /// </summary>
        internal int AuntSueNumber { get; private set; }

        /// <summary>
        /// Gets the number of the real Aunt Sue that sent the gift.
        /// </summary>
        internal int RealAuntSueNumber { get; private set; }

        /// <summary>
        /// Returns the number of the Aunt Sue that sent the gift from the specified Aunt Sue metadata.
        /// </summary>
        /// <param name="metadata">The metadata about all the Aunt Sues.</param>
        /// <param name="compensateForRetroEncabulator">Whether to compensate for the Retro Encabulator.</param>
        /// <returns>
        /// The number of the Aunt Sue which sent the gift based on forensic analysis of <paramref name="metadata"/>.
        /// </returns>
        internal static int WhichAuntSueSentTheGift(ICollection<string> metadata, bool compensateForRetroEncabulator = false)
        {
            var parsed = metadata.Select(AuntSue.Parse);

            foreach (var item in ForensicAnalysis)
            {
                if (compensateForRetroEncabulator && item.Value.Item2 != 0)
                {
                    if (item.Value.Item2 == 1)
                    {
                        parsed = parsed.Where((p) => !p.Metadata.ContainsKey(item.Key) || p.Metadata[item.Key] > item.Value.Item1);
                    }
                    else
                    {
                        parsed = parsed.Where((p) => !p.Metadata.ContainsKey(item.Key) || p.Metadata[item.Key] < item.Value.Item1);
                    }
                }
                else
                {
                    parsed = parsed.Where((p) => !p.Metadata.ContainsKey(item.Key) || p.Metadata[item.Key] == item.Value.Item1);
                }
            }

            return parsed.Single().Number;
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> auntSueMetadata = await ReadResourceAsLinesAsync();

            AuntSueNumber = WhichAuntSueSentTheGift(auntSueMetadata);
            RealAuntSueNumber = WhichAuntSueSentTheGift(auntSueMetadata, compensateForRetroEncabulator: true);

            if (Verbose)
            {
                Logger.WriteLine(
                    "The number of the Aunt Sue that got me the gift was originally thought to be {0}, but it was actually {1}.",
                    AuntSueNumber,
                    RealAuntSueNumber);
            }

            return PuzzleResult.Create(AuntSueNumber, RealAuntSueNumber);
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
            internal IDictionary<string, int> Metadata { get; private set; } = new Dictionary<string, int>();

            /// <summary>
            /// Parses an instance of <see cref="AuntSue"/> from the specified <see cref="string"/>.
            /// </summary>
            /// <param name="value">The value to parse.</param>
            /// <returns>
            /// An instance of <see cref="AuntSue"/> that represents <paramref name="value"/>.
            /// </returns>
            internal static AuntSue Parse(string value)
            {
                var result = new AuntSue();

                string[] split = value.Split(' ');

                result.Number = ParseInt32(split[1].TrimEnd(':'));

                split = string.Join(" ", split, 2, split.Length - 2).Split(',');

                foreach (string item in split)
                {
                    string[] itemSplit = item.Split(':');
                    result.Metadata[itemSplit[0].Trim()] = ParseInt32(itemSplit[1].Trim());
                }

                return result;
            }
        }
    }
}
