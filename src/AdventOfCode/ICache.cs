// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode;

/// <summary>
/// Represents a cache.
/// </summary>
public interface ICache
{
    /// <summary>
    /// Returns the value associated with the specified key as an asynchronous operation.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    /// <param name="key">The key for the item.</param>
    /// <param name="factory">A delegate to a method to use to create the item.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that represents the asynchronous operation which returns the value associated with the specified key.
    /// </returns>
    Task<TItem> GetOrCreateAsync<TItem>(object key, Func<Task<TItem>> factory);
}
