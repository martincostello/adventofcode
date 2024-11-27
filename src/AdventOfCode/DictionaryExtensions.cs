// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MartinCostello.AdventOfCode;

/// <summary>
/// A class containing extension methods for dictionaries. This class cannot be inherited.
/// </summary>
internal static class DictionaryExtensions
{
    /// <summary>
    /// Increments the value of the specified key, or adds it if not already present.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">The dictionary to add or increment the value for.</param>
    /// <param name="key">The key to increment the value of or add to the dictionary.</param>
    /// <param name="value">The value to add to the dictionary if the key is not found.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddOrIncrement<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        where TKey : notnull
        where TValue : notnull, INumber<TValue>
        => dictionary.AddOrIncrement(key, value, TValue.One);

    /// <summary>
    /// Increments the value of the specified key by the specified value, or adds it if not already present.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">The dictionary to add or increment the value for.</param>
    /// <param name="key">The key to increment the value of or add to the dictionary.</param>
    /// <param name="value">The value to add to the dictionary if the key is not found.</param>
    /// <param name="increment">The value to increment by.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddOrIncrement<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value, TValue increment)
        where TKey : notnull
        where TValue : notnull, INumber<TValue>
#pragma warning disable CS8604
        => CollectionsMarshal.GetValueRefOrAddDefault(dictionary, key, out bool found) += found ? increment : value;
#pragma warning restore CS8604

    /// <summary>
    /// Decrements the value of the specified key, or adds it if not already present.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">The dictionary to add or decrement the value for.</param>
    /// <param name="key">The key to decrement the value of or add to the dictionary.</param>
    /// <param name="value">The value to add to the dictionary if the key is not found.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddOrDecrement<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        where TKey : notnull
        where TValue : INumber<TValue>
        => dictionary.AddOrIncrement(key, value, -TValue.One);

    /// <summary>
    /// Decrements the value of the specified key by the specified value, or adds it if not already present.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">The dictionary to add or decrement the value for.</param>
    /// <param name="key">The key to decrement the value of or add to the dictionary.</param>
    /// <param name="value">The value to add to the dictionary if the key is not found.</param>
    /// <param name="decrement">The value to decrement by.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddOrDecrement<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value, TValue decrement)
        where TKey : notnull
        where TValue : INumber<TValue>
        => dictionary.AddOrIncrement(key, value, -decrement);

    /// <summary>
    /// Gets the reference with the specified key, adding it if not already present.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">The dictionary to get the reference for.</param>
    /// <param name="key">The key to get the reference for.</param>
    /// <param name="factory">A delegate to a method to create the reference if not found.</param>
    /// <returns>
    /// The value reference with the specified key.
    /// </returns>
    public static TValue GetOrAdd<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TValue> factory)
        where TValue : class
    {
        if (!dictionary.TryGetValue(key, out var result))
        {
            result = dictionary[key] = factory();
        }

        return result;
    }

    /// <summary>
    /// Gets the reference with the specified key, adding it if not already present.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">The dictionary to get the reference for.</param>
    /// <param name="key">The key to get the reference for.</param>
    /// <returns>
    /// The value reference with the specified key.
    /// </returns>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        where TValue : class, new()
    {
        if (!dictionary.TryGetValue(key, out var result))
        {
            result = dictionary[key] = new();
        }

        return result;
    }

    /// <summary>
    /// Gets the reference with the specified key, adding it if not already present.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="alternate">The dictionary to get the reference for.</param>
    /// <param name="key">The key to get the reference for.</param>
    /// <param name="factory">A delegate to a method to create the reference if not found.</param>
    /// <returns>
    /// The value reference with the specified key.
    /// </returns>
    public static TValue GetOrAdd<TValue>(
        this Dictionary<string, TValue>.AlternateLookup<ReadOnlySpan<char>> alternate,
        ReadOnlySpan<char> key,
        Func<TValue> factory)
        where TValue : class
    {
        if (!alternate.TryGetValue(key, out var result))
        {
            result = alternate[key] = factory();
        }

        return result;
    }

    /// <summary>
    /// Gets the reference with the specified key, adding it if not already present.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="alternate">The dictionary alternate lookup to get the reference for.</param>
    /// <param name="key">The key to get the reference for.</param>
    /// <returns>
    /// The value reference with the specified key.
    /// </returns>
    public static TValue GetOrAdd<TValue>(this Dictionary<string, TValue>.AlternateLookup<ReadOnlySpan<char>> alternate, ReadOnlySpan<char> key)
        where TValue : class, new()
    {
        if (!alternate.TryGetValue(key, out var result))
        {
            result = alternate[key] = new();
        }

        return result;
    }

    /// <summary>
    /// Gets the <see cref="ReadOnlySpan{T}"/> alternate lookup for the specified dictionary of strings.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">The dictionary to get the alternate lookup for.</param>
    /// <returns>
    /// The alternate lookup for spans for the specified dictionary.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary<string, TValue>.AlternateLookup<ReadOnlySpan<char>> GetAlternateLookup<TValue>(this Dictionary<string, TValue> dictionary)
        => dictionary.GetAlternateLookup<ReadOnlySpan<char>>();
}
