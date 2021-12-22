// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode;

/// <summary>
/// Represents a point in 3-dimensional space.
/// </summary>
[System.Diagnostics.DebuggerDisplay("({X}, {Y}, {Z})")]
internal readonly struct Point3D : IEquatable<Point3D>
{
    /// <summary>
    /// The x coordinate.
    /// </summary>
    public readonly int X;

    /// <summary>
    /// The y coordinate.
    /// </summary>
    public readonly int Y;

    /// <summary>
    /// The z coordinate.
    /// </summary>
    public readonly int Z;

    private static readonly Point3D _origin = new(0, 0, 0);

    /// <summary>
    /// Initializes a new instance of the <see cref="Point3D"/> struct.
    /// </summary>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <param name="z">The z coordinate.</param>
    public Point3D(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>
    /// Gets the origin point. This field is read-only.
    /// </summary>
    public static ref readonly Point3D Zero => ref _origin;

    public static Point3D operator +(in Point3D a, in Point3D b)
        => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Point3D operator -(in Point3D a, in Point3D b)
        => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public static bool operator ==(in Point3D a, in Point3D b)
        => a.X == b.X && a.Y == b.Y && a.Z == b.Z;

    public static bool operator !=(in Point3D a, in Point3D b)
        => a.X != b.X || a.Y != b.Y || a.Z != b.Z;

    /// <summary>
    /// Gets the Manhattan distance between two points.
    /// </summary>
    /// <param name="a">The first value.</param>
    /// <param name="b">The second value.</param>
    /// <returns>
    /// The Manhattan distance between <paramref name="a"/> and <paramref name="b"/>.
    /// </returns>
    public static int ManhattanDistance(in Point3D a, in Point3D b)
    {
        Point3D delta = a - b;
        return Math.Abs(delta.X) + Math.Abs(delta.Y) + Math.Abs(delta.Z);
    }

    /// <inheritdoc />
    public override readonly bool Equals(object? obj)
        => obj is Point3D point && this == point;

    /// <inheritdoc />
    public readonly bool Equals(Point3D other)
        => this == other;

    /// <inheritdoc />
    public override readonly int GetHashCode()
        => HashCode.Combine(X, Y, Z);
}
