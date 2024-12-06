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

        (var route, _) = Patrol(lab, origin, cancellationToken);

        var visited = route.Select((p) => p.Location).ToHashSet();

        int loops = 0;

        foreach (var obstruction in visited)
        {
            cancellationToken.ThrowIfCancellationRequested();

            lab[obstruction] = true;

            if (Patrol(lab, origin, cancellationToken) is { Loop: true })
            {
                loops++;
            }

            lab[obstruction] = false;
        }

        return (visited.Count, loops);
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
                bool obstructed = false;
                char contents = row[x];

                if (contents is '^')
                {
                    origin = new(x, y);
                }
                else if (contents is '#')
                {
                    obstructed = true;
                }

                lab[new(x, y)] = obstructed;
            }
        }

        return (lab, origin);
    }

    private static (HashSet<(Point Location, Size Direction)> Route, bool Loop) Patrol(
        Dictionary<Point, bool> lab,
        Point origin,
        CancellationToken cancellationToken)
    {
        var direction = Directions.Up;
        var location = origin;
        var route = new HashSet<(Point, Size)>();

        bool loop = false;

        do
        {
            if (!route.Add((location, direction)))
            {
                // The guard has reached a position and orientation previously visited
                loop = true;
                break;
            }

            Point next = location + direction;

            if (!lab.TryGetValue(next, out bool obstructed))
            {
                // The guard has left the lab
                break;
            }

            if (obstructed)
            {
                // Turn right by 90°
                direction = new(-direction.Height, direction.Width);
            }
            else
            {
                location = next;
            }
        }
        while (!cancellationToken.IsCancellationRequested);

        cancellationToken.ThrowIfCancellationRequested();

        return (route, loop);
    }
}
