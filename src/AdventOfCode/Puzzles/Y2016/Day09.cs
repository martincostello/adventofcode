// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2016/day/9</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day09 : Puzzle2016
    {
        /// <summary>
        /// An array containing the 'x' character. This field is read-only.
        /// </summary>
        private static readonly char[] XArray = new[] { 'x' };

        /// <summary>
        /// Gets the decompressed length of the data.
        /// </summary>
        public int DecompressedLength { get; private set; }

        /// <summary>
        /// Decompresses the specified data.
        /// </summary>
        /// <param name="data">The data to decompress.</param>
        /// <returns>
        /// The <see cref="string"/> value from decompressing <paramref name="data"/>.
        /// </returns>
        internal static string Decompress(string data)
        {
            bool isInMarker = false;
            bool isInRepeat = false;

            var decompressed = new StringBuilder();

            var marker = new StringBuilder();
            var repeat = new StringBuilder();

            int repeatIndex = 0;
            int repeatCount = 0;
            int repeatLength = 0;

            for (int i = 0; i < data.Length; i++)
            {
                char ch = data[i];

                if (isInMarker)
                {
                    if (ch == ')')
                    {
                        isInMarker = false;

                        string[] split = marker.ToString().Split(XArray, StringSplitOptions.None);

                        repeatIndex = 0;
                        repeatLength = int.Parse(split[0], CultureInfo.InvariantCulture);
                        repeatCount = int.Parse(split[1], CultureInfo.InvariantCulture);

                        isInRepeat = true;
                        marker.Clear();
                    }
                    else
                    {
                        marker.Append(ch);
                    }
                }
                else if (ch == '(' && !isInRepeat)
                {
                    isInMarker = true;
                }
                else if (isInRepeat)
                {
                    repeat.Append(ch);

                    if (++repeatIndex == repeatLength)
                    {
                        string chunk = repeat.ToString();

                        for (int j = 0; j < repeatCount; j++)
                        {
                            decompressed.Append(chunk);
                        }

                        isInRepeat = false;
                        repeat.Clear();
                    }
                }
                else if (!char.IsWhiteSpace(ch))
                {
                    decompressed.Append(ch);
                }
            }

            return decompressed.ToString();
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string data = ReadResourceAsString();

            DecompressedLength = Decompress(data).Length;

            Console.WriteLine($"The decompressed length of the data is {DecompressedLength:N0}.");

            return 0;
        }
    }
}
