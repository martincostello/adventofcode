// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/day/24</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day24 : IPuzzle
    {
        /// <summary>
        /// Gets the quantum entanglement of the first group
        /// of packages of the optimum package configuration.
        /// </summary>
        internal long QuantumEntanglementOfFirstGroup { get; private set; }

        /// <inheritdoc />
        public int Solve(string[] args)
        {
            if (args.Length != 1 && args.Length != 2)
            {
                Console.Error.WriteLine("No input file path or number of compartments specified.");
                return -1;
            }

            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine("The input file path specified cannot be found.");
                return -1;
            }

            IList<int> weights = File.ReadAllLines(args[0])
                .Select((p) => int.Parse(p, CultureInfo.InvariantCulture))
                .ToList();

            int compartments = args.Length == 2 ? int.Parse(args[1], CultureInfo.InvariantCulture) : 3;

            QuantumEntanglementOfFirstGroup = GetQuantumEntanglementOfIdealConfiguration(compartments, weights);

            Console.WriteLine(
                "The quantum entanglement of the ideal configuration of {0:N0} packages is {1:N0}.",
                weights.Count,
                QuantumEntanglementOfFirstGroup);

            return 0;
        }

        /// <summary>
        /// Gets the quantum entanglement of the first group of packages of
        /// the ideal configuration for the specified packages and their weights.
        /// </summary>
        /// <param name="compartments">
        /// The number of comparments in the sleigh.
        /// </param>
        /// <param name="weights">
        /// The weights of the packages to find the quantum entanglement for.
        /// </param>
        /// <returns>
        /// The quantum entanglement of the first group of packages of the ideal
        /// configuration of the packages with weights specified by <paramref name="weights"/>.
        /// </returns>
        internal static long GetQuantumEntanglementOfIdealConfiguration(int compartments, IList<int> weights)
        {
            // How much should each compartment weigh?
            int targetCompartmentWeight = weights.Sum() / compartments;

            List<ICollection<long>> result = new List<ICollection<long>>();

            BitArray bits = new BitArray(weights.Count);

            for (int i = 0; i < Math.Pow(2, bits.Length); i++)
            {
                int sum = 0;

                for (int j = 0; j < bits.Length; j++)
                {
                    if (bits[j])
                    {
                        sum += weights[j];
                    }
                }

                if (sum == targetCompartmentWeight)
                {
                    List<long> weightsInCompartment = new List<long>();

                    for (int j = 0; j < bits.Length; j++)
                    {
                        if (bits[j])
                        {
                            weightsInCompartment.Add(weights[j]);
                        }
                    }

                    result.Add(weightsInCompartment);
                }

                Day17.Increment(bits);
            }

            var optimumConfiguration = result
                .Select((p) => new { Count = p.Count, QuantumEntanglement = p.Aggregate((x, y) => x * y) })
                .OrderBy((p) => p.Count)
                .ThenBy((p) => p.QuantumEntanglement)
                .First();

            return optimumConfiguration.QuantumEntanglement;
        }
    }
}
