// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

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
    /// to configure the indicator lights and to configure the required joltage.
    /// </returns>
    public static (int Indicator, int Joltage) GetMinimumButtonPresses(IReadOnlyList<string> manual, CancellationToken cancellationToken)
    {
        var machines = ParseManual(manual);

        int indicatorSum = 0;

        foreach ((_, int desired, var buttons, _) in machines)
        {
            int minimum = int.MaxValue;

            MinimumStepsToTurnOn(0, desired, [], CollectionsMarshal.AsSpan(buttons), ref minimum, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            indicatorSum += minimum;
        }

        int joltageSum = 0;

        foreach ((int count, _, var buttons, var joltage) in machines)
        {
            int minimum = int.MaxValue;

            int[] current = new int[count];

            MinimumStepsToPower(count, current, CollectionsMarshal.AsSpan(joltage), [], CollectionsMarshal.AsSpan(buttons), ref minimum, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            joltageSum += minimum;
        }

        return (indicatorSum, joltageSum);

        static void MinimumStepsToTurnOn(
            int current,
            int desired,
            Stack<int> path,
            ReadOnlySpan<int> buttons,
            ref int minimum,
            CancellationToken cancellationToken)
        {
            if (path.Count >= minimum - 1)
            {
                return;
            }

            for (int i = 0; i < buttons.Length && !cancellationToken.IsCancellationRequested; i++)
            {
                int next = current ^ buttons[i];

                if (next == desired)
                {
                    minimum = Math.Min(minimum, path.Count + 1);
                    continue;
                }

                if (!path.Contains(next))
                {
                    path.Push(next);

                    MinimumStepsToTurnOn(next, desired, path, buttons, ref minimum, cancellationToken);

                    path.Pop();
                }
            }
        }

        static void MinimumStepsToPower(
            int count,
            ReadOnlySpan<int> current,
            ReadOnlySpan<int> desired,
            Stack<int> path,
            ReadOnlySpan<int> buttons,
            ref int minimum,
            CancellationToken cancellationToken)
        {
            if (path.Count >= minimum - 1)
            {
                return;
            }

            for (int i = 0; i < buttons.Length && !cancellationToken.IsCancellationRequested; i++)
            {
                int mask = buttons[i];

                Span<int> next = [.. current];

                bool bust = false;

                for (int j = 0; j < count && !bust; j++)
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

                if (bust)
                {
                    continue;
                }

                if (next.SequenceEqual(desired))
                {
                    minimum = Math.Min(minimum, path.Count + 1);
                    continue;
                }

                int hash = Hash(next);

                if (!path.Contains(hash))
                {
                    path.Push(hash);

                    MinimumStepsToPower(count, next, desired, path, buttons, ref minimum, cancellationToken);

                    path.Pop();
                }
            }

            static int Hash(ReadOnlySpan<int> values)
            {
                HashCode builder = default;

                foreach (int value in values)
                {
                    builder.Add(value);
                }

                return builder.ToHashCode();
            }
        }

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
}
