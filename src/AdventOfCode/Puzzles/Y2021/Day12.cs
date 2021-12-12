// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/12</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 12, RequiresData = true)]
public sealed class Day12 : Puzzle
{
    /// <summary>
    /// Gets the number of paths through the cave system that visit one small cave once.
    /// </summary>
    public int Count1 { get; private set; }

    /// <summary>
    /// Gets the number of paths through the cave system that visit one small cave twice.
    /// </summary>
    public int Count2 { get; private set; }

    /// <summary>
    /// Navigates the specified cave system.
    /// </summary>
    /// <param name="nodes">The nodes of the cave system.</param>
    /// <param name="smallCaveVisitLimit">The limit on the maximum number of times a single small cave can be visited.</param>
    /// <returns>
    /// The number of paths through the cave system that only visit one small cave
    /// the number of times specified by <paramref name="smallCaveVisitLimit"/>.
    /// </returns>
    public static int Navigate(IList<string> nodes, int smallCaveVisitLimit)
    {
        var caves = new CaveSystem();

        foreach (string node in nodes)
        {
            string[] pair = node.Split('-');

            string start = pair[0];
            string end = pair[1];

            caves.Edges.GetOrAdd(start).Add(end);
            caves.Edges.GetOrAdd(end).Add(start);
        }

        return CountPaths(caves, smallCaveVisitLimit);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> nodes = await ReadResourceAsLinesAsync();

        Count1 = Navigate(nodes, smallCaveVisitLimit: 1);
        Count2 = Navigate(nodes, smallCaveVisitLimit: 2);

        if (Verbose)
        {
            Logger.WriteLine(
                "There are {0:N0} paths through the cave system that visit one small cave once at most.",
                Count1);

            Logger.WriteLine(
                "There are {0:N0} paths through the cave system that visit one small cave twice at most.",
                Count2);
        }

        return PuzzleResult.Create(Count1, Count2);
    }

    private static int CountPaths(CaveSystem caves, int smallCaveVisitLimit)
    {
        var visited = caves.Edges.ToDictionary((p) => p.Key, (_) => 0);
        bool allowOneDoubleVisit = smallCaveVisitLimit == 2;

        return CountPaths(caves, "start", "end", visited, allowOneDoubleVisit);

        static int CountPaths(
            CaveSystem caves,
            string current,
            string goal,
            Dictionary<string, int> visited,
            bool allowOneDoubleVisit)
        {
            if (current == goal)
            {
                return 1;
            }

            visited[current]++;

            int count = 0;

            foreach (string next in caves.Neighbors(current))
            {
                // Never return to the first cave
                if (string.Equals(next, "start", StringComparison.Ordinal))
                {
                    continue;
                }

                bool isUpper = char.IsUpper(next[0]);

                if (isUpper)
                {
                    count += CountPaths(caves, next, goal, visited, allowOneDoubleVisit);
                }
                else
                {
                    int visits = visited[next];

                    if (visits < 1)
                    {
                        count += CountPaths(caves, next, goal, visited, allowOneDoubleVisit);
                    }
                    else if (allowOneDoubleVisit)
                    {
                        count += CountPaths(caves, next, goal, visited, false);
                    }
                }
            }

            visited[current]--;

            return count;
        }
    }

    private class CaveSystem : Graph<string>, IWeightedGraph<string>
    {
        public double Cost(string a, string b) =>
            char.IsUpper(b[0]) ? double.PositiveInfinity : 1;

        IEnumerable<string> IWeightedGraph<string>.Neighbors(string id) => Neighbors(id);
    }
}
