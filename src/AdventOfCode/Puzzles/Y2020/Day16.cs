// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
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
        /// Gets the product of the departure fields on your ticket.
        /// </summary>
        public long DepartureProduct { get; private set; }

        /// <summary>
        /// Scans the tickets associated with the specified notes.
        /// </summary>
        /// <param name="notes">The notes containing the ticket information.</param>
        /// <returns>
        /// The ticket scanning error rate and your parsed ticket.
        /// </returns>
        public static (int errorRate, IDictionary<string, int> ticket) ScanTickets(IList<string> notes)
        {
            var rules = new Dictionary<string, ICollection<Range>>();
            var allTickets = new List<IList<int>>();

            int indexOfFirstTicket = notes.IndexOf("your ticket:") + 1;
            int indexOfSecondTicket = notes.IndexOf("nearby tickets:") + 1;

            // Parse the rules
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

            // Parse the nearby tickets
            foreach (string line in notes.Skip(indexOfSecondTicket).Prepend(notes[indexOfFirstTicket]))
            {
                int[] ticket = line
                    .Split(',')
                    .Select((p) => ParseInt32(p))
                    .ToArray();

                allTickets.Add(ticket);
            }

            int invalidValues = 0;
            var validTickets = new List<IList<int>>();

            // Find the valid tickets
            foreach (IList<int> ticket in allTickets.Skip(1))
            {
                bool isValid = true;

                for (int i = 0; i < ticket.Count && isValid; i++)
                {
                    int value = ticket[i];

                    foreach (ICollection<Range> ranges in rules.Values)
                    {
                        isValid = ranges.Any((p) => InRange(value, p));

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

                if (isValid)
                {
                    validTickets.Add(ticket);
                }
            }

            // Store indexes where each field could be located
            var possibleIndexes = new Dictionary<string, List<int>>();
            var allIndexes = Enumerable.Range(0, allTickets[0].Count).ToList();

            foreach (string key in rules.Keys)
            {
                possibleIndexes[key] = new List<int>(allIndexes);
            }

            var indexes = new Dictionary<int, string>();

            while (possibleIndexes.Values.Any((p) => p.Count > 1))
            {
                // For any fields where the index is known, remove it from all the other possibilities
                foreach (var index in possibleIndexes.Where((p) => p.Value.Count == 1))
                {
                    int foundIndex = index.Value[0];

                    indexes[foundIndex] = index.Key;

                    foreach (List<int> other in possibleIndexes.Values.Where((p) => p.Count > 1))
                    {
                        other.Remove(foundIndex);
                    }
                }

                // No need to iterate through the rules for fields we know the index for
                foreach (var rule in rules.Where((p) => possibleIndexes[p.Key].Count > 1))
                {
                    // Check if all the values for this index fit the current rule
                    for (int index = 0; index < possibleIndexes.Count; index++)
                    {
                        bool areAllValid = true;

                        for (int j = 0; j < validTickets.Count; j++)
                        {
                            IList<int> ticket = validTickets[j];
                            int value = ticket[index];

                            bool isInRange = rule.Value.Any((p) => InRange(value, p));

                            if (!isInRange)
                            {
                                // This index cannot be the field associated with this rule
                                areAllValid = false;
                                break;
                            }
                        }

                        if (!areAllValid)
                        {
                            // This index cannot be for the field associated with this rule
                            possibleIndexes[rule.Key].Remove(index);
                        }
                    }
                }
            }

            // Build up the ticket from the indexes found
            var yourTicket = new Dictionary<string, int>();

            foreach (var index in indexes)
            {
                yourTicket[index.Value] = allTickets[0][index.Key];
            }

            return (invalidValues, yourTicket);

            static bool InRange(int value, Range range)
                => value >= range.Start.Value && value <= range.End.Value;
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> values = await ReadResourceAsLinesAsync();

            (int scanningErrorRate, var ticket) = ScanTickets(values);

            ScanningErrorRate = scanningErrorRate;

            DepartureProduct = ticket
                .Where((p) => p.Key.StartsWith("departure", StringComparison.Ordinal))
                .Select((p) => (long)p.Value)
                .Aggregate((x, y) => x * y);

            if (Verbose)
            {
                Logger.WriteLine("The ticket scanning error rate is {0}.", ScanningErrorRate);
                Logger.WriteLine("The product of the ticket fields starting with 'departure' is {0}.", DepartureProduct);
            }

            return PuzzleResult.Create(ScanningErrorRate, DepartureProduct);
        }
    }
}
