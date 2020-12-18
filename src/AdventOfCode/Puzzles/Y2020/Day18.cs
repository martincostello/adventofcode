// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/18</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2020, 18, RequiresData = true)]
    public sealed class Day18 : Puzzle
    {
        /// <summary>
        /// Gets the sum of evaluating the expression(s).
        /// </summary>
        public long Sum { get; private set; }

        /// <summary>
        /// Evaluates the result of the specified expression(s) and returns the sum.
        /// </summary>
        /// <param name="expressions">The expression(s) to evaluate.</param>
        /// <returns>
        /// The sum of from evaluating the expression(s).
        /// </returns>
        public static long Evaluate(IEnumerable<string> expressions)
        {
            return expressions.Sum(Evaluate);

            static long Evaluate(string expression)
            {
                var tokens = new List<string>();
                var current = new StringBuilder();

                for (int i = 0; i < expression.Length; i++)
                {
                    char ch = expression[i];

                    if (char.IsDigit(ch))
                    {
                        current.Append(ch);
                    }
                    else if (ch == ' ')
                    {
                        if (current.Length > 0)
                        {
                            tokens.Add(current.ToString());
                            current.Clear();
                        }
                    }
                    else if (ch == '(')
                    {
                        int openCount = 0;
                        int closedCount = 0;

                        int end = -1;

                        for (int j = i; j < expression.Length; j++)
                        {
                            char thisChar = expression[j];

                            if (thisChar == '(')
                            {
                                openCount++;
                            }
                            else if (thisChar == ')')
                            {
                                closedCount++;
                            }

                            if (openCount == closedCount)
                            {
                                end = j;
                                break;
                            }
                        }

                        string subexpression = expression.Substring(i + 1, end - i - 1);

                        tokens.Add(Evaluate(subexpression).ToString(CultureInfo.InvariantCulture));
                        i += end - i;
                    }
                    else
                    {
                        tokens.Add(ch.ToString());
                    }
                }

                if (current.Length > 0)
                {
                    tokens.Add(current.ToString());
                    current.Clear();
                }

                long result = ParseInt64(tokens[0]);

                for (int i = 1; i < tokens.Count; i += 2)
                {
                    string token = tokens[i];

                    if (string.Equals("+", token, StringComparison.Ordinal))
                    {
                        result += ParseInt64(tokens[i + 1]);
                    }
                    else if (string.Equals("*", token, StringComparison.Ordinal))
                    {
                        result *= ParseInt64(tokens[i + 1]);
                    }
                }

                return result;
            }
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> expressions = await ReadResourceAsLinesAsync();

            Sum = Evaluate(expressions);

            if (Verbose)
            {
                Logger.WriteLine("The sum of the evaluated expressions is {0}.", Sum);
            }

            return PuzzleResult.Create(Sum);
        }
    }
}
