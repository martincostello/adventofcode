// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/10</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 10, "Factory", RequiresData = true, IsHidden = true, IsSlow = true)]
public sealed class Day10 : Puzzle<int, int>
{
    /// <summary>
    /// Gets the fewest button presses required to correctly configure
    /// the indicator lights and the required joltage on all of the machines.
    /// </summary>
    /// <param name="manual">The values to solve the puzzle from.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The sum of minimum number of presses required for all machines to both
    /// configure the indicator lights and configure the required joltage.
    /// </returns>
    public static (int Indicator, int Joltage) GetMinimumButtonPresses(IReadOnlyList<string> manual, CancellationToken cancellationToken)
    {
        var machines = ParseManual(manual);

        int indicatorSum = 0;

        foreach ((_, int desired, var buttons, _) in machines)
        {
            var machine = new MachineIndicator(buttons);
            indicatorSum += (int)PathFinding.AStar(machine, 0, desired, cancellationToken: cancellationToken);
        }

        int joltageSum = 0;

        foreach ((int count, _, var buttons, var joltage) in machines)
        {
            var machine = new JoltageIndicator(count, [.. buttons], [.. joltage]);

            var start = new JoltageState([.. Enumerable.Repeat(0, count)]);
            var goal = new JoltageState([.. joltage]);

            joltageSum += (int)PathFinding.AStar(machine, start, goal, cancellationToken: cancellationToken);
        }

        return (indicatorSum, joltageSum);

        static List<(int Count, int Indicator, List<int> Buttons, List<int> Joltage)> ParseManual(IReadOnlyList<string> manual)
        {
            var machines = new List<(int Count, int Indicator, List<int> Buttons, List<int> Joltage)>(manual.Count);

            foreach (string line in manual)
            {
                var remaining = line.AsSpan();
                int index = remaining.IndexOf('{');

                var joltage = ParseJoltage(remaining[(index + 1)..^1]);

                remaining = remaining[0..(index - 1)];

                index = remaining.IndexOf(']');

                var indicators = remaining[1..index];

                int count = indicators.Length;
                int indicator = ParseIndicator(indicators);

                var buttons = new List<int>();

                remaining = remaining[(index + 1)..];

                while (!remaining.IsEmpty)
                {
                    index = remaining.IndexOf('(');

                    if (index < 0)
                    {
                        break;
                    }

                    remaining = remaining[(index + 1)..];
                    index = remaining.IndexOf(')');

                    int buttonState = ParseMask(remaining[..index], ch => ch - '0');

                    buttons.Add(buttonState);
                    remaining = remaining[(index + 1)..];
                }

                machines.Add((count, indicator, buttons, joltage));
            }

            return machines;

            static int ParseIndicator(ReadOnlySpan<char> span)
            {
                int configuration = 0;

                for (int i = 0; i < span.Length; i++)
                {
                    configuration |= (span[i] == '#' ? 1 : 0) << i;
                }

                return configuration;
            }

            static List<int> ParseJoltage(ReadOnlySpan<char> span)
            {
                var joltage = new List<int>();

                foreach (var range in span.Split(','))
                {
                    joltage.Add(Parse<int>(span[range]));
                }

                return joltage;
            }

            static int ParseMask(ReadOnlySpan<char> span, Func<char, int> converter)
            {
                int mask = 0;

                foreach (var range in span.Split(','))
                {
                    int bit = converter(span[range.Start]);
                    mask |= 1 << bit;
                }

                return mask;
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (manual, logger, cancellationToken) =>
            {
                (int minimumIndicator, int minimumJoltage) = GetMinimumButtonPresses(manual, cancellationToken);

                if (logger is { })
                {
                    logger.WriteLine("The fewest button presses required to correctly configure the indicator lights on all of the machines is {0}.", minimumIndicator);
                    logger.WriteLine("The fewest button presses required to correctly configure the joltage level counters on all of the machines is {0}.", minimumJoltage);
                }

                return (minimumIndicator, minimumJoltage);
            },
            cancellationToken);
    }

    private readonly struct MachineIndicator(List<int> buttons) : IWeightedGraph<int>
    {
        private readonly List<int> _buttons = buttons;

        public readonly long Cost(int a, int b) => 1;

        public readonly bool Equals(int x, int y) => x == y;

        public readonly int GetHashCode([DisallowNull] int obj) => obj;

        public readonly IEnumerable<int> Neighbors(int id)
        {
            foreach (int button in _buttons)
            {
                yield return id ^ button;
            }
        }
    }

    private readonly struct JoltageIndicator(int length, ImmutableArray<int> buttons, ImmutableArray<int> desired) : IWeightedGraph<JoltageState>
    {
        public readonly long Cost(JoltageState a, JoltageState b) => 1;

        public readonly bool Equals(JoltageState? x, JoltageState? y) => x!.Counters.SequenceEqual(y!.Counters);

        public readonly int GetHashCode([DisallowNull] JoltageState obj)
        {
            HashCode builder = default;

            foreach (int counter in obj.Counters)
            {
                builder.Add(counter);
            }

            return builder.ToHashCode();
        }

        public readonly IEnumerable<JoltageState> Neighbors(JoltageState id)
        {
            foreach (int mask in buttons)
            {
                Span<int> next = [.. id.Counters];

                bool bust = false;

                for (int j = 0; j < length && !bust; j++)
                {
                    if ((mask & (1 << j)) != 0)
                    {
                        int value = next[j];

                        if (value >= desired[j])
                        {
                            bust = true;
                            break;
                        }

                        next[j] = value + 1;
                    }
                }

                if (!bust)
                {
                    yield return new JoltageState([.. next]);
                }
            }
        }
    }

    private sealed record JoltageState(ImmutableArray<int> Counters);
}
