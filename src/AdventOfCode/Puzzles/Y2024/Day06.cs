// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/6</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 06, "Guard Gallivant", RequiresData = true)]
public sealed class Day06 : Puzzle<int, int>
{
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

        byte[] visited = new byte[lab.Width * lab.Height];

        _ = Patrol(lab, origin, visited, cancellationToken);

        var positions = new List<Point>();

        for (int y = 0; y < lab.Height; y++)
        {
            for (int x = 0; x < lab.Width; x++)
            {
                if (visited[(y * lab.Width) + x] != 0)
                {
                    positions.Add(new(x, y));
                }
            }
        }

        int loops = 0;

        foreach (var obstruction in positions)
        {
            cancellationToken.ThrowIfCancellationRequested();

            lab[obstruction] = true;

            Array.Clear(visited);

            if (Patrol(lab, origin, visited, cancellationToken))
            {
                loops++;
            }

            lab[obstruction] = false;
        }

        return (positions.Count, loops);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (values, logger, cancellationToken) =>
            {
                (int distinctPositions, int distinctObstructions) = Patrol(values, cancellationToken);

                if (logger is { })
                {
                    logger.WriteLine("{0} distinct positions are visited by the guard before leaving the mapped area.", distinctPositions);
                    logger.WriteLine("{0} distinct positions can be chosen for an obstruction to create a loop.", distinctObstructions);
                }

                return (distinctPositions, distinctObstructions);
            },
            cancellationToken);
    }

    private static (Lab Lab, Point Origin) ParseMap(IList<string> map)
    {
        int height = map.Count;
        int width = map[0].Length;

        var lab = new Lab(width, height);
        var origin = Point.Empty;

        for (int y = 0; y < height; y++)
        {
            string row = map[y];

            for (int x = 0; x < row.Length; x++)
            {
                char contents = row[x];

                if (contents is '^')
                {
                    origin = new(x, y);
                }
                else if (contents is '#')
                {
                    lab[new(x, y)] = true;
                }
            }
        }

        return (lab, origin);
    }

    private static bool Patrol(
        Lab lab,
        Point origin,
        byte[] visited,
        CancellationToken cancellationToken)
    {
        // Each direction the guard can face is tracked as a single bit for each location
        const byte Up = 1;
        const byte Left = 8;

        var direction = Directions.Up;
        var location = origin;
        byte facing = Up;

        visited[lab.Index(location)] |= facing;

        bool loop = false;

        do
        {
            Point next = location + direction;

            if (!lab.InBounds(next))
            {
                // The guard has left the lab
                break;
            }

            if (lab[next])
            {
                // Turn right by 90°
                direction = new(-direction.Height, direction.Width);
                facing = facing == Left ? Up : (byte)(facing << 1);
            }
            else
            {
                ref byte state = ref visited[lab.Index(next)];

                if ((state & facing) != 0)
                {
                    // The guard has reached a position and orientation previously visited
                    loop = true;
                    break;
                }

                state |= facing;
                location = next;
            }
        }
        while (!cancellationToken.IsCancellationRequested);

        cancellationToken.ThrowIfCancellationRequested();

        return loop;
    }

    private sealed class Lab(int width, int height)
    {
        private readonly bool[] _obstructions = new bool[width * height];

        public int Width { get; } = width;

        public int Height { get; } = height;

        public bool this[Point location]
        {
            get => _obstructions[Index(location)];
            set => _obstructions[Index(location)] = value;
        }

        public bool InBounds(Point location)
            => location.X >= 0 && location.X < Width && location.Y >= 0 && location.Y < Height;

        public int Index(Point location) => (location.Y * Width) + location.X;
    }
}
