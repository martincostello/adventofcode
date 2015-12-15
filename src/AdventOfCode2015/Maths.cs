// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class containing mathematics-related methods. This class cannot be inherited.
    /// </summary>
    internal static class Maths
    {
        /// <summary>
        /// Returns all the permutations of the specified collection of values.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="collection">The collection to get the permutations of.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that returns the permutations of <paramref name="collection"/>.
        /// </returns>
        internal static IEnumerable<IEnumerable<T>> GetPermutations<T>(ICollection<T> collection)
        {
            return GetPermutations(collection, collection.Count);
        }

        /// <summary>
        /// Returns all the permutations of the specified collection of strings.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="collection">The collection to get the permutations of.</param>
        /// <param name="count">The number of items to calculate the permutations from.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that returns the permutations of <paramref name="collection"/>.
        /// </returns>
        internal static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> collection, int count)
        {
            if (count == 1)
            {
                return collection.Select((p) => new[] { p });
            }

            return GetPermutations(collection, count - 1)
                .SelectMany((p) => collection.Where((r) => !p.Contains(r)), (set, value) => set.Concat(new[] { value }));
        }
    }
}
