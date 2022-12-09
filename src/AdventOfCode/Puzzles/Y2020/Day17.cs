// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Numerics;

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2020/day/17</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2020, 17, "Conway Cubes", RequiresData = true, IsSlow = true)]
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
        return dimensions switch
        {
            3 => Cubes3D.GetActiveCubes(initialStates, cycles, logger),
            4 => Cubes4D.GetActiveCubes(initialStates, cycles, logger),
            _ => throw new NotSupportedException(),
        };
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

    private static class Cubes3D
    {
        public static (int ActiveCubes, string Visualization) GetActiveCubes(
            IList<string> initialStates,
            int cycles,
            ILogger? logger = null)
        {
            var currentState = new Dictionary<Vector3, char>(initialStates.Count * initialStates.Count);

            for (int y = 0; y < initialStates.Count; y++)
            {
                for (int x = 0; x < initialStates.Count; x++)
                {
                    var point = new Vector3(x, y, 0);
                    currentState[point] = initialStates[y][x];
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

            static void Extend(Dictionary<Vector3, char> states)
            {
                var keys = states.Keys.ToArray();

                states.EnsureCapacity(keys.Length + ((int)Math.Pow(3, 3) * keys.Length));

                foreach (Vector3 point in keys)
                {
                    foreach (Vector3 adjacent in AdjacentCubes(point))
                    {
                        if (!states.ContainsKey(adjacent))
                        {
                            states[adjacent] = Inactive;
                        }
                    }
                }
            }
        }

        private static Dictionary<Vector3, char> Iterate(Dictionary<Vector3, char> states)
        {
            var updated = new Dictionary<Vector3, char>(states);

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

            static int CountActiveCubes(Vector3 point, Dictionary<Vector3, char> states)
            {
                int count = 0;

                foreach (Vector3 adjacent in AdjacentCubes(point))
                {
                    if (IsAdjacentCubeActive(adjacent, states))
                    {
                        count++;
                    }
                }

                return count;
            }

            static bool IsAdjacentCubeActive(Vector3 position, Dictionary<Vector3, char> states)
                => states.GetValueOrDefault(position) == Active;
        }

        private static IEnumerable<Vector3> AdjacentCubes(Vector3 position)
        {
            for (float z = -1; z <= 1; z++)
            {
                for (float y = -1; y <= 1; y++)
                {
                    for (float x = -1; x <= 1; x++)
                    {
                        var adjacent = new Vector3(x, y, z);

                        if (adjacent != Vector3.Zero)
                        {
                            yield return position + adjacent;
                        }
                    }
                }
            }
        }

        private static string WriteState(Dictionary<Vector3, char> states, ILogger? logger)
        {
            var builder = new StringBuilder();

            float maxY = states.Max((p) => p.Key.Y);
            float maxX = states.Max((p) => p.Key.X);
            float maxZ = states.Max((p) => p.Key.Z);
            float minZ = -maxZ;

            for (float z = minZ; z <= maxZ; z++)
            {
                builder.Append("z=")
                       .Append(z)
                       .AppendLine();

                for (float y = 0; y <= maxY; y++)
                {
                    for (float x = 0; x <= maxX; x++)
                    {
                        var point = new Vector3(x, y, z);

                        char state = states.GetValueOrDefault(point, Inactive);

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
    }

    private static class Cubes4D
    {
        public static (int ActiveCubes, string Visualization) GetActiveCubes(
            IList<string> initialStates,
            int cycles,
            ILogger? logger = null)
        {
            var currentState = new Dictionary<Vector4, char>(initialStates.Count * initialStates.Count);

            for (int y = 0; y < initialStates.Count; y++)
            {
                for (int x = 0; x < initialStates.Count; x++)
                {
                    var point = new Vector4(x, y, 0, 0);
                    currentState[point] = initialStates[y][x];
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

            static void Extend(Dictionary<Vector4, char> states)
            {
                var keys = states.Keys.ToArray();

                states.EnsureCapacity(keys.Length + ((int)Math.Pow(3, 4) * keys.Length));

                foreach (Vector4 point in keys)
                {
                    foreach (Vector4 adjacent in AdjacentCubes(point))
                    {
                        if (!states.ContainsKey(adjacent))
                        {
                            states[adjacent] = Inactive;
                        }
                    }
                }
            }
        }

        private static Dictionary<Vector4, char> Iterate(Dictionary<Vector4, char> states)
        {
            var updated = new Dictionary<Vector4, char>(states);

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

            static int CountActiveCubes(Vector4 point, Dictionary<Vector4, char> states)
            {
                int count = 0;

                foreach (Vector4 adjacent in AdjacentCubes(point))
                {
                    if (IsAdjacentCubeActive(adjacent, states))
                    {
                        count++;
                    }
                }

                return count;
            }

            static bool IsAdjacentCubeActive(Vector4 position, Dictionary<Vector4, char> states)
                => states.GetValueOrDefault(position) == Active;
        }

        private static IEnumerable<Vector4> AdjacentCubes(Vector4 position)
        {
            for (float z = -1; z <= 1; z++)
            {
                for (float y = -1; y <= 1; y++)
                {
                    for (float x = -1; x <= 1; x++)
                    {
                        for (float w = -1; w <= 1; w++)
                        {
                            var adjacent = new Vector4(x, y, z, w);

                            if (adjacent != Vector4.Zero)
                            {
                                yield return position + adjacent;
                            }
                        }
                    }
                }
            }
        }

        private static string WriteState(Dictionary<Vector4, char> states, ILogger? logger)
        {
            var builder = new StringBuilder();

            float maxW = states.Max((p) => p.Key.W);
            float maxY = states.Max((p) => p.Key.Y);
            float maxX = states.Max((p) => p.Key.X);
            float maxZ = states.Max((p) => p.Key.Z);
            float minW = -maxW;
            float minZ = -maxZ;

            for (float w = minW; w <= maxW; w++)
            {
                for (float z = minZ; z <= maxZ; z++)
                {
                    builder.Append("z=")
                           .Append(z)
                           .Append(", w=")
                           .Append(w)
                           .AppendLine();

                    for (float y = 0; y <= maxY; y++)
                    {
                        for (float x = 0; x <= maxX; x++)
                        {
                            var point = new Vector4(x, y, z, w);

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
    }
}
