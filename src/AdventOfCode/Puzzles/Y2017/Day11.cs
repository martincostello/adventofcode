// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Numerics;

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2017/day/11</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2017, 11, "Hex Ed", RequiresData = true)]
public sealed class Day11 : Puzzle
{
    /// <summary>
    /// A move north. This field is read-only.
    /// </summary>
    private static readonly Vector2 North = Vector2.UnitY * 2;

    /// <summary>
    /// A move north-east. This field is read-only.
    /// </summary>
    private static readonly Vector2 NorthEast = Vector2.UnitY + Vector2.UnitX;

    /// <summary>
    /// A move north-west. This field is read-only.
    /// </summary>
    private static readonly Vector2 NorthWest = Vector2.UnitY + -Vector2.UnitX;

    /// <summary>
    /// A move south. This field is read-only.
    /// </summary>
    private static readonly Vector2 South = -Vector2.UnitY * 2;

    /// <summary>
    /// A move south-east. This field is read-only.
    /// </summary>
    private static readonly Vector2 SouthEast = -Vector2.UnitY + Vector2.UnitX;

    /// <summary>
    /// A move south-west. This field is read-only.
    /// </summary>
    private static readonly Vector2 SouthWest = -Vector2.UnitY + -Vector2.UnitX;

    /// <summary>
    /// Gets the maximum distance the child process reached from its origin.
    /// </summary>
    public int MaximumDistance { get; private set; }

    /// <summary>
    /// Gets the minimum number of steps required to reach the child process.
    /// </summary>
    public int MinimumSteps { get; private set; }

    /// <summary>
    /// Finds the minimum number of steps required to traverse the specified path and
    /// the maximum number of steps taken from the origin.
    /// </summary>
    /// <param name="path">A string describing the path taken.</param>
    /// <returns>
    /// The minimum number of steps required to traverse the path described by <paramref name="path"/>
    /// and the maximum number of steps taken from the origin point.
    /// </returns>
    public static (int Minimum, int Maximum) FindStepRange(string path)
    {
        var directions = ParsePath(path);

        Span<Vector2> vectors = new Vector2[directions.Length];

        int minimum = 0;
        int maximum = 0;

        for (int i = 0; i < directions.Length; i++)
        {
            Vector2 vector = ToVector(directions[i]);
            vectors[i] = vector;

            int distance = GetSteps(vectors[..(i + 1)]);

            minimum = distance;
            maximum = Math.Max(maximum, distance);
        }

        return (minimum, maximum);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        string path = (await ReadResourceAsStringAsync()).Trim();

        (MinimumSteps, MaximumDistance) = FindStepRange(path);

        if (Verbose)
        {
            Logger.WriteLine($"The minimum number of steps required to reach the child process is {MinimumSteps:N0}.");
            Logger.WriteLine($"The maximum distance reached by the child process was {MaximumDistance:N0}.");
        }

        return PuzzleResult.Create(MinimumSteps, MaximumDistance);
    }

    /// <summary>
    /// Gets the number of steps traversed along the specified vector path.
    /// </summary>
    /// <param name="path">The vector path.</param>
    /// <returns>
    /// The number of steps along the vector path specified by <paramref name="path"/>.
    /// </returns>
    private static int GetSteps(ReadOnlySpan<Vector2> path)
    {
        Vector2 magnitude = path[0];

        for (int i = 1; i < path.Length; i++)
        {
            magnitude += path[i];
        }

        float absoluteX = Math.Abs(magnitude.X);
        float absoluteY = Math.Abs(magnitude.Y);

        return (int)((Math.Abs(absoluteX - absoluteY) / 2) + Math.Min(absoluteX, absoluteY));
    }

    /// <summary>
    /// Returns a vector that represents the specified cardinal direction.
    /// </summary>
    /// <param name="direction">The direction to get the vector represenation of.</param>
    /// <returns>
    /// The <see cref="Vector2"/> that represents <paramref name="direction"/>.
    /// </returns>
    private static Vector2 ToVector(CardinalDirection direction)
    {
        return direction switch
        {
            CardinalDirection.North => North,
            CardinalDirection.NorthEast => NorthEast,
            CardinalDirection.NorthWest => NorthWest,
            CardinalDirection.South => South,
            CardinalDirection.SouthEast => SouthEast,
            CardinalDirection.SouthWest => SouthWest,
            _ => throw new PuzzleException($"Invalid cardinal direction '{direction}'."),
        };
    }

    /// <summary>
    /// Parses the specified path into a series of cardinal directions.
    /// </summary>
    /// <param name="path">The path to parse.</param>
    /// <returns>
    /// An array of <see cref="CardinalDirection"/> containing the parsed directions from the path.
    /// </returns>
    private static CardinalDirection[] ParsePath(string path)
    {
        var result = new CardinalDirection[path.Count(',') + 1];

        int i = 0;

        foreach (var direction in path.Tokenize(','))
        {
            result[i++] = new string(direction) switch
            {
                "n" => CardinalDirection.North,
                "ne" => CardinalDirection.NorthEast,
                "nw" => CardinalDirection.NorthWest,
                "s" => CardinalDirection.South,
                "se" => CardinalDirection.SouthEast,
                "sw" => CardinalDirection.SouthWest,
                _ => throw new PuzzleException($"Unknown direction '{direction}'."),
            };
        }

        return result;
    }
}
