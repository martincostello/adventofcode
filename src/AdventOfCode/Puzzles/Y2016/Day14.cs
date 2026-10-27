// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/14</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2016, 14, "One-Time Pad", MinimumArguments = 1)]
public sealed class Day14 : Puzzle<int, int>
{
    /// <summary>
    /// Returns the index of the one-time pad key for the specified ordinal.
    /// </summary>
    /// <param name="salt">The salt to use to generate the one-time pad key.</param>
    /// <param name="ordinal">The ordinal of the key to return the index of.</param>
    /// <param name="useKeyStretching">Whether to use key stretching.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>
    /// The index of the one-time pad key generated using <paramref name="salt"/>
    /// with the ordinal value specified by <paramref name="ordinal"/>.
    /// </returns>
    internal static int GetOneTimePadKeyIndex(
        string salt,
        int ordinal,
        bool useKeyStretching,
        CancellationToken cancellationToken)
    {
        int current = 0;

        var keys = new KeyCache(salt, useKeyStretching);

        for (int i = 0; current < ordinal; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string key = keys[i];
            char triple = GetTripleCharacter(key);

            if (triple == default)
            {
                continue;
            }

            int next = i + 1;
            int last = next + 1000;

            string quintuple = new(triple, 5);

            for (int j = next; j <= last; j++)
            {
                key = keys[j];

                if (key.Contains(quintuple, StringComparison.Ordinal))
                {
                    if (++current != ordinal)
                    {
                        break;
                    }

                    return i;
                }
            }
        }

        return Unsolved;
    }

    /// <inheritdoc />
    protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return Task.FromResult(SolveWithArgument(
            args,
            static (salt, logger, cancellationToken) =>
            {
                int indexOfKey64 = GetOneTimePadKeyIndex(salt, 64, useKeyStretching: false, cancellationToken);
                int indexOfKey64WithStretching = GetOneTimePadKeyIndex(salt, 64, useKeyStretching: true, cancellationToken);

                if (logger is { })
                {
                    logger.WriteLine($"The index that produces the 64th one-time pad key is {indexOfKey64:N0}.");
                    logger.WriteLine($"The index that produces the 64th one-time pad key when using key stretching is {indexOfKey64WithStretching:N0}.");
                }

                return (indexOfKey64, indexOfKey64WithStretching);
            },
            cancellationToken));
    }

    /// <summary>
    /// Returns the character that first occurs three times consecutively
    /// in the specified <see cref="string"/>, if any.
    /// </summary>
    /// <param name="value">The value to search for three consecutive characters in.</param>
    /// <returns>
    /// The character that first occurs three times consecutively in
    /// <paramref name="value"/>, if any; otherwise the null character.
    /// </returns>
    private static char GetTripleCharacter(string value)
    {
        for (int i = 0; i < value.Length - 2; i++)
        {
            char ch = value[i];

            if (value[i + 1] == ch && value[i + 2] == ch)
            {
                return ch;
            }
        }

        return default;
    }

    /// <summary>
    /// A class that computes and caches the keys for each index. This class cannot be inherited.
    /// </summary>
    /// <param name="salt">The salt to use to generate the keys.</param>
    /// <param name="useKeyStretching">Whether to use key stretching.</param>
    private sealed class KeyCache(string salt, bool useKeyStretching)
    {
        /// <summary>
        /// The number of keys to generate at once.
        /// </summary>
        private const int ChunkSize = 2048;

        private readonly byte[] _salt = Encoding.ASCII.GetBytes(salt);
        private readonly int _rounds = useKeyStretching ? 2017 : 1;
        private readonly List<string> _keys = [];

        /// <summary>
        /// Gets the key for the specified index.
        /// </summary>
        /// <param name="index">The index to get the key for.</param>
        /// <returns>
        /// The key for the specified index.
        /// </returns>
        public string this[int index]
        {
            get
            {
                while (index >= _keys.Count)
                {
                    GenerateKeys();
                }

                return _keys[index];
            }
        }

        /// <summary>
        /// Generates the key for the specified index.
        /// </summary>
        /// <param name="salt">The salt to use.</param>
        /// <param name="index">The index to generate the key for.</param>
        /// <param name="rounds">The number of rounds of hashing to perform.</param>
        /// <returns>
        /// The key for the specified index.
        /// </returns>
        private static string GenerateKey(ReadOnlySpan<byte> salt, int index, int rounds)
        {
            const int MaximumIndexLength = 10;

            Span<byte> hash = stackalloc byte[Md5.HashSizeInBytes];
            Span<byte> buffer = stackalloc byte[Math.Max(salt.Length + MaximumIndexLength, Md5.HashSizeInBytes * 2)];

            salt.CopyTo(buffer);
            index.TryFormat(buffer[salt.Length..], out int written, provider: CultureInfo.InvariantCulture);

            int length = salt.Length + written;

            for (int i = 0; i < rounds; i++)
            {
                Md5.HashData(buffer[..length], hash);
                length = ToHexLower(hash, buffer);
            }

            return Encoding.ASCII.GetString(buffer[..length]);
        }

        /// <summary>
        /// Writes the lowercase hexadecimal representation of the specified bytes to the specified buffer.
        /// </summary>
        /// <param name="bytes">The bytes to convert.</param>
        /// <param name="destination">The buffer to write the hexadecimal characters to.</param>
        /// <returns>
        /// The number of bytes written to <paramref name="destination"/>.
        /// </returns>
        private static int ToHexLower(ReadOnlySpan<byte> bytes, Span<byte> destination)
        {
            ReadOnlySpan<byte> digits = "0123456789abcdef"u8;

            for (int i = 0; i < bytes.Length; i++)
            {
                destination[i * 2] = digits[bytes[i] >> 4];
                destination[(i * 2) + 1] = digits[bytes[i] & 0xF];
            }

            return bytes.Length * 2;
        }

        /// <summary>
        /// Generates the keys for the next chunk of indexes.
        /// </summary>
        private void GenerateKeys()
        {
            int start = _keys.Count;
            string[] keys = new string[ChunkSize];

            Parallel.For(0, keys.Length, (offset) => keys[offset] = GenerateKey(_salt, start + offset, _rounds));

            _keys.AddRange(keys);
        }
    }
}
