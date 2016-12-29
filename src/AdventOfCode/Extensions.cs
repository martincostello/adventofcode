// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    using System.Collections.Generic;

    /// <summary>
    /// A class containing extension methods. This class cannot be inherited.
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Returns the next node in a circular linked list.
        /// </summary>
        /// <typeparam name="T">The type of the items in the linked list.</typeparam>
        /// <param name="node">The node to find the next node for.</param>
        /// <returns>
        /// The next node from <paramref name="node"/> if the linked list is circular.
        /// </returns>
        internal static LinkedListNode<T> NextCircular<T>(this LinkedListNode<T> node)
        {
            return node.Next ?? node.List.First;
        }
    }
}
