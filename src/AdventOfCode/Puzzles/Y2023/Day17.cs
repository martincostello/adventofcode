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

        var start = new Move(Point.Empty, Size.Empty, 0);
        var goal = new Point(width - 1, height - 1);

        var comparer = EqualityComparer<Move>.Create((x, y) => x.Location == goal);

        int costFromLeft = (int)PathFinding.AStar(grid, start, new(goal, Directions.Right, 0), comparer);
        int costFromAbove = (int)PathFinding.AStar(grid, start, new(goal, Directions.Down, 0), comparer);

        return Math.Min(costFromLeft, costFromAbove);
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

    private readonly record struct Move(Point Location, Size Direction, int Steps) : IEquatable<Move>
    {
        public readonly bool Equals(Move other)
            => Location == other.Location && Direction == other.Direction;

        public override readonly int GetHashCode()
            => HashCode.Combine(Location, Direction);
    }

    private sealed class HeatLossMap(int width, int height, Dictionary<Point, int> heatLosses) : IWeightedGraph<Move>
    {
        private readonly Rectangle _bounds = new(0, 0, width, height);

        public long Cost(Move a, Move b) => heatLosses.GetValueOrDefault(b.Location);

        public bool Equals(Move x, Move y) => x.Equals(y);

        public int GetHashCode(Move obj) => obj.GetHashCode();

        public IEnumerable<Move> Neighbors(Move id)
        {
            var backwards = id.Direction * -1;
            var directions = Directions.All;

            for (int i = 0; i < directions.Count; i++)
            {
                var direction = directions[i];

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
