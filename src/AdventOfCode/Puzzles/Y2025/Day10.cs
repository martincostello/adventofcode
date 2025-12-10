// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/10</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 10, "Factory", RequiresData = true, IsHidden = true, IsSlow = true)]
public sealed class Day10 : Puzzle<int>
{
    /// <summary>
    /// Gets the fewest button presses required to correctly configure
    /// the indicator lights on all of the machines.
    /// </summary>
    /// <param name="manual">The values to solve the puzzle from.</param>
    /// <returns>
    /// The solution.
    /// </returns>
    public static int GetMinimumButtonPresses(IReadOnlyList<string> manual)
    {
        var machines = new List<(int Desired, List<int> Buttons)>(manual.Count);

        foreach (string line in manual)
        {
            var remaining = line.AsSpan();
            int index = remaining.IndexOf('{');

            remaining = remaining[0..(index - 1)];

            index = remaining.IndexOf(']');

            var desired = remaining[1..index];

            int state = 0;

            for (int i = 0; i < desired.Length; i++)
            {
                state |= (desired[i] == '#' ? 1 : 0) << i;
            }

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

                int buttonState = 0;

                foreach (var range in remaining[..index].Split(','))
                {
                    int button = remaining[range.Start] - '0';
                    buttonState |= 1 << button;
                }

                buttons.Add(buttonState);
                remaining = remaining[(index + 1)..];
            }

            machines.Add((state, buttons));
        }

        int sum = 0;

        foreach ((int desired, var buttons) in machines)
        {
            int best = int.MaxValue;

            var path = new Stack<int>();
            var ways = new List<List<int>>();

            MinimumStepsToTurnOn(0, desired, path, CollectionsMarshal.AsSpan(buttons), ways, ref best);

            sum += ways.Min((p) => p.Count);
        }

        return sum;

        static void MinimumStepsToTurnOn(
            int current,
            int desired,
            Stack<int> path,
            ReadOnlySpan<int> buttons,
            List<List<int>> ways,
            ref int best)
        {
            if (current == desired)
            {
                ways.Add([.. path]);
                best = Math.Min(best, path.Count);
                return;
            }

            if (best != int.MaxValue && path.Count >= best)
            {
                return;
            }

            for (int i = 0; i < buttons.Length; i++)
            {
                int candidate = current ^ buttons[i];

                if (path.Contains(candidate))
                {
                    continue;
                }

                path.Push(candidate);

                MinimumStepsToTurnOn(candidate, desired, path, buttons, ways, ref best);

                path.Pop();
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (manual, logger, _) =>
            {
                int minimum = GetMinimumButtonPresses(manual);

                if (logger is { })
                {
                    logger.WriteLine("The fewest button presses required to correctly configure the indicator lights on all of the machines is {0}.", minimum);
                }

                return minimum;
            },
            cancellationToken);
    }
}
