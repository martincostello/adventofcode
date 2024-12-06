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
    /// Gets the number of distinct positions where an obstruction can be placed to cause the guard to patrol in a loop.
    /// </summary>
    public int DistinctObstructions { get; private set; }

    /// <summary>
    /// Patrol the lab defined by the specified map.
    /// </summary>
    /// <param name="map">The map of the lab to patrol.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The number of distinct positions visited by the guard before leaving the lab.
    /// </returns>
    public static (int Positions, int Loops) Patrol(IList<string> map, CancellationToken cancellationToken)
    {
        (var lab, var origin) = ParseMap(map);

        (int positions, _) = Patrol(lab, origin, cancellationToken);

        int loops = 0;

        var obstructed = new Dictionary<Point, bool>(lab);

        for (int y = 0; y < map.Count; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var obstruction = new Point(x, y);

                if (!lab[obstruction] || obstruction == origin)
                {
                    // Already obstructed or the current position of the guard
                    continue;
                }

                obstructed[obstruction] = false;

                (_, bool loop) = Patrol(obstructed, origin, cancellationToken);

                if (loop)
                {
                    loops++;
                }

                obstructed[obstruction] = lab[obstruction];
            }
        }

        return (positions, loops);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        (DistinctPositions, DistinctObstructions) = Patrol(values, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("{0} distinct positions are visited by the guard before leaving the mapped area.", DistinctPositions);
            Logger.WriteLine("{0} distinct positions can be chosen for an obstruction to create a loop.", DistinctObstructions);
        }

        return PuzzleResult.Create(DistinctPositions, DistinctObstructions);
    }

    private static (Dictionary<Point, bool> Lab, Point Origin) ParseMap(IList<string> map)
    {
        var lab = new Dictionary<Point, bool>();
        var origin = Point.Empty;

        for (int y = 0; y < map.Count; y++)
        {
            string row = map[y];

            for (int x = 0; x < row.Length; x++)
            {
                bool passable = true;

                switch (row[x])
                {
                    case '^':
                        origin = new(x, y);
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

        return (lab, origin);
    }

    private static (int Locations, bool Loop) Patrol(
        Dictionary<Point, bool> lab,
        Point origin,
        CancellationToken cancellationToken)
    {
        var direction = Directions.Up;
        var location = origin;
        var locations = new HashSet<Point>();

        bool loop = false;

        var path = new HashSet<(Point, Size)>();

        do
        {
            if (!path.Add((location, direction)))
            {
                // The guard has reached a position previously visited
                loop = true;
                break;
            }

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

        return (locations.Count, loop);
    }
}
