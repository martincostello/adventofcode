// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2019/day/8</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day08 : Puzzle2019
    {
        /// <summary>
        /// Gets the checksum of the image.
        /// </summary>
        public int Checksum { get; private set; }

        /// <summary>
        /// Gets the checksum for the specified image.
        /// </summary>
        /// <param name="image">The image data to get the checksum for.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <returns>
        /// The checksum of the image data.
        /// </returns>
        public static int GetImageChecksum(string image, int height, int width)
        {
            var layers = new List<IList<int>>();
            IList<int>? current = null;

            int pixelsPerLayer = height * width;

            for (int i = 0; i < image.Length; i++)
            {
                if (i % pixelsPerLayer == 0)
                {
                    current = new List<int>();
                    layers.Add(current);
                }

                int value = image[i] - '0';

                current!.Add(value);
            }

            var layerWithLeastZeroes = layers
                .OrderBy((p) => p.Count((r) => r == 0))
                .First();

            int ones = layerWithLeastZeroes.Count((p) => p == 1);
            int twos = layerWithLeastZeroes.Count((p) => p == 2);

            return ones * twos;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string image = ReadResourceAsString().TrimEnd('\n');

            Checksum = GetImageChecksum(image, 6, 25);

            if (Verbose)
            {
                Logger.WriteLine("The checksum of the image data is {0}.", Checksum);
            }

            return 0;
        }
    }
}
