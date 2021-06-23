// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/5</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2020, 05, RequiresData = true)]
    public sealed class Day05 : Puzzle
    {
        /// <summary>
        /// Gets the highest seat Id of the scanned boarding passes.
        /// </summary>
        public int HighestSeatId { get; private set; }

        /// <summary>
        /// Gets my seat Id from the scanned boarding passes.
        /// </summary>
        public int MySeatId { get; private set; }

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

            static int Midpoint(Range value)
                => value.Start.Value + ((value.End.Value - value.Start.Value) / 2);

            static void High(ref Range value)
                => value = new Range(Midpoint(value) + 1, value.End);

            static void Low(ref Range value)
                => value = new Range(value.Start, Midpoint(value));

            for (int i = 0; i < boardingPass.Length; i++)
            {
                char ch = boardingPass[i];

                switch (ch)
                {
                    case 'F':
                        Low(ref rowRange);
                        break;

                    case 'B':
                        High(ref rowRange);
                        break;

                    case 'L':
                        Low(ref columnRange);
                        break;

                    case 'R':
                        High(ref columnRange);
                        break;

                    default:
                        throw new PuzzleException($"Invalid character '{ch}'.");
                }
            }

            int row = rowRange.Start.Value;
            int column = columnRange.End.Value;

            int id = (row * 8) + column;

            return (row, column, id);
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> boardingPasses = await ReadResourceAsLinesAsync();

            var ids = new List<int>(boardingPasses.Count);

            foreach (string boardingPass in boardingPasses)
            {
                (_, _, int id) = ScanBoardingPass(boardingPass);
                ids.Add(id);
            }

            ids.Sort();

            int last = ids[0];

            foreach (int id in ids.Skip(1))
            {
                if (id != last + 1)
                {
                    MySeatId = id - 1;
                    break;
                }

                last = id;
            }

            HighestSeatId = ids[^1];

            if (Verbose)
            {
                Logger.WriteLine("The highest seat Id from a boarding pass is {0}.", HighestSeatId);
                Logger.WriteLine("My seat Id is {0}.", MySeatId);
            }

            return PuzzleResult.Create(HighestSeatId, MySeatId);
        }
    }
}
