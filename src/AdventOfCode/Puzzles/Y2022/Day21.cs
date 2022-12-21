// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

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

        var human = monkeys[Human];
        var root = monkeys[RootMonkey];

        human.IsVariable = true;

        if (withEquality)
        {
            root.Operation = '=';
            root.Value = 1;

            Reduce(monkeys, cancellationToken);

            return Reverse(root, human, cancellationToken);
        }
        else
        {
            Reduce(monkeys, cancellationToken);

            return root.Value!.Value;
        }

        static long Reverse(Monkey monkey, Monkey human, CancellationToken cancellationToken)
        {
            while (monkey != human && !cancellationToken.IsCancellationRequested)
            {
                (var variable, long constant) = monkey.Monkey1!.IsVariable ?
                    (monkey.Monkey1!, monkey.Monkey2!.Value!.Value) :
                    (monkey.Monkey2!, monkey.Monkey1!.Value!.Value);

                variable.Value = monkey.Operation switch
                {
                    '=' => constant,
                    '+' => monkey.Value - constant!,
                    '-' => monkey.Value + constant!,
                    '*' => monkey.Value / constant!,
                    '/' => monkey.Value * constant!,
                    _ => throw new PuzzleException($"Unknown operation '{monkey.Operation}'."),
                };

                monkey = variable;
            }

            cancellationToken.ThrowIfCancellationRequested();

            return monkey.Value!.Value!;
        }

        static void Reduce(Dictionary<string, Monkey> monkeys, CancellationToken cancellationToken)
        {
            while (!monkeys.Values.All((p) => p.Value is { }) && !cancellationToken.IsCancellationRequested)
            {
                foreach (var monkey in monkeys.Values)
                {
                    monkey.Reduce();
                }
            }

            cancellationToken.ThrowIfCancellationRequested();
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
                        MonkeyName1 = values[0],
                        MonkeyName2 = values[2],
                        Operation = values[1][0],
                    };
                }

                monkeys[name] = monkey;
            }

            foreach (var monkey in monkeys.Values)
            {
                if (monkey.Monkey1 is null && monkey.MonkeyName1 is not null)
                {
                    monkey.Monkey1 = monkeys[monkey.MonkeyName1];
                }

                if (monkey.Monkey2 is null && monkey.MonkeyName2 is not null)
                {
                    monkey.Monkey2 = monkeys[monkey.MonkeyName2];
                }
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

        public string? MonkeyName1 { get; set; }

        public string? MonkeyName2 { get; set; }

        public Monkey? Monkey1 { get; set; }

        public Monkey? Monkey2 { get; set; }

        public char? Operation { get; set; }

        public bool IsVariable { get; set; }

        public void Reset()
        {
            if (IsVariable)
            {
                Value = null;
            }
        }

        public bool Reduce()
        {
            if (Value is not null)
            {
                return true;
            }

            if (Monkey1?.Value is { } value1 &&
                Monkey2?.Value is { } value2)
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

                IsVariable = Monkey1.IsVariable || Monkey2.IsVariable;

                return true;
            }

            return false;
        }
    }
}
