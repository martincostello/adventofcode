// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace System.Drawing;

/// <summary>
/// A class containing extension methods for the <see cref="Point"/> structure. This class cannot be inherited.
/// </summary>
internal static class PointExtensions
{
    /// <summary>
    /// Returns the Manhattan Distance between two points.
    /// </summary>
    /// <param name="value">The origin value.</param>
    /// <param name="other">The value to calculate the distance to.</param>
    /// <returns>
    /// The Manhattan Distance between <paramref name="value"/> and <paramref name="other"/>.
    /// </returns>
    public static int ManhattanDistance(this Point value, Point other)
        => Math.Abs(value.X - other.X) + Math.Abs(value.Y - other.Y);
}
