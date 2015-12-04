// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Present.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   Present.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day2
{
    using System.Globalization;

    /// <summary>
    /// A class representing a Christmas present. This class cannot be inherited.
    /// </summary>
    internal sealed class Present
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Present"/> class.
        /// </summary>
        /// <param name="length">The length of the present.</param>
        /// <param name="width">The width of the present.</param>
        /// <param name="height">The height of the present.</param>
        internal Present(int length, int width, int height)
        {
            Length = length;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Gets the length of the present.
        /// </summary>
        internal int Length { get; }

        /// <summary>
        /// Gets the width of the present.
        /// </summary>
        internal int Width { get; }

        /// <summary>
        /// Gets the height of the present.
        /// </summary>
        internal int Height { get; }

        /// <summary>
        /// Parses the specified <see cref="string"/> to an instance of <see cref="Present"/>.
        /// </summary>
        /// <param name="value">The present to parse.</param>
        /// <returns>The result of parsing <paramref name="value"/>.</returns>
        internal static Present Parse(string value)
        {
            string[] split = value.Split('x');

            int length = int.Parse(split[0], NumberStyles.Integer, CultureInfo.InvariantCulture);
            int width = int.Parse(split[1], NumberStyles.Integer, CultureInfo.InvariantCulture);
            int height = int.Parse(split[2], NumberStyles.Integer, CultureInfo.InvariantCulture);

            return new Present(length, width, height);
        }
    }
}
