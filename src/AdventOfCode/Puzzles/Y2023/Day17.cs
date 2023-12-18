// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/17</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 17, "Clumsy Crucible", RequiresData = true)]
public sealed class Day17 : Puzzle
{
    /// <summary>
    /// Gets the minimum heat loss that can be incurred.
    /// </summary>
    public int MinimumHeatLoss { get; private set; }

    /// <summary>
    /// Finds the minimum heat loss that can be incurred moving the crucible.
    /// </summary>
    /// <param name="map">A map of the heat losses that are incurred.</param>
    /// <returns>
    /// The minimum heat loss that can be incurred moving the crucible.
    /// </returns>
    public static int Solve(IList<string> map)
    {
        var heatLosses = new Dictionary<Point, int>();

        for (int y = 0; y < map.Count; y++)
        {
            string row = map[y];

            for (int x = 0; x < row.Length; x++)
            {
                heatLosses[new(x, y)] = row[x] - '0';
            }
        }

        int height = map.Count;
        int width = map[0].Length;

        var grid = new HeatLossMap(width, height, heatLosses);
        var goal = new Point(width - 1, height - 1);

        return (int)PathFinding.AStar(grid, new(Point.Empty, Size.Empty, 0), new(goal, Size.Empty, 0));
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        MinimumHeatLoss = Solve(values);

        if (Verbose)
        {
            Logger.WriteLine("The minimum heat loss that can be incurred is {0}.", MinimumHeatLoss);
        }

        return PuzzleResult.Create(MinimumHeatLoss);
    }

    private record struct Move(Point Location, Size Direction, int Steps)
    {
        public override readonly int GetHashCode() => Location.GetHashCode();
    }

    private sealed class HeatLossMap(int width, int height, Dictionary<Point, int> heatLosses) : IWeightedGraph<Move>
    {
        private static readonly ImmutableArray<Size> Directions =
        [
            new(0, 1),
            new(1, 0),
            new(0, -1),
            new(-1, 0),
        ];

        private readonly Rectangle _bounds = new(0, 0, width, height);

        public long Cost(Move a, Move b)
        {
            if (a.Location == b.Location)
            {
                return 0;
            }
            else if (a.Location.ManhattanDistance(b.Location) is 1)
            {
                return heatLosses.GetValueOrDefault(b.Location);
            }
            else
            {
                return long.MaxValue;
            }
        }

        public bool Equals(Move x, Move y) => x.Location == y.Location;

        public int GetHashCode(Move obj) => obj.Location.GetHashCode();

        public IEnumerable<Move> Neighbors(Move id)
        {
            var backwards = id.Direction * -1;

            for (int i = 0; i < Directions.Length; i++)
            {
                var direction = Directions[i];

                if (direction == backwards)
                {
                    continue;
                }

                var next = id.Location + direction;

                if (_bounds.Contains(next))
                {
                    int steps = direction == id.Direction ? id.Steps + 1 : 0;

                    if (steps < 4)
                    {
                        yield return new(next, direction, steps);
                    }
                }
            }
        }
    }
}
