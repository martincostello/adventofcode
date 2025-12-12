// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/12</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 12, "Christmas Tree Farm", RequiresData = true)]
public sealed class Day12 : Puzzle<int>
{
    /// <summary>
    /// Counts the number of regions whose arrangements that can fit all of the required presents.
    /// </summary>
    /// <param name="summary">The values to solve the puzzle from.</param>
    /// <returns>
    /// The count of regions can fit all of the presents.
    /// </returns>
    public static int Arrange(IReadOnlyList<string> summary)
    {
        (var shapes, var regions) = ParseSummary(summary);

        int count = 0;

        foreach (var region in regions)
        {
            int required = 0;

            for (int i = 0; i < region.Quantities.Count; i++)
            {
                required += region.Quantities[i] * shapes[i].Count;
            }

            if (required < region.Bounds.Area())
            {
                count++;
            }
        }

        return count;

        static (List<Shape> Shapes, List<Region> Regions) ParseSummary(IReadOnlyList<string> summary)
        {
            var shapes = new List<Shape>();
            var regions = new List<Region>();

            for (int i = 0; i < summary.Count; i++)
            {
                string first = summary[i];

                if (TryParse(first.TrimEnd(':'), out int index))
                {
                    int y = 0;
                    var shape = new Shape(index);

                    while (!string.IsNullOrEmpty(summary[++i]))
                    {
                        string row = summary[i];

                        for (int x = 0; x < row.Length; x++)
                        {
                            if (row[x] == '#')
                            {
                                shape.Add(new(x, y));
                            }
                        }

                        y++;
                    }

                    shapes.Add(shape);
                }
                else
                {
                    var region = first.AsSpan();
                    index = region.IndexOf(' ');

                    var bounds = region[..(index - 1)];
                    var counts = region[(index + 1)..];

                    var quantities = new List<int>();

                    foreach (var range in counts.Split(' '))
                    {
                        quantities.Add(Parse<int>(counts[range]));
                    }

                    index = bounds.IndexOf('x');
                    int width = Parse<int>(bounds[..index]);
                    int height = Parse<int>(bounds[(index + 1)..]);

                    regions.Add(new(width, height, quantities));
                }
            }

            return (shapes, regions);
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (values, logger, _) =>
            {
                int count = Arrange(values);

                if (logger is { })
                {
                    logger.WriteLine("{0} regions can fit all of the presents.", count);
                }

                return count;
            },
            cancellationToken);
    }

    private sealed class Region(int width, int height, IReadOnlyList<int> quantities)
    {
        public Rectangle Bounds { get; } = new(0, 0, width, height);

        public IReadOnlyList<int> Quantities { get; } = quantities;
    }

    private sealed class Shape(int index) : HashSet<Point>
    {
        public int Index { get; } = index;
    }
}
