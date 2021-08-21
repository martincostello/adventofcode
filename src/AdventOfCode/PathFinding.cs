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
    /// <param name="heuristic">A heuristic to determine the cost of moving from one node to another.</param>
    /// <returns>
    /// The minimum cost to traverse the graph from <paramref name="start"/> to <paramref name="goal"/>.
    /// </returns>
    public static double AStar<T>(IWeightedGraph<T> graph, T start, T goal, Func<T, T, double> heuristic)
        where T : notnull
    {
        var frontier = new PriorityQueue<T, double>();
        frontier.Enqueue(start, 0);

        var cameFrom = new Dictionary<T, T>() { [start] = start };
        var costSoFar = new Dictionary<T, double>() { [start] = 0 };

        while (frontier.Count > 0)
        {
            T current = frontier.Dequeue();

            if (current.Equals(goal))
            {
                break;
            }

            foreach (T next in graph.Neighbors(current))
            {
                double newCost = costSoFar[current] + graph.Cost(current, next);

                if (!costSoFar.TryGetValue(next, out double otherCost) || newCost < otherCost)
                {
                    costSoFar[next] = newCost;

                    double priority = newCost + heuristic(next, goal);

                    frontier.Enqueue(next, priority);

                    cameFrom[next] = current;
                }
            }
        }

        if (!costSoFar.TryGetValue(goal, out double minimumCost))
        {
            minimumCost = double.NaN;
        }

        return minimumCost;
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
    public static IReadOnlySet<T> BreadthFirst<T>(Graph<T> graph, T start)
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
