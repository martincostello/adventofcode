// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

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
    public static void AddOrIncrement<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        where TValue : INumber<TValue>
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key]++;
        }
        else
        {
            dictionary[key] = value;
        }
    }

    /// <summary>
    /// Gets the list with the specified key, adding it if not already present.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">The dictionary to get the list for.</param>
    /// <param name="key">The key to get the list for.</param>
    /// <param name="factory">A delegate to a method to create the list if not found.</param>
    /// <returns>
    /// The list with the specified key.
    /// </returns>
    public static ICollection<TValue> GetOrAdd<TKey, TValue>(
        this IDictionary<TKey, IList<TValue>> dictionary,
        TKey key,
        Func<IList<TValue>> factory)
    {
        if (!dictionary.TryGetValue(key, out var result))
        {
            result = dictionary[key] = factory();
        }

        return result;
    }
}
