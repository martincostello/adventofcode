// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/21</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 21, "Monkey Math", RequiresData = true)]
public sealed class Day21 : Puzzle
{
    /// <summary>
    /// Gets the number that the monkey named <c>root</c> will yell.
    /// </summary>
    public long RootMonkeyNumber { get; private set; }

    /// <summary>
    /// Gets the number that the human should yell to get the
    /// monkey named <c>root</c> to receive two equal values.
    /// </summary>
    public long HumanNumber { get; private set; }

    /// <summary>
    /// Gets the number that the monkey named <c>root</c> will yell given the specified monkey jobs.
    /// </summary>
    /// <param name="jobs">The jobs of each monkey.</param>
    /// <param name="withEquality">Whether the root monkey should test for equality or not.</param>
    /// <param name="cancellationToken">The optional <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The number that the monkey named <c>root</c> will yell based on the specified jobs.
    /// </returns>
    public static long GetRootNumber(IList<string> jobs, bool withEquality, CancellationToken cancellationToken = default)
    {
        const string Human = "humn";
        const string RootMonkey = "root";

        var monkeys = Parse(jobs);

        monkeys[Human].DependsOnHuman = true;

        if (withEquality)
        {
            monkeys[RootMonkey].Operation = '=';

            for (long i = 0; !cancellationToken.IsCancellationRequested; i++)
            {
                monkeys[Human].Value = i;

                Cycle(monkeys);

                if (monkeys[RootMonkey].Value == 1)
                {
                    return i;
                }

                foreach (var monkey in monkeys.Values)
                {
                    monkey.Reset();
                }
            }

            cancellationToken.ThrowIfCancellationRequested();
            throw new UnreachableException();
        }
        else
        {
            Cycle(monkeys);
            return monkeys[RootMonkey].Value ?? 0;
        }

        static void Cycle(Dictionary<string, Monkey> monkeys)
        {
            while (!monkeys.Values.All((p) => p.Value is { }))
            {
                foreach (var monkey in monkeys.Values)
                {
                    monkey.TryReduce(monkeys);
                }
            }
        }

        static Dictionary<string, Monkey> Parse(IList<string> jobs)
        {
            var monkeys = new Dictionary<string, Monkey>(jobs.Count);

            foreach (string job in jobs)
            {
                string[] split = job.Split(':');

                string name = split[0];
                string[] values = split[1].TrimStart().Split(' ');

                Monkey monkey;

                if (values.Length == 1)
                {
                    monkey = new(name)
                    {
                        Value = Parse<long>(values[0]),
                    };
                }
                else
                {
                    monkey = new(name)
                    {
                        Monkey1 = values[0],
                        Monkey2 = values[2],
                        Operation = values[1][0],
                    };
                }

                monkeys[name] = monkey;
            }

            return monkeys;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var values = await ReadResourceAsLinesAsync();

        RootMonkeyNumber = GetRootNumber(values, withEquality: false, cancellationToken);
        HumanNumber = GetRootNumber(values, withEquality: true, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("The monkey named root will yell {0}.", RootMonkeyNumber);
            Logger.WriteLine("You must yell {0} for the monkey named root to pass their equality test.", HumanNumber);
        }

        return PuzzleResult.Create(RootMonkeyNumber, HumanNumber);
    }

    private sealed record Monkey(string Name)
    {
        public long? Value { get; set; }

        public string? Monkey1 { get; set; }

        public string? Monkey2 { get; set; }

        public char? Operation { get; set; }

        public bool DependsOnHuman { get; set; }

        public void Reset()
        {
            if (DependsOnHuman)
            {
                Value = null;
            }
        }

        public bool TryReduce(Dictionary<string, Monkey> monkeys)
        {
            if (!Value.HasValue &&
                monkeys.TryGetValue(Monkey1!, out var monkey1) &&
                monkeys.TryGetValue(Monkey2!, out var monkey2) &&
                monkey1.Value is { } value1 &&
                monkey2.Value is { } value2)
            {
                Value = Operation switch
                {
                    '=' => value1 == value2 ? 1 : 0,
                    '+' => value1 + value2,
                    '-' => value1 - value2,
                    '*' => value1 * value2,
                    '/' => value1 / value2,
                    _ => throw new PuzzleException($"Unknown operation '{Operation}'."),
                };

                DependsOnHuman = monkey1.DependsOnHuman || monkey2.DependsOnHuman;

                return true;
            }

            return false;
        }
    }
}
