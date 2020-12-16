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
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/16</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2020, 16, RequiresData = true)]
    public sealed class Day16 : Puzzle
    {
        /// <summary>
        /// Gets the ticket scanning error rate.
        /// </summary>
        public int ScanningErrorRate { get; private set; }

        /// <summary>
        /// Gets the product of a number of values from the specified set of values
        /// that which when added together equal 2020.
        /// </summary>
        /// <param name="notes">The values to find the 2020 sum's product from.</param>
        /// <returns>
        /// The ticket scanning error rate.
        /// </returns>
        public static int GetScanningErrorRate(IList<string> notes)
        {
            var rules = new Dictionary<string, ICollection<Range>>();
            var tickets = new List<IList<int>>();

            int indexOfFirstTicket = notes.IndexOf("your ticket:") + 1;
            int indexOfSecondTicket = notes.IndexOf("nearby tickets:") + 1;

            foreach (string line in notes.Take(indexOfFirstTicket - 2))
            {
                string[] split = line.Split(':');

                string name = split[0];

                split = split[1].Split(" or ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                var ranges = new List<Range>();

                foreach (string range in split)
                {
                    string[] parts = range.Split('-');

                    int start = ParseInt32(parts[0]);
                    int end = ParseInt32(parts[1]);

                    ranges.Add(new Range(start, end));
                }

                rules[name] = ranges;
            }

            foreach (string line in notes.Skip(indexOfSecondTicket).Prepend(notes[indexOfFirstTicket]))
            {
                int[] ticket = line
                    .Split(',')
                    .Select((p) => ParseInt32(p))
                    .ToArray();

                tickets.Add(ticket);
            }

            int invalidValues = 0;

            foreach (IList<int> ticket in tickets.Skip(1))
            {
                bool isValid = true;

                for (int i = 0; i < ticket.Count && isValid; i++)
                {
                    int value = ticket[i];

                    foreach (ICollection<Range> ranges in rules.Values)
                    {
                        isValid = ranges.Any((p) => value >= p.Start.Value && value <= p.End.Value);

                        if (isValid)
                        {
                            break;
                        }
                    }

                    if (!isValid)
                    {
                        invalidValues += value;
                    }
                }
            }

            return invalidValues;
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> values = await ReadResourceAsLinesAsync();

            ScanningErrorRate = GetScanningErrorRate(values);

            if (Verbose)
            {
                Logger.WriteLine("The ticket scanning error rate is {0}.", ScanningErrorRate);
            }

            return PuzzleResult.Create(ScanningErrorRate);
        }
    }
}
