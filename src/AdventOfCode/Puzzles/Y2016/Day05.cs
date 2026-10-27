// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/5</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2016, 05, "How About a Nice Game of Chess?", MinimumArguments = 1)]
public sealed class Day05 : Puzzle<string, string>
{
    /// <summary>
    /// Generates the password for the door with the specified Id.
    /// </summary>
    /// <param name="doorId">The door Id to generate the password for.</param>
    /// <param name="isPositionSpecifiedByHash">Whether the hash specifies the position containing the password characters.</param>
    /// <returns>
    /// The password for the door specified by <paramref name="doorId"/>.
    /// </returns>
    internal static string GeneratePassword(string doorId, bool isPositionSpecifiedByHash)
    {
        (string password, string passwordWhenPositionIsIndicated) = GeneratePasswords(doorId);
        return isPositionSpecifiedByHash ? passwordWhenPositionIsIndicated : password;
    }

    /// <summary>
    /// Generates both passwords for the door with the specified Id.
    /// </summary>
    /// <param name="doorId">The door Id to generate the passwords for.</param>
    /// <returns>
    /// The password for the door specified by <paramref name="doorId"/> and the
    /// password when the position is specified by the hash.
    /// </returns>
    internal static (string Password, string PasswordWhenPositionIsIndicated) GeneratePasswords(string doorId)
    {
        const int PasswordLength = 8;
        const int WindowSize = 1 << 20;

        Span<char> password = stackalloc char[PasswordLength];
        Span<char> positional = stackalloc char[PasswordLength];

        positional.Clear();

        int passwordCount = 0;
        int positionalCount = 0;

        byte[] prefix = Encoding.ASCII.GetBytes(doorId);

        int chunks = Environment.ProcessorCount;
        int chunkSize = WindowSize / chunks;

        var hits = new List<Hit>();

        for (int start = 0; passwordCount < PasswordLength || positionalCount < PasswordLength; start += WindowSize)
        {
            hits.Clear();

            Parallel.For(
                0,
                chunks,
                () => new List<Hit>(),
                (chunk, _, local) =>
                {
                    int from = start + (chunk * chunkSize);
                    FindHits(prefix, from, from + chunkSize, local);
                    return local;
                },
                (local) =>
                {
                    lock (hits)
                    {
                        hits.AddRange(local);
                    }
                });

            hits.Sort(static (x, y) => x.Index.CompareTo(y.Index));

            foreach (var hit in hits)
            {
                if (passwordCount < PasswordLength)
                {
                    password[passwordCount++] = ToHex(hit.Sixth);
                }

                if (hit.Sixth < PasswordLength && positional[hit.Sixth] == '\0')
                {
                    positional[hit.Sixth] = ToHex(hit.Seventh);
                    positionalCount++;
                }
            }
        }

        return (new(password), new(positional));

        static char ToHex(int value)
            => (char)(value < 10 ? '0' + value : 'a' + value - 10);
    }

    /// <inheritdoc />
    protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken) => SolveWithArgument(args, static (doorId, logger) =>
    {
        (string password, string passwordWhenPositionIsIndicated) = GeneratePasswords(doorId);

        if (logger is { })
        {
            logger.WriteLine($"The password for door '{doorId}' is '{password}'.");
            logger.WriteLine($"The password for door '{doorId}' is '{passwordWhenPositionIsIndicated}' when the position is specified in the hash.");
        }

        return (password, passwordWhenPositionIsIndicated);
    });

    /// <summary>
    /// Finds the hashes whose hexadecimal representation starts with five zeroes for the specified range of indexes.
    /// </summary>
    /// <param name="prefix">The prefix of the data to hash.</param>
    /// <param name="start">The inclusive index to start from.</param>
    /// <param name="end">The exclusive index to end at.</param>
    /// <param name="hits">The collection to add any hits to.</param>
    private static void FindHits(ReadOnlySpan<byte> prefix, int start, int end, List<Hit> hits)
    {
        const int MaximumIndexLength = 10;

        Span<byte> buffer = stackalloc byte[prefix.Length + MaximumIndexLength];
        Span<byte> hash = stackalloc byte[Md5.HashSizeInBytes];

        prefix.CopyTo(buffer);
        var indexBuffer = buffer[prefix.Length..];

        for (int index = start; index < end; index++)
        {
            index.TryFormat(indexBuffer, out int written, provider: CultureInfo.InvariantCulture);

            Md5.HashData(buffer[..(prefix.Length + written)], hash);

            // Are the first five hexadecimal characters 0?
            if (hash[0] == 0 && hash[1] == 0 && (hash[2] & 0xF0) == 0)
            {
                hits.Add(new(index, hash[2] & 0x0F, hash[3] >> 4));
            }
        }
    }

    /// <summary>
    /// Represents a hash that starts with five zeroes.
    /// </summary>
    /// <param name="Index">The index that generated the hash.</param>
    /// <param name="Sixth">The sixth hexadecimal character of the hash.</param>
    /// <param name="Seventh">The seventh hexadecimal character of the hash.</param>
    private readonly record struct Hit(int Index, int Sixth, int Seventh);
}
