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
        internal static int GetContainerCombinationCount(int volume, IList<int> containerVolumes)
        {
            var containers = containerVolumes
                .OrderBy((p) => p)
                .Select((p) => new Container() { Volume = p })
                .ToList();

            IList<ISet<Container>> combinations = new List<ISet<Container>>();

            GetContainerCombinations(new HashSet<Container>(), containers, volume, combinations);

            return combinations.Count;
        }

        /// <summary>
        /// Returns the combinations of containers that can be used to completely fill
        /// one or more containers completely with the specified total volume of eggnog.
        /// </summary>
        /// <param name="current">The current set of used containers.</param>
        /// <param name="containers">The remaining containers that can be used.</param>
        /// <param name="targetVolume">The volume of eggnog.</param>
        /// <param name="combinations">The combinations of containers that can store the volume specified by <paramref name="targetVolume"/>.</param>
        private static void GetContainerCombinations(
            ISet<Container> current,
            IList<Container> containers,
            int targetVolume,
            IList<ISet<Container>> combinations)
        {
            for (int i = 0; i < containers.Count; i++)
            {
                var next = containers[i];

                var sequence = new HashSet<Container>(current)
                {
                    next,
                };

                int sum = sequence.Sum((p) => p.Volume);

                if (sum == targetVolume)
                {
                    if (!combinations.Any((p) => p.SetEquals(sequence)))
                    {
                        combinations.Add(sequence);
                    }
                }
                else if (sum < targetVolume && containers.Count > 1)
                {
                    var remaining = new List<Container>(containers);
                    remaining.Remove(next);

                    remaining = remaining
                        .Where((p) => sum + p.Volume <= targetVolume)
                        .ToList();

                    if (remaining.Sum((p) => p.Volume) >= targetVolume)
                    {
                        GetContainerCombinations(sequence, remaining, targetVolume, combinations);
                    }
                }
            }
        }

        /// <summary>
        /// A class representing a container. This class cannot be inherited.
        /// </summary>
        [System.Diagnostics.DebuggerDisplay("{Volume}")]
        private sealed class Container
        {
            /// <summary>
            /// Gets or sets the volume of the container.
            /// </summary>
            internal int Volume { get; set; }
        }
    }
}
