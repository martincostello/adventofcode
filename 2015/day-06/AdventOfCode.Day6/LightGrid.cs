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
    using System.Drawing;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// A class representing a grid of Christmas lights. This class cannot be inherited.
    /// </summary>
    internal sealed class LightGrid
    {
        /// <summary>
        /// The brightnesses of lights by their position.
        /// </summary>
        private readonly int[,] _lightBrightnesses;

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
            _lightBrightnesses = new int[width, height];
        }

        /// <summary>
        /// Gets the total brightness of the grid.
        /// </summary>
        internal int Brightness
        {
            get
            {
                int result = 0;

                for (int x = 0; x < _lightBrightnesses.GetLength(0); x++)
                {
                    for (int y = 0; y < _lightBrightnesses.GetLength(1); y++)
                    {
                        result += _lightBrightnesses[x, y];
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the number of lights in the grid that have a brightness of at least one.
        /// </summary>
        internal int Count
        {
            get
            {
                int result = 0;

                for (int x = 0; x < _bounds.Width; x++)
                {
                    for (int y = 0; y < _bounds.Height; y++)
                    {
                        result += _lightBrightnesses[x, y] > 0 ? 1 : 0;
                    }
                }

                return result;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            for (int x = 0; x < _bounds.Width; x++)
            {
                for (int y = 0; y < _bounds.Height; y++)
                {
                    builder.AppendFormat(CultureInfo.InvariantCulture, "{0}", _lightBrightnesses[x, y]);
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }

        /// <summary>
        /// Gets the brightness of the light at the specified position.
        /// </summary>
        /// <param name="position">The position of the light to get the state of.</param>
        /// <returns>The current brightness of the light at <paramref name="position"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="position"/> is not in the light grid.</exception>
        internal int this[Point position]
        {
            get
            {
                EnsureInBounds(position);
                return _lightBrightnesses[position.X, position.Y];
            }
        }

        /// <summary>
        /// Increments the brightness of the lights within the specified bounds by the specified amount.
        /// </summary>
        /// <param name="bounds">The bounds of the lights to toggle.</param>
        /// <param name="delta">The brightness to increase (or decrease) the brightness of the lights by.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bounds"/> is not entirely within the light grid.</exception>
        internal void IncrementBrightness(Rectangle bounds, int delta)
        {
            EnsureInBounds(bounds);

            for (int x = 0; x < bounds.Width; x++)
            {
                for (int y = 0; y < bounds.Height; y++)
                {
                    IncrementBrightness(new Point(bounds.X + x, bounds.Y + y), delta);
                }
            }
        }

        /// <summary>
        /// Increments the brightness of the light at the specified position by the specified amount.
        /// </summary>
        /// <param name="position">The position of the light to increment the brightness for.</param>
        /// <param name="delta">The brightness to increase (or decrease) the brightness of the lights by.</param>
        /// <returns>The new brightness of the light at <paramref name="position"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="position"/> is not in the light grid.</exception>
        internal int IncrementBrightness(Point position, int delta) => IncrementOrSetBrightness(position, delta, false);

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
            bool isOff = this[position] == 0;

            if (isOff)
            {
                IncrementBrightness(position, 1);
                return true;
            }
            else
            {
                IncrementBrightness(position, -1);
                return false;
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
        internal void TurnOff(Point position) => IncrementOrSetBrightness(position, 0, true);

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
        internal void TurnOn(Point position) => IncrementOrSetBrightness(position, 1, true);

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

        /// <summary>
        /// Increments or sets the brightness of the light at the specified position by the specified amount.
        /// </summary>
        /// <param name="position">The position of the light to increment the brightness for.</param>
        /// <param name="delta">The brightness to increase (or decrease) the brightness of the lights by .</param>
        /// <param name="set">Whether to set the value rather than increment it.</param>
        /// <returns>The new brightness of the light at <paramref name="position"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="position"/> is not in the light grid.</exception>
        private int IncrementOrSetBrightness(Point position, int delta, bool set = false)
        {
            EnsureInBounds(position);

            if (set)
            {
                return _lightBrightnesses[position.X, position.Y] = delta;
            }
            else
            {
                int newValue = _lightBrightnesses[position.X, position.Y] += delta;

                if (newValue < 0)
                {
                    newValue = 0;
                }

                return _lightBrightnesses[position.X, position.Y] = newValue;
            }
        }
    }
}
