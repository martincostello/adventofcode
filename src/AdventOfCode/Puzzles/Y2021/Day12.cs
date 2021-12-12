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
    /// Gets the number of paths through the cave system that only visit small caves once.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Navigates the specified cave system.
    /// </summary>
    /// <param name="nodes">The nodes of the cave system.</param>
    /// <returns>
    /// The number of paths through the cave system that only visit small caves once.
    /// </returns>
    public static int Navigate(IList<string> nodes)
    {
        var caves = new CaveSystem();

        foreach (string node in nodes)
        {
            string[] pair = node.Split('-');

            string start = pair[0];
            string end = pair[1];

            var edges = caves.Edges.GetOrAdd(start, () => new List<string>());
            edges.Add(end);

            edges = caves.Edges.GetOrAdd(end, () => new List<string>());
            edges.Add(start);
        }

        return CountPaths(caves);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> nodes = await ReadResourceAsLinesAsync();

        Count = Navigate(nodes);

        if (Verbose)
        {
            Logger.WriteLine(
                "There are {0:N0} paths through the cave system that visit small caves once at most.",
                Count);
        }

        return PuzzleResult.Create(Count);
    }

    private static int CountPaths(CaveSystem caves)
    {
        var used = new Dictionary<string, int>();

        return CountPaths(caves, "start", "end", used);

        static int CountPaths(CaveSystem graph, string current, string goal, Dictionary<string, int> used)
        {
            used.AddOrIncrement(current, 1);

            if (current == goal)
            {
                used.Remove(current);
                return 1;
            }

            int count = 0;

            foreach (string next in graph.Neighbors(current))
            {
                bool isUpper = string.Equals(next, next.ToUpperInvariant(), StringComparison.Ordinal);

                if (isUpper || used.GetValueOrDefault(next) < 1)
                {
                    count += CountPaths(graph, next, goal, used);
                }
            }

            used[current]--;

            return count;
        }
    }

    private class CaveSystem : Graph<string>, IWeightedGraph<string>
    {
        public double Cost(string a, string b) =>
            string.Equals(b.ToUpperInvariant(), b, StringComparison.Ordinal) ? double.PositiveInfinity : 1;

        IEnumerable<string> IWeightedGraph<string>.Neighbors(string id) => Neighbors(id);
    }
}
