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
    public int SumOfLengths { get; private set; }

    /// <summary>
    /// Analyzes the specified image of galaxies and returns the
    /// sum of the lengths of the shortest path between each galaxy.
    /// </summary>
    /// <param name="image">The image of galaxies to analyze.</param>
    /// <returns>
    /// The sum of the lengths of the shortest path between each galaxy in the image.
    /// </returns>
    public static int Analyze(IList<string> image)
    {
        var expanded = Expand(image);
        var galaxies = FindGalaxies(expanded);

        var pairs = Maths.GetPermutations(galaxies, 2).ToList();

        Dictionary<(Point, Point), long> distances = [];

        var space = new SquareGrid(expanded[0].Length, expanded.Count);
        space.Locations.IntersectWith(galaxies);

        int sum = 0;

        foreach (var pair in pairs)
        {
            var first = pair.MinBy((p) => p.ToString());
            var second = pair.MaxBy((p) => p.ToString());

            var key = (first, second);

            if (!distances.ContainsKey(key))
            {
                long distance = PathFinding.AStar(space, first, second);
                distances[key] = distance;
                sum += (int)distance;
            }
        }

        return sum;

        static List<string> Expand(IList<string> image)
        {
            int height = image.Count;
            int width = image[0].Length;

            var result = new List<StringBuilder>();

            for (int y = 0; y < height; y++)
            {
                string row = image[y];
                var builder = new StringBuilder(row);

                result.Add(builder);

                if (row.All((p) => p is Empty))
                {
                    result.Add(builder);
                }
            }

            var rows = result.Distinct().ToList();
            int emptyColumns = 0;

            for (int x = 0; x < width; x++)
            {
                bool allEmpty = true;

                for (int y = 0; allEmpty && y < height; y++)
                {
                    allEmpty &= image[y][x] is Empty;
                }

                if (allEmpty)
                {
                    foreach (var row in rows)
                    {
                        row.Insert(x + emptyColumns, Empty);
                    }

                    emptyColumns++;
                }
            }

            return result.Select((p) => p.ToString()).ToList();
        }

        static List<Point> FindGalaxies(IList<string> image)
        {
            var galaxies = new List<Point>();

            for (int y = 0; y < image.Count; y++)
            {
                string row = image[y];

                for (int x = 0; x < row.Length; x++)
                {
                    if (row[x] is Galaxy)
                    {
                        galaxies.Add(new(x, y));
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

        SumOfLengths = Analyze(image);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the shortest path between each galaxy is {0}.", SumOfLengths);
        }

        return PuzzleResult.Create(SumOfLengths);
    }
}
