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
    /// <param name="value">The first vector.</param>
    /// <param name="other">The second vector.</param>
    /// <returns>
    /// The Manhattan distance between <paramref name="value"/> and <paramref name="other"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ManhattanDistance(this in Vector3 value, in Vector3 other)
    {
        var delta = Vector3.Abs(value - other);
        return delta.X + delta.Y + delta.Z;
    }

    /// <summary>
    /// Returns the neighbours of the specified vector.
    /// </summary>
    /// <param name="value">The vector to return the neighbors of.</param>
    /// <returns>
    /// The neighbors of the specified vector.
    /// </returns>
    public static IEnumerable<Vector3> Neighbors(this Vector3 value)
    {
        yield return value + Vector3.UnitX;
        yield return value - Vector3.UnitX;
        yield return value + Vector3.UnitY;
        yield return value - Vector3.UnitY;
        yield return value + Vector3.UnitZ;
        yield return value - Vector3.UnitZ;
    }
}
