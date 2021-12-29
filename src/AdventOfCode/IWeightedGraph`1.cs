// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode;

/// <summary>
/// Defines a weighted graph of nodes.
/// </summary>
/// <typeparam name="T">The type of the nodes.</typeparam>
public interface IWeightedGraph<T> : IGraph<T>
    where T : notnull
{
    /// <summary>
    /// Gets the cost of moving from one node to another.
    /// </summary>
    /// <param name="a">The source node.</param>
    /// <param name="b">The destination node.</param>
    /// <returns>
    /// The cost of moving from node <paramref name="a"/> to node <paramref name="b"/>.
    /// </returns>
    long Cost(T a, T b);
}
