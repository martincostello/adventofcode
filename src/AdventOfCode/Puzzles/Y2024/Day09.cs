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
    /// Gets the checksum of the defragmented disk when optimizing for continuous free space.
    /// </summary>
    public long ChecksumV1 { get; private set; }

    /// <summary>
    /// Gets the checksum of the defragmented disk whe optimizing for contiguous files.
    /// </summary>
    public long ChecksumV2 { get; private set; }

    /// <summary>
    /// Defragments the specified disk.
    /// </summary>
    /// <param name="map">The map for the disk of files and free space.</param>
    /// <param name="contiguousFiles">Whether to optimize for contiguous files or not.</param>
    /// <returns>
    /// The checksum of the defragmented disk.
    /// </returns>
    public static long Defragment(ReadOnlySpan<char> map, bool contiguousFiles)
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

        if (contiguousFiles)
        {
            OptimizeContiguousFiles(disk);
        }
        else
        {
            OptimizeContiguousSpace(disk);
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

        static void OptimizeContiguousSpace(List<int> disk)
        {
            int free = 0;

            for (int i = disk.Count - 1; i > -1; i--)
            {
                int id = disk[i];

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
        }

        static void OptimizeContiguousFiles(List<int> disk)
        {
            if (disk.Count > 0)
            {
                // TODO
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        string map = await ReadResourceAsStringAsync(cancellationToken);

        ChecksumV1 = Defragment(map, contiguousFiles: false);
        ChecksumV2 = Defragment(map, contiguousFiles: true);

        if (Verbose)
        {
            Logger.WriteLine("The filesystem checksum for contiguous free space is {0}.", ChecksumV1);
            Logger.WriteLine("The filesystem checksum for contiguous files is {0}.", ChecksumV2);
        }

        return PuzzleResult.Create(ChecksumV1, ChecksumV2);
    }
}
