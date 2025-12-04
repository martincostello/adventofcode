// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/4</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 04, "Printing Department", RequiresData = true)]
public sealed class Day04 : Puzzle
{
    /// <summary>
    /// Gets the number of rolls of paper that can be initially accessed by a forklift.
    /// </summary>
    public int AccessibleRolls { get; private set; }

    /// <summary>
    /// Gets the number of rolls of paper that can be removed by a forklift.
    /// </summary>
    public int RemovedRolls { get; private set; }

    /// <summary>
    /// Gets the number of rolls of paper that can be accessed by a forklift
    /// as illustrated by the specified diagram.
    /// </summary>
    /// <param name="diagram">A diagram of the locations of the rolls of paper.</param>
    /// <param name="cancellationToken">The optional <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The number of rolls of paper that can be accessed by a forklift.
    /// </returns>
    public static (int Accessible, int Removed) ArrangeRolls(IReadOnlyList<string> diagram, CancellationToken cancellationToken = default)
    {
        var bounds = new Rectangle(0, 0, diagram[0].Length, diagram.Count);
        var warehouse = new Warehouse(bounds);

        warehouse.Visit(diagram, static (grid, location, contents) =>
        {
            if (contents is '@')
            {
                grid.Locations.Add(location);
            }
        });

        var accessible = new HashSet<Point>();

        warehouse.VisitLocations(accessible, FindAccessible);

        int initialAccessible = accessible.Count;
        int initialRolls = warehouse.Locations.Count;

        while (accessible.Count > 0 && !cancellationToken.IsCancellationRequested)
        {
            warehouse.Locations.ExceptWith(accessible);
            accessible.Clear();
            warehouse.VisitLocations(accessible, FindAccessible);
        }

        int removed = initialRolls - warehouse.Locations.Count;

        cancellationToken.ThrowIfCancellationRequested();

        return (initialAccessible, removed);

        static void FindAccessible(SquareGrid grid, Point location, HashSet<Point> accessible)
        {
            const int MoveableLimit = 3;
            int neighbors = 0;

            foreach (var neighbor in grid.Neighbors(location))
            {
                if (grid.Locations.Contains(neighbor))
                {
                    neighbors++;
                }

                if (neighbors > MoveableLimit)
                {
                    return;
                }
            }

            if (neighbors <= MoveableLimit)
            {
                accessible.Add(location);
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var diagram = await ReadResourceAsLinesAsync(cancellationToken);

        (AccessibleRolls, RemovedRolls) = ArrangeRolls(diagram, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("{0} rolls of paper can be accessed by a forklift.", AccessibleRolls);
            Logger.WriteLine("{0} rolls of paper can be removed by a forklift.", RemovedRolls);
        }

        return PuzzleResult.Create(AccessibleRolls, RemovedRolls);
    }

    private sealed class Warehouse(Rectangle bounds) : SquareGrid(bounds)
    {
        protected override ImmutableArray<Size> Vectors { get; } =
        [
            new(0, 1),
            new(1, 0),
            new(0, -1),
            new(-1, 0),
            new(1, 1),
            new(1, -1),
            new(-1, 1),
            new(-1, -1),
        ];
    }
}
