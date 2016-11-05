﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2015/day/17</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day17 : Puzzle
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
        protected override int MinimumArguments => 1;

        /// <summary>
        /// Returns the combinations of containers that can be used to completely fill
        /// one or more containers completely with the specified total volume of eggnog.
        /// </summary>
        /// <param name="volume">The volume of eggnog.</param>
        /// <param name="containerVolumes">The volumes of the containers.</param>
        /// <returns>
        /// The combinations of containers that can store the volume specified by <paramref name="volume"/>.
        /// </returns>
        internal static IList<ICollection<long>> GetContainerCombinations(int volume, IList<int> containerVolumes)
        {
            var containers = containerVolumes
                .OrderByDescending((p) => p)
                .ToList();

            return Maths.GetCombinations(volume, containers);
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            var containerVolumes = ReadResourceAsLines()
                .Select((p) => ParseInt32(p))
                .ToList();

            int volume = ParseInt32(args[0]);

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
    }
}
