// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
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

            Combinations = GetContainerCombinationCount(volume, containerVolumes);

            Console.WriteLine(
                "There are {0:N0} combinations of containers that can store {1:0} liters of eggnog.",
                Combinations,
                volume);

            return 0;
        }

        /// <summary>
        /// Returns the number of combinations of containers that can be used to completely fill
        /// one or more containers completely with the specified total volume of eggnog.
        /// </summary>
        /// <param name="volume">The volume of eggnog.</param>
        /// <param name="containerVolumes">The volumes of the containers.</param>
        /// <returns>
        /// The number of combinations of containers that can store the volume specified by <paramref name="volume"/>.
        /// </returns>
        internal static int GetContainerCombinationCount(int volume, ICollection<int> containerVolumes)
        {
            if (volume > 0 && containerVolumes.Count > 0)
            {
                // TODO Implement
            }

            return 0;
        }
    }
}
