// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2017/day/2</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day02 : Puzzle2017
    {
        /// <summary>
        /// Gets the checksum of the spreadsheet.
        /// </summary>
        public int Checksum { get; private set; }

        /// <summary>
        /// Calculates the checksum of the spreadsheet encoded in the specified string.
        /// </summary>
        /// <param name="spreadsheet">An <see cref="IEnumerable{T}"/> containing the rows of integers in the spreadsheet.</param>
        /// <returns>
        /// The checksum of the spreadsheet encoded by <paramref name="spreadsheet"/>.
        /// </returns>
        public static int CalculateChecksum(IEnumerable<IEnumerable<int>> spreadsheet)
        {
            return spreadsheet.Select(ComputeDifference).Sum();
        }

        /// <summary>
        /// Cimputes the difference for the specified row of a spreadsheet.
        /// </summary>
        /// <param name="row">The values in the row of the spreadsheet.</param>
        /// <returns>
        /// The difference between the minimum and maximum values in the row.
        /// </returns>
        public static int ComputeDifference(IEnumerable<int> row)
        {
            int minimum = row.Min();
            int maximum = row.Max();

            return maximum - minimum;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            var lines = ReadResourceAsLines();
            var spreadsheet = ParseSpreadsheet(lines);

            Checksum = CalculateChecksum(spreadsheet);

            return 0;
        }

        /// <summary>
        /// Parses the lines of the specified spreadsheet.
        /// </summary>
        /// <param name="rows">The rows of the spreadsheet.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> containing the columns for each row of the spreadsheet.
        /// </returns>
        private static IEnumerable<IEnumerable<int>> ParseSpreadsheet(ICollection<string> rows)
        {
            var spreadsheet = new List<IEnumerable<int>>(rows.Count);

            foreach (string line in rows)
            {
                IList<int> columns = line
                    .Split('\t')
                    .Select((p) => int.Parse(p, CultureInfo.InvariantCulture))
                    .ToList();

                spreadsheet.Add(columns);
            }

            return spreadsheet;
        }
    }
}
