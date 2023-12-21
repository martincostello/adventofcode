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
    /// <param name="goalComparer">The optional equality comparer to use for the goal.</param>
    /// <param name="cancellationToken">The optional <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The minimum cost to traverse the graph from <paramref name="start"/> to <paramref name="goal"/>.
    /// </returns>
    public static long AStar<T>(
        IWeightedGraph<T> graph,
        T start,
        T goal,
        IEqualityComparer<T>? goalComparer = null,
        CancellationToken cancellationToken = default)
        where T : notnull
        => AStar(graph, start, goal, graph, goalComparer ?? graph, graph.Cost, cancellationToken);

    /// <summary>
    /// Finds the cheapest path between two nodes of the specified graph.
    /// </summary>
    /// <typeparam name="T">The type of the graph's nodes.</typeparam>
    /// <param name="graph">A graph of nodes.</param>
    /// <param name="start">The starting node.</param>
    /// <param name="goal">The goal node to find a path to.</param>
    /// <param name="heuristic">A heuristic to determine the cost of moving from one node to another.</param>
    /// <param name="cancellationToken">The optional <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The minimum cost to traverse the graph from <paramref name="start"/> to <paramref name="goal"/>.
    /// </returns>
    public static long AStar<T>(
        IGraph<T> graph,
        T start,
        T goal,
        Func<T, T, long> heuristic,
        CancellationToken cancellationToken = default)
        where T : notnull
        => AStar(graph, start, goal, EqualityComparer<T>.Default, heuristic, cancellationToken);

    /// <summary>
    /// Finds the cheapest path between two nodes of the specified graph.
    /// </summary>
    /// <typeparam name="T">The type of the graph's nodes.</typeparam>
    /// <param name="graph">A graph of nodes.</param>
    /// <param name="start">The starting node.</param>
    /// <param name="goal">The goal node to find a path to.</param>
    /// <param name="comparer">The equality comparer to use between nodes.</param>
    /// <param name="heuristic">A heuristic to determine the cost of moving from one node to another.</param>
    /// <param name="cancellationToken">The optional <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The minimum cost to traverse the graph from <paramref name="start"/> to <paramref name="goal"/>.
    /// </returns>
    public static long AStar<T>(
        IGraph<T> graph,
        T start,
        T goal,
        IEqualityComparer<T> comparer,
        Func<T, T, long> heuristic,
        CancellationToken cancellationToken = default)
        where T : notnull
        => AStar(graph, start, goal, comparer, comparer, heuristic, cancellationToken);

    /// <summary>
    /// Finds the cheapest path between two nodes of the specified graph.
    /// </summary>
    /// <typeparam name="T">The type of the graph's nodes.</typeparam>
    /// <param name="graph">A graph of nodes.</param>
    /// <param name="start">The starting node.</param>
    /// <param name="goal">The goal node to find a path to.</param>
    /// <param name="itemComparer">The equality comparer to use between nodes.</param>
    /// <param name="goalComparer">The equality comparer to use for the goal.</param>
    /// <param name="heuristic">A heuristic to determine the cost of moving from one node to another.</param>
    /// <param name="cancellationToken">The optional <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The minimum cost to traverse the graph from <paramref name="start"/> to <paramref name="goal"/>.
    /// </returns>
    public static long AStar<T>(
        IGraph<T> graph,
        T start,
        T goal,
        IEqualityComparer<T> itemComparer,
        IEqualityComparer<T> goalComparer,
        Func<T, T, long> heuristic,
        CancellationToken cancellationToken = default)
        where T : notnull
    {
        var frontier = new PriorityQueue<T, long>();
        frontier.Enqueue(start, 0);

        var costSoFar = new Dictionary<T, long>(itemComparer) { [start] = 0 };

        while (frontier.Count != 0 && !cancellationToken.IsCancellationRequested)
        {
            T current = frontier.Dequeue();

            if (goalComparer.Equals(current, goal))
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

        cancellationToken.ThrowIfCancellationRequested();

        return costSoFar.GetValueOrDefault(goal, long.MaxValue);
    }

    /// <summary>
    /// Performs a breadth-first search of the specified graph.
    /// </summary>
    /// <typeparam name="T">The type of the nodes of the graph.</typeparam>
    /// <param name="graph">The graph to search.</param>
    /// <param name="start">The starting node.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// An <see cref="IReadOnlySet{T}"/> of the visited nodes.
    /// </returns>
    public static IReadOnlySet<T> BreadthFirst<T>(IGraph<T> graph, T start, CancellationToken cancellationToken)
        where T : notnull
        => BreadthFirst(graph.Neighbors, start, cancellationToken);

    /// <summary>
    /// Performs a breadth-first search of the specified graph.
    /// </summary>
    /// <typeparam name="T">The type of the nodes of the graph.</typeparam>
    /// <param name="neighbors">A delegate to a method that can be used to get the neighbours of a node.</param>
    /// <param name="start">The starting node.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// An <see cref="IReadOnlySet{T}"/> of the visited nodes.
    /// </returns>
    public static IReadOnlySet<T> BreadthFirst<T>(
        Func<T, IEnumerable<T>> neighbors,
        T start,
        CancellationToken cancellationToken)
        where T : notnull
        => BreadthFirst(neighbors, start, EqualityComparer<T>.Default, cancellationToken);

    /// <summary>
    /// Performs a breadth-first search of the specified graph.
    /// </summary>
    /// <typeparam name="T">The type of the nodes of the graph.</typeparam>
    /// <param name="neighbors">A delegate to a method that can be used to get the neighbours of a node.</param>
    /// <param name="start">The starting node.</param>
    /// <param name="comparer">The equality comparer to use between nodes.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// An <see cref="IReadOnlySet{T}"/> of the visited nodes.
    /// </returns>
    public static IReadOnlySet<T> BreadthFirst<T>(
        Func<T, IEnumerable<T>> neighbors,
        T start,
        IEqualityComparer<T> comparer,
        CancellationToken cancellationToken)
        where T : notnull
    {
        var frontier = new Queue<T>();
        frontier.Enqueue(start);

        var reached = new HashSet<T>(comparer) { start };

        while (frontier.Count != 0 && !cancellationToken.IsCancellationRequested)
        {
            T current = frontier.Dequeue();

            foreach (T next in neighbors(current))
            {
                if (!reached.Contains(next))
                {
                    frontier.Enqueue(next);
                    reached.Add(next);
                }
            }
        }

        cancellationToken.ThrowIfCancellationRequested();

        return reached;
    }

    /// <summary>
    /// Performs a depth-first search of the specified graph.
    /// </summary>
    /// <typeparam name="T">The type of the nodes of the graph.</typeparam>
    /// <param name="graph">The graph to search.</param>
    /// <param name="start">The starting node.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// An <see cref="IReadOnlySet{T}"/> of the visited nodes.
    /// </returns>
    public static IReadOnlySet<T> DepthFirst<T>(IGraph<T> graph, T start, CancellationToken cancellationToken)
        where T : notnull
        => DepthFirst(graph.Neighbors, start, EqualityComparer<T>.Default, cancellationToken);

    /// <summary>
    /// Performs a depth-first search of the specified graph.
    /// </summary>
    /// <typeparam name="T">The type of the nodes of the graph.</typeparam>
    /// <param name="neighbors">A delegate to a method that can be used to get the neighbours of a node.</param>
    /// <param name="start">The starting node.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// An <see cref="IReadOnlySet{T}"/> of the visited nodes.
    /// </returns>
    public static IReadOnlySet<T> DepthFirst<T>(
        Func<T, IEnumerable<T>> neighbors,
        T start,
        CancellationToken cancellationToken)
        where T : notnull
        => DepthFirst(neighbors, start, EqualityComparer<T>.Default, cancellationToken);

    /// <summary>
    /// Performs a depth-first search of the specified graph.
    /// </summary>
    /// <typeparam name="T">The type of the nodes of the graph.</typeparam>
    /// <param name="neighbors">A delegate to a method that can be used to get the neighbours of a node.</param>
    /// <param name="start">The starting node.</param>
    /// <param name="comparer">The equality comparer to use between nodes.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// An <see cref="IReadOnlySet{T}"/> of the visited nodes.
    /// </returns>
    public static IReadOnlySet<T> DepthFirst<T>(
        Func<T, IEnumerable<T>> neighbors,
        T start,
        IEqualityComparer<T> comparer,
        CancellationToken cancellationToken)
        where T : notnull
    {
        var visited = new HashSet<T>(comparer);

        var frontier = new Stack<T>();
        frontier.Push(start);

        while (frontier.Count != 0 && !cancellationToken.IsCancellationRequested)
        {
            var current = frontier.Pop();

            if (!visited.Add(current))
            {
                continue;
            }

            foreach (var neighbour in neighbors(current))
            {
                if (visited.Contains(neighbour))
                {
                    continue;
                }

                frontier.Push(neighbour);
            }
        }

        cancellationToken.ThrowIfCancellationRequested();

        return visited;
    }
}
