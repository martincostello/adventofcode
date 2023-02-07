// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2020/day/16</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2020, 16, "Ticket Translation", RequiresData = true)]
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
    public static (int ErrorRate, IDictionary<string, int> Ticket) ScanTickets(IList<string> notes)
    {
        int indexOfFirstTicket = notes.IndexOf("your ticket:") + 1;
        int indexOfSecondTicket = notes.IndexOf("nearby tickets:") + 1;

        var rules = new Dictionary<string, ICollection<Range>>(indexOfFirstTicket - 2);

        // Parse the rules
        foreach (string line in notes.Take(indexOfFirstTicket - 2))
        {
            (string name, string instruction) = line.Bifurcate(':');
            string[] split = instruction.Split(" or ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var ranges = new Range[split.Length];

            for (int i = 0; i < split.Length; i++)
            {
                (int start, int end) = split[i].AsNumberPair<int>('-');
                ranges[i] = new(start, end);
            }

            rules[name] = ranges;
        }

        var allTickets = new IList<int>[notes.Count - indexOfSecondTicket + 1];

        allTickets[0] = notes[indexOfFirstTicket]
            .AsNumbers<int>()
            .ToArray();

        // Parse the nearby tickets
        for (int i = indexOfSecondTicket; i < notes.Count; i++)
        {
            allTickets[i - indexOfSecondTicket + 1] = notes[i]
                .AsNumbers<int>()
                .ToArray();
        }

        int invalidValues = 0;
        var validTickets = new List<IList<int>>();

        // Find the valid tickets
        for (int i = 1; i < allTickets.Length; i++)
        {
            var ticket = allTickets[i];
            bool isValid = true;

            for (int j = 0; j < ticket.Count && isValid; j++)
            {
                int value = ticket[j];

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
        var possibleIndexes = new Dictionary<string, List<int>>(rules.Keys.Count);
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
        var yourTicket = new Dictionary<string, int>(indexes.Count);

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
        IList<string> values = await ReadResourceAsLinesAsync(cancellationToken);

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
