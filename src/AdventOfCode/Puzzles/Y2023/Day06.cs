// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/06</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 06, "Wait For It", RequiresData = true)]
public sealed class Day06 : Puzzle
{
    /// <summary>
    /// Gets the product of the number of combinations of ways to beat the record.
    /// </summary>
    public int CombinationsProduct { get; private set; }

    /// <summary>
    /// Races the specified boats.
    /// </summary>
    /// <param name="values">The time and distance records.</param>
    /// <returns>
    /// The product of the number of combinations of ways to beat the record.
    /// </returns>
    public static int Race(IList<string> values)
    {
        var times = values[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(Parse<int>).ToList();
        var distances = values[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(Parse<int>).ToList();

        List<(int Time, int Distance)> timesAndDistances = times
            .Zip(distances, (t, d) => (t, d))
            .ToList();

        var ways = new List<int>();

        foreach ((int duration, int distanceRecord) in timesAndDistances)
        {
            int target = distanceRecord + 1;
            int minimumTime = 1;
            int maximumTime = duration - 1;

            int combinations = 0;

            for (int speed = minimumTime; speed < maximumTime; speed++)
            {
                int time = duration - speed;
                int distance = time * speed;

                if (distance >= target)
                {
                    combinations++;
                }
            }

            if (combinations > 0)
            {
                ways.Add(combinations);
            }
        }

        return ways.Aggregate(1, (x, y) => x * y);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        CombinationsProduct = Race(values);

        if (Verbose)
        {
            Logger.WriteLine("The product of the number of ways to beat the record is {0}.", CombinationsProduct);
        }

        return PuzzleResult.Create(CombinationsProduct);
    }
}
