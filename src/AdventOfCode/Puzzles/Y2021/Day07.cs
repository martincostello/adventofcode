﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/7</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 07, "The Treachery of Whales", RequiresData = true)]
public sealed class Day07 : Puzzle
{
    /// <summary>
    /// Gets the optimal units of fuel consumed to align the crab
    /// submarines when the fuel has a constant burn rate.
    /// </summary>
    public long FuelConsumedWithConstantBurnRate { get; private set; }

    /// <summary>
    /// Gets the optimal units of fuel consumed to align the crab
    /// submarines when the fuel has a variable burn rate.
    /// </summary>
    public long FuelConsumedWithVariableBurnRate { get; private set; }

    /// <summary>
    /// Determines the optimal units of fuel consumed to align the specified crab submarines.
    /// </summary>
    /// <param name="submarines">The initial positions of the submarines.</param>
    /// <param name="withVariableBurnRate">Whether the fuel has a variable burn rate.</param>
    /// <returns>
    /// The total units of fuel consumed to align the submarines with
    /// positions specified by <paramref name="submarines"/>.
    /// </returns>
    public static long AlignSubmarines(IList<int> submarines, bool withVariableBurnRate)
    {
        int limit = submarines.Max();
        var costs = new List<long>(limit);
        var stepCosts = new Dictionary<int, int>(limit)
        {
            [0] = 0,
        };

        if (withVariableBurnRate)
        {
            for (int i = 1; i <= limit; i++)
            {
                stepCosts[i] = stepCosts[i - 1] + i;
            }
        }

        for (int i = 0; i < limit; i++)
        {
            long cost = 0;

            for (int j = 0; j < submarines.Count; j++)
            {
                int steps = Math.Abs(submarines[j] - i);

                if (withVariableBurnRate)
                {
                    steps = stepCosts[steps];
                }

                cost += steps;
            }

            costs.Add(cost);
        }

        return costs.Min();
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<int> submarines = (await ReadResourceAsStringAsync(cancellationToken)).AsNumbers<int>().ToArray();

        FuelConsumedWithConstantBurnRate = AlignSubmarines(submarines, withVariableBurnRate: false);
        FuelConsumedWithVariableBurnRate = AlignSubmarines(submarines, withVariableBurnRate: true);

        if (Verbose)
        {
            Logger.WriteLine(
                "The least fuel consumed to align the crabs' submarines with a constant burn rate is {0:N0}.",
                FuelConsumedWithConstantBurnRate);

            Logger.WriteLine(
                "The least fuel consumed to align the crabs' submarines with a variable burn rate is {0:N0}.",
                FuelConsumedWithVariableBurnRate);
        }

        return PuzzleResult.Create(FuelConsumedWithConstantBurnRate, FuelConsumedWithVariableBurnRate);
    }
}
