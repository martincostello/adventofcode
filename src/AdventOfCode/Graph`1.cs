// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace MartinCostello.AdventOfCode
{
    /// <summary>
    /// A class representing a graph of a nodes. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T">The type of the nodes.</typeparam>
    /// <remarks>
    /// Based on <c>https://www.redblobgames.com/pathfinding/a-star/implementation.html</c>.
    /// </remarks>
    public sealed class Graph<T>
        where T : notnull
    {
        /// <summary>
        /// Gets the edges of the graph.
        /// </summary>
        public IDictionary<T, IList<T>> Edges { get; } = new Dictionary<T, IList<T>>();

        /// <summary>
        /// Returns the neighbors of the specified node.
        /// </summary>
        /// <param name="id">The Id of the node.</param>
        /// <returns>
        /// The neigbors of the specified node.
        /// </returns>
        public IList<T> Neighbors(T id) => Edges[id];
    }
}
