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
    /// Gets the sum of the invalid product IDs.
    /// </summary>
    public long InvalidIdSum { get; private set; }

    /// <summary>
    /// Gets the sum of the invalid product IDs in the specifed ranges.
    /// </summary>
    /// <param name="productIds">The comma-separated list of product ID ranges.</param>
    /// <returns>
    /// The sum of the invalid product IDs.
    /// </returns>
    public static long Validate(ReadOnlySpan<char> productIds)
    {
        long sum = 0;

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
                var digits = Maths.Digits(i);

                if (digits.Count % 2 == 0)
                {
                    int midpoint = digits.Count / 2;
                    var first = digits[0..midpoint];
                    var last = digits[midpoint..];

                    if (first.SequenceEqual(last))
                    {
                        sum += i;
                    }
                }
            }
        }

        return sum;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        string productIds = await ReadResourceAsStringAsync(cancellationToken);

        InvalidIdSum = Validate(productIds);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the invalid IDs is {0}", InvalidIdSum);
        }

        return PuzzleResult.Create(InvalidIdSum);
    }
}
