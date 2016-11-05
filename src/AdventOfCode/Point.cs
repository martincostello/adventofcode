// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    using System;
    using System.Globalization;

    /// <summary>
    /// A structure representing a point.
    /// </summary>
    public struct Point : IEquatable<Point>
    {
        /// <summary>
        /// A <see cref="Point"/> representing an empty point.
        /// </summary>
        public static readonly Point Empty = new Point(0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> structure.
        /// </summary>
        /// <param name="x">The x-coordinate of the point.</param>
        /// <param name="y">The y-coordinate of the point.</param>
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets or sets the x-coordinate of this point.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of this point.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Compares the two specified <see cref="Point"/> values for equality.
        /// </summary>
        /// <param name="left">The first value.</param>
        /// <param name="right">The second value.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are
        /// equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Point left, Point right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        /// <summary>
        /// Compares the two specified <see cref="Point"/> values for inequality.
        /// </summary>
        /// <param name="left">The first value.</param>
        /// <param name="right">The second value.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are
        /// not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Point left, Point right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Translates a <see cref="Point" /> by a given <see cref="Size" />.
        /// </summary>
        /// <param name="pt">The point to add the sie to.</param>
        /// <param name="sz">The size to add to the point.</param>
        /// <returns>
        /// The translated <see cref="Point"/> value.
        /// </returns>
        public static Point operator +(Point pt, Size sz)
        {
            return Add(pt, sz);
        }

        /// <summary>
        /// Translates a <see cref="Point" /> by a given <see cref="Size" />.
        /// </summary>
        /// <param name="pt">The point to add the sie to.</param>
        /// <param name="sz">The size to add to the point.</param>
        /// <returns>
        /// The translated <see cref="Point"/> value.
        /// </returns>
        public static Point Add(Point pt, Size sz)
        {
            return new Point(pt.X + sz.Width, pt.Y + sz.Height);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is Point))
            {
                return false;
            }

            return Equals((Point)obj);
        }

        /// <inheritdoc />
        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return unchecked(X ^ Y);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{{X={X.ToString(CultureInfo.CurrentCulture)},Y={Y.ToString(CultureInfo.CurrentCulture)}}}";
        }
    }
}
