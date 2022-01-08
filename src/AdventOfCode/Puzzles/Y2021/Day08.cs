// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/8</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 08, "Seven Segment Search", RequiresData = true)]
public sealed class Day08 : Puzzle
{
    /// <summary>
    /// Gets the number of instances of digits that use a unique number of LED segments.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Gets the sum of the decoded output values.
    /// </summary>
    public int Sum { get; private set; }

    /// <summary>
    /// Returns the number of instances of digits that use a unique number of LED segments
    /// for the specified entries detailing how the LED segments are lit.
    /// </summary>
    /// <param name="entries">The entries to count the digits for.</param>
    /// <returns>
    /// The number of instances of digits that use a unique number of LED
    /// segments and the sum of the output values from decoding the digits.
    /// </returns>
    public static (int Count, int Sum) DecodeDigits(IList<string> entries)
    {
        var panels = new List<(string[] Signals, string[] Numbers)>(entries.Count);

        foreach (string entry in entries)
        {
            string[] parts = entry.Split(" | ");
            string[] signals = parts[0].Split(' ');
            string[] numbers = parts[1].Split(' ');

            panels.Add((signals, numbers));
        }

        int count = 0;
        int sum = 0;

        foreach ((string[] signals, string[] numbers) in panels)
        {
            int value = Decode(signals, numbers);

            count += Maths.Digits(value).Count((p) => p == 1 || p == 4 || p == 7 || p == 8);
            sum += value;
        }

        return (count, sum);

        static int Decode(string[] signals, string[] numbers)
        {
            var digits = new Dictionary<int, HashSet<char>>(10);

            foreach (string signal in signals)
            {
                int? key = signal.Length switch
                {
                    // A signal with these numbers of elements must be a specific digit
                    2 => 1,
                    3 => 7,
                    4 => 4,
                    7 => 8,
                    _ => null,
                };

                if (key is { } value)
                {
                    digits[value] = new(signal);
                }
            }

            // 3 is the same as 7 plus two other elements
            digits[3] = new(signals
                .Where((p) => p.Length == 5)
                .Where((p) => digits[7].IsSubsetOf(p))
                .Single());

            // 9 is the same as 3 plus one other element
            digits[9] = new(signals
                .Where((p) => p.Length == 6)
                .Where((p) => digits[3].IsSubsetOf(p))
                .Single());

            // 0 is the only remaining unknown signal that contains 7's elements
            digits[0] = new(signals
                .Where((p) => p.Length == 6)
                .Where((p) => digits[7].IsSubsetOf(p))
                .Where((p) => !digits.Any((r) => r.Value.SetEquals(p)))
                .Single());

            // 6 is the only remaining signal with six elements
            digits[6] = new(signals
                .Where((p) => p.Length == 6)
                .Where((p) => !digits.Any((r) => r.Value.SetEquals(p)))
                .Single());

            // 5 is a proper subset of 6
            digits[5] = new(signals
                .Where((p) => p.Length == 5)
                .Where((p) => new HashSet<char>(p).IsProperSubsetOf(digits[6]))
                .Single());

            // 2 is the only remaining signal with 5 elements
            digits[2] = new(signals
                .Where((p) => p.Length == 5)
                .Where((p) => !digits.Any((r) => r.Value.SetEquals(p)))
                .Single());

            var output = new List<int>(numbers.Length);

            foreach (string digit in numbers)
            {
                output.Add(digits.Where((p) => p.Value.SetEquals(digit)).Single().Key);
            }

            return Maths.FromDigits<int>(output);
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> entries = await ReadResourceAsLinesAsync();

        (Count, Sum) = DecodeDigits(entries);

        if (Verbose)
        {
            Logger.WriteLine("There are {0:N0} instances of digits that use a unique number of LED segments.", Count);
            Logger.WriteLine("The sum of the output values is {0:N0}.", Sum);
        }

        return PuzzleResult.Create(Count, Sum);
    }
}
