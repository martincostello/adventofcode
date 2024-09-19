﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/9</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2016, 09, "Explosives in Cyberspace", RequiresData = true)]
public sealed class Day09 : Puzzle
{
    /// <summary>
    /// Gets the decompressed length of the data using version 1 of the decompression algorithm.
    /// </summary>
    public long DecompressedLengthVersion1 { get; private set; }

    /// <summary>
    /// Gets the decompressed length of the data using version 2 of the decompression algorithm.
    /// </summary>
    public long DecompressedLengthVersion2 { get; private set; }

    /// <summary>
    /// Decompresses the specified data and returns the length of the decompressed data.
    /// </summary>
    /// <param name="data">The data to decompress.</param>
    /// <param name="version">The version of the decompression algorithm to use.</param>
    /// <returns>
    /// The length of the value from decompressing <paramref name="data"/>.
    /// </returns>
    internal static long GetDecompressedLength(string data, int version)
        => GetDecompressedLength(data, 0, data.Length, version);

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        string data = await ReadResourceAsStringAsync(cancellationToken);

        DecompressedLengthVersion1 = GetDecompressedLength(data, version: 1);
        DecompressedLengthVersion2 = GetDecompressedLength(data, version: 2);

        if (Verbose)
        {
            Logger.WriteLine($"The decompressed length of the data using version 1 of the algorithm is {DecompressedLengthVersion1:N0}.");
            Logger.WriteLine($"The decompressed length of the data using version 2 of the algorithm is {DecompressedLengthVersion2:N0}.");
        }

        return PuzzleResult.Create(DecompressedLengthVersion1, DecompressedLengthVersion2);
    }

    /// <summary>
    /// Decompresses the specified data and returns the length of the decompressed data.
    /// </summary>
    /// <param name="data">The data to decompress.</param>
    /// <param name="index">The index to decompress up to.</param>
    /// <param name="count">The number of characters to decompress.</param>
    /// <param name="version">The version of the decompression algorithm to use.</param>
    /// <returns>
    /// The length of the value from decompressing <paramref name="data"/>.
    /// </returns>
    private static long GetDecompressedLength(string data, int index, int count, int version)
    {
        bool isInMarker = false;
        bool isInRepeat = false;

        var marker = new StringBuilder();

        int repeatCount = 0;
        int repeatLength = 0;

        long length = 0;

        for (int i = index; i < index + count; i++)
        {
            char ch = data[i];

            if (isInMarker)
            {
                if (ch == ')')
                {
                    isInMarker = false;

                    (repeatLength, repeatCount) = marker.ToString().AsNumberPair<int>('x');

                    isInRepeat = true;
                    marker.Clear();
                }
                else
                {
                    marker.Append(ch);
                }
            }
            else if (ch == '(' && !isInRepeat)
            {
                isInMarker = true;
            }
            else if (isInRepeat)
            {
                long chunkLength =
                    version == 2 ?
                    GetDecompressedLength(data, i, repeatLength, 2) :
                    repeatLength;

                length += chunkLength * repeatCount;

                i += repeatLength - 1;
                isInRepeat = false;
            }
            else if (!char.IsWhiteSpace(ch))
            {
                length++;
            }
        }

        return length;
    }
}
