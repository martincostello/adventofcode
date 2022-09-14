// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode;

/// <summary>
/// A class containing path-finding algorithms. This class cannot be inherited.
/// </summary>
/// <remarks>
/// Based on <c>https://www.redblobgames.com/pathfinding/a-star/implementation.html</c>.
/// </remarks>
public static class PathFinding
{
    /// <summary>
    /// Finds the cheapest path between two nodes of the specified graph.
    /// </summary>
    /// <typeparam name="T">The type of the graph's nodes.</typeparam>
    /// <param name="graph">A graph of nodes.</param>
    /// <param name="start">The starting node.</param>
    /// <param name="goal">The goal node to find a path to.</param>
    /// <param name="heuristic">An optional heuristic to determine the cost of moving from one node to another.</param>
    /// <returns>
    /// The minimum cost to traverse the graph from <paramref name="start"/> to <paramref name="goal"/>.
    /// </returns>
    public static long AStar<T>(IWeightedGraph<T> graph, T start, T goal, Func<T, T, long>? heuristic = default)
        where T : notnull
    {
        heuristic ??= graph.Cost;

        var frontier = new PriorityQueue<T, long>();
        frontier.Enqueue(start, 0);

        var costSoFar = new Dictionary<T, long>() { [start] = 0 };

        while (frontier.Count > 0)
        {
            T current = frontier.Dequeue();

            if (current.Equals(goal))
            {
                break;
            }

            foreach (T next in graph.Neighbors(current))
            {
                long newCost = costSoFar[current] + heuristic(current, next);

                if (!costSoFar.TryGetValue(next, out long otherCost) || newCost < otherCost)
                {
                    costSoFar[next] = newCost;

                    long goalCost = heuristic(next, goal);
                    long priority = newCost + goalCost;

                    frontier.Enqueue(next, priority);
                }
            }
        }

        return costSoFar.GetValueOrDefault(goal, long.MaxValue);
    }

    /// <summary>
    /// Performs a breadth-first search of the specified graph.
    /// </summary>
    /// <typeparam name="T">The type of the nodes of the graph.</typeparam>
    /// <param name="graph">The graph to search.</param>
    /// <param name="start">The starting node.</param>
    /// <returns>
    /// An <see cref="IReadOnlySet{T}"/> of the visited nodes.
    /// </returns>
    public static IReadOnlySet<T> BreadthFirst<T>(IGraph<T> graph, T start)
        where T : notnull
    {
        var frontier = new Queue<T>();
        frontier.Enqueue(start);

        var reached = new HashSet<T>() { start };

        while (frontier.Count > 0)
        {
            T current = frontier.Dequeue();

            foreach (T next in graph.Neighbors(current))
            {
                if (!reached.Contains(next))
                {
                    frontier.Enqueue(next);
                    reached.Add(next);
                }
            }
        }

        return reached;
    }
}
