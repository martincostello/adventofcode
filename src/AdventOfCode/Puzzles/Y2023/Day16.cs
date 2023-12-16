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
    /// Gets how many tiles are energized.
    /// </summary>
    public int EnergizedTiles { get; private set; }

    /// <summary>
    /// Determines how many tiles are energized in the specified contraption.
    /// </summary>
    /// <param name="layout">The layout of the contraption.</param>
    /// <returns>
    /// The number of tiles in the contraption that are energized.
    /// </returns>
    public static (int EnergizedTiles, string Visualization) Energize(IList<string> layout)
    {
        var bounds = new Rectangle(0, 0, layout[0].Length, layout.Count);
        var energized = new Dictionary<Point, int>();
        var visited = new HashSet<(Point Location, Size Direction)>();

        var location = new Point(-1, 0);
        var direction = new Size(1, 0);

        Trace(location, direction, layout, bounds, energized, visited);

        string visualization = Visualize(bounds, energized);

        return (energized.Count, visualization);

        static void Trace(
            Point location,
            Size direction,
            IList<string> contraption,
            Rectangle bounds,
            Dictionary<Point, int> energized,
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
                bool wasEnergized = false;

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
                        else
                        {
                            wasEnergized = true;
                        }

                        break;

                    case '-':
                        if (direction.Height != 0)
                        {
                            direction = new(1, 0);
                            split = true;
                        }
                        else
                        {
                            wasEnergized = true;
                        }

                        break;

                    case '.':
                    default:
                        wasEnergized = true;
                        break;
                }

                if (wasEnergized || !energized.ContainsKey(location))
                {
                    energized.AddOrIncrement(location, 1);
                }

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

        static string Visualize(Rectangle bounds, Dictionary<Point, int> energized)
        {
            var builder = new StringBuilder(bounds.Area() + (Environment.NewLine.Length * bounds.Height));

            for (int y = 0; y < bounds.Height; y++)
            {
                for (int x = 0; x < bounds.Width; x++)
                {
                    char c;

                    if (energized.TryGetValue(new(x, y), out int value))
                    {
                        c = value switch
                        {
                            1 => '#',
                            _ => (char)(value + '0'),
                        };
                    }
                    else
                    {
                        c = '.';
                    }

                    builder.Append(c);
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

        (EnergizedTiles, string energized) = Energize(layout);

        Logger.WriteLine(energized);

        if (Verbose)
        {
            Logger.WriteLine("{0} tiles are energized.", EnergizedTiles);
        }

        var result = PuzzleResult.Create(EnergizedTiles);

        result.Visualizations.Add(energized);

        return result;
    }
}
