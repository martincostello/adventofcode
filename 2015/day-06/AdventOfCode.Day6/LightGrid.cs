// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LightGrid.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   LightGrid.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day6
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;

    /// <summary>
    /// A class representing a grid of Christmas lights. This class cannot be inherited.
    /// </summary>
    internal sealed class LightGrid
    {
        /// <summary>
        /// The positions of the lights in the grid that are considered to be on. This field is read-only.
        /// </summary>
        /// <remarks>
        /// By convention, if the position is not present, the light is off.
        /// </remarks>
        private readonly HashSet<Point> _lightsThatAreOn = new HashSet<Point>();

        /// <summary>
        /// The bounds of the grid. This field is read-only.
        /// </summary>
        private readonly Rectangle _bounds;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightGrid"/> class.
        /// </summary>
        /// <param name="width">The width of the light grid.</param>
        /// <param name="height">The height of the light grid.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="width"/> or <paramref name="height"/> is less than one.</exception>
        /// <remarks>The initial state of each light in the grid is that it is off.</remarks>
        internal LightGrid(int width, int height)
        {
            if (width < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(width), width, $"{nameof(width)} cannot be less than zero.");
            }

            if (height < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(height), height, $"{nameof(height)} cannot be less than zero.");
            }

            _bounds = new Rectangle(0, 0, width, height);
        }

        /// <summary>
        /// Gets the number of lights in the grid that are turned on.
        /// </summary>
        internal int Count => _lightsThatAreOn.Count;

        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            for (int x = 0; x < _bounds.Width; x++)
            {
                for (int y = 0; y < _bounds.Height; y++)
                {
                    builder.Append(_lightsThatAreOn.Contains(new Point(x, y)) ? "x" : " ");
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }

        /// <summary>
        /// Gets a value indicating whether the light at the specified position is on or off.
        /// </summary>
        /// <param name="position">The position of the light to get the state of.</param>
        /// <returns><see langword="true"/> if the light at <paramref name="position"/> is on; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="position"/> is not in the light grid.</exception>
        internal bool this[Point position]
        {
            get
            {
                EnsureInBounds(position);
                return _lightsThatAreOn.Contains(position);
            }
        }

        /// <summary>
        /// Toggles the lights within the specified bounds.
        /// </summary>
        /// <param name="bounds">The bounds of the lights to toggle.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bounds"/> is not entirely within the light grid.</exception>
        internal void Toggle(Rectangle bounds)
        {
            EnsureInBounds(bounds);

            for (int x = 0; x < bounds.Width; x++)
            {
                for (int y = 0; y < bounds.Height; y++)
                {
                    Toggle(new Point(bounds.X + x, bounds.Y + y));
                }
            }
        }

        /// <summary>
        /// Toggles the state of the light at the specified position.
        /// </summary>
        /// <param name="position">The position of the light to toggle.</param>
        /// <returns><see langword="true"/> if the light at <paramref name="position"/> is now on; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="position"/> is not in the light grid.</exception>
        internal bool Toggle(Point position)
        {
            EnsureInBounds(position);

            if (_lightsThatAreOn.Remove(position))
            {
                return false;
            }
            else
            {
                return _lightsThatAreOn.Add(position);
            }
        }

        /// <summary>
        /// Turns off the lights within the specified bounds.
        /// </summary>
        /// <param name="bounds">The bounds of the lights to turn off.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bounds"/> is not entirely within the light grid.</exception>
        internal void TurnOff(Rectangle bounds)
        {
            EnsureInBounds(bounds);

            for (int x = 0; x < bounds.Width; x++)
            {
                for (int y = 0; y < bounds.Height; y++)
                {
                    TurnOff(new Point(bounds.X + x, bounds.Y + y));
                }
            }
        }

        /// <summary>
        /// Turns off the light at the specified position.
        /// </summary>
        /// <param name="position">The position of the light to turn off.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="position"/> is not in the light grid.</exception>
        internal void TurnOff(Point position)
        {
            EnsureInBounds(position);
            _lightsThatAreOn.Remove(position);
        }

        /// <summary>
        /// Turns on the lights within the specified bounds.
        /// </summary>
        /// <param name="bounds">The bounds of the lights to turn on.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bounds"/> is not entirely within the light grid.</exception>
        internal void TurnOn(Rectangle bounds)
        {
            EnsureInBounds(bounds);

            for (int x = 0; x < bounds.Width; x++)
            {
                for (int y = 0; y < bounds.Height; y++)
                {
                    TurnOn(new Point(bounds.X + x, bounds.Y + y));
                }
            }
        }

        /// <summary>
        /// Turns on the light at the specified position.
        /// </summary>
        /// <param name="position">The position of the light to turn on.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="position"/> is not in the light grid.</exception>
        internal void TurnOn(Point position)
        {
            EnsureInBounds(position);
            _lightsThatAreOn.Add(position);
        }

        /// <summary>
        /// Validates that the specified position is within the light grid.
        /// </summary>
        /// <param name="position">The position to validate.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="position"/> is not in the light grid.</exception>
        private void EnsureInBounds(Point position)
        {
            if (!_bounds.Contains(position))
            {
                throw new ArgumentOutOfRangeException(nameof(position), position, "The specified position is not in the light grid.");
            }
        }

        /// <summary>
        /// Validates that the specified position is within the light grid.
        /// </summary>
        /// <param name="position">The position to validate.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="position"/> is not in the light grid.</exception>
        private void EnsureInBounds(Rectangle bounds)
        {
            if (!_bounds.Contains(bounds))
            {
                throw new ArgumentOutOfRangeException(nameof(bounds), bounds, "The specified bounds are not entirely within the light grid.");
            }
        }
    }
}
