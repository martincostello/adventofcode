// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/6</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 06, "Lanternfish", RequiresData = true)]
public sealed class Day06 : Puzzle
{
    /// <summary>
    /// Gets the number of lantern fish present after 80 days.
    /// </summary>
    public long FishCount80 { get; private set; }

    /// <summary>
    /// Gets the number of lantern fish present after 256 days.
    /// </summary>
    public long FishCount256 { get; private set; }

    /// <summary>
    /// Counts the number of fish present after the specified number of days.
    /// </summary>
    /// <param name="fish">The initial fish states.</param>
    /// <param name="days">The number of days to simulate reproduction for.</param>
    /// <returns>
    /// The total number of fish present after the number of days specified by <paramref name="days"/>.
    /// </returns>
    public static long CountFish(IEnumerable<int> fish, int days)
    {
        var states = new Dictionary<int, long>()
        {
            [0] = 0,
            [1] = 0,
            [2] = 0,
            [3] = 0,
            [4] = 0,
            [5] = 0,
            [6] = 0,
            [7] = 0,
            [8] = 0,
        };

        foreach (int state in fish)
        {
            states[state]++;
        }

        for (int i = 1; i <= days; i++)
        {
            long newFish = 0;

            foreach (int state in states.Keys)
            {
                long count = states[state];
                states[state] = 0;

                if (state == 0)
                {
                    newFish += count;
                    states[7] += count;
                }
                else
                {
                    states[state - 1] += count;
                }
            }

            if (newFish > 0)
            {
                states[8] += newFish;
            }
        }

        return states.Values.Sum();
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<int> fish = (await ReadResourceAsStringAsync()).AsNumbers<int>().ToArray();

        FishCount80 = CountFish(fish, days: 80);
        FishCount256 = CountFish(fish, days: 256);

        if (Verbose)
        {
            Logger.WriteLine("There are {0:N0} lanternfish after 80 days.", FishCount80);
            Logger.WriteLine("There are {0:N0} lanternfish after 256 days.", FishCount256);
        }

        return PuzzleResult.Create(FishCount80, FishCount256);
    }
}
