// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/10</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 10, "Factory", RequiresData = true, IsHidden = true)]
public sealed class Day10 : Puzzle<int>
{
    /// <summary>
    /// Gets the fewest button presses required to correctly configure
    /// the indicator lights on all of the machines.
    /// </summary>
    /// <param name="manual">The values to solve the puzzle from.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The solution.
    /// </returns>
    public static int GetMinimumButtonPresses(IReadOnlyList<string> manual, CancellationToken cancellationToken)
    {
        var machines = ParseManual(manual);

        int sum = 0;

        foreach ((int desired, var buttons, _) in machines)
        {
            int minimum = int.MaxValue;

            MinimumStepsToTurnOn(0, desired, [], CollectionsMarshal.AsSpan(buttons), ref minimum, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            sum += minimum;
        }

        return sum;

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

        static List<(int Indicator, List<int> Buttons, List<int> Joltage)> ParseManual(IReadOnlyList<string> manual)
        {
            var machines = new List<(int Indicator, List<int> Buttons, List<int> Joltage)>(manual.Count);

            foreach (string line in manual)
            {
                var remaining = line.AsSpan();
                int index = remaining.IndexOf('{');

                var joltage = ParseJoltage(remaining[(index + 1)..^1]);

                remaining = remaining[0..(index - 1)];

                index = remaining.IndexOf(']');

                int indicator = ParseIndicator(remaining[1..index]);

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

                machines.Add((indicator, buttons, joltage));
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
                int minimum = GetMinimumButtonPresses(manual, cancellationToken);

                if (logger is { })
                {
                    logger.WriteLine("The fewest button presses required to correctly configure the indicator lights on all of the machines is {0}.", minimum);
                }

                return minimum;
            },
            cancellationToken);
    }
}
