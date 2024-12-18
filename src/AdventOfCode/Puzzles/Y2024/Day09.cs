// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/9</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 09, "Disk Fragmenter", RequiresData = true)]
public sealed class Day09 : Puzzle
{
    /// <summary>
    /// Gets the checksum of the defragmented disk.
    /// </summary>
    public long Checksum { get; private set; }

    /// <summary>
    /// Defragments the specified disk.
    /// </summary>
    /// <param name="map">The map for the disk of files and free space.</param>
    /// <returns>
    /// The checksum of the defragmented disk.
    /// </returns>
    public static long Defragment(ReadOnlySpan<char> map)
    {
        const int Empty = -1;

        var disk = new List<int>();

        int id = 0;
        int offset = 0;

        for (int i = 0; i < map.Length; i++)
        {
            int length = map[i] - '0';

            if (i % 2 is 0)
            {
                for (int j = 0; j < length; j++)
                {
                    disk.Add(id);
                }

                id++;
            }
            else
            {
                for (int j = 0; j < length; j++)
                {
                    disk.Add(Empty);
                }
            }

            offset += length;
        }

        int free = 0;

        for (int i = disk.Count - 1; i > -1; i--)
        {
            id = disk[i];

            if (id is Empty)
            {
                continue;
            }

            while (disk[free] > Empty)
            {
                free++;
            }

            if (free >= i)
            {
                break;
            }

            disk[free] = id;
            disk[i] = Empty;
        }

        long checksum = 0;

        for (int i = 0; i < disk.Count; i++)
        {
            id = disk[i];

            if (id is not -1)
            {
                checksum += i * id;
            }
        }

        return checksum;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        string map = await ReadResourceAsStringAsync(cancellationToken);

        Checksum = Defragment(map);

        if (Verbose)
        {
            Logger.WriteLine("The filesystem checksum is {0}.", Checksum);
        }

        return PuzzleResult.Create(Checksum);
    }
}
