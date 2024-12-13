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
    public int Checksum { get; private set; }

    /// <summary>
    /// Defragments the specified disk.
    /// </summary>
    /// <param name="map">The map for the disk of files and free space.</param>
    /// <returns>
    /// The checksum of the defragmented disk.
    /// </returns>
    public static int Defragment(ReadOnlySpan<char> map)
    {
        var files = new List<(int Id, int Length, Range Location)>();
        var spaces = new List<(int Length, Range Location)>();

        int id = 0;
        int length = 0;

        for (int i = 0; i < map.Length; i++)
        {
            int count = map[i] - '0';
            var range = new Range(length, length + count);

            if (i % 2 is 0)
            {
                files.Add((id++, count, range));
            }
            else
            {
                spaces.Add((count, range));
            }

            length += count;
        }

        while (spaces.Count > 1 && spaces[0].Location.GetOffsetAndLength(spaces[0].Length).Length != length - 1)
        {
            var space = spaces[0];
            spaces.RemoveAt(0);

            while (space.Length > 0)
            {
                (id, int size, var location) = files[^1];

                files.RemoveAt(files.Count - 1);

                if (size == space.Length)
                {
                    files.Add((id, size, space.Location));
                    space = (0, space.Location.Start..space.Location.Start);
                }
                else if (size < space.Length)
                {
                    var newLocation = space.Location.Start..size;

                    int ix = files.FindLastIndex((p) => p.Location.End.Value == newLocation.Start.Value);

                    files.Insert(ix + 1, (id, size, newLocation));
                    spaces.Add((space.Length, location));
                    space = (size, space.Location.Start..size);
                }
                else if (size > remaining)
                {
                    int ix = files.FindLastIndex((p) => p.Location.End.Value == space.Start.Value);

                    files.Insert(ix, (id, remaining, space));

                    files.Add((id, size - remaining, location.Start..^remaining));
                    space = space.Start..space.Start;
                }
            }
        }

        files.Sort((x, y) => y.Location.Start.Value - x.Location.Start.Value);

        return -1;
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
