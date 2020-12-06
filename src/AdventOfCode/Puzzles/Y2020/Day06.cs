// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/6</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2020, 06, RequiresData = true)]
    public sealed class Day06 : Puzzle
    {
        /// <summary>
        /// Gets the sum of the questions answered by the provided groups.
        /// </summary>
        public int SumOfQuestions { get; private set; }

        /// <summary>
        /// Gets the sum of the questions answered as yes for the groups and responses
        /// specified by a collection of values.
        /// </summary>
        /// <param name="values">The questions answered yes sorted into groups.</param>
        /// <returns>
        /// The sum of the questions answered yes in each group.
        /// </returns>
        public static int GetSumOfQuestionsWithYesAnswers(ICollection<string> values)
        {
            var responses = new List<HashSet<char>>();
            var current = new HashSet<char>();

            foreach (string line in values)
            {
                if (string.IsNullOrEmpty(line))
                {
                    responses.Add(current);
                    current = new HashSet<char>();
                    continue;
                }

                foreach (char question in line)
                {
                    if (!current.Contains(question))
                    {
                        current.Add(question);
                    }
                }
            }

            if (current.Count > 0)
            {
                responses.Add(current);
            }

            return responses.Sum((p) => p.Count);
        }

        /// <inheritdoc />
        protected override object[] SolveCore(string[] args)
        {
            IList<string> values = ReadResourceAsLines();

            SumOfQuestions = GetSumOfQuestionsWithYesAnswers(values);

            if (Verbose)
            {
                Logger.WriteLine("The sum of the questions answered \"yes\" by the groups is {0}.", SumOfQuestions);
            }

            return new object[]
            {
                SumOfQuestions,
            };
        }
    }
}
