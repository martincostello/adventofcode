// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode;

/// <summary>
/// A class containing extensions for the <see cref="Rectangle"/> struct. This class cannot be inherited.
/// </summary>
internal static class RectangleExtensions
{
    /// <summary>
    /// Returns the area of the specified <see cref="Rectangle"/>.
    /// </summary>
    /// <param name="rectangle">The rectangle to get the area of.</param>
    /// <returns>
    /// The area of <paramref name="rectangle"/>.
    /// </returns>
    public static int Area(this Rectangle rectangle)
        => rectangle.Width * rectangle.Height;

    /// <summary>
    /// Returns the border of the specified <see cref="Rectangle"/>.
    /// </summary>
    /// <param name="rectangle">The rectangle to return the border of.</param>
    /// <returns>
    /// The points that make up the border of <paramref name="rectangle"/>.
    /// </returns>
    public static IEnumerable<Point> Border(this Rectangle rectangle)
    {
        for (int x = 0; x < rectangle.Width; x++)
        {
            yield return new Point(x, 0);
            yield return new Point(x, rectangle.Height - 1);
        }

        for (int y = 1; y < rectangle.Height - 1; y++)
        {
            yield return new Point(0, y);
            yield return new Point(rectangle.Width - 1, y);
        }
    }
}
