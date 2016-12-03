// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class containing mathematics-related methods. This class cannot be inherited.
    /// </summary>
    internal static class Maths
    {
        /// <summary>
        /// Returns the 32-bit integer represented by the specified digits.
        /// </summary>
        /// <param name="collection">The digits of the number.</param>
        /// <returns>
        /// The <see cref="int"/> represented by the digits in <paramref name="collection"/>.
        /// </returns>
        internal static int FromDigits(IList<int> collection)
        {
            double result = 0;

            for (int j = 0; j < collection.Count - 1; j++)
            {
                result += collection[j] * Math.Pow(10, collection.Count - j - 1);
            }

            result += collection[collection.Count - 1];

            return (int)result;
        }

        /// <summary>
        /// Returns the combinations of values that add up to the specified total.
        /// </summary>
        /// <param name="total">The total required for the combination(s).</param>
        /// <param name="values">The values to generate the combinations for.</param>
        /// <returns>
        /// The combinations of values whose total is the value specified by <paramref name="total"/>.
        /// </returns>
        internal static IList<ICollection<long>> GetCombinations(long total, IList<int> values)
        {
            List<ICollection<long>> result = new List<ICollection<long>>();

            BitArray bits = new BitArray(values.Count);

            for (int i = 0; i < Math.Pow(2, bits.Length); i++)
            {
                int sum = 0;

                for (int j = 0; j < bits.Length; j++)
                {
                    if (bits[j])
                    {
                        sum += values[j];
                    }
                }

                if (sum == total)
                {
                    List<long> combination = new List<long>();

                    for (int j = 0; j < bits.Length; j++)
                    {
                        if (bits[j])
                        {
                            combination.Add(values[j]);
                        }
                    }

                    result.Add(combination);
                }

                Increment(bits);
            }

            return result;
        }

        /// <summary>
        /// Returns the factors of the specified number.
        /// </summary>
        /// <param name="value">The value to get the factors for.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> containing the factors of the specified number.
        /// </returns>
        /// <remarks>
        /// The values returned are unsorted.
        /// </remarks>
        internal static IEnumerable<int> GetFactorsUnordered(int value)
        {
            for (int i = 1; i * i <= value; i++)
            {
                if (value % i == 0)
                {
                    yield return i;

                    if (i * i != value)
                    {
                        yield return value / i;
                    }
                }
            }
        }

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
        /// Returns all the permutations of the specified collection of values.
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

        /// <summary>
        /// Increments the value of the specified <see cref="BitArray"/>.
        /// </summary>
        /// <param name="value">The value to increment.</param>
        private static void Increment(BitArray value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                bool previous = value[i];
                value[i] = !previous;

                if (!previous)
                {
                    return;
                }
            }
        }
    }
}
