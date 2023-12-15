// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/14</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 14, "Parabolic Reflector Dish", RequiresData = true)]
public sealed class Day14 : Puzzle
{
    /// <summary>
    /// Gets the total load on the northern support beams.
    /// </summary>
    public int TotalLoad { get; private set; }

    /// <summary>
    /// Gets the total load on the northern support beams after 1 billion rotations of the spin cycle.
    /// </summary>
    public int TotalLoadWithSpinCycle { get; private set; }

    /// <summary>
    /// Computes the total load on the northern support beams.
    /// </summary>
    /// <param name="positions">The positions of the rocks.</param>
    /// <param name="rotations">The number of rotations of the spin cycle to perform.</param>
    /// <param name="logger">The optional logger to use.</param>
    /// <param name="cancellationToken">The optional cancellation token to use.</param>
    /// <returns>
    /// The total load on the northern support beams and a visualization of the rocks.
    /// </returns>
    public static (int Load, string Visualization) ComputeLoad(
        IList<string> positions,
        int rotations,
        ILogger? logger = null,
        CancellationToken cancellationToken = default)
    {
        var platform = new SquareGrid(positions[0].Length, positions.Count);

        for (int y = 0; y < platform.Height; y++)
        {
            string row = positions[y];

            for (int x = 0; x < platform.Width; x++)
            {
                switch (row[x])
                {
                    case 'O':
                        platform.Locations.Add(new(x, y));
                        break;

                    case '#':
                        platform.Borders.Add(new(x, y));
                        break;

                    default:
                        break;
                }
            }
        }

        var north = new Size(0, -1);
        var south = new Size(0, 1);
        var east = new Size(1, 0);
        var west = new Size(-1, 0);

        int load;

        if (rotations == 0)
        {
            SlideY(platform, north);
            load = ComputeLoad(platform);
        }
        else
        {
            var distributions = new List<(int Arrangement, int Load)>();

            int hash = 0;

            for (int i = 0; ; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                SlideY(platform, north);
                SlideX(platform, west);
                SlideY(platform, south);
                hash = SlideX(platform, east, computeHash: true);

                var distribution = (hash, ComputeLoad(platform));

                if (distributions.Contains(distribution))
                {
                    break;
                }

                distributions.Add(distribution);
            }

            int start = distributions.IndexOf((hash, ComputeLoad(platform)));
            int length = distributions.Count - start;

            int cycles = (rotations - start) / length;
            int offset = cycles == 0 ? rotations : (rotations - start) % cycles;
            int index = cycles == 0 ? offset - 1 : start + offset - 1;

            load = distributions[index].Load;
        }

        string visualization = Visualize(platform);

        logger?.WriteLine(visualization);

        return (load, visualization);

        static int ComputeLoad(SquareGrid platforms)
            => platforms.Locations.Sum((p) => platforms.Height - p.Y);

        static void SlideY(SquareGrid platform, Size direction)
        {
            int deltaY = -Math.Sign(direction.Height);
            int minY = deltaY > 0 ? 1 : platform.Height - 2;
            int maxY = deltaY > 0 ? platform.Height : -1;

            for (int x = 0; x < platform.Width; x++)
            {
                for (int y = minY; y != maxY; y += deltaY)
                {
                    var current = new Point(x, y);
                    var next = current + direction;

                    while (platform.InBounds(next) &&
                           platform.Locations.Contains(current) &&
                           !platform.Borders.Contains(next) &&
                           !platform.Locations.Contains(next))
                    {
                        platform.Locations.Remove(current);
                        platform.Locations.Add(next);
                        current = next;
                        next = new(current.X, current.Y - deltaY);
                    }
                }
            }
        }

        static int SlideX(SquareGrid platform, Size direction, bool computeHash = false)
        {
            int deltaX = -Math.Sign(direction.Width);
            int minX = deltaX > 0 ? 1 : platform.Width - 2;
            int maxX = deltaX > 0 ? platform.Width : -1;

            int hash = 0;

            for (int y = 0; y < platform.Height; y++)
            {
                for (int x = minX; x != maxX; x += deltaX)
                {
                    var current = new Point(x, y);
                    var next = current + direction;
                    bool moved = false;

                    while (platform.InBounds(next) &&
                           platform.Locations.Contains(current) &&
                           !platform.Borders.Contains(next) &&
                           !platform.Locations.Contains(next))
                    {
                        platform.Locations.Remove(current);
                        platform.Locations.Add(next);
                        current = next;
                        next = new(current.X - deltaX, current.Y);
                        moved = true;
                    }

                    if (moved && computeHash)
                    {
                        hash = HashCode.Combine(hash, current);
                    }
                }
            }

            return hash;
        }

        static string Visualize(SquareGrid platform)
        {
            var builder = new StringBuilder(platform.Width);

            for (int y = 0; y < platform.Height; y++)
            {
                for (int x = 0; x < platform.Width; x++)
                {
                    var location = new Point(x, y);
                    char contents = platform.Locations.Contains(location) ? 'O' : platform.Borders.Contains(location) ? '#' : '.';
                    builder.Append(contents);
                }

                builder.AppendLine();
            }

            builder.AppendLine();

            return builder.ToString();
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var positions = await ReadResourceAsLinesAsync(cancellationToken);

        (TotalLoad, string visualizationNorth) = ComputeLoad(positions, rotations: 0, Logger, cancellationToken);
        (TotalLoadWithSpinCycle, string visualizationCycles) = ComputeLoad(positions, rotations: 1_000_000_000, Logger, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("The total load on the north support beams is {0}.", TotalLoad);
            Logger.WriteLine("The total load on the north support beams after 1,000,000,000 spins is {0}.", TotalLoad, TotalLoadWithSpinCycle);
        }

        var result = PuzzleResult.Create(TotalLoad, TotalLoadWithSpinCycle);

        result.Visualizations.Add(visualizationNorth);
        result.Visualizations.Add(visualizationCycles);

        return result;
    }
}
