// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2016/day/3</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day03 : Puzzle2016
    {
        /// <summary>
        /// Gets the number of possible triangles by columns.
        /// </summary>
        public int PossibleTrianglesByColumns { get; private set; }

        /// <summary>
        /// Gets the number of possible triangles by rows.
        /// </summary>
        public int PossibleTrianglesByRows { get; private set; }

        /// <summary>
        /// Returns the number of valid triangles from the specified triangle instructions.
        /// </summary>
        /// <param name="dimensions">A collection of strings containing the dimensions of possible triangles.</param>
        /// <param name="readAsColumns">Whether to parse the dimensions as columns instead of rows.</param>
        /// <returns>
        /// The number of valid triangles in the dimensions specified in <paramref name="dimensions"/>.
        /// </returns>
        internal static int GetPossibleTriangleCount(ICollection<string> dimensions, bool readAsColumns)
        {
            IList<Tuple<int, int, int>> triangles = ParseTriangles(dimensions, readAsColumns);
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

            PossibleTrianglesByRows = GetPossibleTriangleCount(dimensions, readAsColumns: false);
            PossibleTrianglesByColumns = GetPossibleTriangleCount(dimensions, readAsColumns: true);

            if (Verbose)
            {
                Console.WriteLine("The number of possible triangles using rows is {0:N0}.", PossibleTrianglesByRows);
                Console.WriteLine("The number of possible triangles using columns is {0:N0}.", PossibleTrianglesByColumns);
            }

            return 0;
        }

        /// <summary>
        /// Parses the specified set of triangle dimensions.
        /// </summary>
        /// <param name="dimensions">The triangle dimensions to parse.</param>
        /// <param name="readAsColumns">Whether to parse the dimensions as columns instead of rows.</param>
        /// <returns>
        /// An <see cref="IList{T}"/> containing the parsed triangle dimensions.
        /// </returns>
        private static IList<Tuple<int, int, int>> ParseTriangles(ICollection<string> dimensions, bool readAsColumns)
        {
            var result = new List<Tuple<int, int, int>>(dimensions.Count);

            foreach (string dimension in dimensions)
            {
                string[] components = dimension.Trim().Split(Arrays.Space, StringSplitOptions.RemoveEmptyEntries);

                int a = ParseInt32(components[0]);
                int b = ParseInt32(components[1]);
                int c = ParseInt32(components[2]);

                result.Add(Tuple.Create(a, b, c));
            }

            if (readAsColumns)
            {
                var resultFromColumns = new List<Tuple<int, int, int>>();

                for (int i = 0; i < result.Count; i += 3)
                {
                    var first = result[i];
                    var second = result[i + 1];
                    var third = result[i + 2];

                    resultFromColumns.Add(Tuple.Create(first.Item1, second.Item1, third.Item1));
                    resultFromColumns.Add(Tuple.Create(first.Item2, second.Item2, third.Item2));
                    resultFromColumns.Add(Tuple.Create(first.Item3, second.Item3, third.Item3));
                }

                result = resultFromColumns;
            }

            return result;
        }
    }
}
