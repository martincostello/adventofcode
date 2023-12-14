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
    /// Computes the total load on the northern support beams.
    /// </summary>
    /// <param name="positions">The positions of the rocks.</param>
    /// <returns>
    /// The total load on the northern support beams.
    /// </returns>
    public static int ComputeLoad(IList<string> positions)
    {
        var platform = new SquareGrid(positions[0].Length, positions.Count);

        for (int y = 0; y < platform.Height; y++)
        {
            string row = positions[y];

            for (int x = 0; x < platform.Width; x++)
            {
                char contents = row[x];
                if (contents == 'O')
                {
                    platform.Locations.Add(new(x, y));
                }
                else if (contents == '#')
                {
                    platform.Borders.Add(new(x, y));
                }
            }
        }

        for (int x = 0; x < platform.Width; x++)
        {
            for (int y = 1; y < platform.Height; y++)
            {
                var location = new Point(x, y);
                var north = new Point(x, y - 1);

                while (platform.InBounds(north) &&
                       platform.Locations.Contains(location) &&
                       !platform.Borders.Contains(north) &&
                       !platform.Locations.Contains(north))
                {
                    platform.Locations.Remove(location);
                    platform.Locations.Add(north);
                    location = north;
                    north = new Point(location.X, location.Y - 1);
                }
            }
        }

        return platform.Locations.Sum((p) => platform.Height - p.Y);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var positions = await ReadResourceAsLinesAsync(cancellationToken);

        TotalLoad = ComputeLoad(positions);

        if (Verbose)
        {
            Logger.WriteLine("The total load on the north support beams is {0}.", TotalLoad);
        }

        return PuzzleResult.Create(TotalLoad);
    }
}
