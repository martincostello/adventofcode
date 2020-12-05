// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2017/day/9</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day09 : Puzzle
    {
        /// <summary>
        /// Gets the total score for all groups in the stream.
        /// </summary>
        public int TotalScore { get; private set; }

        /// <summary>
        /// Gets the number of garbage characters in the stream.
        /// </summary>
        public int GarbageCount { get; private set; }

        /// <summary>
        /// Computes the total score for all the groups in the specified stream.
        /// </summary>
        /// <param name="stream">The stream to get the score for.</param>
        /// <returns>
        /// The total score for the groups in the stream specified by <paramref name="stream"/>.
        /// </returns>
        public static (int score, int garbageCount) ParseStream(string stream)
        {
            int score = 0;
            int level = 0;
            int garbageCount = 0;
            bool withinGarbage = false;

            for (int i = 0; i < stream.Length; i++)
            {
                char c = stream[i];

                switch (c)
                {
                    case '{':
                        if (withinGarbage)
                        {
                            garbageCount++;
                        }
                        else
                        {
                            level++;
                            score += level;
                        }

                        break;

                    case '}':
                        if (withinGarbage)
                        {
                            garbageCount++;
                        }
                        else
                        {
                            level--;
                        }

                        break;

                    case '<':
                        if (withinGarbage)
                        {
                            garbageCount++;
                        }
                        else
                        {
                            withinGarbage = true;
                        }

                        break;

                    case '>':
                        withinGarbage = false;
                        break;

                    case '!':
                        i++;
                        break;

                    case ',':
                    default:
                        if (withinGarbage)
                        {
                            garbageCount++;
                        }

                        break;
                }
            }

            return (score, garbageCount);
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string stream = ReadResourceAsString().Trim();

            (int score, int garbageCount) = ParseStream(stream);

            TotalScore = score;
            GarbageCount = garbageCount;

            if (Verbose)
            {
                Logger.WriteLine($"The total score for all the groups is {TotalScore:N0}.");
                Logger.WriteLine($"There are {GarbageCount:N0} non-canceled characters within the garbage.");
            }

            return 0;
        }
    }
}
