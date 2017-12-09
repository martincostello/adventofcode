// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using System;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2017/day/9</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day09 : Puzzle2017
    {
        /// <summary>
        /// Gets the total score for all groups in the stream.
        /// </summary>
        public int TotalScore { get; private set; }

        /// <summary>
        /// Computes the total score for all the groups in the specified stream.
        /// </summary>
        /// <param name="stream">The stream to get the score for.</param>
        /// <returns>
        /// The total score for the groups in the stream specified by <paramref name="stream"/>.
        /// </returns>
        public static int ComputeTotalScore(string stream)
        {
            int score = 0;
            int level = 0;
            bool withinGarbage = false;

            for (int i = 0; i < stream.Length; i++)
            {
                char c = stream[i];

                switch (c)
                {
                    case '{':
                        if (!withinGarbage)
                        {
                            level++;
                            score += level;
                        }

                        break;

                    case '}':
                        if (!withinGarbage)
                        {
                            level--;
                        }

                        break;

                    case '<':
                        withinGarbage = true;
                        break;

                    case '>':
                        withinGarbage = false;
                        break;

                    case '!':
                        i++;
                        break;

                    case ',':
                        break;

                    default:
                        break;
                }
            }

            return score;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string stream = ReadResourceAsString().Trim();

            TotalScore = ComputeTotalScore(stream);

            Console.WriteLine($"The total score for all the groups is {TotalScore:N0}.");

            return 0;
        }
    }
}
