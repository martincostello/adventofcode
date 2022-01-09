// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2020/day/17</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2020, 17, "Conway Cubes", RequiresData = true)]
public sealed class Day17 : Puzzle
{
    /// <summary>
    /// An active cube.
    /// </summary>
    private const char Active = '#';

    /// <summary>
    /// An inactive cube.
    /// </summary>
    private const char Inactive = '.';

    /// <summary>
    /// Gets the number of active cubes in three dimensions after the specified number of cycles.
    /// </summary>
    public int ActiveCubes3D { get; private set; }

    /// <summary>
    /// Gets the number of active cubes in four dimensions after the specified number of cycles.
    /// </summary>
    public int ActiveCubes4D { get; private set; }

    /// <summary>
    /// Gets the number of active cubes for the specified initial states.
    /// </summary>
    /// <param name="initialStates">The initial cube states.</param>
    /// <param name="cycles">The number of cycles to perform.</param>
    /// <param name="dimensions">The number of dimensions.</param>
    /// <param name="logger">The optional logger to use.</param>
    /// <returns>
    /// The number of active cubes after the specified number of cycles and a visualization of the final states.
    /// </returns>
    public static (int ActiveCubes, string Visualization) GetActiveCubes(
        IList<string> initialStates,
        int cycles,
        int dimensions,
        ILogger? logger = null)
    {
        var currentState = new Dictionary<Point, char>(initialStates.Count * initialStates.Count);

        for (int y = 0; y < initialStates.Count; y++)
        {
            for (int x = 0; x < initialStates.Count; x++)
            {
                var point = dimensions == 4 ? new Point(x, y, 0, 0) : new Point(x, y, 0);
                currentState[point] = initialStates[y][x];
            }
        }

        string visualization = WriteState(currentState, logger);

        for (int i = 0; i < cycles; i++)
        {
            Extend(currentState, dimensions);

            currentState = Iterate(currentState);

            visualization = WriteState(currentState, logger);
        }

        int activeCubes = currentState.Values
            .Where((p) => p == Active)
            .Count();

        return (activeCubes, visualization);

        static void Extend(Dictionary<Point, char> states, int dimensions)
        {
            var keys = states.Keys.ToArray();

            states.EnsureCapacity(keys.Length + ((int)Math.Pow(3, dimensions) * keys.Length));

            foreach (Point point in keys)
            {
                foreach (Point adjacent in AdjacentCubes(point))
                {
                    if (!states.ContainsKey(adjacent))
                    {
                        states[adjacent] = Inactive;
                    }
                }
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> layout = await ReadResourceAsLinesAsync();

        int cycles = 6;

        (int activeCubes3D, string visualization3D) = GetActiveCubes(layout, cycles, dimensions: 3);
        (int activeCubes4D, string visualization4D) = GetActiveCubes(layout, cycles, dimensions: 4);

        ActiveCubes3D = activeCubes3D;
        ActiveCubes4D = activeCubes4D;

        if (Verbose)
        {
            Logger.WriteLine("There are {0} active cubes after {1} cycles in 3 dimensions.", ActiveCubes3D, cycles);
            Logger.WriteLine("There are {0} active cubes after {1} cycles in 4 dimensions.", ActiveCubes4D, cycles);
        }

        var result = new PuzzleResult();

        result.Solutions.Add(ActiveCubes3D);
        result.Visualizations.Add(visualization3D);

        result.Solutions.Add(ActiveCubes4D);
        result.Visualizations.Add(visualization4D);

        return result;
    }

    /// <summary>
    /// Iterates the state of the cubes.
    /// </summary>
    /// <param name="states">The states to iterate.</param>
    /// <returns>
    /// The new states.
    /// </returns>
    private static Dictionary<Point, char> Iterate(Dictionary<Point, char> states)
    {
        var updated = new Dictionary<Point, char>(states);

        foreach (var point in states)
        {
            char state = point.Value;

            int count = CountActiveCubes(point.Key, states);

            if (state == Active && count != 2 && count != 3)
            {
                updated[point.Key] = Inactive;
            }
            else if (state == Inactive && count == 3)
            {
                updated[point.Key] = Active;
            }
        }

        return updated;

        static int CountActiveCubes(Point point, Dictionary<Point, char> states)
        {
            int count = 0;

            foreach (Point adjacent in AdjacentCubes(point))
            {
                if (IsAdjacentCubeActive(adjacent, states))
                {
                    count++;
                }
            }

            return count;
        }

        static bool IsAdjacentCubeActive(Point position, Dictionary<Point, char> states)
            => states.GetValueOrDefault(position) == Active;
    }

    /// <summary>
    /// Gets the adjacent cubes for the specified point.
    /// </summary>
    /// <param name="position">The point to enumerate the adjacent points for.</param>
    /// <returns>
    /// A sequence of adjacent points in space to <paramref name="position"/>.
    /// </returns>
    private static IEnumerable<Point> AdjacentCubes(Point position)
    {
        for (int z = -1; z <= 1; z++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if (position.W.HasValue)
                    {
                        for (int w = -1; w <= 1; w++)
                        {
                            var adjacent = new Point(x, y, z, w);

                            if (adjacent != Point.Zero4D)
                            {
                                yield return position + adjacent;
                            }
                        }
                    }
                    else
                    {
                        var adjacent = new Point(x, y, z);

                        if (adjacent != Point.Zero3D)
                        {
                            yield return position + adjacent;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Writes the specified message.
    /// </summary>
    /// <param name="states">The message to write.</param>
    /// <param name="logger">The logger to write the message to.</param>
    /// <returns>
    /// The visualization of the data.
    /// </returns>
    private static string WriteState(Dictionary<Point, char> states, ILogger? logger)
    {
        var builder = new StringBuilder();

        int maxY = states.Max((p) => p.Key.Y);
        int maxX = states.Max((p) => p.Key.X);
        int maxZ = states.Max((p) => p.Key.Z);
        int minZ = -maxZ;

        bool is4D = states.Keys.First().W.HasValue;

        if (is4D)
        {
            int maxW = states.Max((p) => p.Key.W!.Value);
            int minW = -maxW;

            for (int w = minW; w <= maxW; w++)
            {
                for (int z = minZ; z <= maxZ; z++)
                {
                    builder.Append("z=")
                           .Append(z)
                           .Append(", w=")
                           .Append(w)
                           .AppendLine();

                    for (int y = 0; y <= maxY; y++)
                    {
                        for (int x = 0; x <= maxX; x++)
                        {
                            var point = new Point(x, y, z, w);

                            char state = states.GetValueOrDefault(point, Inactive);

                            builder.Append(state);
                        }

                        builder.AppendLine();
                    }

                    builder.AppendLine();
                }
            }
        }
        else
        {
            for (int z = minZ; z <= maxZ; z++)
            {
                builder.Append("z=")
                       .Append(z)
                       .AppendLine();

                for (int y = 0; y <= maxY; y++)
                {
                    for (int x = 0; x <= maxX; x++)
                    {
                        var point = new Point(x, y, z);

                        char state = states.GetValueOrDefault(point, Inactive);

                        builder.Append(state);
                    }

                    builder.AppendLine();
                }

                builder.AppendLine();
            }
        }

        string visualization = builder.ToString();

        logger?.WriteLine(visualization);

        return visualization;
    }

    /// <summary>
    /// Represents a point (or vector) in 3 or 4 dimensional space.
    /// </summary>
    private readonly struct Point : IEquatable<Point>
    {
        public static Point Zero3D = new(0, 0, 0);

        public static Point Zero4D = new(0, 0, 0, 0);

        public readonly int? W;

        public readonly int X;

        public readonly int Y;

        public readonly int Z;

        public Point(int x, int y, int z, int? w = default)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public static Point operator +(Point point, Point vector)
        {
            return new(
                point.X + vector.X,
                point.Y + vector.Y,
                point.Z + vector.Z,
                point.W + vector.W);
        }

        public static bool operator ==(Point a, Point b)
            => a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W;

        public static bool operator !=(Point a, Point b)
            => a.X != b.X || a.Y != b.Y || a.Z != b.Z || a.W != b.W;

        public override readonly bool Equals(object? obj)
        {
            if (obj is not Point p)
            {
                return false;
            }

            return this == p;
        }

        public readonly bool Equals(Point other)
            => this == other;

        public override readonly int GetHashCode()
            => HashCode.Combine(X, Y, Z, W);

        public override readonly string ToString()
        {
            if (W.HasValue)
            {
                return '(' + string.Join(", ", X, Y, Z, W.Value) + ')';
            }
            else
            {
                return '(' + string.Join(", ", X, Y, Z) + ')';
            }
        }
    }
}
