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
    /// A class representing the puzzle for <c>http://adventofcode.com/day/17</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day17 : IPuzzle
    {
        /// <summary>
        /// Gets the number of combinations of containers that can be used.
        /// </summary>
        internal int Combinations { get; private set; }

        /// <summary>
        /// Gets the number of combinations of the minimum number of containers that can be used.
        /// </summary>
        internal int CombinationsWithMinimumContainers { get; private set; }

        /// <inheritdoc />
        public int Solve(string[] args)
        {
            if (args.Length != 2)
            {
                Console.Error.WriteLine("No input file path and volume specified.");
                return -1;
            }

            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine("The input file path specified cannot be found.");
                return -1;
            }

            var containerVolumes = File.ReadAllLines(args[0])
                .Select((p) => int.Parse(p, CultureInfo.InvariantCulture))
                .ToList();

            int volume = int.Parse(args[1], CultureInfo.InvariantCulture);

            var combinations = GetContainerCombinations(volume, containerVolumes);
            var combinationsWithLeastContainers = combinations.GroupBy((p) => p.Count).OrderBy((p) => p.Key).First();

            Combinations = combinations.Count;
            CombinationsWithMinimumContainers = combinationsWithLeastContainers.Count();

            Console.WriteLine(
                "There are {0:N0} combinations of containers that can store {1:0} liters of eggnog.",
                Combinations,
                volume);

            Console.WriteLine(
                "There are {0:N0} combinations of containers that can store {1:0} liters of eggnog using {2} containers.",
                CombinationsWithMinimumContainers,
                volume,
                combinationsWithLeastContainers.Key);

            return 0;
        }

        /// <summary>
        /// Returns the combinations of containers that can be used to completely fill
        /// one or more containers completely with the specified total volume of eggnog.
        /// </summary>
        /// <param name="volume">The volume of eggnog.</param>
        /// <param name="containerVolumes">The volumes of the containers.</param>
        /// <returns>
        /// The combinations of containers that can store the volume specified by <paramref name="volume"/>.
        /// </returns>
        internal static IList<ICollection<int>> GetContainerCombinations(int volume, IList<int> containerVolumes)
        {
            var containers = containerVolumes
                .OrderByDescending((p) => p)
                .ToList();

            List<ICollection<int>> result = new List<ICollection<int>>();

            BitArray bits = new BitArray(containerVolumes.Count);

            for (int i = 0; i < Math.Pow(2, bits.Length); i++)
            {
                int sum = 0;

                for (int j = 0; j < bits.Length; j++)
                {
                    if (bits[j])
                    {
                        sum += containers[j];
                    }
                }

                if (sum == volume)
                {
                    List<int> volumes = new List<int>();

                    for (int j = 0; j < bits.Length; j++)
                    {
                        if (bits[j])
                        {
                            volumes.Add(containers[j]);
                        }
                    }

                    result.Add(volumes);
                }

                Increment(bits);
            }

            return result;
        }

        /// <summary>
        /// Increments the value of the specified <see cref="BitArray"/>.
        /// </summary>
        /// <param name="value">The value to increment.</param>
        internal static void Increment(BitArray value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                bool previous = value[i];
                value[i] = !previous;

                if (!previous)
                {
                    return;
                }
            }
        }
    }
}
