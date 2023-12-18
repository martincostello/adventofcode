// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace MartinCostello.AdventOfCode;

/// <summary>
/// A class representing a square grid of locations. This class cannot be inherited.
/// </summary>
/// <remarks>
/// Based on <c>https://www.redblobgames.com/pathfinding/a-star/implementation.html</c>.
/// </remarks>
public class SquareGrid(int width, int height) : IWeightedGraph<Point>
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
    /// Gets the height of the grid.
    /// </summary>
    public int Height => height;

    /// <summary>
    /// Gets the width of the grid.
    /// </summary>
    public int Width => width;

    /// <summary>
    /// Gets the locations within the grid.
    /// </summary>
    public HashSet<Point> Locations { get; } = [];

    /// <summary>
    /// Gets the borders within the grid.
    /// </summary>
    public HashSet<Point> Borders { get; } = [];

    /// <inheritdoc/>
    public bool Equals(Point x, Point y) => x == y;

    /// <inheritdoc/>
    public int GetHashCode([DisallowNull] Point obj) => obj.GetHashCode();

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
    public virtual long Cost(Point a, Point b) => 1;

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
