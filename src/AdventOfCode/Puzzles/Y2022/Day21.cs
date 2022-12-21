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
    /// Gets the number that the monkey named <c>root</c> will yell given the specified monkey jobs.
    /// </summary>
    /// <param name="jobs">The jobs of each monkey.</param>
    /// <returns>
    /// The number that the monkey named <c>root</c> will yell based on the specified jobs.
    /// </returns>
    public static long GetRootNumber(IList<string> jobs)
    {
        var monkeys = Parse(jobs);

        while (!monkeys.Values.All((p) => p.Value is { }))
        {
            foreach (var monkey in monkeys.Values)
            {
                monkey.TryReduce(monkeys);
            }
        }

        return monkeys["root"]?.Value ?? 0;

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

        RootMonkeyNumber = GetRootNumber(values);

        if (Verbose)
        {
            Logger.WriteLine("The monkey named root will yell {0}.", RootMonkeyNumber);
        }

        return PuzzleResult.Create(RootMonkeyNumber);
    }

    private sealed record Monkey(string Name)
    {
        public long? Value { get; set; }

        public string? Monkey1 { get; set; }

        public string? Monkey2 { get; set; }

        public char? Operation { get; set; }

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
                    '+' => value1 + value2,
                    '-' => value1 - value2,
                    '*' => value1 * value2,
                    '/' => value1 / value2,
                    _ => throw new PuzzleException($"Unknown operation '{Operation}'."),
                };

                return true;
            }

            return false;
        }
    }
}
