// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2020/day/12</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2020, 12, RequiresData = true)]
public sealed class Day12 : Puzzle
{
    /// <summary>
    /// A dictionary of vectors keyed by headings. This field is read-only.
    /// </summary>
    private static readonly Dictionary<int, Size> Vectors = new()
    {
        [Headings.North] = new(0, 1),
        [Headings.South] = new(0, -1),
        [Headings.East] = new(1, 0),
        [Headings.West] = new(-1, 0),
    };

    /// <summary>
    /// Gets the Manhattan distance the ship has travelled.
    /// </summary>
    public int ManhattanDistance { get; private set; }

    /// <summary>
    /// Gets the Manhattan distance the ship has travelled when the waypoint is used.
    /// </summary>
    public int ManhattanDistanceWithWaypoint { get; private set; }

    /// <summary>
    /// Gets the Manhattan distance travelled by the ship navigating the specified instructions.
    /// </summary>
    /// <param name="instructions">The navigation instructions for the ship.</param>
    /// <returns>
    /// The Manhattan distance the ship travels by following the instructions.
    /// </returns>
    public static int GetDistanceTravelled(IEnumerable<string> instructions)
    {
        int heading = Headings.East;
        var ship = Point.Empty;

        foreach (string instruction in instructions)
        {
            int units = Parse<int>(instruction[1..]);

            switch (instruction[0..1])
            {
                case "L":
                    heading = Math.Abs((heading - units + 360) % 360);
                    break;

                case "R":
                    heading = Math.Abs((heading + units) % 360);
                    break;

                case "F":
                    ship += Vectors[heading] * units;
                    break;

                case "N":
                    ship += Vectors[Headings.North] * units;
                    break;

                case "S":
                    ship += Vectors[Headings.South] * units;
                    break;

                case "E":
                    ship += Vectors[Headings.East] * units;
                    break;

                case "W":
                    ship += Vectors[Headings.West] * units;
                    break;

                default:
                    break;
            }
        }

        return ship.ManhattanDistance();
    }

    /// <summary>
    /// Gets the Manhattan distance travelled by the ship navigating the specified instructions as a waypoint.
    /// </summary>
    /// <param name="instructions">The navigation instructions for the ship.</param>
    /// <returns>
    /// The Manhattan distance the ship travels by following the instructions.
    /// </returns>
    public static int GetDistanceTravelledWithWaypoint(IEnumerable<string> instructions)
    {
        var ship = Point.Empty;
        var waypoint = new Point(10, 1);

        foreach (string instruction in instructions)
        {
            int units = Parse<int>(instruction[1..]);

            switch (instruction[0..1])
            {
                case "L":
                    waypoint = units switch
                    {
                        90 => new(-waypoint.Y, waypoint.X),
                        180 => new(-waypoint.X, -waypoint.Y),
                        270 => new(waypoint.Y, -waypoint.X),
                        _ => waypoint,
                    };
                    break;

                case "R":
                    waypoint = units switch
                    {
                        90 => new(waypoint.Y, -waypoint.X),
                        180 => new(-waypoint.X, -waypoint.Y),
                        270 => new(-waypoint.Y, waypoint.X),
                        _ => waypoint,
                    };
                    break;

                case "F":
                    ship += new Size(waypoint) * units;
                    break;

                case "N":
                    waypoint += Vectors[Headings.North] * units;
                    break;

                case "S":
                    waypoint += Vectors[Headings.South] * units;
                    break;

                case "E":
                    waypoint += Vectors[Headings.East] * units;
                    break;

                case "W":
                    waypoint += Vectors[Headings.West] * units;
                    break;

                default:
                    break;
            }
        }

        return ship.ManhattanDistance();
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> instructions = await ReadResourceAsLinesAsync();

        ManhattanDistance = GetDistanceTravelled(instructions);
        ManhattanDistanceWithWaypoint = GetDistanceTravelledWithWaypoint(instructions);

        if (Verbose)
        {
            Logger.WriteLine("The Manhattan distance travelled by the ship is {0}.", ManhattanDistance);
            Logger.WriteLine("The Manhattan distance travelled by the ship when using the waypoint is {0}.", ManhattanDistanceWithWaypoint);
        }

        return PuzzleResult.Create(ManhattanDistance, ManhattanDistanceWithWaypoint);
    }

    /// <summary>
    /// A class containing headings for cardinal directions.
    /// </summary>
    private static class Headings
    {
        /// <summary>
        /// The heading for north.
        /// </summary>
        internal const int North = 0;

        /// <summary>
        /// The heading for south.
        /// </summary>
        internal const int South = 180;

        /// <summary>
        /// The heading for east.
        /// </summary>
        internal const int East = 90;

        /// <summary>
        /// The heading for west.
        /// </summary>
        internal const int West = 270;
    }
}
