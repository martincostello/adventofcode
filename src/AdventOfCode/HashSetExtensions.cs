// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace System.Collections.Generic;

/// <summary>
/// A class containing extension methods for <see cref="HashSet{T}"/>. This class cannot be inherited.
/// </summary>
public static class HashSetExtensions
{
    /// <summary>
    /// Modifies the specified <see cref="HashSet{T}"/> object to contain only elements
    /// that are present in that object and in the specified collection.
    /// </summary>
    /// <typeparam name="T">The type of the items in the set.</typeparam>
    /// <param name="set">The <see cref="HashSet{T}"/> to modify.</param>
    /// <param name="other">The collection to compare to the current <see cref="HashSet{T}"/> object.</param>
    public static void And<T>(this HashSet<T> set, IEnumerable<T> other) => set.IntersectWith(other);

    /// <summary>
    /// Removes all elements in the specified collection from the current <see cref="HashSet{T}"/> object.
    /// </summary>
    /// <typeparam name="T">The type of the items in the set.</typeparam>
    /// <param name="set">The <see cref="HashSet{T}"/> to modify.</param>
    /// <param name="other">The collection to compare to the current <see cref="HashSet{T}"/> object.</param>
    public static void Not<T>(this HashSet<T> set, IEnumerable<T> other) => set.ExceptWith(other);

    /// <summary>
    /// Modifies the current <see cref="HashSet{T}"/> object to contain all elements
    /// that are present in itself, the specified collection, or both.
    /// </summary>
    /// <typeparam name="T">The type of the items in the set.</typeparam>
    /// <param name="set">The <see cref="HashSet{T}"/> to modify.</param>
    /// <param name="other">The collection to compare to the current <see cref="HashSet{T}"/> object.</param>
    public static void Or<T>(this HashSet<T> set, IEnumerable<T> other) => set.UnionWith(other);

    /// <summary>
    /// Modifies the current <see cref="HashSet{T}"/> object to contain only elements
    /// that are present either in that object or in the specified collection, but not both.
    /// </summary>
    /// <typeparam name="T">The type of the items in the set.</typeparam>
    /// <param name="set">The <see cref="HashSet{T}"/> to modify.</param>
    /// <param name="other">The collection to compare to the current <see cref="HashSet{T}"/> object.</param>
    public static void Xor<T>(this HashSet<T> set, IEnumerable<T> other) => set.SymmetricExceptWith(other);
}
