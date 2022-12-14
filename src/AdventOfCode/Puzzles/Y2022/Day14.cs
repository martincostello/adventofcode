// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/14</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 14, "Regolith Reservoir", RequiresData = true)]
public sealed class Day14 : Puzzle
{
    private static readonly Size Down = new(0, 1);
    private static readonly Size Left = new(-1, 1);
    private static readonly Size Right = new(1, 1);

    private enum Content
    {
        None = 0,
        Rock,
        Sand,
        Floor,
    }

    /// <summary>
    /// Gets the number of grains of sand that come to rest
    /// before sand starts flowing into the abyss below.
    /// </summary>
    public int GrainsOfSandWithVoid { get; private set; }

    /// <summary>
    /// Gets the number of grains of sand that come to rest
    /// before the source of the sand becomes blocked.
    /// </summary>
    public int GrainsOfSandWithFloor { get; private set; }

    /// <summary>
    /// Simulates the flow of sand through the specified cave.
    /// </summary>
    /// <param name="paths">The path of each solid rock structure of the cave to simulate sand flow through.</param>
    /// <param name="hasFloor">Whether the cave has a floor.</param>
    /// <param name="cancellationToken">The optional <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The number of grains of sand that come to rest and a visualization of the sand flow.
    /// </returns>
    public static (int Grains, string Visualization) Simulate(
        IList<string> paths,
        bool hasFloor,
        CancellationToken cancellationToken = default)
    {
        var cave = Parse(paths);

        int maxX = cave.Keys.MaxBy((p) => p.X).X;
        int minX = cave.Keys.MinBy((p) => p.X).X;
        int maxY = cave.Keys.MaxBy((p) => p.Y).Y;
        int minY = 0;

        if (hasFloor)
        {
            maxY += 2;

            for (int x = minX; x <= maxX; x++)
            {
                cave[new(x, maxY)] = Content.Floor;
            }
        }

        var origin = new Point(500, 0);
        var bounds = new Rectangle(minX, minY, maxX - minX, maxY);

        int grains = Simulate(cave, origin, bounds, hasFloor, cancellationToken);

        string visualization = Visualize(cave, origin);

        return (grains, visualization);

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
            Rectangle bounds,
            bool hasFloor,
            CancellationToken cancellationToken)
        {
            for (int i = 1; !cancellationToken.IsCancellationRequested; i++)
            {
                var current = origin;

                while (bounds.Contains(current))
                {
                    var next = Next(current, cave);

                    if (next is not { } value)
                    {
                        cave[current] = Content.Sand;
                        break;
                    }

                    current = value;

                    // Extend the bounds to account for the "infinite" floor so sand can still flow
                    if (hasFloor && (current.X == bounds.Left + 1 || current.X == bounds.Right - 1))
                    {
                        bounds = new Rectangle(bounds.X - 1, bounds.Top, bounds.Width + 2, bounds.Height);
                        cave[new(bounds.Left, bounds.Bottom)] = Content.Floor;
                        cave[new(bounds.Right, bounds.Bottom)] = Content.Floor;
                    }
                }

                if (!bounds.Contains(current))
                {
                    return i - 1;
                }
                else if (hasFloor && current == origin)
                {
                    return i;
                }
            }

            static Point? Next(Point location, Dictionary<Point, Content> cave)
            {
                var next = location + Down;

                if (!cave.ContainsKey(next))
                {
                    return next;
                }

                next = location + Left;

                if (!cave.ContainsKey(next))
                {
                    return next;
                }

                next = location + Right;

                if (!cave.ContainsKey(next))
                {
                    return next;
                }

                return null;
            }

            return -1;
        }

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
                        builder.Append(content == Content.Sand ? 'o' : '#');
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
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var paths = await ReadResourceAsLinesAsync();

        (GrainsOfSandWithVoid, string visualization1) = Simulate(paths, hasFloor: false, cancellationToken);
        (GrainsOfSandWithFloor, string visualization2) = Simulate(paths, hasFloor: true, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("{0} grains of sand come to rest before sand starts flowing into the abyss below.", GrainsOfSandWithVoid);
            Logger.WriteLine(visualization1);

            Logger.WriteLine("{0} grains of sand come to rest before sand blocks the source.", GrainsOfSandWithFloor);
            Logger.WriteLine(visualization2);
        }

        var result = new PuzzleResult();

        result.Solutions.Add(GrainsOfSandWithVoid);
        result.Solutions.Add(GrainsOfSandWithFloor);

        result.Visualizations.Add(visualization1);
        result.Visualizations.Add(visualization2);

        return result;
    }
}
