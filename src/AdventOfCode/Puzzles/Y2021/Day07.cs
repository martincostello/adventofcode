// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/7</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 07, RequiresData = true)]
public sealed class Day07 : Puzzle
{
    /// <summary>
    /// Gets the optimal units of fuel consumed to align the crab submarines.
    /// </summary>
    public long FuelConsumed { get; private set; }

    /// <summary>
    /// Determines the optimal units of fuel consumed to align the specified crab submarines.
    /// </summary>
    /// <param name="submarines">The initial positions of the submarines.</param>
    /// <returns>
    /// The total units of fuel consumed to align the submarines with
    /// positions specified by <paramref name="submarines"/>.
    /// </returns>
    public static long AlignSubmarines(IList<int> submarines)
    {
        int limit = submarines.Max();

        var fuelCosts = new Dictionary<int, long>(limit);

        for (int i = 0; i < limit; i++)
        {
            long fuelCost = 0;

            for (int j = 0; j < submarines.Count; j++)
            {
                fuelCost += Math.Abs(submarines[j] - i);
            }

            fuelCosts[i] = fuelCost;
        }

        return fuelCosts.MinBy((p) => p.Value).Value;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<int> submarines = (await ReadResourceAsStringAsync()).AsNumbers<int>().ToArray();

        FuelConsumed = AlignSubmarines(submarines);

        if (Verbose)
        {
            Logger.WriteLine("The least fuel consumed to align the crabs' submarines is {0:N0}.", FuelConsumed);
        }

        return PuzzleResult.Create(FuelConsumed);
    }
}
