// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/16</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 16, "The Floor Will Be Lava", RequiresData = true)]
public sealed class Day16 : Puzzle
{
    /// <summary>
    /// Gets how many tiles are energized starting the beam from <c>0,0</c>.
    /// </summary>
    public int EnergizedTiles00 { get; private set; }

    /// <summary>
    /// Gets how many tiles are energized starting the beam from the optimum position.
    /// </summary>
    public int EnergizedTilesOptimum { get; private set; }

    /// <summary>
    /// Determines how many tiles are energized in the specified contraption.
    /// </summary>
    /// <param name="layout">The layout of the contraption.</param>
    /// <param name="optimize">Whether to optimize the number of tiles that are energized.</param>
    /// <returns>
    /// The number of tiles in the contraption that are energized.
    /// </returns>
    public static (int EnergizedTiles, string Visualization) Energize(IList<string> layout, bool optimize)
    {
        var bounds = new Rectangle(0, 0, layout[0].Length, layout.Count);
        var origins = new HashSet<(Point Location, Size Direction)>();

        var up = new Size(0, -1);
        var down = new Size(0, 1);
        var left = new Size(-1, 0);
        var right = new Size(1, 0);

        if (optimize)
        {
            int above = bounds.Top - 1;
            int below = bounds.Bottom + 1;
            int leftOf = bounds.Left - 1;
            int rightOf = bounds.Right + 1;

            for (int x = 0; x < bounds.Width; x++)
            {
                origins.Add((new(x, above), down));
                origins.Add((new(x, below), up));
            }

            for (int y = 0; y < bounds.Height; y++)
            {
                origins.Add((new(leftOf, y), right));
                origins.Add((new(rightOf, y), left));
            }
        }
        else
        {
            origins.Add((new(bounds.Left - 1, bounds.Top), right));
        }

        var energized = new HashSet<Point>();
        var visited = new HashSet<(Point Location, Size Direction)>();

        HashSet<Point> optimum = [];

        foreach (var (location, direction) in origins)
        {
            energized.Clear();
            visited.Clear();

            Trace(location, direction, layout, bounds, energized, visited);

            if (energized.Count > optimum.Count)
            {
                optimum = energized;
                energized = [];
            }
        }

        string visualization = Visualize(bounds, optimum);

        return (optimum.Count, visualization);

        static void Trace(
            Point location,
            Size direction,
            IList<string> contraption,
            Rectangle bounds,
            HashSet<Point> energized,
            HashSet<(Point Location, Size Direction)> visited)
        {
            location += direction;

            if (!visited.Add((location, direction)) || !bounds.Contains(location))
            {
                return;
            }

            do
            {
                bool split = false;

                switch (contraption[location.Y][location.X])
                {
                    case '/':
                        direction = direction switch
                        {
                            { Height: 0 } => new(0, -direction.Width),
                            { Width: 0 } => new(-direction.Height, 0),
                            _ => throw new UnreachableException(),
                        };
                        break;

                    case '\\':
                        direction = direction switch
                        {
                            { Height: 0 } => new(0, direction.Width),
                            { Width: 0 } => new(direction.Height, 0),
                            _ => throw new UnreachableException(),
                        };
                        break;

                    case '|':
                        if (direction.Width != 0)
                        {
                            direction = new(0, 1);
                            split = true;
                        }

                        break;

                    case '-':
                        if (direction.Height != 0)
                        {
                            direction = new(1, 0);
                            split = true;
                        }

                        break;

                    case '.':
                    default:
                        break;
                }

                energized.Add(location);

                if (split)
                {
                    Trace(location, direction, contraption, bounds, energized, visited);
                    Trace(location, direction * -1, contraption, bounds, energized, visited);
                    break;
                }

                location += direction;
            }
            while (bounds.Contains(location));
        }

        static string Visualize(Rectangle bounds, HashSet<Point> energized)
        {
            var builder = new StringBuilder(bounds.Area() + (Environment.NewLine.Length * bounds.Height));

            for (int y = 0; y < bounds.Height; y++)
            {
                for (int x = 0; x < bounds.Width; x++)
                {
                    builder.Append(energized.Contains(new(x, y)) ? '#' : '.');
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

        var layout = await ReadResourceAsLinesAsync(cancellationToken);

        (EnergizedTiles00, string energized) = Energize(layout, optimize: false);
        (EnergizedTilesOptimum, string optimized) = Energize(layout, optimize: true);

        if (Verbose)
        {
            Logger.WriteLine("{0} tiles are energized starting at 0,0.", EnergizedTiles00);
            Logger.WriteLine(energized);

            Logger.WriteLine("{0} tiles are energized starting at the optimum position.", EnergizedTilesOptimum);
            Logger.WriteLine(optimized);
        }

        var result = PuzzleResult.Create(EnergizedTiles00, EnergizedTilesOptimum);

        result.Visualizations.Add(energized);
        result.Visualizations.Add(optimized);

        return result;
    }
}
