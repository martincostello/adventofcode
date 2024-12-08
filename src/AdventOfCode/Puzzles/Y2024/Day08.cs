// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/8</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 08, "Resonant Collinearity", RequiresData = true)]
public sealed class Day08 : Puzzle
{
    /// <summary>
    /// Gets the number of unique locations within the bounds of the map that contain an antinode.
    /// </summary>
    public int UniqueAntinodes { get; private set; }

    /// <summary>
    /// Solves the puzzle for the specified map.
    /// </summary>
    /// <param name="map">The map of anntenae.</param>
    /// <returns>
    /// The number of unique locations within the bounds of the map that contain an antinode.
    /// </returns>
    public static int FindAntinodes(IList<string> map)
    {
        var antinodes = new HashSet<Point>();
        var frequencies = new Dictionary<char, List<Point>>();

        for (int y = 0; y < map.Count; y++)
        {
            string row = map[y];

            for (int x = 0; x < row.Length; x++)
            {
                char value = row[x];

                if (value is '.')
                {
                    continue;
                }

                if (!frequencies.TryGetValue(value, out var antennae))
                {
                    frequencies[value] = antennae = [];
                }

                antennae.Add(new(x, y));
            }
        }

        var bounds = new Rectangle(0, 0, map[0].Length, map.Count);

        foreach ((_, var antennae) in frequencies)
        {
            for (int i = 0; i < antennae.Count; i++)
            {
                for (int j = 0; j < antennae.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    var first = antennae[i];
                    var second = antennae[j];

                    var vector = new Size(second.X - first.X, second.Y - first.Y);

                    var antinode = first - vector;

                    if (bounds.Contains(antinode))
                    {
                        antinodes.Add(antinode);
                    }

                    antinode = second + vector;

                    if (bounds.Contains(antinode))
                    {
                        antinodes.Add(antinode);
                    }
                }
            }
        }

        return antinodes.Count;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        UniqueAntinodes = FindAntinodes(values);

        if (Verbose)
        {
            Logger.WriteLine("{0} unique locations within the bounds of the map contain an antinode.", UniqueAntinodes);
        }

        return PuzzleResult.Create(UniqueAntinodes);
    }
}
