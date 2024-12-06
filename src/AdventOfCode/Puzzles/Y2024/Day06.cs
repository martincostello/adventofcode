// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/6</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 06, "Guard Gallivant", RequiresData = true)]
public sealed class Day06 : Puzzle
{
    /// <summary>
    /// Gets the number of distinct positions visited by the guard.
    /// </summary>
    public int DistinctPositions { get; private set; }

    /// <summary>
    /// Patrol the lab defined by the specified map.
    /// </summary>
    /// <param name="map">The map of the lab to patrol.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The number of distinct positions visited by the guard before leaving the lab.
    /// </returns>
    public static int Patrol(IList<string> map, CancellationToken cancellationToken)
    {
        var lab = new Dictionary<Point, bool>();
        var location = Point.Empty;
        var direction = Directions.Up;

        for (int y = 0; y < map.Count; y++)
        {
            string row = map[y];

            for (int x = 0; x < row.Length; x++)
            {
                bool passable = true;

                switch (row[x])
                {
                    case '^':
                        location = new(x, y);
                        break;

                    case '#':
                        passable = false;
                        break;

                    default:
                        break;
                }

                lab[new(x, y)] = passable;
            }
        }

        var locations = new HashSet<Point>();

        do
        {
            locations.Add(location);

            Point next = location + direction;

            if (!lab.TryGetValue(next, out bool passable))
            {
                // The guard has left the lab
                break;
            }

            if (passable)
            {
                location = next;
            }
            else
            {
                direction = Directions.TurnRight(direction);
            }
        }
        while (!cancellationToken.IsCancellationRequested);

        cancellationToken.ThrowIfCancellationRequested();

        return locations.Count;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        DistinctPositions = Patrol(values, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("{0} distinct positions are visited by the guard before leaving the mapped area.", DistinctPositions);
        }

        return PuzzleResult.Create(DistinctPositions);
    }
}
