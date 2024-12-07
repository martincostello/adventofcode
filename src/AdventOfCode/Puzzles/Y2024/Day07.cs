// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/7</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 07, "Bridge Repair", RequiresData = true)]
public sealed class Day07 : Puzzle
{
    /// <summary>
    /// Gets the calibration result for the bridge.
    /// </summary>
    public long CalibrationResult { get; private set; }

    /// <summary>
    /// Calibrates the bridge using the specified equations.
    /// </summary>
    /// <param name="equations">The equations to use to calibrate the bridge.</param>
    /// <returns>
    /// The calibration result for the bridge.
    /// </returns>
    public static long Calibrate(IList<string> equations)
    {
        var parsed = new List<(long Target, Stack<int> Values)>(equations.Count);

        foreach (string equation in equations)
        {
            string[] parts = equation.Split(':');

            long target = Parse<long>(parts[0]);

            string[] values = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Array.Reverse(values);

            var stack = new Stack<int>(values.Length);

            foreach (string value in values)
            {
                stack.Push(Parse<int>(value));
            }

            parsed.Add((target, stack));
        }

        long result = 0;

        foreach ((long target, var values) in parsed)
        {
            if (TrySolve(values, 0, target))
            {
                result += target;
            }
        }

        return result;

        static bool TrySolve(Stack<int> values, long current, long target)
        {
            if (current > target)
            {
                return false;
            }

            if (values.Count == 0)
            {
                return current == target;
            }

            int value = values.Pop();

            if (TrySolve(values, current + value, target))
            {
                return true;
            }

            if (TrySolve(values, current * value, target))
            {
                return true;
            }

            values.Push(value);

            return false;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var equations = await ReadResourceAsLinesAsync(cancellationToken);

        CalibrationResult = Calibrate(equations);

        if (Verbose)
        {
            Logger.WriteLine("The total calibration result is {0}.", CalibrationResult);
        }

        return PuzzleResult.Create(CalibrationResult);
    }
}
