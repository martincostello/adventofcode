// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/17</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2020, 17, RequiresData = true)]
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
        /// Gets the number of active cubes after the specified number of cycles.
        /// </summary>
        public int ActiveCubes { get; private set; }

        /// <summary>
        /// Gets the number of active cubes for the specified initial states.
        /// </summary>
        /// <param name="initialStates">The initial cube states.</param>
        /// <param name="cycles">The number of cycles to perform.</param>
        /// <param name="logger">The optional logger to use.</param>
        /// <returns>
        /// The number of active cubes after the specified number of cycles and a visualization of the final states.
        /// </returns>
        public static (int activeCubes, string visualization) GetActiveCubes(
            IList<string> initialStates,
            int cycles,
            ILogger? logger = null)
        {
            var currentState = new Dictionary<Point, char>();

            for (int y = 0; y < initialStates.Count; y++)
            {
                for (int x = 0; x < initialStates.Count; x++)
                {
                    currentState[new Point(x, y, z: 0)] = initialStates[y][x];
                }
            }

            string visualization = WriteState(currentState, logger);

            for (int i = 0; i < cycles; i++)
            {
                Extend(currentState);

                currentState = Iterate(currentState);

                visualization = WriteState(currentState, logger);
            }

            int activeCubes = currentState.Values
                .Where((p) => p == Active)
                .Count();

            return (activeCubes, visualization);

            static void Extend(IDictionary<Point, char> states)
            {
                foreach (Point point in states.Keys.ToArray())
                {
                    foreach (Point adjacent in AdjacentCubes(point))
                    {
                        if (!states.TryGetValue(adjacent, out _))
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

            (int activeCubes, string visualization) = GetActiveCubes(layout, cycles);

            ActiveCubes = activeCubes;

            if (Verbose)
            {
                Logger.WriteLine("There are {0} active cubes after {1} cycles.", ActiveCubes, cycles);
            }

            var result = new PuzzleResult();

            result.Solutions.Add(ActiveCubes);
            result.Visualizations.Add(visualization);

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
                char currentState = point.Value;

                int count = CountActiveCubes(point.Key, states);

                char nextState = currentState;

                if (currentState == Active && count != 2 && count != 3)
                {
                    nextState = Inactive;
                }
                else if (currentState == Inactive && count == 3)
                {
                    nextState = Active;
                }

                updated[point.Key] = nextState;
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
            {
                if (!states.TryGetValue(position, out char state))
                {
                    return false;
                }

                return state == Active;
            }
        }

        /// <summary>
        /// Gets the adjacent cubes for the specified point.
        /// </summary>
        /// <param name="position">The point to enumerate the adjacent points for.</param>
        /// <returns>
        /// A sequence of adjacent points in 3D space to <paramref name="position"/>.
        /// </returns>
        private static IEnumerable<Point> AdjacentCubes(Point position)
        {
            for (int z = -1; z <= 1; z++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        var adjacent = new Point(x, y, z);

                        if (adjacent != Point.Zero)
                        {
                            yield return position + adjacent;
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
        private static string WriteState(IDictionary<Point, char> states, ILogger? logger)
        {
            int maxHeight = states.Max((p) => p.Key.Y);
            int maxWidth = states.Max((p) => p.Key.X);
            int maxDepth = states.Max((p) => p.Key.Z);
            int minDepth = -maxDepth;

            var builder = new StringBuilder();

            for (int z = minDepth; z <= maxDepth; z++)
            {
                builder.Append("z=")
                       .Append(z)
                       .AppendLine();

                for (int y = 0; y <= maxHeight; y++)
                {
                    for (int x = 0; x <= maxWidth; x++)
                    {
                        var point = new Point(x, y, z);

                        if (!states.TryGetValue(point, out char state))
                        {
                            state = Inactive;
                        }

                        builder.Append(state);
                    }

                    builder.AppendLine();
                }

                builder.AppendLine();
            }

            string visualization = builder.ToString();

            logger?.WriteLine(visualization);

            return visualization;
        }

        /// <summary>
        /// Represents a point in 3D space.
        /// </summary>
        private struct Point : IEquatable<Point>
        {
            /// <summary>
            /// The origin point for a 3D space.
            /// </summary>
            public static Point Zero = new Point(0, 0, 0);

            /// <summary>
            /// The X coordinate.
            /// </summary>
            public int X;

            /// <summary>
            /// The Y coordinate.
            /// </summary>
            public int Y;

            /// <summary>
            /// The Z coordinate.
            /// </summary>
            public int Z;

            /// <summary>
            /// Initializes a new instance of the <see cref="Point"/> struct.
            /// </summary>
            /// <param name="x">The X coordinate.</param>
            /// <param name="y">The Y coordinate.</param>
            /// <param name="z">The Z coordinate.</param>
            public Point(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public static Point operator +(Point point, Point vector)
            {
                return new Point(
                    point.X + vector.X,
                    point.Y + vector.Y,
                    point.Z + vector.Z);
            }

            public static bool operator ==(Point a, Point b)
                => a.X == b.X && a.Y == b.Y && a.Z == b.Z;

            public static bool operator !=(Point a, Point b)
                => a.X != b.X || a.Y != b.Y || a.Z != b.Z;

            /// <inheritdoc />
            public override bool Equals(object? obj)
            {
                if (obj is not Point p)
                {
                    return false;
                }

                return this == p;
            }

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>
            /// <see langword="true"/> if the current object is equal to the other parameter; otherwise, <see langword="false"/>.
            /// </returns>
            public bool Equals(Point other) => this == other;

            /// <inheritdoc />
            public override int GetHashCode()
                => HashCode.Combine(X, Y, Z);

            /// <inheritdoc />
            public override string ToString()
                => "(" + string.Join(", ", X, Y, Z) + ")";
        }
    }
}
