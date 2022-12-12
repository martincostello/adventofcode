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
    public int MinimumSteps { get; private set; }

    /// <summary>
    /// Returns the minimum number of steps required to go from the starting
    /// position to the highest point in the specified heightmap.
    /// </summary>
    /// <param name="heightmap">The height map to traverse.</param>
    /// <returns>
    /// The fewest steps required to move from the current position
    /// to the highest location in the heightmap specified by <paramref name="heightmap"/>.
    /// </returns>
    public static int GetMinimumSteps(IList<string> heightmap)
    {
        var map = BuildMap(heightmap);

        return (int)PathFinding.AStar(map, map.Start, map.End);

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

                    map.Locations.Add(location);
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

        MinimumSteps = GetMinimumSteps(heightMap);

        if (Verbose)
        {
            Logger.WriteLine(
                "The fewest steps required to move from your current position to the location that should get the best signal is {0}.",
                MinimumSteps);
        }

        return PuzzleResult.Create(MinimumSteps);
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
            int heightId = Elevations[id];

            foreach (var point in base.Neighbors(id))
            {
                int heightPoint = Elevations[point];
                int delta = heightPoint - heightId;

                if (delta < 2)
                {
                    yield return point;
                }
            }
        }
    }
}
