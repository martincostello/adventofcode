// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2016/day/16</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2016, 16, MinimumArguments = 2)]
    public sealed class Day16 : Puzzle
    {
        /// <summary>
        /// Gets the value of the checksum for the disk.
        /// </summary>
        public string? Checksum { get; private set; }

        /// <inheritdoc />
        protected override int MinimumArguments => 2;

        /// <summary>
        /// Returns the checksum for random disk data generated for the
        /// specified initial state and disk size.
        /// </summary>
        /// <param name="initial">The initial state to use to generate the data.</param>
        /// <param name="size">The size of the disk to generate data for.</param>
        /// <returns>
        /// A <see cref="string"/> containing the checksum for the data generated
        /// for a disk of the size specified by <paramref name="size"/> using <paramref name="initial"/>.
        /// </returns>
        internal static string GetDiskChecksum(string initial, int size)
        {
            IList<char> data = GenerateData(initial, size);
            return ComputeChecksum(data);
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string initial = args[0];
            int size = ParseInt32(args[1]);

            Checksum = GetDiskChecksum(initial, size);

            if (Verbose)
            {
                Logger.WriteLine($"The checksum for the generated disk data is '{Checksum}'.");
            }

            return 0;
        }

        /// <summary>
        /// Computes the checksum of the specified data.
        /// </summary>
        /// <param name="data">The data to compute the checksum for.</param>
        /// <returns>
        /// The checksum for the data specified by <paramref name="data"/>.
        /// </returns>
        private static string ComputeChecksum(IList<char> data)
        {
            string? result = null;

            while (result == null)
            {
                int length = data.Count - 1;
                var builder = new StringBuilder(length / 2);

                for (int i = 0; i < length; i += 2)
                {
                    char next = data[i] == data[i + 1] ? '1' : '0';
                    builder.Append(next);
                }

                string checksum = builder.ToString();

                if (builder.Length % 2 != 0)
                {
                    result = checksum;
                }
                else
                {
                    data = new List<char>(checksum);
                }
            }

            return result;
        }

        /// <summary>
        /// Generates data to fill a disk of the specified size.
        /// </summary>
        /// <param name="initial">The initial state to use to generate the data.</param>
        /// <param name="size">The size of the disk to generate data for.</param>
        /// <returns>
        /// An <see cref="IList{T}"/> containing the generated data for
        /// the disk which is equal in length to the value of <paramref name="size"/>.
        /// </returns>
        private static IList<char> GenerateData(string initial, int size)
        {
            var a = new List<char>(initial);

            while (a.Count < size)
            {
                var b = new List<char>(a);
                b.Reverse();

                for (int i = 0; i < b.Count; i++)
                {
                    switch (b[i])
                    {
                        case '0':
                            b[i] = '1';
                            break;

                        case '1':
                            b[i] = '0';
                            break;

                        default:
                            break;
                    }
                }

                a = new List<char>(a.Concat(new[] { '0' }).Concat(b));
            }

            if (a.Count > size)
            {
                a = a.Take(size).ToList();
            }

            return a;
        }
    }
}
