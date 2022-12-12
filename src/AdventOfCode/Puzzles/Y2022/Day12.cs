// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/12</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 12, "Hill Climbing Algorithm", RequiresData = true)]
public sealed class Day12 : Puzzle
{
    /// <summary>
    /// Gets the fewest steps required to move from your current
    /// position to the location that should get the best signal.
    /// </summary>
    public int MinimumStepsFromStart { get; private set; }

    /// <summary>
    /// Gets the fewest steps required to move from any position at
    /// ground level to the location that should get the best signal.
    /// </summary>
    public int MinimumStepsFromGroundLevel { get; private set; }

    /// <summary>
    /// Returns the minimum number of steps required to go to the
    /// highest point in the specified heightmap from the starting
    /// position and from any location at ground level.
    /// </summary>
    /// <param name="heightmap">The height map to traverse.</param>
    /// <returns>
    /// The fewest steps required to move to get to the highest location
    /// in the heightmap specified by <paramref name="heightmap"/> from
    /// the starting position and from any location at ground level.
    /// </returns>
    public static (int MinimumStepsFromStart, int MinimumStepsFromGroundLevel) GetMinimumSteps(IList<string> heightmap)
    {
        var map = BuildMap(heightmap);

        var locationsAtGroundLevel = map.Locations
            .Where((p) => map.Elevations[p] == 0)
            .ToList();

        var distances = new Dictionary<Point, int>(locationsAtGroundLevel.Count);

        foreach (var start in locationsAtGroundLevel)
        {
            int distance = (int)PathFinding.AStar(map, start, map.End);

            // Ignore starting locations that cannot reach the end
            if (distance > 0)
            {
                distances[start] = distance;
            }
        }

        return (distances[map.Start], distances.Values.Min((p) => p));

        static Map BuildMap(IList<string> heightmap)
        {
            int width = heightmap[0].Length;
            int height = heightmap.Count;

            var map = new Map(width, height);

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    var location = new Point(x, y);
                    map.Locations.Add(location);

                    char ch = heightmap[y][x];

                    map.Elevations[location] = ch switch
                    {
                        'S' => 0,
                        'E' => 'z' - 'a',
                        _ => ch - 'a',
                    };

                    if (ch == 'S')
                    {
                        map.Start = location;
                    }
                    else if (ch == 'E')
                    {
                        map.End = location;
                    }
                }
            }

            return map;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var heightMap = await ReadResourceAsLinesAsync();

        (MinimumStepsFromStart, MinimumStepsFromGroundLevel) = GetMinimumSteps(heightMap);

        if (Verbose)
        {
            Logger.WriteLine(
                "The fewest steps required to move from your current position to the location that should get the best signal is {0}.",
                MinimumStepsFromStart);

            Logger.WriteLine(
                "The fewest steps required to move from any position at ground level to the location that should get the best signal is {0}.",
                MinimumStepsFromGroundLevel);
        }

        return PuzzleResult.Create(MinimumStepsFromStart, MinimumStepsFromGroundLevel);
    }

    private sealed class Map : SquareGrid
    {
        public Map(int width, int height)
            : base(width, height)
        {
            Elevations = new(Width * Height);
        }

        public Point Start { get; set; }

        public Point End { get; set; }

        public Dictionary<Point, int> Elevations { get; }

        public override long Cost(Point a, Point b) => a.ManhattanDistance(b);

        public override IEnumerable<Point> Neighbors(Point id)
        {
            int thisHeight = Elevations[id];

            foreach (var point in base.Neighbors(id))
            {
                int nextHeight = Elevations[point];
                int delta = nextHeight - thisHeight;

                if (delta < 2)
                {
                    yield return point;
                }
            }
        }
    }
}
