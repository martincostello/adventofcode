// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Collections;
using System.Runtime.CompilerServices;

namespace MartinCostello.AdventOfCode;

/// <summary>
/// A class containing mathematics-related methods. This class cannot be inherited.
/// </summary>
internal static class Maths
{
    /// <summary>
    /// Returns the digits of the specified value in base 10.
    /// </summary>
    /// <param name="value">The value to get the digits for.</param>
    /// <returns>
    /// The digits of <paramref name="value"/> in base 10.
    /// </returns>
    internal static IReadOnlyList<int> Digits(int value)
    {
        if (value == 0)
        {
            return new[] { 0 };
        }

        value = Math.Abs(value);

        var digits = new List<int>();

        while (value > 0)
        {
            (int div, int rem) = Math.DivRem(value, 10);

            digits.Add(rem);
            value = div;
        }

        digits.Reverse();

        return digits;
    }

    /// <summary>
    /// Returns the number represented by the specified digits.
    /// </summary>
    /// <typeparam name="T">The type of the number.</typeparam>
    /// <param name="collection">The digits of the number.</param>
    /// <returns>
    /// The <typeparamref name="T"/> represented by the digits in <paramref name="collection"/>.
    /// </returns>
    internal static T FromDigits<T>(IList<int> collection)
        where T : INumber<T>
    {
        double result = 0;

        for (int i = 0; i < collection.Count - 1; i++)
        {
            result += collection[i] * Math.Pow(10, collection.Count - i - 1);
        }

        result += collection[collection.Count - 1];

        return T.CreateChecked(result);
    }

    /// <summary>
    /// Returns the combinations of values that add up to the specified total.
    /// </summary>
    /// <typeparam name="TTotal">The type of the total.</typeparam>
    /// <typeparam name="TValue">The type of the values.</typeparam>
    /// <param name="total">The total required for the combination(s).</param>
    /// <param name="values">The values to generate the combinations for.</param>
    /// <returns>
    /// The combinations of values whose total is the value specified by <paramref name="total"/>.
    /// </returns>
    internal static IList<ICollection<TTotal>> GetCombinations<TTotal, TValue>(TTotal total, IList<TValue> values)
        where TTotal : INumber<TTotal>
        where TValue : INumber<TValue>, IComparisonOperators<TValue, TTotal>
    {
        int length = values.Count;
        var bits = new BitArray(length);

        int limit = (int)Math.Pow(2, length);
        var result = new List<ICollection<TTotal>>(limit);

        for (int i = 0; i < limit; i++)
        {
            TValue sum = TValue.Zero;

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
                var combination = new List<TTotal>(length);

                for (int j = 0; j < length; j++)
                {
                    if (bits[j])
                    {
                        combination.Add(TTotal.CreateChecked(values[j]));
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
    /// <typeparam name="T">The type of the number.</typeparam>
    /// <param name="value">The value to get the factors for.</param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> containing the factors of the specified number.
    /// </returns>
    /// <remarks>
    /// The values returned are unsorted.
    /// </remarks>
    internal static IEnumerable<T> GetFactorsUnordered<T>(T value)
        where T : INumber<T>
    {
        for (T i = T.One; i * i <= value; i++)
        {
            if (value % i == T.Zero)
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
            .SelectMany((p) => collection.Except(p), (set, value) => set.Append(value));
    }

    /// <summary>
    /// Returns the Greatest Common Divisor of the two specified numbers.
    /// </summary>
    /// <typeparam name="T">The type of the numbers.</typeparam>
    /// <param name="a">The first number.</param>
    /// <param name="b">The second number.</param>
    /// <returns>
    /// The greatest common divisor of <paramref name="a"/> and <paramref name="b"/>.
    /// </returns>
    internal static T GreatestCommonDivisor<T>(T a, T b)
        where T : INumber<T>
    {
        while (b != T.Zero)
        {
            T x = b;
            b = a % b;
            a = x;
        }

        return a;
    }

    /// <summary>
    /// Returns the Lowest Common Multiple of the two specified numbers.
    /// </summary>
    /// <typeparam name="T">The type of the numbers.</typeparam>
    /// <param name="a">The first number.</param>
    /// <param name="b">The second number.</param>
    /// <returns>
    /// The lowest common multiple of <paramref name="a"/> and <paramref name="b"/>.
    /// </returns>
    internal static T LowestCommonMultiple<T>(T a, T b)
        where T : INumber<T>
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
