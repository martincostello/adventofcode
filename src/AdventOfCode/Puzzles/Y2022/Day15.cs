// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/15</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 15, "Beacon Exclusion Zone", RequiresData = true)]
public sealed class Day15 : Puzzle
{
    /// <summary>
    /// Gets the number of positions with no beacons in row 2,000,000.
    /// </summary>
    public int PositionsWithNoBeacons { get; private set; }

    /// <summary>
    /// Finds the number of positions in the specified row that cannot contain a beacon.
    /// </summary>
    /// <param name="values">The positions of the sensors and beacons.</param>
    /// <param name="row">The row to count the number of positions that cannot contain a beacon.</param>
    /// <returns>
    /// The number of positions with no beacons in the row specified by <paramref name="row"/>.
    /// </returns>
    public static int FindBeacons(IList<string> values, int row)
    {
        var readings = Parse(values);

        var zones = new List<Rectangle>();

        foreach ((var sensor, var beacon) in readings)
        {
        }

        return -1;

        static List<(Point Sensor, Point Beacon)> Parse(IList<string> values)
        {
            var readings = new List<(Point Sensor, Point Beacon)>(values.Count);

            foreach (string value in values)
            {
                string[] parts = value.Split(':');
                string[] sensorParts = parts[0].Split(',');
                string[] beaconParts = parts[1].Split(',');

                string sensorX = sensorParts[0].Split('=')[1];
                string sensorY = sensorParts[1].Split('=')[1];

                string beaconX = beaconParts[0].Split('=')[1];
                string beaconY = beaconParts[1].Split('=')[1];

                var sensor = new Point(Parse<int>(sensorX), Parse<int>(sensorY));
                var beacon = new Point(Parse<int>(beaconX), Parse<int>(beaconY));

                readings.Add((sensor, beacon));
            }

            return readings;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync();

        PositionsWithNoBeacons = FindBeacons(values, row: 2_000_000);

        if (Verbose)
        {
            Logger.WriteLine("{0} positions cannot contain a beacon in row 2,000,000.", PositionsWithNoBeacons);
        }

        return PuzzleResult.Create(PositionsWithNoBeacons);
    }
}
