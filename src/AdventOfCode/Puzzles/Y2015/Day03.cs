﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/3</c>.
/// </summary>
[Puzzle(2015, 03, "Perfectly Spherical Houses in a Vacuum", RequiresData = true)]
public class Day03 : Puzzle
{
    /// <summary>
    /// Gets the number of houses with presents delivered to by Santa.
    /// </summary>
    internal int HousesWithPresentsFromSanta { get; private set; }

    /// <summary>
    /// Gets the number of houses with presents delivered to by Santa and Robo-Santa.
    /// </summary>
    internal int HousesWithPresentsFromSantaAndRoboSanta { get; private set; }

    /// <summary>
    /// Gets the number of unique houses that Santa delivers at least one present to.
    /// </summary>
    /// <param name="instructions">The instructions Santa should follow.</param>
    /// <returns>The number of unique houses that receive a delivery of at least one present.</returns>
    internal static int GetUniqueHousesVisitedBySanta(string instructions)
    {
        var directions = GetDirections(instructions);

        var santa = new SantaGps();
        var coordinates = new HashSet<Point>();

        foreach (var direction in directions)
        {
            coordinates.Add(santa.Location);
            santa.Move(direction);
        }

        return coordinates.Count;
    }

    /// <summary>
    /// Gets the number of unique houses that Santa and Robo-Santa deliver at least one present to.
    /// </summary>
    /// <param name="instructions">The instructions that Santa and Robo-Santa should follow.</param>
    /// <returns>The number of unique houses that receive a delivery of at least one present.</returns>
    internal static int GetUniqueHousesVisitedBySantaAndRoboSanta(string instructions)
    {
        var directions = GetDirections(instructions);

        var santa = new SantaGps();
        var roboSanta = new SantaGps();

        var current = santa;
        var previous = roboSanta;

        var coordinates = new HashSet<Point>()
        {
            Point.Empty,
        };

        foreach (var direction in directions)
        {
            current.Move(direction);
            coordinates.Add(current.Location);
            (current, previous) = (previous, current);
        }

        return coordinates.Count;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        string instructions = await ReadResourceAsStringAsync(cancellationToken);

        HousesWithPresentsFromSanta = GetUniqueHousesVisitedBySanta(instructions);
        HousesWithPresentsFromSantaAndRoboSanta = GetUniqueHousesVisitedBySantaAndRoboSanta(instructions);

        if (Verbose)
        {
            Logger.WriteLine(
                "In 2015, Santa delivered presents to {0:N0} houses.",
                HousesWithPresentsFromSanta);

            Logger.WriteLine(
                "In 2016, Santa and Robo-Santa delivered presents to {0:N0} houses.",
                HousesWithPresentsFromSantaAndRoboSanta);
        }

        return PuzzleResult.Create(HousesWithPresentsFromSanta, HousesWithPresentsFromSantaAndRoboSanta);
    }

    /// <summary>
    /// Reads the directions from the specified file.
    /// </summary>
    /// <param name="instructions">The path of the file containing the directions.</param>
    /// <returns>An <see cref="List{T}"/> containing the directions from from the specified file.</returns>
    private static List<CardinalDirection> GetDirections(string instructions)
    {
        var directions = new List<CardinalDirection>(instructions.Length);

        for (int i = 0; i < instructions.Length; i++)
        {
            CardinalDirection direction = instructions[i] switch
            {
                '^' => CardinalDirection.North,
                'v' => CardinalDirection.South,
                '>' => CardinalDirection.East,
                '<' => CardinalDirection.West,
                _ => throw new PuzzleException($"Invalid direction: '{instructions[i]}'."),
            };

            directions.Add(direction);
        }

        return directions;
    }

    /// <summary>
    /// A class representing a GPS locator for a Santa-type figure. This class cannot be inherited.
    /// </summary>
    private sealed class SantaGps
    {
        /// <summary>
        /// Gets the location of the Santa-type figure.
        /// </summary>
        internal Point Location { get; private set; }

        /// <summary>
        /// Moves the Santa-type figure in the specified direction.
        /// </summary>
        /// <param name="direction">The direction to move in.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="direction"/> is invalid.</exception>
        internal void Move(CardinalDirection direction)
        {
            Location += direction switch
            {
                CardinalDirection.East => new(1, 0),
                CardinalDirection.North => new(0, 1),
                CardinalDirection.South => new(0, -1),
                CardinalDirection.West => new(-1, 0),
                _ => throw new PuzzleException($"The specified direction '{direction}' is invalid."),
            };
        }
    }
}
