// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2018/day/5</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2018, 05, "Alchemical Reduction", RequiresData = true)]
public sealed class Day05 : Puzzle<int, int>
{
    /// <summary>
    /// Reduces the specified polymer.
    /// </summary>
    /// <param name="polymer">The polymer to reduce.</param>
    /// <returns>
    /// The polymer remaining after reducing the specified <paramref name="polymer"/>
    /// value until it can be reduced no further.
    /// </returns>
    public static string Reduce(string polymer)
    {
        char[] units = polymer.ToCharArray();
        int length = units.Length;

        while (true)
        {
            int before = length;

            length = ReduceOnce(units.AsSpan(0, length));

            if (before == length)
            {
                break;
            }
        }

        return new(units, 0, length);
    }

    /// <summary>
    /// Reduces the specified polymer optimally.
    /// </summary>
    /// <param name="polymer">The polymer to reduce optimally.</param>
    /// <returns>
    /// The polymer remaining after reducing the specified <paramref name="polymer"/>
    /// value until it can be reduced no further with optimizations in use.
    /// </returns>
    public static string ReduceWithOptimization(string polymer)
    {
        string[] units =
        [
            ..polymer.Select((p) => char.ToLowerInvariant(p).ToString(CultureInfo.InvariantCulture))
                     .Distinct()
                     .Order(),
        ];

        string optimized = units
            .Select((p) => polymer.Replace(p, string.Empty, StringComparison.OrdinalIgnoreCase))
            .Select(Reduce)
            .OrderBy((p) => p.Length)
            .First();

        return optimized;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithStringAsync(
            static (polymer, logger, _) =>
            {
                polymer = polymer.Trim('\r', '\n');

                int remainingUnits = Reduce(polymer).Length;
                int remainingUnitsOptimized = ReduceWithOptimization(polymer).Length;

                if (logger is { })
                {
                    logger.WriteLine($"The number of units that remain after fully reacting the polymer is {remainingUnits:N0}.");
                    logger.WriteLine($"The number of units that remain after fully reacting the polymer with optimization is {remainingUnitsOptimized:N0}.");
                }

                return (remainingUnits, remainingUnitsOptimized);
            },
            cancellationToken);
    }

    /// <summary>
    /// Reduces the specified polymer once.
    /// </summary>
    /// <param name="polymer">The polymer to reduce in-place.</param>
    /// <returns>
    /// The length of the polymer remaining after reducing the specified <paramref name="polymer"/> value.
    /// </returns>
    private static int ReduceOnce(Span<char> polymer)
    {
        const int Shift = 'a' - 'A';

        int length = 0;

        for (int i = 0; i < polymer.Length; i++)
        {
            char x = polymer[i];

            if (i < polymer.Length - 1 && Math.Abs(x - polymer[i + 1]) == Shift)
            {
                // Skip over both units as they react with each other
                i++;
                continue;
            }

            polymer[length++] = x;
        }

        return length;
    }
}
