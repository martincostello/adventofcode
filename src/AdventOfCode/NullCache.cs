// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode;

/// <summary>
/// A class representing an <see cref="ICache"/> that does not cache anything. This class cannot be inherited.
/// </summary>
public sealed class NullCache : ICache
{
    /// <summary>
    /// The default instance of the <see cref="NullCache"/> class. This field is read-only.
    /// </summary>
    public static readonly NullCache Instance = new();

    private NullCache()
    {
    }

    /// <inheritdoc/>
    public Task<TItem> GetOrCreateAsync<TItem>(object key, Func<Task<TItem>> factory)
        => factory();
}
