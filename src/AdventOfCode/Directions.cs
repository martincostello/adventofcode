// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode;

/// <summary>
/// A class containing directions. This class cannot be inherited.
/// </summary>
internal static class Directions
{
    /// <summary>
    /// The size which moves a point up by one unit.
    /// </summary>
    public static readonly Size Up = new(0, -1);

    /// <summary>
    /// The size which moves a point down by one unit.
    /// </summary>
    public static readonly Size Down = new(0, 1);

    /// <summary>
    /// The size which moves a point left by one unit.
    /// </summary>
    public static readonly Size Left = new(-1, 0);

    /// <summary>
    /// The size which moves a point right by one unit.
    /// </summary>
    public static readonly Size Right = new(1, 0);

    /// <summary>
    /// The sizes to move a point in each cardinal direction.
    /// </summary>
    public static readonly IReadOnlyList<Size> All =
    [
        Left,
        Right,
        Up,
        Down,
    ];

    /// <summary>
    /// Returns the direction to the right of the specified direction.
    /// </summary>
    /// <param name="direction">The direction.</param>
    /// <returns>
    /// The direction after turning right by 90 degrees.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="direction"/> is not a valid direction.
    /// </exception>
    public static Size TurnRight(Size direction)
    {
        if (direction == Up)
        {
            return Right;
        }
        else if (direction == Right)
        {
            return Down;
        }
        else if (direction == Down)
        {
            return Left;
        }
        else if (direction == Left)
        {
            return Up;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(direction), direction, "Invalid direction.");
        }
    }
}
