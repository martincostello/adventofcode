// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel;

namespace MartinCostello.AdventOfCode
{
    /// <summary>
    /// A class containing extension methods for <see cref="LinkedList{T}"/>. This class cannot be inherited.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class LinkedListExtensions
    {
        /// <summary>
        /// Returns the next node clockwise from the specified node.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the linked list.</typeparam>
        /// <param name="list">The linked list to get the next clockwise element from.</param>
        /// <param name="node">The node to get the next clockwise element from.</param>
        /// <returns>
        /// The node clockwise of <paramref name="node"/>.
        /// </returns>
        public static LinkedListNode<T> Clockwise<T>(this LinkedList<T> list, LinkedListNode<T> node)
            => node.Next ?? list!.First!;
    }
}
