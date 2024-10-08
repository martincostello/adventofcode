﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/19</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 19, "Aplenty", RequiresData = true, Unsolved = true)]
public sealed class Day19 : Puzzle
{
    /*
    private const int MinimumRating = 1;
    private const int MaximumRating = 4000;
    private static readonly char[] Categories = ['x', 'm', 'a', 's'];
    private static readonly Range All = new(MinimumRating, MaximumRating + 1);
    */

    private delegate (string? Next, bool? Result) Analyzer(Part part);

    /// <summary>
    /// Gets the sum of the rating numbers of all the parts that are accepted.
    /// </summary>
    public int RatingNumbersSum { get; private set; }

    /// <summary>
    /// Gets how many distinct combinations of ratings will be accepted.
    /// </summary>
    public long CombinationsAccepted { get; private set; }

    /// <summary>
    /// Runs the workflows for the specified parts.
    /// </summary>
    /// <param name="values">The workflows and parts to run.</param>
    /// <returns>
    /// The sum of the rating numbers of all the parts that are accepted
    /// and how many distinct combinations of ratings will be accepted.
    /// </returns>
    public static (int Sum, long Combinations) Run(IList<string> values)
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

        int sum = accepted.Sum((p) => p.RatingNumber);

        // TODO Implement
        long combinations = long.MaxValue;

        return (sum, combinations);

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
            var alternate = workflows.GetAlternateLookup();

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

                    int delimiter = rule.IndexOf(':');

                    if (delimiter is -1)
                    {
                        switch (rule[0])
                        {
                            case 'A':
                                analyzers.Add(accept);
                                break;

                            case 'R':
                                analyzers.Add(reject);
                                break;

                            default:
                                string next = rule.ToString();
                                analyzers.Add(new((_) => (next, null)));
                                break;
                        }
                    }
                    else
                    {
                        char category = rule[0];
                        char operation = rule[1];
                        int operand = Parse<int>(rule[2..delimiter]);
                        string next = rule[(delimiter + 1)..].ToString();

                        int sign = operation switch
                        {
                            '<' => -1,
                            '>' => 1,
                            _ => throw new PuzzleException($"Unknown operator '{operation}'."),
                        };

                        (string? Next, bool? Result) result = next switch
                        {
                            "A" => (null, true),
                            "R" => (null, false),
                            _ => (next, null),
                        };

                        analyzers.Add((p) =>
                        {
                            int value = p[category];
                            int comparison = comparer.Compare(value, operand);
                            bool isMatch = Math.Sign(comparison) == sign;

                            return isMatch ? result : (null, null);
                        });
                    }

                    if (index is -1)
                    {
                        break;
                    }

                    rules = rules[(index + 1)..];
                }

                alternate[name] = new(analyzers, workflows);
            }

            return workflows;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        (RatingNumbersSum, CombinationsAccepted) = Run(values);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the rating numbers of all the accepted parts is {0}.", RatingNumbersSum);
            Logger.WriteLine("{0} distinct combinations of ratings will be accepted.", CombinationsAccepted);
        }

        return PuzzleResult.Create(RatingNumbersSum, CombinationsAccepted);
    }

    private sealed record Part(int X, int M, int A, int S)
    {
        public int RatingNumber { get; } = X + M + A + S;

        public int this[char category]
        {
            get => category switch
            {
                'x' => X,
                'm' => M,
                'a' => A,
                's' => S,
                _ => throw new ArgumentOutOfRangeException(nameof(category), category, $"Unknown category '{category}'."),
            };
        }
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
