// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/11</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 11, "Monkey in the Middle", RequiresData = true)]
public sealed class Day11 : Puzzle
{
    /// <summary>
    /// Gets the level of monkey business after 20 rounds of stuff-slinging simian shenanigans.
    /// </summary>
    public long MonkeyBusiness { get; private set; }

    /// <summary>
    /// Returns the level of monkey business after a number of rounds of stuff-slinging simian shenanigans.
    /// </summary>
    /// <param name="observations">The observations for the monkey's behavior.</param>
    /// <param name="rounds">.The number of rounds to determine the level of monkey business for.</param>
    /// <returns>
    /// The level of monkey business after the specified number of rounds of stuff-slinging simian shenanigans.
    /// </returns>
    public static long GetMonkeyBusiness(IList<string> observations, int rounds)
    {
        var monkeys = Parse(observations);

        for (int round = 0; round < rounds; round++)
        {
            foreach (var monkey in monkeys)
            {
                monkey.Inspect();
            }
        }

        return monkeys
            .OrderByDescending((p) => p.Inspections)
            .Take(2)
            .Aggregate(1L, (x, y) => x * y.Inspections);

        static ICollection<Monkey> Parse(IList<string> observations)
        {
            const string MonkeyPrefix = "Monkey ";
            const string ItemsPrefix = "  Starting items: ";
            const string OperationPrefix = "  Operation: new = old ";
            const string TestPrefix = "  Test: divisible by ";
            const string TruePrefix = "    If true: throw to monkey ";
            const string FalsePrefix = "    If false: throw to monkey ";

            var monkeys = new Dictionary<int, Monkey>();

            for (int i = 0; i < observations.Count; i += 7)
            {
                string monkeyLine = observations[i][MonkeyPrefix.Length..].TrimEnd(':');
                int monkey = Parse<int>(monkeyLine);

                long[] items = observations[i + 1][ItemsPrefix.Length..]
                    .Split(", ")
                    .Select(Parse<long>)
                    .ToArray();

                monkeys[monkey] = new(monkey, items);
            }

            for (int i = 0; i < observations.Count; i += 7)
            {
                string operation = observations[i + 2][OperationPrefix.Length..];
                string test = observations[i + 3][TestPrefix.Length..];
                string monkeyForTrue = observations[i + 4][TruePrefix.Length..];
                string monkeyForFalse = observations[i + 5][FalsePrefix.Length..];

                var monkey = monkeys[i / 7];

                string operationString = operation[2..];

                if (operation.StartsWith('+'))
                {
                    if (operationString == "old")
                    {
                        monkey.Operation = (p) => p + p;
                    }
                    else
                    {
                        long operationValue = Parse<long>(operation[2..]);
                        monkey.Operation = (p) => p + operationValue;
                    }
                }
                else if (operation.StartsWith('*'))
                {
                    if (operationString == "old")
                    {
                        monkey.Operation = (p) => p * p;
                    }
                    else
                    {
                        long operationValue = Parse<long>(operation[2..]);
                        monkey.Operation = (p) => p * operationValue;
                    }
                }
                else
                {
                    throw new PuzzleException($"Invalid operation '{operation[0]}'.");
                }

                var recipientForTrue = monkeys[Parse<int>(monkeyForTrue)];
                var recipientForFalse = monkeys[Parse<int>(monkeyForFalse)];
                long testValue = Parse<long>(test);

                monkey.Next = (p) =>
                {
                    if (p % testValue == 0)
                    {
                        return recipientForTrue;
                    }
                    else
                    {
                        return recipientForFalse;
                    }
                };
            }

            return monkeys.Values;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var observations = await ReadResourceAsLinesAsync();

        MonkeyBusiness = GetMonkeyBusiness(observations, rounds: 20);

        if (Verbose)
        {
            Logger.WriteLine(
                "The level of monkey business after 20 rounds of stuff-slinging simian shenanigans is {0}.",
                MonkeyBusiness);
        }

        return PuzzleResult.Create(MonkeyBusiness);
    }

    [DebuggerDisplay("{Number}")]
    private sealed class Monkey
    {
        public Monkey(int number, IEnumerable<long> items)
        {
            Number = number;
            Items = new(items);
        }

        public long Inspections { get; private set; }

        public Queue<long> Items { get; }

        public Func<long, Monkey> Next { get; set; } = default!;

        public int Number { get; }

        public Func<long, long> Operation { get; set; } = default!;

        public void Inspect()
        {
            while (Items.Count > 0)
            {
                long item = Items.Dequeue();
                item = Operation(item) / 3;

                Inspections++;

                var recipient = Next(item);
                recipient.Throw(item);
            }
        }

        public void Throw(long item)
            => Items.Enqueue(item);
    }
}
