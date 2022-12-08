// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/8</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 08, "Treetop Tree House", RequiresData = true)]
public sealed class Day08 : Puzzle
{
    /// <summary>
    /// Gets how many trees are visible from outside the grid.
    /// </summary>
    public int VisibleTrees { get; private set; }

    /// <summary>
    /// Gets the maximum scenic score that can be achieved for a treehouse in a tree in the grid.
    /// </summary>
    public int MaximumScenicScore { get; private set; }

    /// <summary>
    /// Returns how many trees are visible from outside the specified grid.
    /// </summary>
    /// <param name="grid">The grid of trees and heights.</param>
    /// <returns>
    /// How many trees are visible from outside the grid specified by <paramref name="grid"/>
    /// and the maximum scenic score that can be achieved for a treehouse in a tree in the grid.
    /// </returns>
    public static (int VisibleTrees, int MaximumScenicScore) CountVisibleTrees(IList<string> grid)
    {
        int height = grid.Count;
        int width = grid[0].Length;

        var trees = new Dictionary<Point, Tree>(height * width);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var location = new Point(x, y);
                trees[location] = new Tree(location, grid[y][x] - '0');
            }
        }

        var up = new Size(0, -1);
        var down = new Size(0, 1);
        var left = new Size(-1, 0);
        var right = new Size(1, 0);

        var topLeft = new Point(0, 0);
        var topRight = new Point(width - 1, 0);
        var bottomLeft = new Point(width - 1, height - 1);

        var directions = new (Point Origin, Size Next, Size Forward)[]
        {
            (topLeft,    down,  right), // Left to right
            (topRight,   down,  left), // Right to left
            (topLeft,    right, down), // Top to bottom
            (bottomLeft, left,  up), // Bottom to top
        };

        foreach (var (origin, next, forward) in directions)
        {
            var front = origin;

            do
            {
                Sweep(trees, front, forward);
                front += next;
            }
            while (trees.ContainsKey(front));
        }

        int visibleTrees = trees.Count((p) => p.Value.IsVisible);

        int maximumScenicScore = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var tree = trees[new(x, y)];

                if (tree.IsVisible)
                {
                    int scoreUp = 0;
                    int scoreDown = 0;
                    int scoreLeft = 0;
                    int scoreRight = 0;

                    // Up
                    for (int delta = y - 1; delta > -1; delta--)
                    {
                        var next = trees[new(x, delta)];

                        scoreUp++;

                        if (next.Height >= tree.Height)
                        {
                            break;
                        }
                    }

                    // Down
                    for (int delta = y + 1; delta < height; delta++)
                    {
                        var next = trees[new(x, delta)];

                        scoreDown++;

                        if (next.Height >= tree.Height)
                        {
                            break;
                        }
                    }

                    // Left
                    for (int delta = x - 1; delta > -1; delta--)
                    {
                        var next = trees[new(delta, y)];

                        scoreLeft++;

                        if (next.Height >= tree.Height)
                        {
                            break;
                        }
                    }

                    // Right
                    for (int delta = x + 1; delta < width; delta++)
                    {
                        var next = trees[new(delta, y)];

                        scoreRight++;

                        if (next.Height >= tree.Height)
                        {
                            break;
                        }
                    }

                    int scenicScore = scoreUp * scoreDown * scoreLeft * scoreRight;
                    maximumScenicScore = Math.Max(scenicScore, maximumScenicScore);
                }
            }
        }

        return (visibleTrees, maximumScenicScore);

        static void Sweep(Dictionary<Point, Tree> trees, Point origin, Size direction)
        {
            var location = origin;
            var current = trees[location];
            current.MarkVisible();

            while (trees.TryGetValue(location += direction, out var next))
            {
                if (next.Height > current.Height)
                {
                    next.MarkVisible();
                    current = next;
                }
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var grid = await ReadResourceAsLinesAsync();

        (VisibleTrees, MaximumScenicScore) = CountVisibleTrees(grid);

        if (Verbose)
        {
            Logger.WriteLine("There are {0} trees visible from outside the grid.", VisibleTrees);
            Logger.WriteLine("The highest scenic score is {0}.", MaximumScenicScore);
        }

        return PuzzleResult.Create(VisibleTrees, MaximumScenicScore);
    }

    private sealed record Tree(Point Location, int Height)
    {
        public bool IsVisible { get; private set; }

        public void MarkVisible() => IsVisible = true;
    }
}
