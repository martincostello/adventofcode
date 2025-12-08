// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/4</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2016, 04, "Security Through Obscurity", RequiresData = true)]
public sealed class Day04 : Puzzle<int, int>
{
    /// <summary>
    /// Decrypts the name of the specified room.
    /// </summary>
    /// <param name="name">The encrypted name of the room.</param>
    /// <returns>
    /// The decrypted name of the room.
    /// </returns>
    internal static string DecryptRoomName(string name)
        => DecryptRoomName(name, out string _);

    /// <summary>
    /// Returns whether the specified room is real.
    /// </summary>
    /// <param name="name">The encrypted name of the room.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="name"/> is a real room; otherwise <see langword="false"/>.
    /// </returns>
    internal static bool IsRoomReal(string name)
        => IsRoomReal(name, out string _, out string _);

    /// <summary>
    /// Returns the sum of the sector Ids of the specified room names which are real.
    /// </summary>
    /// <param name="names">The names of the rooms to compute the sum from.</param>
    /// <returns>
    /// The sum of the sector Ids for room names specified by <paramref name="names"/> which are real.
    /// </returns>
    internal static int GetSectorIdOfNorthPoleObjectsRoom(IEnumerable<string> names)
    {
        foreach (string name in names)
        {
            string decryptedName = DecryptRoomName(name, out string sectorIdString);

            if (decryptedName is "northpole object storage")
            {
                return Parse<int>(sectorIdString);
            }
        }

        return Unsolved;
    }

    /// <summary>
    /// Returns the sum of the sector Ids of the specified room names which are real.
    /// </summary>
    /// <param name="names">The names of the rooms to compute the sum from.</param>
    /// <returns>
    /// The sum of the sector Ids for room names specified by <paramref name="names"/> which are real.
    /// </returns>
    internal static int SumOfRealRoomSectorIds(IEnumerable<string> names)
    {
        int sum = 0;

        foreach (string name in names)
        {
            if (IsRoomReal(name, out string encryptedName, out string sectorIdString))
            {
                sum += Parse<int>(sectorIdString);
            }
        }

        return sum;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (names, logger, _) =>
            {
                int sectorIdOfNorthPoleObjectsRoom = SumOfRealRoomSectorIds(names);
                int sumOfSectorIdsOfRealRooms = GetSectorIdOfNorthPoleObjectsRoom(names);

                if (logger is { })
                {
                    logger.WriteLine("The sum of the sector Ids of the real rooms is {0:N0}.", sectorIdOfNorthPoleObjectsRoom);
                    logger.WriteLine("The sector ID of the room where North Pole objects are stored is {0:N0}.", sumOfSectorIdsOfRealRooms);
                }

                return (sectorIdOfNorthPoleObjectsRoom, sumOfSectorIdsOfRealRooms);
            },
            cancellationToken);
    }

    /// <summary>
    /// Decrypts the name of the specified room.
    /// </summary>
    /// <param name="name">The encrypted name of the room.</param>
    /// <param name="sectorId">When the method returns, contains the sector Id of the room.</param>
    /// <returns>
    /// The decrypted name of the room if the room is real; otherwise the empty string.
    /// </returns>
    private static string DecryptRoomName(string name, out string sectorId)
    {
        if (!IsRoomReal(name, out string encryptedName, out sectorId))
        {
            return string.Empty;
        }

        var builder = new StringBuilder(encryptedName.Length);
        int sectorIdValue = Parse<int>(sectorId);

        foreach (char ch in encryptedName)
        {
            if (ch is '-')
            {
                builder.Append(' ');
            }
            else
            {
                int shift = sectorIdValue % 26;
                int shifted = ch + shift;

                if (shifted > 'z')
                {
                    shifted -= 26;
                }

                builder.Append((char)shifted);
            }
        }

        return builder.ToString();
    }

    /// <summary>
    /// Returns whether the specified room is real.
    /// </summary>
    /// <param name="name">The encrypted name of the room.</param>
    /// <param name="encryptedName">When the method returns, contains the encrypted name of the room.</param>
    /// <param name="sectorId">When the method returns, contains the sector Id of the room.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="name"/> is a real room; otherwise <see langword="false"/>.
    /// </returns>
    private static bool IsRoomReal(string name, out string encryptedName, out string sectorId)
    {
        var nameSpan = name.AsSpan();
        int indexOfChecksum = nameSpan.IndexOf('[');

        string checksum = name.Substring(indexOfChecksum + 1, 5);

        int indexOfLastDash = nameSpan.LastIndexOf('-');

        sectorId = name.Substring(indexOfLastDash + 1, indexOfChecksum - indexOfLastDash - 1);

        encryptedName = name[..indexOfLastDash];

        var top5Letters = encryptedName
            .Where((p) => p is not '-')
            .CountBy((p) => p)
            .OrderByDescending((p) => p.Value)
            .ThenBy((p) => p.Key)
            .Take(5)
            .Select((p) => p.Key);

        string computedChecksum = string.Join(string.Empty, top5Letters);

        return string.Equals(computedChecksum, checksum, StringComparison.Ordinal);
    }
}
