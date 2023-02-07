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

        Reduce(monkeys, cancellationToken);

        if (withEquality)
        {
            human.Value = null;
            root.Operation = '=';
            root.Value = 1;

            return Reverse(root, human, cancellationToken);
        }
        else
        {
            return root.Value!.Value;
        }

        static long Reverse(Monkey monkey, Monkey human, CancellationToken cancellationToken)
        {
            while (monkey != human && !cancellationToken.IsCancellationRequested)
            {
                bool assignLeft = monkey.Left!.IsVariable;

                (var variable, long constant) = assignLeft ?
                    (monkey.Left!, monkey.Right!.Value!.Value) :
                    (monkey.Right!, monkey.Left!.Value!.Value);

                variable.Value = (monkey.Operation, assignLeft) switch
                {
                    ('=', _) => constant,
                    ('+', _) => monkey.Value - constant,
                    ('*', _) => monkey.Value / constant,
                    ('-', false) => constant - monkey.Value,
                    ('-', true) => monkey.Value + constant,
                    ('/', false) => constant / monkey.Value,
                    ('/', true) => monkey.Value * constant,
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
                        LeftName = values[0],
                        RightName = values[2],
                        Operation = values[1][0],
                    };
                }

                monkeys[name] = monkey;
            }

            foreach (var monkey in monkeys.Values)
            {
                if (monkey.Left is null && monkey.LeftName is not null)
                {
                    monkey.Left = monkeys[monkey.LeftName];
                }

                if (monkey.Right is null && monkey.RightName is not null)
                {
                    monkey.Right = monkeys[monkey.RightName];
                }
            }

            return monkeys;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var values = await ReadResourceAsLinesAsync(cancellationToken);

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

        public string? LeftName { get; set; }

        public string? RightName { get; set; }

        public Monkey? Left { get; set; }

        public Monkey? Right { get; set; }

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

            if (Left?.Value is { } left &&
                Right?.Value is { } right)
            {
                Value = Operation switch
                {
                    '=' => left == right ? 1 : 0,
                    '+' => left + right,
                    '-' => left - right,
                    '*' => left * right,
                    '/' => left / right,
                    _ => throw new PuzzleException($"Unknown operation '{Operation}'."),
                };

                IsVariable = Left.IsVariable || Right.IsVariable;

                return true;
            }

            return false;
        }
    }
}
