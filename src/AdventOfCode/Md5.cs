// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Buffers.Binary;

namespace MartinCostello.AdventOfCode;

/// <summary>
/// A class containing a managed implementation of the MD5 message digest algorithm. This class cannot be inherited.
/// </summary>
/// <remarks>
/// The MD5 implementation provided by the runtime delegates to the operating system, which has a high
/// fixed overhead for each call for the small inputs hashed by puzzles and which does not scale when
/// used from multiple threads on Windows. This implementation is intended only for use by puzzles.
/// </remarks>
internal static class Md5
{
    /// <summary>
    /// The size of an MD5 hash, in bytes.
    /// </summary>
    public const int HashSizeInBytes = 16;

    /// <summary>
    /// The size of a block of data processed by the algorithm, in bytes.
    /// </summary>
    private const int BlockSizeInBytes = 64;

    /// <summary>
    /// Gets the per-round additive constants.
    /// </summary>
    private static ReadOnlySpan<uint> K =>
    [
        0xd76aa478, 0xe8c7b756, 0x242070db, 0xc1bdceee,
        0xf57c0faf, 0x4787c62a, 0xa8304613, 0xfd469501,
        0x698098d8, 0x8b44f7af, 0xffff5bb1, 0x895cd7be,
        0x6b901122, 0xfd987193, 0xa679438e, 0x49b40821,
        0xf61e2562, 0xc040b340, 0x265e5a51, 0xe9b6c7aa,
        0xd62f105d, 0x02441453, 0xd8a1e681, 0xe7d3fbc8,
        0x21e1cde6, 0xc33707d6, 0xf4d50d87, 0x455a14ed,
        0xa9e3e905, 0xfcefa3f8, 0x676f02d9, 0x8d2a4c8a,
        0xfffa3942, 0x8771f681, 0x6d9d6122, 0xfde5380c,
        0xa4beea44, 0x4bdecfa9, 0xf6bb4b60, 0xbebfbc70,
        0x289b7ec6, 0xeaa127fa, 0xd4ef3085, 0x04881d05,
        0xd9d4d039, 0xe6db99e5, 0x1fa27cf8, 0xc4ac5665,
        0xf4292244, 0x432aff97, 0xab9423a7, 0xfc93a039,
        0x655b59c3, 0x8f0ccc92, 0xffeff47d, 0x85845dd1,
        0x6fa87e4f, 0xfe2ce6e0, 0xa3014314, 0x4e0811a1,
        0xf7537e82, 0xbd3af235, 0x2ad7d2bb, 0xeb86d391,
    ];

    /// <summary>
    /// Gets the per-round left rotation amounts.
    /// </summary>
    private static ReadOnlySpan<byte> S =>
    [
        7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22,
        5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20,
        4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23,
        6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21,
    ];

    /// <summary>
    /// Computes the MD5 hash of the specified data.
    /// </summary>
    /// <param name="source">The data to hash.</param>
    /// <param name="destination">The buffer to receive the hash value, which must be at least 16 bytes long.</param>
    public static void HashData(ReadOnlySpan<byte> source, Span<byte> destination)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(destination.Length, HashSizeInBytes);

        ulong lengthInBits = (ulong)source.Length * 8;

        uint a0 = 0x67452301;
        uint b0 = 0xefcdab89;
        uint c0 = 0x98badcfe;
        uint d0 = 0x10325476;

        // Process all of the complete blocks
        while (source.Length >= BlockSizeInBytes)
        {
            Compress(source[..BlockSizeInBytes], ref a0, ref b0, ref c0, ref d0);
            source = source[BlockSizeInBytes..];
        }

        // Pad the remaining data to a multiple of the block size, which
        // may require an extra block if there is no space for the length.
        Span<byte> block = stackalloc byte[BlockSizeInBytes];
        block.Clear();

        source.CopyTo(block);
        block[source.Length] = 0x80;

        if (source.Length >= BlockSizeInBytes - sizeof(ulong))
        {
            Compress(block, ref a0, ref b0, ref c0, ref d0);
            block.Clear();
        }

        BinaryPrimitives.WriteUInt64LittleEndian(block[(BlockSizeInBytes - sizeof(ulong))..], lengthInBits);

        Compress(block, ref a0, ref b0, ref c0, ref d0);

        BinaryPrimitives.WriteUInt32LittleEndian(destination, a0);
        BinaryPrimitives.WriteUInt32LittleEndian(destination[4..], b0);
        BinaryPrimitives.WriteUInt32LittleEndian(destination[8..], c0);
        BinaryPrimitives.WriteUInt32LittleEndian(destination[12..], d0);
    }

    /// <summary>
    /// Compresses a single block of data into the hash state.
    /// </summary>
    /// <param name="block">The block of data to compress.</param>
    /// <param name="a0">The first word of the hash state.</param>
    /// <param name="b0">The second word of the hash state.</param>
    /// <param name="c0">The third word of the hash state.</param>
    /// <param name="d0">The fourth word of the hash state.</param>
    private static void Compress(ReadOnlySpan<byte> block, ref uint a0, ref uint b0, ref uint c0, ref uint d0)
    {
        Span<uint> m = stackalloc uint[16];

        for (int i = 0; i < m.Length; i++)
        {
            m[i] = BinaryPrimitives.ReadUInt32LittleEndian(block.Slice(i * sizeof(uint), sizeof(uint)));
        }

        var k = K;
        var s = S;

        uint a = a0;
        uint b = b0;
        uint c = c0;
        uint d = d0;

        for (int i = 0; i < 16; i++)
        {
            uint f = d ^ (b & (c ^ d));
            uint next = b + BitOperations.RotateLeft(a + f + k[i] + m[i], s[i]);
            a = d;
            d = c;
            c = b;
            b = next;
        }

        for (int i = 16; i < 32; i++)
        {
            uint f = c ^ (d & (b ^ c));
            uint next = b + BitOperations.RotateLeft(a + f + k[i] + m[((5 * i) + 1) & 15], s[i]);
            a = d;
            d = c;
            c = b;
            b = next;
        }

        for (int i = 32; i < 48; i++)
        {
            uint f = b ^ c ^ d;
            uint next = b + BitOperations.RotateLeft(a + f + k[i] + m[((3 * i) + 5) & 15], s[i]);
            a = d;
            d = c;
            c = b;
            b = next;
        }

        for (int i = 48; i < 64; i++)
        {
            uint f = c ^ (b | ~d);
            uint next = b + BitOperations.RotateLeft(a + f + k[i] + m[(7 * i) & 15], s[i]);
            a = d;
            d = c;
            c = b;
            b = next;
        }

        a0 += a;
        b0 += b;
        c0 += c;
        d0 += d;
    }
}
