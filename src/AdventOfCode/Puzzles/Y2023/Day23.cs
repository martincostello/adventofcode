// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/23</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 23, "A Long Walk", RequiresData = true)]
public sealed class Day23 : Puzzle
{
    /// <summary>
    /// Gets the number of steps for the longest walk through the hiking trail.
    /// </summary>
    public int MaximumSteps { get; private set; }

    /// <summary>
    /// Walks the specified hiking trail and returns the number of steps for the longest walk.
    /// </summary>
    /// <param name="hikingTrail">The hiking trail.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>
    /// The number of steps for the longest walk through the hiking trail.
    /// </returns>
    public static int Walk(IList<string> hikingTrail, CancellationToken cancellationToken)
    {
        int height = hikingTrail.Count;
        int width = hikingTrail[0].Length;

        var trail = new HikingTrail(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                switch (hikingTrail[y][x])
                {
                    case '#':
                        trail.Borders.Add(new(x, y));
                        break;

                    case '<':
                        trail.Slopes[new(x, y)] = Directions.Left;
                        break;

                    case '>':
                        trail.Slopes[new(x, y)] = Directions.Right;
                        break;

                    case '^':
                        trail.Slopes[new(x, y)] = Directions.Up;
                        break;

                    case 'v':
                        trail.Slopes[new(x, y)] = Directions.Down;
                        break;

                    default:
                        trail.Locations.Add(new(x, y));
                        break;
                }
            }
        }

        var start = new Point(1, 0);
        var goal = new Point(width - 2, height - 1);

        return (int)PathFinding.AStar(trail, start, goal, cancellationToken);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var hikingTrail = await ReadResourceAsLinesAsync(cancellationToken);

        MaximumSteps = Walk(hikingTrail, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("The longest hike is {0} steps.", MaximumSteps);
        }

        return PuzzleResult.Create(MaximumSteps);
    }

    private sealed class HikingTrail(int width, int height) : SquareGrid(width, height)
    {
        public Dictionary<Point, Size> Slopes { get; } = [];

        public override IEnumerable<Point> Neighbors(Point id)
        {
            if (Slopes.TryGetValue(id, out var slope))
            {
                yield return id + slope;
                yield break;
            }

            foreach (var item in base.Neighbors(id))
            {
                yield return item;
            }
        }
    }
}
