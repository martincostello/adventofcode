// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2018/day/2</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2018, 02, RequiresData = true)]
    public sealed class Day02 : Puzzle
    {
        /// <summary>
        /// Gets the checksum of the box Ids.
        /// </summary>
        public int Checksum { get; private set; }

        /// <summary>
        /// Gets the common letters between the two similar box Ids.
        /// </summary>
        public string? CommonLettersForBoxes { get; private set; }

        /// <summary>
        /// Calculates the checksum for the specified box Ids.
        /// </summary>
        /// <param name="boxIds">The box Ids to calculate the checksum for.</param>
        /// <returns>
        /// The checksum calculated from <paramref name="boxIds"/>.
        /// </returns>
        public static int CalculateChecksum(IEnumerable<string> boxIds)
        {
            int count2 = 0;
            int count3 = 0;

            foreach (string id in boxIds)
            {
                (int c2, int c3) = GetBoxScore(id);

                count2 += c2;
                count3 += c3;
            }

            return count2 * count3;
        }

        /// <summary>
        /// Gets the scores for the box with the specified Id.
        /// </summary>
        /// <param name="id">The box Id to get the score for.</param>
        /// <returns>
        /// The scores of the specified Id.
        /// </returns>
        public static (int count2, int count3) GetBoxScore(string id)
        {
            var counts = id.Distinct().ToDictionary((k) => k, (v) => 0);

            foreach (char ch in id)
            {
                counts[ch]++;
            }

            bool hasDoubles = counts.Values.Any((p) => p == 2);
            bool hasTriples = counts.Values.Any((p) => p == 3);

            return (hasDoubles ? 1 : 0, hasTriples ? 1 : 0);
        }

        /// <summary>
        /// Gets the common letters between the two similar box Ids.
        /// </summary>
        /// <param name="boxIds">The box Ids to find the letters for the two similar box Ids.</param>
        /// <returns>
        /// A string containing the common letters calculated from <paramref name="boxIds"/>.
        /// </returns>
        public static string GetCommonLetters(IList<string> boxIds)
        {
            int index1 = 0;
            int index2 = 0;

            for (int i = 0; i < boxIds.Count; i++)
            {
                string id1 = boxIds[i];

                for (int j = 0; j < boxIds.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    string id2 = boxIds[j];

                    int differences = 0;

                    for (int k = 0; k < id1.Length && differences < 2; k++)
                    {
                        char ch1 = id1[k];
                        char ch2 = id2[k];

                        if (ch1 != ch2)
                        {
                            differences++;
                        }
                    }

                    if (differences == 1)
                    {
                        index1 = i;
                        index2 = j;
                        break;
                    }
                }
            }

            string first = boxIds[index1];
            string second = boxIds[index2];

            var common = new StringBuilder(first.Length);

            for (int i = 0; i < first.Length; i++)
            {
                char ch = first[i];

                if (ch == second[i])
                {
                    common.Append(ch);
                }
            }

            return common.ToString();
        }

        /// <inheritdoc />
        protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> ids = ReadResourceAsLines();

            Checksum = CalculateChecksum(ids);
            CommonLettersForBoxes = GetCommonLetters(ids);

            if (Verbose)
            {
                Logger.WriteLine($"The checksum is {Checksum:N0}.");
                Logger.WriteLine($"The common letters are {CommonLettersForBoxes}.");
            }

            return PuzzleResult.Create(Checksum, CommonLettersForBoxes);
        }
    }
}
