// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MartinCostello.AdventOfCode
{
    /// <summary>
    /// A class containing mathematics-related methods. This class cannot be inherited.
    /// </summary>
    internal static class Maths
    {
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
            int length = values.Count;
            var bits = new BitArray(length);

            int limit = (int)Math.Pow(2, length);
            var result = new List<ICollection<long>>(limit);

            for (int i = 0; i < limit; i++)
            {
                int sum = 0;

                for (int j = 0; j < length; j++)
                {
                    if (bits[j])
                    {
                        sum += values[j];

                        if (sum > total)
                        {
                            break;
                        }
                    }
                }

                if (sum == total)
                {
                    var combination = new List<long>(length);

                    for (int j = 0; j < length; j++)
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
            => GetPermutations(collection, collection.Count);

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
                .SelectMany((p) => collection.Where((r) => !p.Contains(r)), (set, value) => set.Append(value));
        }

        /// <summary>
        /// Returns the Greatest Common Divisor of the two specified numbers.
        /// </summary>
        /// <param name="a">The first number.</param>
        /// <param name="b">The second number.</param>
        /// <returns>
        /// The greatest common divisor of <paramref name="a"/> and <paramref name="b"/>.
        /// </returns>
        internal static long GreatestCommonDivisor(long a, long b)
        {
            while (b != 0)
            {
                long x = b;
                b = a % b;
                a = x;
            }

            return a;
        }

        /// <summary>
        /// Returns the Lowest Common Multiple of the two specified numbers.
        /// </summary>
        /// <param name="a">The first number.</param>
        /// <param name="b">The second number.</param>
        /// <returns>
        /// The lowest common multiple of <paramref name="a"/> and <paramref name="b"/>.
        /// </returns>
        internal static long LowestCommonMultiple(long a, long b)
            => a / GreatestCommonDivisor(a, b) * b;

        /// <summary>
        /// Increments the value of the specified <see cref="BitArray"/>.
        /// </summary>
        /// <param name="value">The value to increment.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Increment(BitArray value)
        {
            int length = value.Length;

            for (int i = 0; i < length; i++)
            {
                bool previous = value.Get(i);
                value.Set(i, !previous);

                if (!previous)
                {
                    return;
                }
            }
        }
    }
}
