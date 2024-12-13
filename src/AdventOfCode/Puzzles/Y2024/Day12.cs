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
    /// Gets the total price for the fencing without a bulk discount.
    /// </summary>
    public int TotalPriceWithoutDiscount { get; private set; }

    /// <summary>
    /// Gets the total price for the fencing with a bulk discount.
    /// </summary>
    public int TotalPriceWithDiscount { get; private set; }

    /// <summary>
    /// Computes the price of adding fencing to separate the regions of the specified map.
    /// </summary>
    /// <param name="map">The map to compute the fencing price for.</param>
    /// <param name="bulkDiscount">Whether to compute the price for a bulk discount.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The total price for the fencing.
    /// </returns>
    public static int Compute(IList<string> map, bool bulkDiscount, CancellationToken cancellationToken)
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
            int factor = bulkDiscount ? Sides(region) : Perimeter(region);
            price += region.Count * factor;
        }

        return price;

        static int Perimeter(HashSet<Point> region)
        {
            int perimeter = 0;

            foreach (var point in region)
            {
                var direction = new Size(0, 1);

                for (int i = 0; i < 4; i++)
                {
                    if (!region.Contains(point + direction))
                    {
                        perimeter++;
                    }

                    direction = Right(direction);
                }
            }

            return perimeter;
        }

        static int Sides(HashSet<Point> region)
            => Perimeter(region); // TODO Walk all the borders of the region and count the number of turns

        static Size Right(Size direction) => new(-direction.Height, direction.Width);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        TotalPriceWithoutDiscount = Compute(values, bulkDiscount: false, cancellationToken);
        TotalPriceWithDiscount = Compute(values, bulkDiscount: true, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("The total price of fencing all regions of the map is {0} with no discount.", TotalPriceWithoutDiscount);
            Logger.WriteLine("The total price of fencing all regions of the map is {0} with a bulk discount.", TotalPriceWithDiscount);
        }

        return PuzzleResult.Create(TotalPriceWithoutDiscount, TotalPriceWithDiscount);
    }

    private sealed class Garden(char plant, IList<string> plants, Rectangle bounds) : SquareGrid(bounds)
    {
        public override bool IsPassable(Point id) => plants[id.Y][id.X] == plant;
    }
}
