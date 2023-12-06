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
    public long CombinationsProduct { get; private set; }

    /// <summary>
    /// Gets the product of the number of combinations of ways to beat the record with the kerning fixed.
    /// </summary>
    public long CombinationsProductWithFix { get; private set; }

    /// <summary>
    /// Races the specified boats.
    /// </summary>
    /// <param name="values">The time and distance records.</param>
    /// <param name="fixKerning">Whether to fix the kerning in the values.</param>
    /// <returns>
    /// The product of the number of combinations of ways to beat the record.
    /// </returns>
    public static long Race(IList<string> values, bool fixKerning)
    {
        string timeValues = values[0].Split(':')[1];
        string distanceValues = values[1].Split(':')[1];

        List<long> times;
        List<long> distances;

        if (fixKerning)
        {
            times = [Parse<long>(timeValues.Replace(" ", string.Empty, StringComparison.Ordinal))];
            distances = [Parse<long>(distanceValues.Replace(" ", string.Empty, StringComparison.Ordinal))];
        }
        else
        {
            times = timeValues.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(Parse<long>).ToList();
            distances = distanceValues.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(Parse<long>).ToList();
        }

        List<(long Time, long Distance)> timesAndDistances = times
            .Zip(distances, (t, d) => (t, d))
            .ToList();

        var ways = new List<long>();

        foreach ((long duration, long distanceRecord) in timesAndDistances)
        {
            long target = distanceRecord + 1;

            long speed = 1;
            long minimumSpeed = -1;
            long maximumTime = duration - 1;

            do
            {
                long time = duration - speed;
                long distance = time * speed;

                if (distance >= target)
                {
                    minimumSpeed = speed;
                    break;
                }
            }
            while (++speed < maximumTime);

            speed = maximumTime;
            long maximumSpeed = -1;

            do
            {
                long time = duration - speed;
                long distance = time * speed;

                if (distance >= target)
                {
                    maximumSpeed = speed;
                    break;
                }
            }
            while (--speed > minimumSpeed);

            long combinations = maximumSpeed - minimumSpeed + 1;

            if (combinations > 0)
            {
                ways.Add(combinations);
            }
        }

        return ways.Aggregate(1L, (x, y) => x * y);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        CombinationsProduct = Race(values, fixKerning: false);
        CombinationsProductWithFix = Race(values, fixKerning: true);

        if (Verbose)
        {
            Logger.WriteLine("The product of the number of ways to beat the record is {0}.", CombinationsProduct);
            Logger.WriteLine("The product of the number of ways to beat the record with the kerning fixed is {0}.", CombinationsProductWithFix);
        }

        return PuzzleResult.Create(CombinationsProduct, CombinationsProductWithFix);
    }
}
