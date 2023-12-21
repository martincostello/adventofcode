﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/1</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2016, 01, "No Time for a Taxicab", RequiresData = true)]
public sealed class Day01 : Puzzle
{
    private static readonly char[] Separators = [',', ' '];

    /// <summary>
    /// An enumeration of directions.
    /// </summary>
    internal enum Direction
    {
        /// <summary>
        /// Turn left.
        /// </summary>
        Left,

        /// <summary>
        /// Turn right.
        /// </summary>
        Right,
    }

    /// <summary>
    /// Gets the number of blocks to the Easter Bunny's headquarters.
    /// </summary>
    public int BlocksToEasterBunnyHQ { get; private set; }

    /// <summary>
    /// Gets the number of blocks to the Easter Bunny's headquarters ignoring duplicates.
    /// </summary>
    public int BlocksToEasterBunnyHQIgnoringDuplicates { get; private set; }

    /// <summary>
    /// Calculate the distance, in blocks, by following the specified instructions.
    /// </summary>
    /// <param name="input">The instructions to follow.</param>
    /// <param name="ignoreDuplicates">Whether to ignore duplicate locations.</param>
    /// <returns>
    /// The shortest distance, in blocks, away from the origin following the instructions would take you.
    /// </returns>
    internal static int CalculateDistance(string input, bool ignoreDuplicates)
    {
        var bearing = CardinalDirection.North;
        var position = Point.Empty;

        var instructions = ParseDirections(input);
        var positions = new List<Point>(instructions.Length);

        foreach (Instruction instruction in instructions)
        {
            bearing = Turn(bearing, instruction.Direction);

            for (int i = 0; i < instruction.Distance; i++)
            {
                if (!ignoreDuplicates)
                {
                    if (positions.Contains(position))
                    {
                        break;
                    }

                    positions.Add(position);
                }

                Size delta = bearing switch
                {
                    CardinalDirection.North => Directions.Down,
                    CardinalDirection.South => Directions.Up,
                    CardinalDirection.East => Directions.Right,
                    CardinalDirection.West => Directions.Left,
                    _ => throw new PuzzleException($"The bearing {bearing} is not known."),
                };

                position += delta;
            }
        }

        return position.ManhattanDistance();
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        string instructions = await ReadResourceAsStringAsync(cancellationToken);

        BlocksToEasterBunnyHQIgnoringDuplicates = CalculateDistance(instructions, ignoreDuplicates: true);
        BlocksToEasterBunnyHQ = CalculateDistance(instructions, ignoreDuplicates: false);

        if (Verbose)
        {
            Logger.WriteLine(
                "The Easter Bunny's headquarters are {0:N0} blocks away.",
                BlocksToEasterBunnyHQIgnoringDuplicates);

            Logger.WriteLine(
                "The Easter Bunny's headquarters are {0:N0} blocks away if it is the first location visited twice.",
                BlocksToEasterBunnyHQ);
        }

        return PuzzleResult.Create(BlocksToEasterBunnyHQIgnoringDuplicates, BlocksToEasterBunnyHQ);
    }

    /// <summary>
    /// Parses the directions to the Easter Bunny headquarters from the specified string.
    /// </summary>
    /// <param name="input">The directions to parse.</param>
    /// <returns>
    /// An array of <see cref="Instruction"/> containing the directions to Easter Bunny HQ.
    /// </returns>
    private static Instruction[] ParseDirections(string input)
    {
        string[] instructions = input.Split(Separators, StringSplitOptions.RemoveEmptyEntries);

        var result = new Instruction[instructions.Length];

        for (int i = 0; i < instructions.Length; i++)
        {
            ReadOnlySpan<char> rawInstruction = instructions[i];

            result[i] = new Instruction()
            {
                Direction = rawInstruction[0] == 'L' ? Direction.Left : Direction.Right,
                Distance = Parse<int>(rawInstruction[1..]),
            };
        }

        return result;
    }

    /// <summary>
    /// Returns the new bearing after turning left or right from the specified bearing.
    /// </summary>
    /// <param name="bearing">The current bearing.</param>
    /// <param name="direction">The direction to turn in.</param>
    /// <returns>
    /// The new bearing.
    /// </returns>
    private static CardinalDirection Turn(CardinalDirection bearing, Direction direction)
    {
        return bearing switch
        {
            CardinalDirection.East => direction == Direction.Left ? CardinalDirection.North : CardinalDirection.South,
            CardinalDirection.North => direction == Direction.Left ? CardinalDirection.West : CardinalDirection.East,
            CardinalDirection.South => direction == Direction.Left ? CardinalDirection.East : CardinalDirection.West,
            CardinalDirection.West => direction == Direction.Left ? CardinalDirection.South : CardinalDirection.North,
            _ => throw new PuzzleException($"Invalid bearing '{bearing}'."),
        };
    }

    /// <summary>
    /// A class representing an instruction for movement. This class cannot be inherited.
    /// </summary>
    internal sealed class Instruction
    {
        /// <summary>
        /// Gets or sets the direction to travel.
        /// </summary>
        internal Direction Direction { get; set; }

        /// <summary>
        /// Gets or sets the distance to travel.
        /// </summary>
        internal int Distance { get; set; }
    }
}
