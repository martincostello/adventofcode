﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/5</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day05 : Puzzle2020
    {
        /// <summary>
        /// Gets the highest seat Id of the scanned boarding passes.
        /// </summary>
        public int HighestSeatId { get; private set; }

        /// <summary>
        /// Scans the specified boarding pass to get the seat information.
        /// </summary>
        /// <param name="boardingPass">The boarding pass to scan.</param>
        /// <returns>
        /// The row, column and Id of the specified boarding pass.
        /// </returns>
        public static (int row, int column, int id) ScanBoardingPass(string boardingPass)
        {
            var rowRange = new Range(0, 127);
            var columnRange = new Range(0, 7);

            static void High(ref Range value)
                => value = new Range(value.Start, value.Start.Value + ((value.End.Value - value.Start.Value) / 2));

            static void Low(ref Range value)
                => value = new Range(value.Start.Value + ((value.End.Value - value.Start.Value) / 2) + 1, value.End.Value);

            for (int i = 0; i < boardingPass.Length; i++)
            {
                char ch = boardingPass[i];

                switch (ch)
                {
                    case 'F':
                        High(ref rowRange);
                        break;

                    case 'B':
                        Low(ref rowRange);
                        break;

                    case 'L':
                        High(ref columnRange);
                        break;

                    case 'R':
                        Low(ref columnRange);
                        break;

                    default:
                        throw new InvalidOperationException($"Invalid character '{ch}'.");
                }
            }

            int row = rowRange.Start.Value;
            int column = columnRange.End.Value;

            int id = (row * 8) + column;

            return (row, column, id);
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> boardingPasses = ReadResourceAsLines();

            var ids = new List<int>(boardingPasses.Count);

            foreach (string boardingPass in boardingPasses)
            {
                (_, _, int id) = ScanBoardingPass(boardingPass);
                ids.Add(id);
            }

            HighestSeatId = ids.Max();

            if (Verbose)
            {
                Logger.WriteLine("The highest seat Id from a boarding pass is {0}.", HighestSeatId);
            }

            return 0;
        }
    }
}
