// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Drawing;

/// <summary>
/// A class containing extension methods for the <see cref="Point"/> structure. This class cannot be inherited.
/// </summary>
internal static class PointExtensions
{
    /// <summary>
    /// Returns the absolute Manhattan Distance of a point.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// The Manhattan Distance of <paramref name="value"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ManhattanDistance(this Point value) => value.ManhattanDistance(Point.Empty);

    /// <summary>
    /// Returns the Manhattan Distance between two points.
    /// </summary>
    /// <param name="value">The origin value.</param>
    /// <param name="other">The value to calculate the distance to.</param>
    /// <returns>
    /// The Manhattan Distance between <paramref name="value"/> and <paramref name="other"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ManhattanDistance(this Point value, Point other)
        => Math.Abs(value.X - other.X) + Math.Abs(value.Y - other.Y);

    /// <summary>
    /// Returns the neighbors in a 2D grid of the specified point from top-left to bottom-right.
    /// </summary>
    /// <param name="value">The value to get the neighbors of.</param>
    /// <param name="includeSelf">Whether to return the value itself.</param>
    /// <returns>
    /// A sequence which returns the neighbors of the specified point.
    /// </returns>
    public static IEnumerable<Point> Neighbors(this Point value, bool includeSelf = false)
    {
        int y;
        int x1;
        int x2 = value.X;
        int x3;

        yield return new(x1 = x2 - 1, y = value.Y - 1);
        yield return new(x2 = value.X, y);
        yield return new(x3 = x2 + 1, y++);

        yield return new(x1, y);

        if (includeSelf)
        {
            yield return value;
        }

        yield return new(x3, y++);

        yield return new(x1, y);
        yield return new(x2, y);
        yield return new(x3, y);
    }

    /// <summary>
    /// Walks from the specified point to another other point returning each location along the way.
    /// </summary>
    /// <param name="origin">The origin of the walk.</param>
    /// <param name="destination">The destination to walk to.</param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> that returns the points between <paramref name="origin"/>
    /// and <paramref name="destination"/>, where the walk ends.
    /// </returns>
    public static IEnumerable<Point> WalkTo(this Point origin, Point destination)
    {
        var direction = new Size(destination.X - origin.X, destination.Y - origin.Y);

        int magnitude = Math.Max(Math.Abs(direction.Width), Math.Abs(direction.Height));
        var unit = direction / magnitude;

        var current = origin;

        while (current != destination)
        {
            current += unit;
            yield return current;
        }
    }

    /// <summary>
    /// Returns whether the specified point is above another point.
    /// </summary>
    /// <param name="value">The point.</param>
    /// <param name="other">The other point.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is above
    /// <paramref name="other"/>; otherwise <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAbove(this Point value, Point other) => value.Y < other.Y;

    /// <summary>
    /// Returns whether the specified point is below another point.
    /// </summary>
    /// <param name="value">The point.</param>
    /// <param name="other">The other point.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is below
    /// <paramref name="other"/>; otherwise <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBelow(this Point value, Point other) => value.Y > other.Y;

    /// <summary>
    /// Returns whether the specified point is to the left of another point.
    /// </summary>
    /// <param name="value">The point.</param>
    /// <param name="other">The other point.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is to the left of
    /// <paramref name="other"/>; otherwise <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLeftOf(this Point value, Point other) => value.X < other.X;

    /// <summary>
    /// Returns whether the specified point is to the right of another point.
    /// </summary>
    /// <param name="value">The point.</param>
    /// <param name="other">The other point.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is to the right of
    /// <paramref name="other"/>; otherwise <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsRightOf(this Point value, Point other) => value.X > other.X;
}
