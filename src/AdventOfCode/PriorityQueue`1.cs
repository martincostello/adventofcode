// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace MartinCostello.AdventOfCode
{
    /// <summary>
    /// A class representing a priority-based queue. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T">The type of the queue's elements.</typeparam>
    /// <remarks>
    /// Based on <c>https://www.redblobgames.com/pathfinding/a-star/implementation.html</c>.
    /// </remarks>
#pragma warning disable CA1711
    public sealed class PriorityQueue<T>
#pragma warning restore CA1711
    {
        /// <summary>
        /// The elements that back the queue. This field is read-only.
        /// </summary>
        private readonly List<(T item, double priority)> _elements = new List<(T item, double priority)>();

        /// <summary>
        /// Gets the number of items in the queue.
        /// </summary>
        public int Count => _elements.Count;

        /// <summary>
        /// Adds a new item to the queue.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="priority">The priority of the item.</param>
        public void Enqueue(T item, double priority)
            => _elements.Add((item, priority));

        /// <summary>
        /// Dequeues an item from the queue.
        /// </summary>
        /// <returns>
        /// The dequeued item.
        /// </returns>
        public T Dequeue()
        {
            int bestIndex = 0;

            for (int i = 0; i < _elements.Count; i++)
            {
                if (_elements[i].priority < _elements[bestIndex].priority)
                {
                    bestIndex = i;
                }
            }

            T bestItem = _elements[bestIndex].item;

            _elements.RemoveAt(bestIndex);

            return bestItem;
        }
    }
}
