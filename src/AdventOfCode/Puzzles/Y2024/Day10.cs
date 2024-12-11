// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/10</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 10, "Hoof It", RequiresData = true)]
public sealed class Day10 : Puzzle
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
    public static (int SumOfScores, int SumOfRatings) Explore(IList<string> map)
    {
        var grid = new TopographicMap(new(0, 0, map[0].Length, map.Count));

        for (int y = 0; y < map.Count; y++)
        {
            string row = map[y];

            for (int x = 0; x < row.Length; x++)
            {
                char tile = row[x];

                if (!char.IsDigit(tile))
                {
                    continue;
                }

                grid.Heights[new(x, y)] = tile - '0';
            }
        }

        int scoresSum = 0;
        int ratingsSum = 0;

        for (int y = 0; y < map.Count; y++)
        {
            string row = map[y];

            for (int x = 0; x < row.Length; x++)
            {
                var origin = new Point(x, y);

                if (!grid.Heights.TryGetValue(origin, out int height) || height is not 0)
                {
                    continue;
                }

                var summits = new HashSet<Point>();

                ratingsSum += Explore(grid, origin, 0, summits);
                scoresSum += summits.Count;
            }
        }

        return (scoresSum, ratingsSum);

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

        return PuzzleResult.Create(SumOfScores, SumOfRatings);
    }

    private sealed class TopographicMap(Rectangle bounds) : SquareGrid(bounds)
    {
        public Dictionary<Point, int> Heights { get; } = new(bounds.Area());
    }
}
