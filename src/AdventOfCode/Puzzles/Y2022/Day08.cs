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
        var trees = Parse(grid);
        int count = CountVisibleTrees(trees);
        int score = GetMaximumScenicScore(trees);

        return (count, score);

        static Dictionary<Point, Tree> Parse(IList<string> grid)
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

            return trees;
        }

        static int CountVisibleTrees(Dictionary<Point, Tree> trees)
        {
            int height = trees.Keys.Max((p) => p.Y) + 1;
            int width = trees.Keys.Max((p) => p.X) + 1;

            var topLeft = new Point(0, 0);
            var topRight = new Point(width - 1, 0);
            var bottomLeft = new Point(width - 1, height - 1);

            var directions = new (Point Origin, Size Next, Size Forward)[]
            {
                (topLeft,    Directions.Down,  Directions.Right), // Left to right
                (topRight,   Directions.Down,  Directions.Left), // Right to left
                (topLeft,    Directions.Right, Directions.Down), // Top to bottom
                (bottomLeft, Directions.Left,  Directions.Up), // Bottom to top
            };

            foreach (var (first, next, direction) in directions)
            {
                var origin = first;

                do
                {
                    Sweep(origin, direction, trees);
                    origin += next;
                }
                while (trees.ContainsKey(origin));
            }

            return trees.Count((p) => p.Value.IsVisible);
        }

        static void Sweep(Point origin, Size direction, Dictionary<Point, Tree> trees)
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

        static int Score(Tree origin, Size direction, Dictionary<Point, Tree> trees)
        {
            var location = origin.Location;

            int score = 0;

            while (trees.TryGetValue(location += direction, out var next))
            {
                score++;

                if (next.Height >= origin.Height)
                {
                    break;
                }
            }

            return score;
        }

        static int GetMaximumScenicScore(Dictionary<Point, Tree> trees)
        {
            int height = trees.Keys.Max((p) => p.Y) + 1;
            int width = trees.Keys.Max((p) => p.X) + 1;

            var directions = new[]
            {
                Directions.Up,
                Directions.Down,
                Directions.Left,
                Directions.Right,
            };

            int maximum = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var tree = trees[new(x, y)];

                    if (tree.IsVisible)
                    {
                        int scenicScore = 1;

                        foreach (var direction in directions)
                        {
                            scenicScore *= Score(tree, direction, trees);
                        }

                        maximum = Math.Max(scenicScore, maximum);
                    }
                }
            }

            return maximum;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var grid = await ReadResourceAsLinesAsync(cancellationToken);

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
