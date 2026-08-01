// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Buffers.Text;
using System.Security.Cryptography;

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/4</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2015, 04, "The Ideal Stocking Stuffer", MinimumArguments = 1)]
public sealed class Day04 : Puzzle<int, int>
{
    /// <summary>
    /// Gets the lowest positive integer which when combined with a secret key has an MD5 hash whose
    /// hexadecimal representation starts with the specified number of zeroes.
    /// </summary>
    /// <param name="secretKey">The secret key to use.</param>
    /// <param name="zeroes">The number of zeroes to get the value for.</param>
    /// <param name="cancellationToken"> The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The lowest positive integer that generates an MD5 hash with the number of zeroes specified.
    /// </returns>
    public static int GetLowestPositiveNumberWithStartingZeroes(string secretKey, int zeroes, CancellationToken cancellationToken)
    {
        const int BatchSize = 100_000;
        const int MaxValueLength = 11;

        byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);
        int answer = -1;

        for (int i = 1; answer is -1 && !cancellationToken.IsCancellationRequested; i += BatchSize)
        {
            int start = i;

            Parallel.For(
                0,
                BatchSize,
                () =>
                {
                    byte[] buffer = new byte[keyBytes.Length + MaxValueLength];
                    keyBytes.CopyTo(buffer, 0);
                    return (Hash: MD5.Create(), Buffer: buffer);
                },
                (offset, state, local) =>
                {
                    int value = start + offset;

                    if (IsSolution(local.Hash, local.Buffer, keyBytes.Length, value, zeroes))
                    {
                        int current;

                        do
                        {
                            current = Volatile.Read(ref answer);

                            if (current is not -1 && current <= value)
                            {
                                break;
                            }
                        }
                        while (Interlocked.CompareExchange(ref answer, value, current) != current);

                        state.Break();
                    }

                    return local;
                },
                (local) => local.Hash.Dispose());
        }

        return answer is not -1 ? answer : throw new PuzzleException("No answer was found for the specified secret key.");
    }

    /// <inheritdoc />
    protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return SolveWithArgument(
            args,
            static (secretKey, logger, token) =>
            {
                int lowestZeroHash5 = GetLowestPositiveNumberWithStartingZeroes(secretKey, zeroes: 5, token);
                int lowestZeroHash6 = GetLowestPositiveNumberWithStartingZeroes(secretKey, zeroes: 6, token);

                if (logger is { })
                {
                    logger.WriteLine("The lowest positive number for a hash starting with 5 zeroes is {0:N0}.", lowestZeroHash5);
                    logger.WriteLine("The lowest positive number for a hash starting with 6 zeroes is {0:N0}.", lowestZeroHash6);
                }

                return (lowestZeroHash5, lowestZeroHash6);
            },
            cancellationToken);
    }

    /// <summary>
    /// Returns whether the specified integer is a solution for the specified secret key and number of zeroes.
    /// </summary>
    /// <param name="hash">The <see cref="MD5"/> instance to use to compute the hash.</param>
    /// <param name="buffer">The buffer containing the secret key, with room after it for the value.</param>
    /// <param name="keyLength">The length, in bytes, of the secret key at the start of <paramref name="buffer"/>.</param>
    /// <param name="value">The value to test.</param>
    /// <param name="zeroes">The number of zeroes to get the value for.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is a solution; otherwise <see langword="false"/>.
    /// </returns>
    private static bool IsSolution(MD5 hash, byte[] buffer, int keyLength, int value, int zeroes)
    {
        _ = Utf8Formatter.TryFormat(value, buffer.AsSpan(keyLength), out int written);

        Span<byte> hashBytes = stackalloc byte[MD5.HashSizeInBytes];
        _ = hash.TryComputeHash(buffer.AsSpan(0, keyLength + written), hashBytes, out _);

        (int wholeBytes, int remainder) = Math.DivRem(zeroes, 2);

        // Are the whole bytes all zero?
        foreach (byte b in hashBytes[..wholeBytes])
        {
            if (b is not 0)
            {
                return false;
            }
        }

        // The current value is a solution if there is an even number
        // of zeroes or if the low bits of the odd byte are zero.
        return remainder is not 1 || hashBytes[wholeBytes] < 0x10;
    }
}
