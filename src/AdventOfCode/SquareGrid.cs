// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode;

/// <summary>
/// A class representing a square grid of locations. This class cannot be inherited.
/// </summary>
/// <remarks>
/// Based on <c>https://www.redblobgames.com/pathfinding/a-star/implementation.html</c>.
/// </remarks>
public abstract class SquareGrid : IWeightedGraph<Point>
{
    /// <summary>
    /// The valid vectors of movement around the grid.
    /// </summary>
    private static readonly ImmutableArray<Size> Vectors =
    [
        new(0, 1),
        new(1, 0),
        new(0, -1),
        new(-1, 0),
    ];

    /// <summary>
    /// Initializes a new instance of the <see cref="SquareGrid"/> class.
    /// </summary>
    /// <param name="width">The width of the grid.</param>
    /// <param name="height">The height of the grid.</param>
    protected SquareGrid(int width, int height)
    {
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Gets the height of the grid.
    /// </summary>
    public int Height { get; private set; }

    /// <summary>
    /// Gets the width of the grid.
    /// </summary>
    public int Width { get; private set; }

    /// <summary>
    /// Gets the locations within the grid.
    /// </summary>
    public HashSet<Point> Locations { get; } = new();

    /// <summary>
    /// Gets the borders within the grid.
    /// </summary>
    public HashSet<Point> Borders { get; } = new();

    /// <summary>
    /// Returns whether the specified point is in the bounds of the grid.
    /// </summary>
    /// <param name="id">The Id of the point.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="id"/> is in bounds; otherwise <see langword="false"/>.
    /// </returns>
    public bool InBounds(Point id) => id.X >= 0 && id.X < Width && id.Y >= 0 && id.Y < Height;

    /// <summary>
    /// Returns whether the specified point is passable.
    /// </summary>
    /// <param name="id">The Id of the point.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="id"/> is passable; otherwise <see langword="false"/>.
    /// </returns>
    public bool IsPassable(Point id) => !Borders.Contains(id);

    /// <inheritdoc/>
    public abstract long Cost(Point a, Point b);

    /// <inheritdoc/>
    public virtual IEnumerable<Point> Neighbors(Point id)
    {
        for (int i = 0; i < Vectors.Length; i++)
        {
            Point next = id + Vectors[i];

            if (InBounds(next) && IsPassable(next))
            {
                yield return next;
            }
        }
    }
}
