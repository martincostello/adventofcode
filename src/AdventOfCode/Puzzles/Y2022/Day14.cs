// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/14</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 14, "Regolith Reservoir", RequiresData = true)]
public sealed class Day14 : Puzzle
{
    private enum Content
    {
        None = 0,
        Rock,
        Sand,
    }

    /// <summary>
    /// Gets the number of grains of sand that come to rest
    /// before sand starts flowing into the abyss below.
    /// </summary>
    public int GrainsOfSand { get; private set; }

    /// <summary>
    /// Simulates the flow of sand through the specified cave.
    /// </summary>
    /// <param name="paths">The path of each solid rock structure of the cave to simulate sand flow through.</param>
    /// <returns>
    /// The number of grains of sand that come to rest
    /// before sand starts flowing into the abyss below.
    /// </returns>
    public static int Simulate(IList<string> paths)
    {
        var cave = Parse(paths);

        int maxX = cave.Keys.MaxBy((p) => p.X).X;
        int minX = cave.Keys.MinBy((p) => p.X).X;
        int maxY = cave.Keys.MaxBy((p) => p.Y).Y;
        int minY = 0;

        var origin = new Point(500, 0);
        var bounds = new Rectangle(minX, minY, maxX - minX, maxY);

        int grains = Simulate(cave, origin, bounds);

#if DEBUG
        string display = Visualize(cave, origin);
#endif

        return grains;

        static Dictionary<Point, Content> Parse(IList<string> paths)
        {
            var cave = new Dictionary<Point, Content>();

            foreach (string path in paths)
            {
                var points = path
                    .Split(" -> ")
                    .Select((p) => p.AsNumberPair<int>())
                    .Select((p) => new Point(p.First, p.Second))
                    .ToList();

                for (int i = 0; i < points.Count - 1; i++)
                {
                    var start = points[i];
                    var end = points[i + 1];

                    cave[start] = Content.Rock;

                    foreach (Point point in start.WalkTo(end))
                    {
                        cave[point] = Content.Rock;
                    }
                }
            }

            return cave;
        }

        static int Simulate(
            Dictionary<Point, Content> cave,
            Point origin,
            Rectangle bounds)
        {
            for (int i = 1; ; i++)
            {
                var current = origin;

                while (bounds.Contains(current))
                {
                    var next = Next(current);

                    if (next is not { } value)
                    {
                        cave[current] = Content.Sand;
                        break;
                    }

                    current = value;
                }

                if (!bounds.Contains(current))
                {
                    return i - 1;
                }
            }

            Point? Next(Point location)
            {
                var down = new Size(0, 1);
                var left = new Size(-1, 1);
                var right = new Size(1, 1);

                var next = location + down;

                if (!cave.TryGetValue(next, out var content))
                {
                    return next;
                }

                next = location + left;

                if (!cave.ContainsKey(next))
                {
                    return next;
                }

                next = location + right;

                if (!cave.ContainsKey(next))
                {
                    return next;
                }

                return null;
            }
        }

#if DEBUG
        static string Visualize(Dictionary<Point, Content> cave, Point origin)
        {
            int maxX = cave.Keys.MaxBy((p) => p.X).X;
            int minX = cave.Keys.MinBy((p) => p.X).X;
            int maxY = cave.Keys.MaxBy((p) => p.Y).Y;
            int minY = 0;

            var builder = new StringBuilder();

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var location = new Point(x, y);

                    if (location == origin)
                    {
                        builder.Append('+');
                    }
                    else if (cave.TryGetValue(location, out var content))
                    {
                        builder.Append(content == Content.Rock ? '#' : 'o');
                    }
                    else
                    {
                        builder.Append('.');
                    }
                }

                builder.Append('\n');
            }

            return builder.ToString();
        }
#endif
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync();

        GrainsOfSand = Simulate(values);

        if (Verbose)
        {
            Logger.WriteLine("{0} grains of sand come to rest before sand starts flowing into the abyss below.", GrainsOfSand);
        }

        return PuzzleResult.Create(GrainsOfSand);
    }
}
