// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/18</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 18, "Lavaduct Lagoon", RequiresData = true, IsHidden = true)]
public sealed class Day18 : Puzzle
{
    /// <summary>
    /// Gets the volume of lava the lagoon can hold.
    /// </summary>
    public int Volume { get; private set; }

    /// <summary>
    /// Digs out the lagoon specified in the plan and returns its volume.
    /// </summary>
    /// <param name="plan">The plan to dig the perimeter of the lagoon.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>
    /// The volume of lava the lagoon can hold and a visualization of the lagoon.
    /// </returns>
    public static (int Volume, string Visualization) Dig(IList<string> plan, CancellationToken cancellationToken)
    {
        var location = Point.Empty;
        HashSet<Point> walls = [location];

        foreach (string instruction in plan)
        {
            instruction.AsSpan().Trifurcate(' ', out var direction, out var distance, out _);

            int steps = Parse<int>(distance);
            Size vector = direction[0] switch
            {
                'U' => new(0, -1),
                'D' => new(0, 1),
                'L' => new(-1, 0),
                'R' => new(1, 0),
                _ => throw new PuzzleException($"Unknown direction '{direction}'."),
            };

            for (int i = 0; i < steps; i++)
            {
                walls.Add(location += vector);
            }
        }

        int minX = walls.Min((p) => p.X);
        int minY = walls.Min((p) => p.Y);
        int maxX = walls.Max((p) => p.X);
        int maxY = walls.Max((p) => p.Y);

        var bounds = new Rectangle(minX, minY, maxX - minX, maxY - minY);

        var lagoon = new SquareGrid(bounds);
        lagoon.Borders.Or(walls);

        var interior = PathFinding.DepthFirst(lagoon, new(1, 1), cancellationToken);
        int volume = walls.Count + interior.Count;

        string visualization = Visualize(lagoon);

        return (volume, visualization);

        static string Visualize(SquareGrid grid)
        {
            var builder = new StringBuilder(grid.Bounds.Area() + (Environment.NewLine.Length * grid.Height));

            for (int y = grid.Bounds.Top; y <= grid.Bounds.Bottom; y++)
            {
                for (int x = grid.Bounds.Left; x <= grid.Bounds.Right; x++)
                {
                    builder.Append(grid.Borders.Contains(new(x, y)) ? '#' : '.');
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var plan = await ReadResourceAsLinesAsync(cancellationToken);

        (Volume, string lagoon) = Dig(plan, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("The lagoon can hold {0} cubic meters of lava.", Volume);
            Logger.WriteLine(lagoon);
        }

        var result = PuzzleResult.Create(Volume);

        result.Visualizations.Add(lagoon);

        return result;
    }
}
