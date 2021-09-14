// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Shouldly;

internal static class ShouldlyTaskExtensions
{
    public static async Task ShouldBe<T>(this Task<T> task, T expected)
    {
        var actual = await task;
        actual.ShouldBe(expected);
    }

    public static async Task ShouldBe<T>(this Task<IList<T>> task, IEnumerable<T> expected)
    {
        var actual = await task;
        actual.ShouldBe(expected);
    }
}
