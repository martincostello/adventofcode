// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/13</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2020, 13, RequiresData = true)]
    public sealed class Day13 : Puzzle
    {
        /// <summary>
        /// Gets the product of the ID of the earliest bus and the number of minutes to wait for it.
        /// </summary>
        public int BusWaitProduct { get; private set; }

        /// <summary>
        /// Gets the product of the earliest busand the number of minutes to wait for it.
        /// </summary>
        /// <param name="notes">The notes for the bus schedules.</param>
        /// <returns>
        /// The product of the ID of the earliest bus and the number of minutes to wait for it.
        /// </returns>
        public static int GetEarliestBusWaitProduct(IList<string> notes)
        {
            int timestamp = ParseInt32(notes[0]);

            int[] buses = notes[1]
                .Split(',')
                .Where((p) => !string.Equals(p, "x", StringComparison.Ordinal))
                .Select((p) => ParseInt32(p))
                .ToArray();

            var nextBusesInMinutes = new Dictionary<int, int>(buses.Length);

            foreach (int id in buses)
            {
                int busesSoFar = timestamp / id;
                int lastBusAt = busesSoFar * id;
                int nextBusAt = lastBusAt + id;

                nextBusesInMinutes[id] = nextBusAt - timestamp;
            }

            var nextBus = nextBusesInMinutes
                .OrderBy((p) => p.Value)
                .Select((p) => new { Id = p.Key, WaitInMinutes = p.Value })
                .First();

            return nextBus.Id * nextBus.WaitInMinutes;
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> notes = await ReadResourceAsLinesAsync();

            BusWaitProduct = GetEarliestBusWaitProduct(notes);

            if (Verbose)
            {
                Logger.WriteLine("The product of the ID of the earliest and the number of minutes to wait is {0}.", BusWaitProduct);
            }

            return PuzzleResult.Create(BusWaitProduct);
        }
    }
}
