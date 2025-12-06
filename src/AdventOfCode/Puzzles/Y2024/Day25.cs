// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/25</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 25, "Code Chronicle", RequiresData = true)]
public sealed class Day25 : Puzzle<int, int>
{
    /// <summary>
    /// Gets the number of unique lock/key pairs fit together without overlapping in any column.
    /// </summary>
    public int UniquePairs { get; private set; }

    /// <summary>
    /// Simulates the specified lock and key patterns.
    /// </summary>
    /// <param name="schematics">The schematics of the locks and keys.</param>
    /// <returns>
    /// The number of unique lock/key pairs fit together without overlapping in any column.
    /// </returns>
    public static int Simulate(IList<string> schematics)
    {
        var grids = new List<string[]>();

        for (int i = 0; i < schematics.Count; i += 8)
        {
            string[] grid =
            [
                schematics[i],
                schematics[i + 1],
                schematics[i + 2],
                schematics[i + 3],
                schematics[i + 4],
                schematics[i + 5],
                schematics[i + 6],
            ];

            grids.Add(grid);
        }

        var locks = new List<HashSet<Point>>();
        var keys = new List<HashSet<Point>>();

        foreach (string[] grid in grids)
        {
            bool isLock = grid[0] is "#####";
            var item = new HashSet<Point>();

            for (int y = 0; y < grid.Length; y++)
            {
                string row = grid[y];

                for (int x = 0; x < row.Length; x++)
                {
                    if (row[x] == '#')
                    {
                        item.Add(new(x, y));
                    }
                }
            }

            if (isLock)
            {
                locks.Add(item);
            }
            else
            {
                keys.Add(item);
            }
        }

        int combinations = 0;

        for (int i = 0; i < locks.Count; i++)
        {
            for (int j = 0; j < keys.Count; j++)
            {
                if (!locks[i].Intersect(keys[j]).Any())
                {
                    combinations++;
                }
            }
        }

        return combinations;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        UniquePairs = Simulate(values);

        if (Verbose)
        {
            Logger.WriteLine("{0} unique lock/key pairs fit together without overlapping in any column.", UniquePairs);
        }

        Solution1 = UniquePairs;
        Solution2 = Unsolved;

        return Result();
    }
}
