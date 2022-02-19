// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Numerics;

/// <summary>
/// A class containing extension methods for the <see cref="Vector3"/> struct. This class cannot be inherited.
/// </summary>
internal static class Vector3Extensions
{
    /// <summary>
    /// Gets the Manhattan distance between two vectors.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    /// <returns>
    /// The Manhattan distance between <paramref name="a"/> and <paramref name="b"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ManhattanDistance(this in Vector3 a, in Vector3 b)
    {
        var abs = Vector3.Abs(a - b);
        return abs.X + abs.Y + abs.Z;
    }
}
