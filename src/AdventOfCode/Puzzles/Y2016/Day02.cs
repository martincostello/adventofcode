﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/2</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2016, 02, "Bathroom Security", RequiresData = true)]
public sealed class Day02 : Puzzle
{
    /// <summary>
    /// An enumeration of directions.
    /// </summary>
    private enum Direction
    {
        /// <summary>
        /// Up.
        /// </summary>
        Up,

        /// <summary>
        /// Down.
        /// </summary>
        Down,

        /// <summary>
        /// Left.
        /// </summary>
        Left,

        /// <summary>
        /// Right.
        /// </summary>
        Right,
    }

    /// <summary>
    /// Gets the code for the bathroom using an alphanumeric keypad.
    /// </summary>
    public string? BathroomCodeAlphanumericKeypad { get; private set; }

    /// <summary>
    /// Gets the code for the bathroom using a keypad with only digits.
    /// </summary>
    public string? BathroomCodeDigitKeypad { get; private set; }

    /// <summary>
    /// Gets the keypad grid containing digits and letters.
    /// </summary>
    internal static char[,] AlphanumericGrid { get; } =
    {
        { '\0', '\0', '1', '\0', '\0' },
        { '\0', '2', '3', '4', '\0' },
        { '5', '6', '7', '8', '9' },
        { '\0', 'A', 'B', 'C', '\0' },
        { '\0', '\0', 'D', '\0', '\0' },
    };

    /// <summary>
    /// Gets the keypad grid containing only digits.
    /// </summary>
    internal static char[,] DigitGrid { get; } =
    {
        { '1', '2', '3' },
        { '4', '5', '6' },
        { '7', '8', '9' },
    };

    /// <summary>
    /// Returns the bathroom code associated with the specified instructions and grid.
    /// </summary>
    /// <param name="instructions">The instructions to determine the bathroom code from.</param>
    /// <param name="grid">The grid representing the keypad in use.</param>
    /// <returns>
    /// The bathroom code to use given the instructions in <paramref name="instructions"/>.
    /// </returns>
    internal static string GetBathroomCode(ICollection<string> instructions, char[,] grid)
    {
        var directions = ParseInstructions(instructions);
        var code = new StringBuilder(instructions.Count);

        var origin = Point.Empty;

        int height = grid.GetLength(0);
        int width = grid.GetLength(1);

        for (int y = 0; y < height && origin == Point.Empty; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[y, x] == '5')
                {
                    origin = new(x, y);
                    break;
                }
            }
        }

        Point position = origin;

        foreach (var sequence in directions)
        {
            foreach (Direction direction in sequence)
            {
                Point next = position + SizeFromDirection(direction);

                if (IsInBounds(grid, next))
                {
                    position = next;
                }
            }

            code.Append(grid[position.Y, position.X]);
        }

        return code.ToString();
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var instructions = await ReadResourceAsLinesAsync(cancellationToken);

        BathroomCodeDigitKeypad = GetBathroomCode(instructions, DigitGrid);
        BathroomCodeAlphanumericKeypad = GetBathroomCode(instructions, AlphanumericGrid);

        if (Verbose)
        {
            Logger.WriteLine(
                "The code for the bathroom with a digit keypad is {0}.",
                BathroomCodeDigitKeypad);

            Logger.WriteLine(
                "The code for the bathroom with an alphanumeric keypad is {0}.",
                BathroomCodeAlphanumericKeypad);
        }

        return PuzzleResult.Create(BathroomCodeDigitKeypad, BathroomCodeAlphanumericKeypad);
    }

    /// <summary>
    /// Returns a size representing the move in the specified direction.
    /// </summary>
    /// <param name="direction">The direction to move in.</param>
    /// <returns>
    /// A <see cref="Size"/> representing a move in that direction.
    /// </returns>
    private static Size SizeFromDirection(Direction direction)
    {
        return direction switch
        {
            Direction.Down => Directions.Down,
            Direction.Left => Directions.Left,
            Direction.Right => Directions.Right,
            Direction.Up => Directions.Up,
            _ => default,
        };
    }

    /// <summary>
    /// Returns whether the specified position in the bounds of the specified array.
    /// </summary>
    /// <param name="grid">The grid to test the position's bounds for.</param>
    /// <param name="position">The position to test for being in the bounds of the grid.</param>
    /// <returns>
    /// <see langword="true"/> if the position is within the bounds of the grid; otherwise <see langword="false"/>.
    /// </returns>
    private static bool IsInBounds(char[,] grid, Point position)
    {
        return
            position.Y >= 0 &&
            position.Y < grid.GetLength(0) &&
            position.X >= 0 &&
            position.X < grid.GetLength(1) &&
            grid[position.Y, position.X] != '\0';
    }

    /// <summary>
    /// Parses the instructions to unlock the bathroom from the specified string.
    /// </summary>
    /// <param name="instructions">The instructions to parse.</param>
    /// <returns>
    /// An <see cref="IList{T}"/> containing the instructions to open the bathroom door.
    /// </returns>
    private static List<Direction[]> ParseInstructions(ICollection<string> instructions)
    {
        var result = new List<Direction[]>(instructions.Count);

        foreach (string instruction in instructions)
        {
            var instructionsForDigit = new Direction[instruction.Length];

            for (int i = 0; i < instruction.Length; i++)
            {
                instructionsForDigit[i] = instruction[i] switch
                {
                    'D' => Direction.Down,
                    'L' => Direction.Left,
                    'R' => Direction.Right,
                    'U' => Direction.Up,
                    _ => default,
                };
            }

            result.Add(instructionsForDigit);
        }

        return result;
    }
}
