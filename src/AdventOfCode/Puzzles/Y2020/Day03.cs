// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2020/day/3</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2020, 03, "Toboggan Trajectory", RequiresData = true)]
public sealed class Day03 : Puzzle
{
    /// <summary>
    /// Gets the number of trees collided with when traversing the grid.
    /// </summary>
    public long TreeCollisions { get; private set; }

    /// <summary>
    /// Gets the product of the number of trees collided with when traversing the grids.
    /// </summary>
    public long ProductOfTreeCollisions { get; private set; }

    /// <summary>
    /// Gets the number of trees that would be collided with by
    /// traversing the specified grid in the specified direction.
    /// </summary>
    /// <param name="grid">The grid to traverse.</param>
    /// <param name="x">The horizontal component of the slope to traverse with.</param>
    /// <param name="y">The vertical component of the slope to traverse with.</param>
    /// <returns>
    /// The number of trees that would be collided with when traversing the grid.
    /// </returns>
    public static int GetTreeCollisionCount(IList<string> grid, int x, int y)
    {
        int width = grid[0].Length;
        int height = grid.Count;

        bool[,] space = new bool[height, width];

        for (int i = 0; i < height; i++)
        {
            string row = grid[i];

            for (int j = 0; j < width; j++)
            {
                if (row[j] == '#')
                {
                    space[i, j] = true;
                }
            }
        }

        var deltaX = new Size(x, 0);
        var deltaY = new Size(0, y);
        var wrap = new Size(width, 0);

        int collisions = 0;
        var position = Point.Empty;

        while (position.Y < height)
        {
            if (space[position.Y, position.X])
            {
                collisions++;
            }

            position += deltaX;
            position += deltaY;

            if (position.X >= width)
            {
                position -= wrap;
            }
        }

        return collisions;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var grid = await ReadResourceAsLinesAsync(cancellationToken);

        var slopes = new[]
        {
            new Point(1, 1),
            new Point(5, 1),
            new Point(7, 1),
            new Point(1, 2),
        };

        TreeCollisions = GetTreeCollisionCount(grid, 3, 1);

        long product = TreeCollisions;

        for (int i = 0; i < slopes.Length; i++)
        {
            var slope = slopes[i];
            product *= GetTreeCollisionCount(grid, slope.X, slope.Y);
        }

        ProductOfTreeCollisions = product;

        if (Verbose)
        {
            Logger.WriteLine("{0} trees would be encountered using a right-3/down-1 slope.", TreeCollisions);
            Logger.WriteLine("The product of the collisions from traversing the slopes is {0}.", ProductOfTreeCollisions);
        }

        return PuzzleResult.Create(TreeCollisions, ProductOfTreeCollisions);
    }
}
