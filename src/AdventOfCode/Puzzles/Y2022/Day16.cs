// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/16</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 16, "Proboscidea Volcanium", RequiresData = true)]
public sealed class Day16 : Puzzle
{
    /// <summary>
    /// Gets the most pressure that can be released.
    /// </summary>
    public int MaximumPressure { get; private set; }

    /// <summary>
    /// Returns the most pressure that can be released from the values in the specified report.
    /// </summary>
    /// <param name="report">The lines to the report to determine the pressure from.</param>
    /// <returns>
    /// The most pressure that can be released.
    /// </returns>
    public static int GetMaximumPressure(IList<string> report)
    {
        var network = Parse(report);

        var pressuresReleased = new List<long>();

        /*
        foreach (string location in network.Edges.Keys)
        {
            pressuresReleased.AddRange(GetRouteDistances(network, location));
        }
        */

        pressuresReleased.AddRange(GetRouteDistances(network, "DD"));

        return (int)pressuresReleased.Max();

        static PipeNetwork Parse(IList<string> report)
        {
            var network = new PipeNetwork();

            foreach (string line in report)
            {
                string[] split = line.Split(';');

                string valve = split[0].Split(' ')[1];
                string rate = split[0].Split('=')[1];

                int index = split[1].IndexOf("tunnel leads to valve ", StringComparison.Ordinal);

                string valves;

                if (index > -1)
                {
                    valves = split[1][^2..];
                }
                else
                {
                    valves = split[1][" tunnels lead to valves ".Length..];
                }

                network.Edges[valve] = new(valves.Split(", "));
                network.FlowRates[valve] = Parse<int>(rate);
            }

            return network;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync();

        MaximumPressure = GetMaximumPressure(values);

        if (Verbose)
        {
            Logger.WriteLine("The most pressure you can release is {0}.", MaximumPressure);
        }

        return PuzzleResult.Create(MaximumPressure);
    }

    private static IList<long> GetRouteDistances(PipeNetwork map, string start)
    {
        var visited = new List<string>(map.Edges.Count);
        return GetRouteDistances(map, start, visited);

        static IList<long> GetRouteDistances(PipeNetwork map, string location, List<string> visited)
        {
            visited.Add(location);

            if (visited.Count == map.Edges.Count)
            {
                string current = visited[0];
                long pressureReleased = map.FlowRates[current] * 28;

                for (int i = 1; i < visited.Count; i++)
                {
                    string next = visited[i];

                    pressureReleased += map.Cost(current, next) * (28 - (i * 2));

                    current = next;
                }

                visited.Remove(location);

                return new[] { pressureReleased };
            }

            var distances = new List<long>();

            foreach (string next in map.Neighbors(location))
            {
                if (!visited.Contains(next))
                {
                    distances.AddRange(GetRouteDistances(map, next, visited));
                }
            }

            if (distances.Count == 0)
            {
                string current = visited[0];
                long pressureReleased = map.FlowRates[current] * 28;

                for (int i = 1; i < visited.Count; i++)
                {
                    string next = visited[i];

                    pressureReleased += map.Cost(current, next) * (28 - (i * 2));

                    current = next;
                }

                visited.Remove(location);

                return new[] { pressureReleased };
            }

            visited.Remove(location);

            return distances;
        }
    }

    private sealed class PipeNetwork : Graph<string>, IWeightedGraph<string>
    {
        /// <summary>
        /// Gets the flow rates for the valves.
        /// </summary>
        public Dictionary<string, int> FlowRates { get; } = new();

        /// <inheritdoc/>
        public long Cost(string a, string b) => FlowRates[b];
    }
}
