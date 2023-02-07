// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2020/day/18</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2020, 18, "Operation Order", RequiresData = true)]
public sealed class Day18 : Puzzle
{
    /// <summary>
    /// Gets the sum of evaluating the expression(s) using V1 of the precedence rules.
    /// </summary>
    public long SumV1 { get; private set; }

    /// <summary>
    /// Gets the sum of evaluating the expression(s) using V2 of the precedence rules.
    /// </summary>
    public long SumV2 { get; private set; }

    /// <summary>
    /// Evaluates the result of the specified expression(s) and returns the sum.
    /// </summary>
    /// <param name="expressions">The expression(s) to evaluate.</param>
    /// <param name="version">The version of the precedence rules to use.</param>
    /// <returns>
    /// The sum of from evaluating the expression(s).
    /// </returns>
    public static long Evaluate(IEnumerable<string> expressions, int version)
    {
        return expressions.Sum((p) => Evaluate(p, version));

        static long Evaluate(ReadOnlySpan<char> expression, int version)
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

                    var subexpression = expression.Slice(i + 1, end - i - 1);

                    tokens.Add(Evaluate(subexpression, version).ToString(CultureInfo.InvariantCulture));
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

            long result = Parse<long>(tokens[0]);

            if (version == 1)
            {
                for (int i = 1; i < tokens.Count; i += 2)
                {
                    string token = tokens[i];

                    if (string.Equals("+", token, StringComparison.Ordinal))
                    {
                        result += Parse<long>(tokens[i + 1]);
                    }
                    else if (string.Equals("*", token, StringComparison.Ordinal))
                    {
                        result *= Parse<long>(tokens[i + 1]);
                    }
                }
            }
            else
            {
                var products = new List<long>();

                for (int i = 1; i < tokens.Count; i += 2)
                {
                    string token = tokens[i];

                    if (string.Equals("+", token, StringComparison.Ordinal))
                    {
                        result += Parse<long>(tokens[i + 1]);
                    }
                    else if (string.Equals("*", token, StringComparison.Ordinal))
                    {
                        products.Add(result);
                        result = Parse<long>(tokens[i + 1]);
                    }
                }

                foreach (long i in products)
                {
                    result *= i;
                }
            }

            return result;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> expressions = await ReadResourceAsLinesAsync(cancellationToken);

        SumV1 = Evaluate(expressions, version: 1);
        SumV2 = Evaluate(expressions, version: 2);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the evaluated expressions with the first precedence rules is {0}.", SumV1);
            Logger.WriteLine("The sum of the evaluated expressions with the second precedence rules is {0}.", SumV2);
        }

        return PuzzleResult.Create(SumV1, SumV2);
    }
}
