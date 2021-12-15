// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/15</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 15, RequiresData = true)]
public sealed class Day15 : Puzzle
{
    /// <summary>
    /// Gets the path through the cave with the lowest total risk.
    /// </summary>
    public int RiskLevel { get; private set; }

    /// <summary>
    /// Gets the minimum risk level from the specified path.
    /// </summary>
    /// <param name="riskMap">The map of the risk in the cave to determine the risk level for.</param>
    /// <returns>
    /// The path through the cave with the lowest total risk.
    /// </returns>
    public static int GetRiskLevel(IList<string> riskMap)
    {
        int width = riskMap[0].Length;
        int height = riskMap.Count;

        var risks = new Dictionary<Point, int>(width * height);

        for (int y = 0; y < height; y++)
        {
            string row = riskMap[y];

            for (int x = 0; x < width; x++)
            {
                risks[new(x, y)] = row[x] - '0';
            }
        }

        var map = new RiskMap(width, height, risks);

        foreach (Point point in risks.Keys)
        {
            map.Locations.Add(point);
        }

        var start = new Point(0, 0);
        var goal = new Point(width - 1, height - 1);

        return (int)PathFinding.AStar(
            map,
            start,
            goal,
            (x, y) => map.Cost(x, y));
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> riskMap = await ReadResourceAsLinesAsync();

        RiskLevel = GetRiskLevel(riskMap);

        if (Verbose)
        {
            Logger.WriteLine("The lowest total risk of any path from the top left to the bottom right is {0:N0}.", RiskLevel);
        }

        return PuzzleResult.Create(RiskLevel);
    }

    private sealed class RiskMap : SquareGrid
    {
        private readonly Dictionary<Point, int> _risks;

        public RiskMap(int width, int height, Dictionary<Point, int> risks)
            : base(width, height)
        {
            _risks = risks;
        }

        public override double Cost(Point a, Point b) => _risks.GetValueOrDefault(b);
    }
}
