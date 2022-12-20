// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace MartinCostello.AdventOfCode;

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

    /// <summary>
    /// Moves the specified node the specified number of positions.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the linked list.</typeparam>
    /// <param name="list">The linked list to move the next element within.</param>
    /// <param name="value">The value of the node to move.</param>
    /// <param name="count">The number of steps left (negative) or right to move the node.</param>
    public static void Move<T>(this LinkedList<T> list, T value, int count)
    {
        bool shitLeft = count < 0;
        count = Math.Abs(count) % list.Count;

        if (count == 0)
        {
            return;
        }

        var source = list.Find(value)!;
        LinkedListNode<T> destination = source;

        Func<LinkedListNode<T>> shift =
            shitLeft ?
            () => destination.Previous ?? list.Last! :
            () => destination.Next ?? list.First!;

        for (int i = 0; i < count; i++)
        {
            destination = shift();
        }

        bool shouldWrap = (shitLeft ? destination.Previous : destination.Next) is null;

        list.Remove(source);

        if (shitLeft)
        {
            if (shouldWrap)
            {
                list.AddLast(source);
            }
            else
            {
                list.AddBefore(destination, source);
            }
        }
        else
        {
            if (shouldWrap)
            {
                list.AddFirst(source);
            }
            else
            {
                list.AddAfter(destination, source);
            }
        }
    }
}
