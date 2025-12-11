// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/7</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 07, "Bridge Repair", RequiresData = true)]
public sealed class Day07 : Puzzle<long, long>
{
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
        long result = 0;

        Parallel.ForEach(equations, (equation) =>
        {
            string[] parts = equation.Split(':');

            long target = Parse<long>(parts[0]);
            string[] rawValues = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            long[] values = new long[rawValues.Length];

            for (int i = 0; i < rawValues.Length; i++)
            {
                values[i] = Parse<long>(rawValues[i]);
            }

            if (TrySolve(values, 0, 0, target, useConcatenation))
            {
                Interlocked.Add(ref result, target);
            }
        });

        return result;

        static bool TrySolve(long[] values, int index, long current, long target, bool useConcatenation)
        {
            if (current > target)
            {
                return false;
            }

            if (index >= values.Length)
            {
                return current == target;
            }

            long value = values[index++];

            if (TrySolve(values, index, current + value, target, useConcatenation))
            {
                return true;
            }

            if (TrySolve(values, index, current * value, target, useConcatenation))
            {
                return true;
            }

            if (useConcatenation && TrySolve(values, index, Concat(current, value), target, true))
            {
                return true;
            }

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
                float tens = MathF.Floor(log10 + 1);
                return (long)MathF.Pow(10, tens);
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var equations = await ReadResourceAsLinesAsync(cancellationToken);

        Solution1 = Calibrate(equations, useConcatenation: false);
        Solution2 = Calibrate(equations, useConcatenation: true);

        if (Verbose)
        {
            Logger.WriteLine("The total calibration result is {0}.", Solution1);
            Logger.WriteLine("The total calibration result using concatenation is {0}.", Solution2);
        }

        return Result();
    }
}
