// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/9</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2015, 09, "All in a Single Night", RequiresData = true)]
public sealed class Day09 : Puzzle
{
    /// <summary>
    /// Gets the shortest distance.
    /// </summary>
    public int ShortestDistance { get; private set; }

    /// <summary>
    /// Gets the longest distance.
    /// </summary>
    public int LongestDistance { get; private set; }

    /// <summary>
    /// Gets the distance to visit all of the specified locations exactly
    /// once and starting and ending at distinct separate points.
    /// </summary>
    /// <param name="collection">A collection of distances.</param>
    /// <param name="findLongest">Whether to find the longest distance.</param>
    /// <returns>
    /// The shortest or longest possible distance to visit all the specified locations exactly once.
    /// </returns>
    public static int GetDistanceBetweenPoints(IList<string> collection, bool findLongest)
    {
        var distances = new Dictionary<string, int>(collection.Count);
        var vectors = new (string Start, string End)[collection.Count];

        for (int i = 0; i < collection.Count; i++)
        {
            string[] split = collection[i].Split(" = ");
            string[] locations = split[0].Split(" to ");

            string start = locations[0];
            string end = locations[1];

            vectors[i] = (start, end);

            int distance = Parse<int>(split[1]);

            distances[$"{start} to {end}"] = distance;
            distances[$"{end} to {start}"] = distance;
        }

        var map = new Map(distances);

        foreach ((string start, string end) in vectors)
        {
            map.Edges.GetOrAdd(start).Add(end);
            map.Edges.GetOrAdd(end).Add(start);
        }

        var routeDistances = new List<int>();

        foreach (string location in map.Edges.Keys)
        {
            routeDistances.AddRange(GetRouteDistances(map, location));
        }

        return findLongest ? routeDistances.Max() : routeDistances.Min();
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var collection = await ReadResourceAsLinesAsync();

        ShortestDistance = GetDistanceBetweenPoints(collection, findLongest: false);
        LongestDistance = GetDistanceBetweenPoints(collection, findLongest: true);

        if (Verbose)
        {
            Logger.WriteLine("The distance of the shortest route is {0:N0}.", ShortestDistance);
            Logger.WriteLine("The distance of the longest route is {0:N0}.", LongestDistance);
        }

        return PuzzleResult.Create(ShortestDistance, LongestDistance);
    }

    private static IList<int> GetRouteDistances(Map map, string start)
    {
        var visited = new List<string>(map.Edges.Count);
        return GetRouteDistances(map, start, visited);

        static IList<int> GetRouteDistances(Map map, string location, List<string> visited)
        {
            visited.Add(location);

            if (visited.Count == map.Edges.Count)
            {
                string current = visited[0];
                int distance = 0;

                for (int i = 1; i < visited.Count; i++)
                {
                    string next = visited[i];

                    distance += map.Cost(current, next);

                    current = next;
                }

                visited.Remove(location);

                return new[] { distance };
            }

            var distances = new List<int>();

            foreach (string next in map.Neighbors(location))
            {
                if (!visited.Contains(next))
                {
                    distances.AddRange(GetRouteDistances(map, next, visited));
                }
            }

            visited.Remove(location);

            return distances;
        }
    }

    private sealed class Map : Graph<string>
    {
        private readonly Dictionary<string, int> _distances;

        public Map(Dictionary<string, int> distances)
        {
            _distances = distances;
        }

        public int Cost(string a, string b) => _distances[$"{b} to {a}"];
    }
}
