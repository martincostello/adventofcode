// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/12</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 12, "Garden Groups", RequiresData = true)]
public sealed class Day12 : Puzzle
{
    /// <summary>
    /// Gets the total price for the fencing.
    /// </summary>
    public int TotalPrice { get; private set; }

    /// <summary>
    /// Computes the price of adding fencing to separate the regions of the specified map.
    /// </summary>
    /// <param name="map">The map to compute the fencing price for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The total price for the fencing.
    /// </returns>
    public static int Compute(IList<string> map, CancellationToken cancellationToken)
    {
        var bounds = new Rectangle(0, 0, map[0].Length, map.Count);
        var regions = new List<HashSet<Point>>();
        var unexplored = new HashSet<Point>();

        for (int y = 0; y < map.Count; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                unexplored.Add(new(x, y));
            }
        }

        while (unexplored.Count > 0 && !cancellationToken.IsCancellationRequested)
        {
            var origin = unexplored.First();
            char plant = map[origin.Y][origin.X];

            var garden = new Garden(plant, map, bounds);

            var region = PathFinding.BreadthFirst(garden, origin, cancellationToken);
            regions.Add(region);

            unexplored.ExceptWith(region);
        }

        cancellationToken.ThrowIfCancellationRequested();

        int price = 0;

        foreach (var region in regions)
        {
            int perimeter = 0;

            foreach (var point in region)
            {
                if (!region.Contains(new(point.X, point.Y - 1)))
                {
                    perimeter++;
                }

                if (!region.Contains(new(point.X, point.Y + 1))!)
                {
                    perimeter++;
                }

                if (!region.Contains(new(point.X - 1, point.Y)))
                {
                    perimeter++;
                }

                if (!region.Contains(new(point.X + 1, point.Y)))
                {
                    perimeter++;
                }
            }

            price += region.Count * perimeter;
        }

        return price;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        TotalPrice = Compute(values, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("The total price of fencing all regions of the map is {0}.", TotalPrice);
        }

        return PuzzleResult.Create(TotalPrice);
    }

    private sealed class Garden(char plant, IList<string> plants, Rectangle bounds) : SquareGrid(bounds)
    {
        public override bool IsPassable(Point id) => plants[id.Y][id.X] == plant;
    }
}
