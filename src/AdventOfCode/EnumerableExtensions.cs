// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode;

/// <summary>
/// A class containing extensions for <see cref="IEnumerable{T}"/>. This class cannot be inherited.
/// </summary>
internal static class EnumerableExtensions
{
    /// <summary>
    /// Returns whether the count of the specified collection is exactly the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    /// <param name="collection">The collection to count the number of items in.</param>
    /// <param name="value">The value to compare the collection's count to.</param>
    /// <returns>
    /// <see langword="true"/> if the count of items in <paramref name="collection"/> is
    /// <paramref name="value"/>; otherwise <see langword="false"/>.
    /// </returns>
    public static bool ExactCount<T>(this IEnumerable<T> collection, int value)
    {
        if (collection.TryGetNonEnumeratedCount(out int count))
        {
            return count == value;
        }

        count = 0;

        foreach (T item in collection)
        {
            if (++count > value)
            {
                return false;
            }
        }

        return count == value;
    }

    /// <summary>
    /// Enumerates the specified source and returns the items from it in pairs.
    /// </summary>
    /// <typeparam name="TSource">The type of the source elements.</typeparam>
    /// <typeparam name="TResult">The type of the result pairs.</typeparam>
    /// <param name="source">The source to enumerate for pairs.</param>
    /// <param name="selector">A delegate that selects a value from two pairs.</param>
    /// <returns>
    /// The results enumerated from the pairs in the source.
    /// </returns>
    public static IEnumerable<TResult> Pairwise<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, TSource, TResult> selector)
    {
        using var enumerator = source.GetEnumerator();

        if (!enumerator.MoveNext())
        {
            yield break;
        }

        var previous = enumerator.Current;
        while (enumerator.MoveNext())
        {
            yield return selector(previous, enumerator.Current);
            previous = enumerator.Current;
        }
    }
}
