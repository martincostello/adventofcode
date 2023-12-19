// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/19</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 19, "Aplenty", RequiresData = true)]
public sealed class Day19 : Puzzle
{
    private delegate (string? Next, bool? Result) Analyzer(Part part);

    /// <summary>
    /// Gets the sum of the rating numbers of all the parts that are accepted.
    /// </summary>
    public int RatingNumbersSum { get; private set; }

    /// <summary>
    /// Runs the workflows for the specified parts.
    /// </summary>
    /// <param name="values">The workflows and parts to run.</param>
    /// <returns>
    /// The sum of the rating numbers of all the parts that are accepted.
    /// </returns>
    public static int Run(IList<string> values)
    {
        var rawWorkflows = new List<string>();
        var rawParts = new List<string>();
        var current = rawWorkflows;

        foreach (string value in values)
        {
            if (string.IsNullOrEmpty(value))
            {
                current = rawParts;
                continue;
            }

            current.Add(value);
        }

        var parts = ParseParts(rawParts);
        var workflows = ParseWorkflows(rawWorkflows);

        var accepted = new List<Part>();
        var workflow = workflows["in"];

        foreach (var part in parts)
        {
            if (workflow.Analyze(part))
            {
                accepted.Add(part);
            }
        }

        return accepted.Sum((p) => p.RatingNumber);

        static List<Part> ParseParts(List<string> values)
        {
            var parts = new List<Part>(values.Count);

            foreach (string part in values)
            {
                string[] categories = part[1..^1].Split(',');

                int x = Parse<int>(categories[0][2..]);
                int m = Parse<int>(categories[1][2..]);
                int a = Parse<int>(categories[2][2..]);
                int s = Parse<int>(categories[3][2..]);

                parts.Add(new(x, m, a, s));
            }

            return parts;
        }

        static Dictionary<string, Workflow> ParseWorkflows(List<string> values)
        {
            var workflows = new Dictionary<string, Workflow>(values.Count);

            var comparer = Comparer<int>.Default;
            var accept = new Analyzer(static (p) => (null, true));
            var reject = new Analyzer(static (p) => (null, false));

            foreach (string value in values)
            {
                var span = value.AsSpan();
                int index = span.IndexOf('{');

                var name = span[..index];
                var rules = span[(index + 1)..^1];

                var analyzers = new List<Analyzer>();

                while (rules.Length > 0)
                {
                    index = rules.IndexOf(',');

                    var rule = rules;

                    if (index is not -1)
                    {
                        rule = rules[..index];
                    }

                    int pointer = rule.IndexOf(':');

                    if (pointer is -1)
                    {
                        string other = new(rule);

                        analyzers.Add(rule[0] switch
                        {
                            'A' => accept,
                            'R' => reject,
                            _ => new((_) => (other, null)),
                        });
                    }
                    else
                    {
                        char category = rule[0];
                        char op = rule[1];
                        int operand = Parse<int>(rule[2..pointer]);
                        string other = new(rule[(pointer + 1)..]);

                        int sign = op switch
                        {
                            '<' => -1,
                            '>' => 1,
                            _ => throw new PuzzleException($"Unknown operator '{op}'."),
                        };

                        Func<Part, int> categoryProvider = category switch
                        {
                            'x' => (p) => p.X,
                            'm' => (p) => p.M,
                            'a' => (p) => p.A,
                            's' => (p) => p.S,
                            _ => throw new PuzzleException($"Unknown category '{category}'."),
                        };

                        (string? Next, bool? Result) result = other switch
                        {
                            "A" => (null, true),
                            "R" => (null, false),
                            _ => (other, null),
                        };

                        analyzers.Add((p) =>
                        {
                            int value = categoryProvider(p);
                            int comparison = comparer.Compare(value, operand);
                            bool matches = Math.Sign(comparison) == sign;

                            if (matches)
                            {
                                return result;
                            }

                            return (null, null);
                        });
                    }

                    if (index is -1)
                    {
                        break;
                    }

                    rules = rules[(index + 1)..];
                }

                workflows[new(name)] = new(analyzers, workflows);
            }

            return workflows;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        RatingNumbersSum = Run(values);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the rating numbers of all the accepted parts is {0}.", RatingNumbersSum);
        }

        return PuzzleResult.Create(RatingNumbersSum);
    }

    private sealed record Part(int X, int M, int A, int S)
    {
        public int RatingNumber { get; } = X + M + A + S;
    }

    private sealed class Workflow(List<Analyzer> analyzers, Dictionary<string, Workflow> workflows)
    {
        public bool Analyze(Part part)
        {
            foreach (var analyzer in analyzers)
            {
                var (next, result) = analyzer(part);

                if (next is { } name)
                {
                    return workflows[name].Analyze(part);
                }
                else if (result is { } decision)
                {
                    return decision;
                }
            }

            return false;
        }
    }
}
