// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/4</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2015, 04, "The Ideal Stocking Stuffer", MinimumArguments = 1, IsSlow = true)]
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
        int rangeSize = 5000;
        var solutions = new ConcurrentBag<int>();

        for (int i = 0; !cancellationToken.IsCancellationRequested; i += rangeSize)
        {
            Parallel.For(i, i + rangeSize, (j, state) =>
            {
                if (IsSolution(j, secretKey, zeroes))
                {
                    solutions.Add(j);
                    return;
                }
            });

            if (!solutions.IsEmpty)
            {
                return solutions.Min();
            }
        }

        throw new PuzzleException("No answer was found for the specified secret key.");
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
    /// <param name="value">The value to test.</param>
    /// <param name="secretKey">The secret key to use.</param>
    /// <param name="zeroes">The number of zeroes to get the value for.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is a solution; otherwise <see langword="false"/>.
    /// </returns>
    private static bool IsSolution(int value, string secretKey, int zeroes)
    {
        string formatted = secretKey + value.ToString(CultureInfo.InvariantCulture);
        byte[] buffer = Encoding.UTF8.GetBytes(formatted);
        byte[] hash = MD5.HashData(buffer);

        (int wholeBytes, int remainder) = Math.DivRem(zeroes, 2);

        // Are the whole bytes all zero?
        foreach (byte b in hash.AsSpan(0, wholeBytes))
        {
            if (b is not 0)
            {
                return false;
            }
        }

        // The current value is a solution if there is an even number
        // of zeroes or if the low bits of the odd byte are zero.
        return remainder is not 1 || hash[wholeBytes] < 0x10;
    }
}
