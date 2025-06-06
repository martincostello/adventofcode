﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

using Move = (System.Drawing.Point Location, System.Drawing.Size Direction);

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/16</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 16, "Reindeer Maze", RequiresData = true)]
public sealed class Day16 : Puzzle
{
    /// <summary>
    /// Gets the winning score.
    /// </summary>
    public int WinningScore { get; private set; }

    /// <summary>
    /// Races the reindeer through the maze and returns the winning score.
    /// </summary>
    /// <param name="course">The layout of the race course.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The winning score.
    /// </returns>
    public static int Race(IList<string> course, CancellationToken cancellationToken)
    {
        int height = course.Count;
        int width = course[0].Length;

        var start = (Point.Empty, Directions.Right);
        var finish = (Point.Empty, Size.Empty);
        var grid = new RaceCourse(width, height);

        for (int y = 0; y < height; y++)
        {
            string row = course[y];

            for (int x = 0; x < width; x++)
            {
                switch (row[x])
                {
                    case 'S':
                        start = (new(x, y), Directions.Right);
                        break;

                    case 'E':
                        finish = (new(x, y), Size.Empty);
                        break;

                    case '#':
                        grid.Borders.Add(new(x, y));
                        break;

                    default:
                        grid.Locations.Add(new(x, y));
                        break;
                }
            }
        }

        return (int)PathFinding.AStar(grid, start, finish, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        WinningScore = Race(values, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("The lowest score a Reindeer could possibly get is {0}.", WinningScore);
        }

        return PuzzleResult.Create(WinningScore);
    }

    private sealed class RaceCourse(int width, int height) : IWeightedGraph<Move>
    {
        private static readonly ImmutableArray<Size> Vectors =
        [
            Directions.Down,
            Directions.Right,
            Directions.Up,
            Directions.Left,
        ];

        private readonly Rectangle _bounds = new(0, 0, width, height);

        public HashSet<Point> Borders { get; } = [];

        public HashSet<Point> Locations { get; } = [];

        public long Cost(Move a, Move b)
        {
            long cost = 1;

            if (a.Direction != b.Direction)
            {
                cost += 1_000;
            }

            return cost;
        }

        public bool Equals(Move x, Move y) => x.Location == y.Location;

        public int GetHashCode([DisallowNull] Move obj) => obj.Location.GetHashCode();

        public IEnumerable<Move> Neighbors(Move id)
        {
            for (int i = 0; i < Vectors.Length; i++)
            {
                var direction = Vectors[i];
                var next = id.Location + direction;

                if (!Borders.Contains(next) && _bounds.Contains(next))
                {
                    yield return (next, direction);
                }
            }
        }
    }
}
