// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Collections;

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/24</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2015, 24, "It Hangs in the Balance", RequiresData = true, IsHidden = true)]
public sealed class Day24 : Puzzle
{
    /// <summary>
    /// Gets the quantum entanglement of the first group
    /// of packages of the optimum package configuration
    /// when there are 3 compartments in the sleigh.
    /// </summary>
    internal long QuantumEntanglementFor3 { get; private set; }

    /// <summary>
    /// Gets the quantum entanglement of the first group
    /// of packages of the optimum package configuration
    /// when there are 4 compartments in the sleigh.
    /// </summary>
    internal long QuantumEntanglementFor4 { get; private set; }

    /// <summary>
    /// Gets the quantum entanglement of the first group of packages of
    /// the ideal configuration for the specified packages and their weights.
    /// </summary>
    /// <param name="compartments">
    /// The number of compartments in the sleigh.
    /// </param>
    /// <param name="weights">
    /// The weights of the packages to find the quantum entanglement for.
    /// </param>
    /// <returns>
    /// The quantum entanglement of the first group of packages of the ideal
    /// configuration of the packages with weights specified by <paramref name="weights"/>.
    /// </returns>
    internal static long GetQuantumEntanglementOfIdealConfiguration(int compartments, List<long> weights)
    {
        // How much should each compartment weigh?
        long total = weights.Sum() / compartments;

        int length = weights.Count;
        var bits = new BitArray(length);
        int limit = (int)Math.Pow(2, length);

        weights.Reverse();

        int minCount = int.MaxValue;
        long minEntanglement = long.MaxValue;

        for (int i = 0; i < limit; i++)
        {
            long sum = 0;

            for (int j = 0; j < length && sum < total; j++)
            {
                if (bits[j])
                {
                    sum += weights[j];
                }
            }

            if (sum == total)
            {
                int count = Count(bits);

                if (count < minCount)
                {
                    minCount = count;
                    minEntanglement = Entanglement(bits, weights);
                }
                else if (count == minCount)
                {
                    minEntanglement = Math.Min(minEntanglement, Entanglement(bits, weights));
                }
            }

            for (int j = 0; j < length; j++)
            {
                if (bits[j] = !bits[j])
                {
                    break;
                }
            }
        }

        return minEntanglement;

        static int Count(BitArray bits)
        {
            uint[] buffer = new uint[(bits.Count >> 5) + 1];
            bits.CopyTo(buffer, 0);
            int count = 0;

            for (int i = 0; i < buffer.Length; i++)
            {
                count += BitOperations.PopCount(buffer[i]);
            }

            return count;
        }

        static long Entanglement(BitArray bits, List<long> weights)
        {
            long entanglement = 1;
            int length = bits.Length;

            for (int j = 0; j < length; j++)
            {
                if (bits[j])
                {
                    entanglement *= weights[j];
                }
            }

            return entanglement;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var weights = await ReadResourceAsNumbersAsync<long>(cancellationToken);

        QuantumEntanglementFor3 = GetQuantumEntanglementOfIdealConfiguration(compartments: 3, weights);
        QuantumEntanglementFor4 = GetQuantumEntanglementOfIdealConfiguration(compartments: 4, weights);

        if (Verbose)
        {
            Logger.WriteLine(
                "The quantum entanglement of the ideal configuration of {0:N0} packages in 3 compartments is {1:N0}.",
                weights.Count,
                QuantumEntanglementFor3);

            Logger.WriteLine(
                "The quantum entanglement of the ideal configuration of {0:N0} packages in 4 compartments is {1:N0}.",
                weights.Count,
                QuantumEntanglementFor4);
        }

        return PuzzleResult.Create(QuantumEntanglementFor3);
    }
}
