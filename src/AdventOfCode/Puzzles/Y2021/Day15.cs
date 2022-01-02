// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/15</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 15, "Chiton", RequiresData = true)]
public sealed class Day15 : Puzzle
{
    /// <summary>
    /// Gets the path through the cave with the lowest total risk using the small map.
    /// </summary>
    public int RiskLevelSmall { get; private set; }

    /// <summary>
    /// Gets the path through the cave with the lowest total risk using the large map.
    /// </summary>
    public int RiskLevelLarge { get; private set; }

    /// <summary>
    /// Gets the minimum risk level from the specified path.
    /// </summary>
    /// <param name="riskMap">The map of the risk in the cave to determine the risk level for.</param>
    /// <param name="largeMap">Whether the map is actually 5 times larger than specified.</param>
    /// <returns>
    /// The path through the cave with the lowest total risk.
    /// </returns>
    public static int GetRiskLevel(IList<string> riskMap, bool largeMap)
    {
        const int LargeMapFactor = 5;

        int width = riskMap[0].Length;
        int height = riskMap.Count;

        int capacity = width * height;

        if (largeMap)
        {
            capacity *= LargeMapFactor;
            capacity *= LargeMapFactor;
        }

        var risks = new Dictionary<Point, int>(capacity);

        for (int y = 0; y < height; y++)
        {
            string row = riskMap[y];

            for (int x = 0; x < width; x++)
            {
                risks[new(x, y)] = row[x] - '0';
            }
        }

        if (largeMap)
        {
            int initialHeight = height;
            int initialWidth = width;

            height *= LargeMapFactor;
            width *= LargeMapFactor;

            var firstArea = new HashSet<Point>(risks.Keys);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var point = new Point(x, y);

                    if (firstArea.Contains(point))
                    {
                        // We already have the risk level
                        continue;
                    }

                    if (!risks.TryGetValue(new(point.X, point.Y - initialHeight), out int risk))
                    {
                        risk = risks[new(point.X - initialWidth, point.Y)];
                    }

                    risks[point] = risk > 8 ? 1 : ++risk;
                }
            }
        }

        var map = new RiskMap(width, height, risks);

        foreach (Point point in risks.Keys)
        {
            map.Locations.Add(point);
        }

        var start = Point.Empty;
        var goal = new Point(width - 1, height - 1);

        return (int)PathFinding.AStar(map, start, goal);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> riskMap = await ReadResourceAsLinesAsync();

        RiskLevelSmall = GetRiskLevel(riskMap, largeMap: false);
        RiskLevelLarge = GetRiskLevel(riskMap, largeMap: true);

        if (Verbose)
        {
            Logger.WriteLine(
                "The lowest total risk of any path from the top left to the bottom right using the small map is {0:N0}.",
                RiskLevelSmall);

            Logger.WriteLine(
                "The lowest total risk of any path from the top left to the bottom right using the large map is {0:N0}.",
                RiskLevelLarge);
        }

        return PuzzleResult.Create(RiskLevelSmall, RiskLevelLarge);
    }

    private sealed class RiskMap : SquareGrid
    {
        private readonly Dictionary<Point, int> _risks;

        public RiskMap(int width, int height, Dictionary<Point, int> risks)
            : base(width, height)
        {
            _risks = risks;
        }

        public override long Cost(Point a, Point b) => _risks.GetValueOrDefault(b);
    }
}
