// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles.Impl
{
    using System;
    using System.Drawing;

    /// <summary>
    /// A class representing a GPS locator for a Santa-type figure. This class cannot be inherited.
    /// </summary>
    internal sealed class SantaGps
    {
        /// <summary>
        /// Gets or sets the location of the Santa-type figure.
        /// </summary>
        internal Point Location { get; set; }

        /// <summary>
        /// Moves the Santa-type figure in the specified direction.
        /// </summary>
        /// <param name="direction">The direction to move in.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="direction"/> is invalid.</exception>
        internal void Move(CardinalDirection direction)
        {
            switch (direction)
            {
                case CardinalDirection.East:
                    Location += Moves.East;
                    break;

                case CardinalDirection.North:
                    Location += Moves.North;
                    break;

                case CardinalDirection.South:
                    Location += Moves.South;
                    break;

                case CardinalDirection.West:
                    Location += Moves.West;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, "The specified direction is invalid.");
            }
        }

        /// <summary>
        /// A class containing the moves that Santa can make. This class cannot be inherited.
        /// </summary>
        private static class Moves
        {
            /// <summary>
            /// A move north. This field is read-only.
            /// </summary>
            internal static readonly Size North = new Size(0, 1);

            /// <summary>
            /// A move east. This field is read-only.
            /// </summary>
            internal static readonly Size East = new Size(1, 0);

            /// <summary>
            /// A move south. This field is read-only.
            /// </summary>
            internal static readonly Size South = new Size(0, -1);

            /// <summary>
            /// A move west. This field is read-only.
            /// </summary>
            internal static readonly Size West = new Size(-1, 0);
        }
    }
}
