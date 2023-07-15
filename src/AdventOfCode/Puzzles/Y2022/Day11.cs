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
    /// Gets the level of monkey business after 20 rounds of
    /// stuff-slinging simian shenanigans with baseline anxiety.
    /// </summary>
    public long MonkeyBusiness20 { get; private set; }

    /// <summary>
    /// Gets the level of monkey business after 10,000 rounds of
    /// stuff-slinging simian shenanigans wth higher anxiety.
    /// </summary>
    public long MonkeyBusiness10000 { get; private set; }

    /// <summary>
    /// Returns the level of monkey business after a number of rounds of stuff-slinging simian shenanigans.
    /// </summary>
    /// <param name="observations">The observations for the monkey's behavior.</param>
    /// <param name="rounds">.The number of rounds to determine the level of monkey business for.</param>
    /// <param name="highAnxiety">Whether your anxiety is high.</param>
    /// <returns>
    /// The level of monkey business after the specified number of rounds of stuff-slinging simian shenanigans.
    /// </returns>
    public static long GetMonkeyBusiness(IList<string> observations, int rounds, bool highAnxiety)
    {
        var monkeys = Parse(observations, highAnxiety);

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

        static ICollection<Monkey> Parse(IList<string> observations, bool highAnxiety)
        {
            const string MonkeyPrefix = "Monkey ";
            const string ItemsPrefix = "  Starting items: ";
            const string OperationPrefix = "  Operation: new = old ";
            const string TestPrefix = "  Test: divisible by ";
            const string TruePrefix = "    If true: throw to monkey ";
            const string FalsePrefix = "    If false: throw to monkey ";

            var monkeys = new Dictionary<int, (Monkey Monkey, long Divisor)>();
            long commonDivisor = 1;

            for (int i = 0; i < observations.Count; i += 7)
            {
                string monkeyLine = observations[i][MonkeyPrefix.Length..^1];
                string testValue = observations[i + 3][TestPrefix.Length..];

                int monkey = Parse<int>(monkeyLine);

                long[] items = observations[i + 1][ItemsPrefix.Length..]
                    .Split(", ")
                    .Select(Parse<long>)
                    .ToArray();

                long divisor = Parse<long>(testValue);
                commonDivisor *= divisor;

                monkeys[monkey] = (new(monkey, items), divisor);
            }

            Func<long, long> reducer = highAnxiety ? (p) => p % commonDivisor : static (p) => p / 3;

            for (int i = 0; i < observations.Count; i += 7)
            {
                string operation = observations[i + 2][OperationPrefix.Length..];
                string monkeyForTrue = observations[i + 4][TruePrefix.Length..];
                string monkeyForFalse = observations[i + 5][FalsePrefix.Length..];

                (var monkey, long divisor) = monkeys[i / 7];

                monkey.Reducer = reducer;

                string operationString = operation[2..];

                if (operation.StartsWith('+'))
                {
                    if (operationString == "old")
                    {
                        monkey.Inspector = static (p) => p + p;
                    }
                    else
                    {
                        long operationValue = Parse<long>(operation[2..]);
                        monkey.Inspector = (p) => p + operationValue;
                    }
                }
                else if (operation.StartsWith('*'))
                {
                    if (operationString == "old")
                    {
                        monkey.Inspector = (p) => p * p;
                    }
                    else
                    {
                        long operationValue = Parse<long>(operation[2..]);
                        monkey.Inspector = (p) => p * operationValue;
                    }
                }
                else
                {
                    throw new PuzzleException($"Invalid operation '{operation[0]}'.");
                }

                var recipientForTrue = monkeys[Parse<int>(monkeyForTrue)].Monkey;
                var recipientForFalse = monkeys[Parse<int>(monkeyForFalse)].Monkey;

                monkey.Next = (p) =>
                {
                    if (p % divisor == 0)
                    {
                        return recipientForTrue;
                    }
                    else
                    {
                        return recipientForFalse;
                    }
                };
            }

            return monkeys.Values
                .Select((p) => p.Monkey)
                .ToArray();
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var observations = await ReadResourceAsLinesAsync(cancellationToken);

        MonkeyBusiness20 = GetMonkeyBusiness(observations, rounds: 20, highAnxiety: false);
        MonkeyBusiness10000 = GetMonkeyBusiness(observations, rounds: 10_000, highAnxiety: true);

        if (Verbose)
        {
            Logger.WriteLine(
                "The level of monkey business after 20 rounds of stuff-slinging simian shenanigans is {0}.",
                MonkeyBusiness20);

            Logger.WriteLine(
                "The level of monkey business after 10,000 rounds of stuff-slinging simian shenanigans is {0}.",
                MonkeyBusiness10000);
        }

        return PuzzleResult.Create(MonkeyBusiness20, MonkeyBusiness10000);
    }

    [DebuggerDisplay("{Number}")]
    private sealed class Monkey
    {
        public Monkey(int number, IEnumerable<long> items)
        {
            Number = number;
            Items = new(items);
        }

        public int Inspections { get; private set; }

        public Queue<long> Items { get; }

        public Func<long, Monkey> Next { get; set; } = default!;

        public int Number { get; }

        public Func<long, long> Inspector { get; set; } = default!;

        public Func<long, long> Reducer { get; set; } = default!;

        public void Inspect()
        {
            while (Items.Count > 0)
            {
                long item = Items.Dequeue();

                item = Inspector(item);
                item = Reducer(item);

                Inspections++;

                var recipient = Next(item);
                recipient.Throw(item);
            }
        }

        public void Throw(long item)
            => Items.Enqueue(item);
    }
}
