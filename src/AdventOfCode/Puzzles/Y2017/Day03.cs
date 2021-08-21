// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2017/day/3</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2017, 03, MinimumArguments = 1)]
public sealed class Day03 : Puzzle
{
    /// <summary>
    /// A move one unit left. This field is read-only.
    /// </summary>
    private static readonly Size Left = new(width: -1, height: 0);

    /// <summary>
    /// A move one unit right. This field is read-only.
    /// </summary>
    private static readonly Size Right = new(width: 1, height: 0);

    /// <summary>
    /// A move one unit up. This field is read-only.
    /// </summary>
    private static readonly Size Up = new(width: 0, height: 1);

    /// <summary>
    /// A move one unit down. This field is read-only.
    /// </summary>
    private static readonly Size Down = new(width: 0, height: -1);

    /// <summary>
    /// Gets the number steps that are required to carry the data from the input value to the access port.
    /// </summary>
    public int Steps { get; private set; }

    /// <summary>
    /// Gets the first value written that is larger than the input value.
    /// </summary>
    public int FirstStorageLargerThanInput { get; private set; }

    /// <summary>
    /// Computes how many steps are required to carry the data from the specified square identified all the way to the access port.
    /// </summary>
    /// <param name="square">The number of the square to get the number of steps to retrieve data from.</param>
    /// <returns>
    /// The number of steps required to carry the data from the square specified by <paramref name="square"/> to the access port.
    /// </returns>
    internal static int ComputeSteps(int square)
    {
        var position = Point.Empty;
        int ring = 2;
        int currentSquare = 1;

        while (currentSquare < square)
        {
            int length = LengthOfRing(ring);
            int perimeter = PerimiterOfRing(ring);

            // Is the target square in the current ring?
            if (currentSquare + perimeter >= square)
            {
                position += Right;
                currentSquare++;

                int movesUp = length - 2;
                int movesOther = movesUp + 1;

                for (int i = 0; currentSquare != square && i < movesUp; i++)
                {
                    position += Up;
                    currentSquare++;
                }

                foreach (Size direction in new[] { Left, Down, Right })
                {
                    for (int i = 0; currentSquare != square && i < movesOther; i++)
                    {
                        position += direction;
                        currentSquare++;
                    }

                    if (currentSquare == square)
                    {
                        break;
                    }
                }

                if (currentSquare == square)
                {
                    break;
                }
            }

            // Skip to the next ring
            currentSquare += PerimiterOfRing(ring++);
            position += Right;
            position += Down;
        }

        int manhattenDistance = Math.Abs(position.X) + Math.Abs(position.Y);

        return manhattenDistance;
    }

    /// <summary>
    /// Computes the first value written that is larger than the specified square.
    /// </summary>
    /// <param name="square">The number of the square to get first larger written value for.</param>
    /// <returns>
    /// The value that is written first that is larger than <paramref name="square"/>.
    /// </returns>
    internal static int ComputeFirstLargestWrittenValue(int square)
    {
        var position = Point.Empty;
        int ring = 2;
        int currentSquare = 1;

        var bounds = new[] { Left, Down, Right };
        var memory = new Memory();
        int lastValueWritten = memory.Write(position);

        while (lastValueWritten <= square)
        {
            int length = LengthOfRing(ring);

            position += Right;
            currentSquare++;

            lastValueWritten = memory.Write(position);

            int movesUp = length - 2;
            int movesOther = movesUp + 1;

            for (int i = 0; lastValueWritten <= square && i < movesUp; i++)
            {
                position += Up;
                currentSquare++;

                lastValueWritten = memory.Write(position);
            }

            foreach (Size direction in bounds)
            {
                for (int i = 0; lastValueWritten <= square && i < movesOther; i++)
                {
                    position += direction;
                    currentSquare++;

                    lastValueWritten = memory.Write(position);
                }

                if (lastValueWritten >= square)
                {
                    break;
                }
            }

            if (lastValueWritten >= square)
            {
                break;
            }

            ring++;
        }

        return lastValueWritten;
    }

    /// <inheritdoc />
    protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        int square = ParseInt32(args[0]);

        Steps = ComputeSteps(square);
        FirstStorageLargerThanInput = ComputeFirstLargestWrittenValue(square);

        if (Verbose)
        {
            Logger.WriteLine($"The number of steps required to carry the data from square {square:N0} all the way to the access port is {Steps:N0}.");
            Logger.WriteLine($"The first value written that is larger than square {square:N0} is {FirstStorageLargerThanInput:N0}.");
        }

        return PuzzleResult.Create(Steps, FirstStorageLargerThanInput);
    }

    /// <summary>
    /// Gets the length of the specified ring number.
    /// </summary>
    /// <param name="ring">The number of the ring.</param>
    /// <returns>
    /// The length of the ring number specified by <paramref name="ring"/>.
    /// </returns>
    private static int LengthOfRing(int ring) => 1 + (2 * (ring - 1));

    /// <summary>
    /// Gets the perimeter of the specified ring number.
    /// </summary>
    /// <param name="ring">The number of the ring.</param>
    /// <returns>
    /// The perimeter of the ring number specified by <paramref name="ring"/>.
    /// </returns>
    private static int PerimiterOfRing(int ring) => (2 * LengthOfRing(ring)) + (2 * LengthOfRing(ring - 1));

    /// <summary>
    /// A class representing values in a grid of two-dimensional memory. This class cannot be inherited.
    /// </summary>
    private sealed class Memory : Dictionary<Point, int>
    {
        /// <summary>
        /// An array of offsets that may surround an address in the grid.
        /// </summary>
        private static readonly Size[] Offsets = new[]
        {
            new Size(0, 1),
            new Size(1, 1),
            new Size(1, 0),
            new Size(1, -1),
            new Size(0, -1),
            new Size(-1, -1),
            new Size(-1, 0),
            new Size(-1, 1),
        };

        /// <summary>
        /// Writes the value for the specified address to memory.
        /// </summary>
        /// <param name="address">The address to write the value of.</param>
        /// <returns>
        /// The value that was written for <paramref name="address"/>.
        /// </returns>
        public int Write(Point address)
            => this[address] = GetSumOfAdjacentAddresses(address);

        /// <summary>
        /// Gets the sum of the values stored in the adjacent addresses.
        /// </summary>
        /// <param name="address">The memory address to get the sum for.</param>
        /// <returns>
        /// The sum of the memory addresses adjacent to <paramref name="address"/>.
        /// </returns>
        private int GetSumOfAdjacentAddresses(Point address)
        {
            if (address == Point.Empty)
            {
                return 1;
            }

            int sum = 0;

            foreach (Size offset in Offsets)
            {
                if (TryGetValue(address + offset, out int value))
                {
                    sum += value;
                }
            }

            return sum;
        }
    }
}
