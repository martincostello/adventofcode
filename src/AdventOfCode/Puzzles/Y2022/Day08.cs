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

        var trees = new List<List<Tree>>(grid.Count);

        foreach (string row in grid)
        {
            var rowTrees = new List<Tree>(row.Length);

            foreach (char tree in row)
            {
                rowTrees.Add(new Tree(tree - '0'));
            }

            trees.Add(rowTrees);
        }

        // Left to right
        for (int y = 0; y < height; y++)
        {
            var previous = trees[y][0];
            previous.MarkVisible();

            for (int x = 1; x < width; x++)
            {
                var nextTree = trees[y][x];

                if (nextTree.Height > previous.Height)
                {
                    nextTree.MarkVisible();
                    previous = nextTree;
                }
            }
        }

        // Right to left
        for (int y = 0; y < height; y++)
        {
            var previous = trees[y][width - 1];
            previous.MarkVisible();

            for (int x = width - 2; x > -1; x--)
            {
                var nextTree = trees[y][x];

                if (nextTree.Height > previous.Height)
                {
                    nextTree.MarkVisible();
                    previous = nextTree;
                }
            }
        }

        // Top to bottom
        for (int x = 0; x < width; x++)
        {
            var previous = trees[0][x];
            previous.MarkVisible();

            for (int y = 1; y < height; y++)
            {
                var nextTree = trees[y][x];

                if (nextTree.Height > previous.Height)
                {
                    nextTree.MarkVisible();
                    previous = nextTree;
                }
            }
        }

        // Bottom to top
        for (int x = 0; x < width; x++)
        {
            var previous = trees[height - 1][x];
            previous.MarkVisible();

            for (int y = height - 2; y > -1; y--)
            {
                var nextTree = trees[y][x];

                if (nextTree.Height > previous.Height)
                {
                    nextTree.MarkVisible();
                    previous = nextTree;
                }
            }
        }

        int visibleTrees = trees
            .SelectMany((p) => p)
            .Count((p) => p.IsVisible);

        int maximumScenicScore = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var tree = trees[y][x];

                if (tree.IsVisible)
                {
                    int scoreUp = 0;
                    int scoreDown = 0;
                    int scoreLeft = 0;
                    int scoreRight = 0;

                    // Up
                    for (int delta = y - 1; delta > -1; delta--)
                    {
                        var next = trees[delta][x];

                        scoreUp++;

                        if (next.Height >= tree.Height)
                        {
                            break;
                        }
                    }

                    // Down
                    for (int delta = y + 1; delta < height; delta++)
                    {
                        var next = trees[delta][x];

                        scoreDown++;

                        if (next.Height >= tree.Height)
                        {
                            break;
                        }
                    }

                    // Left
                    for (int delta = x - 1; delta > -1; delta--)
                    {
                        var next = trees[y][delta];

                        scoreLeft++;

                        if (next.Height >= tree.Height)
                        {
                            break;
                        }
                    }

                    // Right
                    for (int delta = x + 1; delta < width; delta++)
                    {
                        var next = trees[y][delta];

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

    private sealed record Tree(int Height)
    {
        public bool IsVisible { get; private set; }

        public void MarkVisible() => IsVisible = true;
    }
}
