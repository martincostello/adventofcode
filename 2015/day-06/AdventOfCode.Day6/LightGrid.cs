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
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A class representing a grid of Christmas lights. This class cannot be inherited.
    /// </summary>
    internal sealed class LightGrid
    {
        /// <summary>
        /// The brightnesses of lights by their position.
        /// </summary>
        /// <remarks>
        /// By convention, if the position is not present, the light has never been on.
        /// </remarks>
        private readonly IDictionary<Point, LightBrightness> _lightBrightnesses = new Dictionary<Point, LightBrightness>();

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
        /// Gets the total brightness of the grid.
        /// </summary>
        internal int Brightness => _lightBrightnesses.Sum((p) => p.Value.Value);

        /// <summary>
        /// Gets the number of lights in the grid that have a brightness of at least one.
        /// </summary>
        internal int Count => _lightBrightnesses.Count((p) => p.Value.Value > 0);

        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            for (int x = 0; x < _bounds.Width; x++)
            {
                for (int y = 0; y < _bounds.Height; y++)
                {
                    int value = 0;
                    LightBrightness brightness;

                    if (_lightBrightnesses.TryGetValue(new Point(x, y), out brightness))
                    {
                        value = brightness.Value;
                    }

                    builder.AppendFormat(CultureInfo.InvariantCulture, "{0}", value);
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

                int value = 0;
                LightBrightness brightness;

                if (_lightBrightnesses.TryGetValue(position, out brightness))
                {
                    value = brightness.Value;
                }

                return value;
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

            LightBrightness current;

            if (!_lightBrightnesses.TryGetValue(position, out current))
            {
                // Only update the dictionary if there's a positive value of brightness
                if (delta > 0)
                {
                    current = new LightBrightness();
                    current.Set(delta);

                    _lightBrightnesses[position] = current;
                }
            }
            else
            {
                if (set)
                {
                    current.Set(delta);
                }
                else
                {
                    current.Increment(delta);
                }
            }

            return 0;
        }

        /// <summary>
        /// A class representing the brightness of a light. This class cannot be inherited.
        /// </summary>
        /// <remarks>
        /// A reference class is used to reduce the amount of writes to the dictionary holiding the lights' brightnesses.
        /// </remarks>
        private sealed class LightBrightness
        {
            /// <summary>
            /// Gets the current brightness of the light.
            /// </summary>
            internal int Value { get; private set; }

            /// <summary>
            /// Increments the brightness of the light.
            /// </summary>
            /// <param name="delta">The amount to change the brightness by.</param>
            internal void Increment(int delta)
            {
                Value += delta;

                if (Value < 0)
                {
                    Value = 0;
                }
            }

            /// <summary>
            /// Sets the brightness of the light.
            /// </summary>
            /// <param name="value">The new brightness of the light.</param>
            internal void Set(int value)
            {
                Value = value < 1 ? 0 : value;
            }
        }
    }
}
