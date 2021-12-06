// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/6</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 06, RequiresData = true)]
public sealed class Day06 : Puzzle
{
    /// <summary>
    /// Gets the number of lantern fish present after 80 days.
    /// </summary>
    public int FishCount { get; private set; }

    /// <summary>
    /// Counts the number of fish present after the specified number of days.
    /// </summary>
    /// <param name="fish">The initial fish states.</param>
    /// <param name="days">The number of days to simulate reproduction for.</param>
    /// <returns>
    /// The total number of fish present after the number of days specified by <paramref name="days"/>.
    /// </returns>
    public static int CountFish(IEnumerable<int> fish, int days)
    {
        var states = new List<int>(fish);

        for (int i = 1; i <= days; i++)
        {
            var newFish = new List<int>();

            for (int j = 0; j < states.Count; j++)
            {
                int state = states[j];

                if (state == 0)
                {
                    state = 7;
                    newFish.Add(8);
                }

                states[j] = --state;
            }

            states.AddRange(newFish);
        }

        return states.Count;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<int> fish = (await ReadResourceAsStringAsync()).AsNumbers<int>().ToArray();

        FishCount = CountFish(fish, days: 80);

        if (Verbose)
        {
            Logger.WriteLine("There are {0:N0} lanternfish after 80 days.", FishCount);
        }

        return PuzzleResult.Create(FishCount);
    }
}
