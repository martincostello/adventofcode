// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Collections;

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2017/day/14</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2017, 14, MinimumArguments = 1)]
    public sealed class Day14 : Puzzle
    {
        /// <summary>
        /// Gets the severity of the trip through the firewall.
        /// </summary>
        public int SquaresUsed { get; private set; }

        /// <summary>
        /// Gets the number of squares used for the specified key.
        /// </summary>
        /// <param name="key">The key to use to find the number of squares used.</param>
        /// <returns>
        /// The number of squares used for the specified key string.
        /// </returns>
        public static int GetSquaresUsed(string key)
        {
            int squaresUsed = 0;

            for (int i = 0; i < 128; i++)
            {
                string input = $"{key}-{i}";
                string knotHash = Day10.ComputeHash(input);

                int[] numbers = knotHash
                    .Select((p) => ParseInt32(p.ToString(), NumberStyles.HexNumber))
                    .ToArray();

                var bits = numbers
                    .Select(BitConverter.GetBytes)
                    .Select((p) => new BitArray(p))
                    .ToArray();

                squaresUsed += bits
                    .SelectMany((p) => p.OfType<bool>().Take(4))
                    .Where((p) => p)
                    .Count();
            }

            return squaresUsed;
        }

        /// <inheritdoc />
        protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            string key = args[0];

            SquaresUsed = GetSquaresUsed(key);

            if (Verbose)
            {
                Logger.WriteLine($"The number of squares used for key {key} is {SquaresUsed:N0}.");
            }

            return PuzzleResult.Create(SquaresUsed);
        }
    }
}
