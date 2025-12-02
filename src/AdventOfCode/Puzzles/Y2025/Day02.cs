// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/2</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 02, "Gift Shop", RequiresData = true)]
public sealed class Day02 : Puzzle
{
    /// <summary>
    /// Gets the sum of the invalid product IDs for the first version of the validation rules.
    /// </summary>
    public long InvalidIdSumV1 { get; private set; }

    /// <summary>
    /// Gets the sum of the invalid product IDs for the second version of the validation rules.
    /// </summary>
    public long InvalidIdSumV2 { get; private set; }

    /// <summary>
    /// Gets the sum of the invalid product IDs in the specified ranges.
    /// </summary>
    /// <param name="productIds">The comma-separated list of product ID ranges.</param>
    /// <param name="anyRepeatingSequence">Whether to validate the product ID has no repeated sequence of digits.</param>
    /// <returns>
    /// The sum of the invalid product IDs.
    /// </returns>
    public static long Validate(ReadOnlySpan<char> productIds, bool anyRepeatingSequence)
    {
        long sum = 0;

        Func<long, bool> validator = anyRepeatingSequence ? IsValidV2 : IsValidV1;

        foreach (var span in productIds.Split(','))
        {
            var range = productIds[span];

            using var parts = range.Split('-');

            parts.MoveNext();
            long start = Parse<long>(range[parts.Current]);

            parts.MoveNext();
            long end = Parse<long>(range[parts.Current]);

            for (long i = start; i <= end; i++)
            {
                if (!validator(i))
                {
                    sum += i;
                }
            }
        }

        return sum;

        static bool IsValidV1(long value)
            => IsValid(Maths.Digits(value), chunks: 2);

        static bool IsValidV2(long value)
        {
            var digits = Maths.Digits(value);

            foreach (int factor in Maths.GetFactorsUnordered(digits.Count))
            {
                if (factor is not 1 && !IsValid(digits, factor))
                {
                    return false;
                }
            }

            return true;
        }

        static bool IsValid(List<int> digits, int chunks)
        {
            if (digits.Count % chunks != 0)
            {
                return true;
            }

            int length = digits.Count / chunks;
            var first = digits[0..length];

            for (int i = chunks - 1; i > 0; i--)
            {
                var next = digits.Slice(i * length, length);

                if (!first.SequenceEqual(next))
                {
                    return true;
                }
            }

            return false;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        string productIds = await ReadResourceAsStringAsync(cancellationToken);

        InvalidIdSumV1 = Validate(productIds, anyRepeatingSequence: false);
        InvalidIdSumV2 = Validate(productIds, anyRepeatingSequence: true);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the invalid IDs with the first set of rules is {0}", InvalidIdSumV1);
            Logger.WriteLine("The sum of the invalid IDs with the second set of rules is {0}", InvalidIdSumV2);
        }

        return PuzzleResult.Create(InvalidIdSumV1, InvalidIdSumV2);
    }
}
