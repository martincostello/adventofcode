// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/15</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 15, "Warehouse Woes", RequiresData = true)]
public sealed class Day15 : Puzzle
{
    private enum Square
    {
        Box,
        Empty,
        Robot,
        Wall,
    }

    /// <summary>
    /// Gets the sum of the GPS coordinates of the boxes in the warehouse.
    /// </summary>
    public int GpsSum { get; private set; }

    /// <summary>
    /// Moves the boxes in the warehouse around using the specified instructions.
    /// </summary>
    /// <param name="values">The map of the warehouse and the moves to make.</param>
    /// <returns>
    /// The sum of the GPS coordinates of the boxes in the warehouse after moving them.
    /// </returns>
    public static int Move(IList<string> values)
    {
        int delimiter = values.IndexOf(string.Empty);

        var robot = Point.Empty;
        var warehouse = new Dictionary<Point, Square>();

        for (int y = 0; y < delimiter; y++)
        {
            for (int x = 0; x < values[y].Length; x++)
            {
                var square = values[y][x] switch
                {
                    'O' => Square.Box,
                    '@' => Square.Robot,
                    '#' => Square.Wall,
                    _ => Square.Empty,
                };

                if (square is Square.Robot)
                {
                    robot = new(x, y);
                }

                warehouse[new(x, y)] = square;
            }
        }

        var instructions = new StringBuilder();

        for (int i = delimiter + 1; i < values.Count; i++)
        {
            instructions.Append(values[i]);
        }

        var current = robot;
        string moves = instructions.ToString();

        for (int i = 0; i < moves.Length; i++)
        {
            var direction = moves[i] switch
            {
                '^' => Directions.Up,
                'v' => Directions.Down,
                '<' => Directions.Left,
                '>' => Directions.Right,
                _ => throw new UnreachableException(),
            };

            Point next = current + direction;

            if (!warehouse.TryGetValue(next, out var square) || square is Square.Wall)
            {
                continue;
            }
            else if (square is Square.Empty)
            {
                warehouse[current] = Square.Empty;
                warehouse[next] = Square.Robot;
                current = next;
            }
            else
            {
                var ahead = next;

                while (warehouse.TryGetValue(ahead += direction, out square) && square is Square.Box)
                {
                }

                if (square is Square.Empty)
                {
                    warehouse[current] = Square.Empty;
                    warehouse[next] = Square.Robot;
                    warehouse[ahead] = Square.Box;
                    current = next;
                }
            }
        }

        int sum = 0;

        foreach (var (point, square) in warehouse)
        {
            if (square is Square.Box)
            {
                sum += (100 * point.Y) + point.X;
            }
        }

        return sum;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        GpsSum = Move(values);

        if (Verbose)
        {
            Logger.WriteLine("The sum of all boxes' GPS coordinates is {0}.", GpsSum);
        }

        return PuzzleResult.Create(GpsSum);
    }
}
