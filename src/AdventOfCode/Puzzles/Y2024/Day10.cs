// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/10</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 10, "Hoof It", RequiresData = true)]
public sealed class Day10 : Puzzle<int, int>
{
    /// <summary>
    /// Gets the sum of the trailhead scores.
    /// </summary>
    public int SumOfScores { get; private set; }

    /// <summary>
    /// Gets the sum of the trailhead ratings.
    /// </summary>
    public int SumOfRatings { get; private set; }

    /// <summary>
    /// Explores the specified map of trails.
    /// </summary>
    /// <param name="map">The topographic map of trails.</param>
    /// <returns>
    /// The sum of the trailhead scores and ratings for the specified map.
    /// </returns>
    public static (int SumOfScores, int SumOfRatings) Explore(IReadOnlyList<string> map)
    {
        var grid = new TopographicMap(new(0, 0, map[0].Length, map.Count));

        grid.VisitCells(map, static (grid, point, tile) =>
        {
            if (char.IsDigit(tile))
            {
                grid.Heights[point] = tile - '0';
            }
        });

        (int Summits, int Ratings) sums = (0, 0);

        return grid.VisitCells(sums, static (grid, origin, sums) =>
        {
            if (!grid.Heights.TryGetValue(origin, out int height) || height is not 0)
            {
                return sums;
            }

            var summits = new HashSet<Point>();

            int ratings = Explore(grid, origin, 0, summits);

            return (sums.Summits + summits.Count, sums.Ratings + ratings);
        });

        static int Explore(TopographicMap grid, Point origin, int height, HashSet<Point> summits)
        {
            if (height is 9)
            {
                summits.Add(origin);
                return 1;
            }

            int rating = 0;

            foreach (var next in grid.Neighbors(origin))
            {
                if (!grid.Heights.TryGetValue(next, out int nextHeight) || nextHeight - height is not 1)
                {
                    continue;
                }

                rating += Explore(grid, next, nextHeight, summits);
            }

            return rating;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        (SumOfScores, SumOfRatings) = Explore(values);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the scores of all trailheads in the topographic map is {0}.", SumOfScores);
            Logger.WriteLine("The sum of the ratings of all trailheads in the topographic map is {0}.", SumOfRatings);
        }

        Solution1 = SumOfScores;
        Solution2 = SumOfRatings;

        return Result();
    }

    private sealed class TopographicMap(Rectangle bounds) : SquareGrid(bounds)
    {
        public Dictionary<Point, int> Heights { get; } = new(bounds.Area());
    }
}
