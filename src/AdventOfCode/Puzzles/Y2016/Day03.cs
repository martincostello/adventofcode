// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2016/day/3</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day03 : Puzzle2016
    {
        /// <summary>
        /// Gets the number of possible triangles.
        /// </summary>
        public int PossibleTriangles { get; private set; }

        /// <summary>
        /// Returns the number of valid triangles from the specified triangle instructions.
        /// </summary>
        /// <param name="dimensions">A collection of strings containing the dimensions of possible triangles.</param>
        /// <returns>
        /// The number of valid triangles in the dimensions specified in <paramref name="dimensions"/>.
        /// </returns>
        internal static int GetPossibleTriangleCount(ICollection<string> dimensions)
        {
            IList<Tuple<int, int, int>> triangles = ParseTriangles(dimensions);
            return triangles.Count((p) => IsValidTriangle(p.Item1, p.Item2, p.Item3));
        }

        /// <summary>
        /// Retuns whether the dimensions of the specified triangle are valid.
        /// </summary>
        /// <param name="a">The length of the first side.</param>
        /// <param name="b">The length of the second side.</param>
        /// <param name="c">The length of the third side.</param>
        /// <returns>
        /// <see langword="true"/> if the triangle with the specified dimensions is valid; otherwise <see langword="false"/>.
        /// </returns>
        internal static bool IsValidTriangle(int a, int b, int c)
        {
            return
                a + b > c &&
                a + c > b &&
                b + c > a;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> dimensions = ReadResourceAsLines();

            PossibleTriangles = GetPossibleTriangleCount(dimensions);

            Console.Write("The number of possible triangles is {0:N0}.", PossibleTriangles);

            return 0;
        }

        /// <summary>
        /// Parses the specified set of triangle dimensions.
        /// </summary>
        /// <param name="dimensions">The triangle dimensions to parse.</param>
        /// <returns>
        /// An <see cref="IList{T}"/> containing the parsed triangle dimensions.
        /// </returns>
        private static IList<Tuple<int, int, int>> ParseTriangles(ICollection<string> dimensions)
        {
            var result = new List<Tuple<int, int, int>>(dimensions.Count);
            var separator = new[] { ' ' };

            foreach (string dimension in dimensions)
            {
                string[] components = dimension.Trim().Split(separator, StringSplitOptions.RemoveEmptyEntries);

                int a = int.Parse(components[0], CultureInfo.InvariantCulture);
                int b = int.Parse(components[1], CultureInfo.InvariantCulture);
                int c = int.Parse(components[2], CultureInfo.InvariantCulture);

                result.Add(Tuple.Create(a, b, c));
            }

            return result;
        }
    }
}
