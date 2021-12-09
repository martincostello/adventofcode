// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/24</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2016, 24, RequiresData = true)]
public sealed class Day24 : Puzzle
{
    /// <summary>
    /// Gets the fewest steps that can be taken to visit all of the locations.
    /// </summary>
    public int FewestStepsToVisitAllLocations { get; private set; }

    /// <summary>
    /// Gets the fewest steps that can be taken to visit all of the locations and return to the origin.
    /// </summary>
    public int FewestStepsToVisitAllLocationsAndReturn { get; private set; }

    /// <summary>
    /// Returns the minimum number of steps required to visit all of the locations in the maze.
    /// </summary>
    /// <param name="layout">The layout of the maze.</param>
    /// <param name="returnToOrigin">Whether to return to the origin point.</param>
    /// <returns>
    /// The minimum number of steps required to visit all locations.
    /// </returns>
    public static int GetMinimumStepsToVisitLocations(IList<string> layout, bool returnToOrigin)
    {
        (SquareGrid maze, Point origin, IList<Point> waypoints) = BuildMaze(layout);

        double minimumCost = double.MaxValue;

        var costs = new Dictionary<(Point A, Point B), double>();

        Point[] allWaypoints = waypoints
            .Prepend(origin)
            .ToArray();

        foreach (Point a in allWaypoints)
        {
            foreach (Point b in allWaypoints)
            {
                if (a == b)
                {
                    break;
                }

                costs[(b, a)] = costs[(a, b)] = PathFinding.AStar(maze, a, b, (a, b) => maze.Cost(a, b));
            }
        }

        foreach (IEnumerable<Point> goals in Maths.GetPermutations(waypoints))
        {
            double cost = 0;
            Point current = origin;

            foreach (Point goal in goals)
            {
                cost += costs[(current, goal)];

                if (cost > minimumCost)
                {
                    break;
                }

                current = goal;
            }

            if (returnToOrigin && cost < minimumCost)
            {
                cost += costs[(current, origin)];
            }

            if (cost < minimumCost)
            {
                minimumCost = cost;
            }
        }

        return (int)minimumCost;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> layout = await ReadResourceAsLinesAsync();

        FewestStepsToVisitAllLocations = GetMinimumStepsToVisitLocations(layout, returnToOrigin: false);
        FewestStepsToVisitAllLocationsAndReturn = GetMinimumStepsToVisitLocations(layout, returnToOrigin: true);

        if (Verbose)
        {
            Logger.WriteLine(
                "The fewest number of steps required to visit every location is {0}.",
                FewestStepsToVisitAllLocations);

            Logger.WriteLine(
                "The fewest number of steps required to visit every location and return to the origin is {0}.",
                FewestStepsToVisitAllLocationsAndReturn);
        }

        return PuzzleResult.Create(FewestStepsToVisitAllLocations, FewestStepsToVisitAllLocationsAndReturn);
    }

    /// <summary>
    /// Builds a maze with the specified layout.
    /// </summary>
    /// <param name="layout">The layout of the grid.</param>
    /// <returns>
    /// A <see cref="SquareGrid"/> representing the maze, the origin point and the goal locations in the maze.
    /// </returns>
    private static (SquareGrid Maze, Point Origin, IList<Point> Goals) BuildMaze(IList<string> layout)
    {
        var maze = new Maze(layout[0].Length, layout.Count);
        var origin = Point.Empty;
        var waypoints = new List<Point>();

        for (int y = 0; y < maze.Height; y++)
        {
            for (int x = 0; x < maze.Width; x++)
            {
                char content = layout[y][x];

                if (content == '#')
                {
                    maze.Borders.Add(new(x, y));
                }
                else if (content == '0')
                {
                    origin = new(x, y);
                }
                else if (content != '.')
                {
                    waypoints.Add(new(x, y));
                }
            }
        }

        return (maze, origin, waypoints);
    }

    private sealed class Maze : SquareGrid
    {
        public Maze(int width, int height)
            : base(width, height)
        {
        }

        public override double Cost(Point a, Point b)
            => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }
}
