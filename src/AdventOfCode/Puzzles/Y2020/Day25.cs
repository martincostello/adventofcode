// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2020/day/25</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2020, 25, "Combo Breaker", RequiresData = true)]
public sealed class Day25 : Puzzle<long>
{
    /// <summary>
    /// Gets the private key associated with the specified card and door public keys.
    /// </summary>
    /// <param name="cardPublicKey">The public key of the key card.</param>
    /// <param name="doorPublicKey">The public key of the door.</param>
    /// <returns>
    /// The shared private key of the card and door.
    /// </returns>
    public static long GetPrivateKey(int cardPublicKey, int doorPublicKey)
    {
        int loopSize = GetLoopSize(cardPublicKey);

        long privateKey = 1;

        for (int i = 0; i < loopSize; i++)
        {
            privateKey = Transform(privateKey, doorPublicKey);
        }

        return privateKey;

        static int GetLoopSize(int publicKey)
        {
            long value = 1;

            foreach (int i in Enumerable.InfiniteSequence(1, 1))
            {
                value = Transform(value, subjectNumber: 7);

                if (value == publicKey)
                {
                    return i;
                }
            }

            return Unsolved;
        }

        static long Transform(long value, int subjectNumber)
            => (value *= subjectNumber) % 2020_12_27;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (values, logger, cancellationToken) =>
            {
                int cardPublicKey = Parse<int>(values[0]);
                int doorPublicKey = Parse<int>(values[1]);

                long privateKey = GetPrivateKey(cardPublicKey, doorPublicKey);

                if (logger is { })
                {
                    logger.WriteLine("The encryption key the handshake is trying to establish is {0}.", privateKey);
                }

                return privateKey;
            },
            cancellationToken);
    }
}
