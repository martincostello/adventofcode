// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/11</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 11, "Cosmic Expansion", RequiresData = true)]
public sealed class Day11 : Puzzle
{
    private const char Empty = '.';
    private const char Galaxy = '#';

    /// <summary>
    /// Gets the sum of the lengths of shortest path between each galaxy.
    /// </summary>
    public long SumOfLengthsSmall { get; private set; }

    /// <summary>
    /// Gets the sum of the lengths of shortest path between each galaxy.
    /// </summary>
    public long SumOfLengthsLarge { get; private set; }

    /// <summary>
    /// Analyzes the specified image of galaxies and returns the
    /// sum of the lengths of the shortest path between each galaxy.
    /// </summary>
    /// <param name="image">The image of galaxies to analyze.</param>
    /// <param name="expansion">The rate of expansion to use.</param>
    /// <returns>
    /// The sum of the lengths of the shortest path between each galaxy in the image.
    /// </returns>
    public static long Analyze(IList<string> image, int expansion)
    {
        var galaxies = Expand(image, expansion);
        long sum = 0;

        for (int i = 0; i < galaxies.Count; i++)
        {
            var origin = galaxies[i];

            for (int j = 0; j < galaxies.Count; j++)
            {
                sum += origin.ManhattanDistance(galaxies[j]);
            }
        }

        // TODO Optimise by not calculating the same distances twice
        return sum / 2;

        static List<Point> Expand(IList<string> image, int expansion)
        {
            int height = image.Count;
            int width = image[0].Length;

            List<Point> galaxies = [];
            List<int> columnsToExpand = [];
            List<int> rowsToExpand = [];

            for (int y = 0; y < height; y++)
            {
                string row = image[y];
                bool allEmpty = true;

                for (int x = 0; x < width; x++)
                {
                    if (row[x] is Galaxy)
                    {
                        galaxies.Add(new(x, y));
                        allEmpty = false;
                    }
                }

                if (allEmpty)
                {
                    rowsToExpand.Add(y);
                }
            }

            for (int x = 0; x < width; x++)
            {
                bool allEmpty = true;

                for (int y = 0; allEmpty && y < height; y++)
                {
                    allEmpty &= image[y][x] is Empty;
                }

                if (allEmpty)
                {
                    columnsToExpand.Add(x);
                }
            }

            if (rowsToExpand.Count > 0)
            {
                var delta = new Size(0, expansion);

                for (int i = 0; i < rowsToExpand.Count; i++)
                {
                    int y = rowsToExpand[i] + (expansion * i);

                    for (int j = 0; j < galaxies.Count; j++)
                    {
                        var galaxy = galaxies[j];

                        if (galaxy.Y > y)
                        {
                            galaxies[j] += delta;
                        }
                    }
                }
            }

            if (columnsToExpand.Count > 0)
            {
                var delta = new Size(expansion, 0);

                for (int i = 0; i < columnsToExpand.Count; i++)
                {
                    int x = columnsToExpand[i] + (expansion * i);

                    for (int j = 0; j < galaxies.Count; j++)
                    {
                        var galaxy = galaxies[j];

                        if (galaxy.X > x)
                        {
                            galaxies[j] += delta;
                        }
                    }
                }
            }

            return galaxies;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var image = await ReadResourceAsLinesAsync(cancellationToken);

        SumOfLengthsSmall = Analyze(image, expansion: 1);
        SumOfLengthsLarge = Analyze(image, expansion: 1_000_000);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the shortest path between each galaxy with an expansion of 1 is {0}.", SumOfLengthsSmall);
            Logger.WriteLine("The sum of the shortest path between each galaxy with an expansion of 1,000,000 is {0}.", SumOfLengthsLarge);
        }

        return PuzzleResult.Create(SumOfLengthsSmall, SumOfLengthsLarge);
    }
}
