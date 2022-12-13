// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/13</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 13, "Distress Signal", RequiresData = true)]
public sealed class Day13 : Puzzle
{
    /// <summary>
    /// Gets the sum of the indices of the already correctly sorted pairs.
    /// </summary>
    public int SumOfPresortedIndicies { get; private set; }

    /// <summary>
    /// Gets the decoder key for the distress signal.
    /// </summary>
    public int DecoderKey { get; private set; }

    /// <summary>
    /// Returns the sum of the indices of the correctly sorted packet pairs.
    /// </summary>
    /// <param name="values">The packets to analyze.</param>
    /// <returns>
    /// The sum of the indices of the correctly sorted pairs and the decoder key.
    /// </returns>
    public static (int SumOfPresortedIndicies, int DecoderKey) GetSumOfSortedPackets(IList<string> values)
    {
        var pairs = Parse(values);

        int sum = 0;

        for (int i = 0; i < pairs.Count; i++)
        {
            (var left, var right) = pairs[i];

            bool sortedCorrectly = left.CompareTo(right) < 0;

            if (sortedCorrectly)
            {
                sum += i + 1;
            }
        }

        var divider1 = new Packet();
        divider1.Values.Add(new(2));

        var divider2 = new Packet();
        divider2.Values.Add(new(6));

        var sorted = pairs
            .SelectMany((p) => new[] { p.Left, p.Right })
            .Append(divider1)
            .Append(divider2)
            .Order()
            .ToList();

        int dividerIndex1 = 1 + sorted.IndexOf(divider1);
        int dividerIndex2 = 1 + sorted.IndexOf(divider2);

        int decoderKey = dividerIndex1 * dividerIndex2;

        return (sum, decoderKey);

        static List<(Packet Left, Packet Right)> Parse(IList<string> values)
        {
            var pairs = new List<(Packet Left, Packet Right)>();

            for (int i = 0; i < values.Count; i += 3)
            {
                var left = new Packet();
                var right = new Packet();

                _ = Parse(left, values[i].AsSpan()[1..^1]);
                _ = Parse(right, values[i + 1].AsSpan()[1..^1]);

                pairs.Add((left, right));
            }

            return pairs;

            static int Parse(Packet packet, ReadOnlySpan<char> value)
            {
                int read = 0;

                for (int i = 0; i < value.Length; i++)
                {
                    char c = value[i];

                    switch (c)
                    {
                        case '[':
                            var child = new Packet();
                            i += Parse(child, value[(i + 1)..]);
                            packet.Values.Add(child);
                            break;

                        case ']':
                            return i + 1;

                        case ',':
                            break;

                        default:
                            int packetValue = c - '0';

                            if (i < value.Length - 1 && value[i + 1] == '0')
                            {
                                packetValue += 10;
                                i++;
                            }

                            packet.Values.Add(new(packetValue));
                            break;
                    }
                }

                return read;
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync();

        (SumOfPresortedIndicies, DecoderKey) = GetSumOfSortedPackets(values);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the indices of the pairs of packets already in the right order is {0}.", SumOfPresortedIndicies);
            Logger.WriteLine("The decoder key for the distress signal is {0}.", DecoderKey);
        }

        return PuzzleResult.Create(SumOfPresortedIndicies, DecoderKey);
    }

    private sealed class Packet : IComparable<Packet>
    {
        public Packet()
        {
            Values = new List<Packet>();
        }

        public Packet(int value)
        {
            Value = value;
            Values = Array.Empty<Packet>();
        }

        public int? Value { get; }

        public IList<Packet> Values { get; }

        public int CompareTo(Packet? other)
        {
            ArgumentNullException.ThrowIfNull(other);

            if (Value is { } x && other.Value is { } y)
            {
                // If both values are integers, the lower integer should come first.
                // If the left integer is lower than the right integer, the inputs are
                // in the right order.If the left integer is higher than the right
                // integer, the inputs are not in the right order.Otherwise, the inputs
                // are the same integer; continue checking the next part of the input.
                return x.CompareTo(y);
            }
            else if (Value is not { } && other.Value is not { })
            {
                // If both values are lists, compare the first value of each list, then
                // the second value, and so on. If the left list runs out of items first,
                // the inputs are in the right order. If the right list runs out of items
                // first, the inputs are not in the right order. If the lists are the same
                // length and no comparison makes a decision about the order, continue
                // checking the next part of the input.
                for (int i = 0; i < Values.Count && i < other.Values.Count; i++)
                {
                    int comparison = Values[i].CompareTo(other.Values[i]);

                    if (comparison != 0)
                    {
                        return comparison;
                    }
                }

                return Values.Count.CompareTo(other.Values.Count);
            }
            else
            {
                // If exactly one value is an integer, convert the integer to a list which
                // contains that integer as its only value, then retry the comparison.
                var thisAsList = this;
                var otherAsList = other;

                if (Value is { } value)
                {
                    thisAsList = new Packet();
                    thisAsList.Values.Add(new(value));
                }
                else
                {
                    otherAsList = new Packet();
                    otherAsList.Values.Add(new(other.Value.GetValueOrDefault()));
                }

                return thisAsList.CompareTo(otherAsList);
            }
        }

        public override string ToString()
        {
            if (Value is { } value)
            {
                return value.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                return $"[{string.Join(',', Values)}]";
            }
        }
    }
}
