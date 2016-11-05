// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    /// <summary>
    /// A structure representing a rectangle.
    /// </summary>
    public struct Rectangle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> structure.
        /// </summary>
        /// <param name="x">The x-coordinate of upper-left corner of the rectangle.</param>
        /// <param name="y">The y-coordinate of upper-left corner of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Gets or sets the height of this <see cref="Rectangle"/>.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the width of this <see cref="Rectangle"/>.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the x-coordinate of upper-left corner of this <see cref="Rectangle"/>.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of upper-left corner of this <see cref="Rectangle"/>.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets the size of this <see cref="Rectangle"/>
        /// </summary>
        public Size Size => new Size(Width, Height);

        /// <summary>
        /// Creates a new <see cref="Rectangle"/> with the specified location and size.
        /// </summary>
        /// <param name="left">The coordinate of the left of the rectangle.</param>
        /// <param name="top">The coordinate of the top of the rectangle.</param>
        /// <param name="right">The coordinate of the right of the rectangle.</param>
        /// <param name="bottom">The coordinate of the bottom of the rectangle.</param>
        /// <returns>
        /// The <see cref="Rectangle"/> constructed from the specified coordinates.
        /// </returns>
        public static Rectangle FromLTRB(int left, int top, int right, int bottom)
        {
            return new Rectangle(left, top, right - left, bottom - top);
        }
    }
}
