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
    /// Gets the first calibration result for the bridge.
    /// </summary>
    public long CalibrationResult1 { get; private set; }

    /// <summary>
    /// Gets the second calibration result for the bridge.
    /// </summary>
    public long CalibrationResult2 { get; private set; }

    /// <summary>
    /// Calibrates the bridge using the specified equations.
    /// </summary>
    /// <param name="equations">The equations to use to calibrate the bridge.</param>
    /// <param name="useConcatenation">Whether to use the concatenation operator.</param>
    /// <returns>
    /// The calibration result for the bridge.
    /// </returns>
    public static long Calibrate(IList<string> equations, bool useConcatenation)
    {
        var parsed = new List<(long Target, Stack<long> Values)>(equations.Count);

        foreach (string equation in equations)
        {
            string[] parts = equation.Split(':');

            long target = Parse<long>(parts[0]);
            string[] values = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var stack = new Stack<long>(values.Length);

            for (int i = values.Length - 1; i > -1; i--)
            {
                stack.Push(Parse<long>(values[i]));
            }

            parsed.Add((target, stack));
        }

        long result = 0;

        foreach ((long target, var values) in parsed)
        {
            if (TrySolve(values, 0, target, useConcatenation))
            {
                result += target;
            }
        }

        return result;

        static bool TrySolve(Stack<long> values, long current, long target, bool useConcatenation)
        {
            if (current > target)
            {
                return false;
            }

            if (values.Count == 0)
            {
                return current == target;
            }

            long value = values.Pop();

            if (TrySolve(values, current + value, target, useConcatenation))
            {
                return true;
            }

            if (TrySolve(values, current * value, target, useConcatenation))
            {
                return true;
            }

            if (useConcatenation && TrySolve(values, Concat(current, value), target, true))
            {
                return true;
            }

            values.Push(value);

            return false;
        }

        static long Concat(long x, long y)
        {
            long factor = y switch
            {
                < 10 => 10,
                < 100 => 100,
                < 1_000 => 1_000,
                _ => GetFactorSlow(y),
            };

            return (factor * x) + y;

            static long GetFactorSlow(long y)
            {
                float log10 = MathF.Log10(y);
                float tens = MathF.Floor(log10);
                return (long)MathF.Pow(10, tens + 1);
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var equations = await ReadResourceAsLinesAsync(cancellationToken);

        CalibrationResult1 = Calibrate(equations, useConcatenation: false);
        CalibrationResult2 = Calibrate(equations, useConcatenation: true);

        if (Verbose)
        {
            Logger.WriteLine("The total calibration result is {0}.", CalibrationResult1);
            Logger.WriteLine("The total calibration result using concatenation is {0}.", CalibrationResult2);
        }

        return PuzzleResult.Create(CalibrationResult1, CalibrationResult2);
    }
}
