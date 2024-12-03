// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Text.RegularExpressions;

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/3</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 03, "Mull It Over", RequiresData = true, IsHidden = true)]
public sealed partial class Day03 : Puzzle
{
    /// <summary>
    /// Gets the sum of the multiplications.
    /// </summary>
    public int Sum { get; private set; }

    [GeneratedRegex(@"mul\([0-9]+\,[0-9]+\)")]
    private static partial Regex Instructions { get; }

    /// <summary>
    /// Scans the specified list of instructions and returns the sum of the multiplications.
    /// </summary>
    /// <param name="memory">The memory to scan.</param>
    /// <returns>
    /// The sum of the multiplications.
    /// </returns>
    public static int Scan(ReadOnlySpan<char> memory)
    {
        int sum = 0;

        foreach (var match in Instructions.EnumerateMatches(memory))
        {
            const string Prefix = "mul(";
            var digits = memory.Slice(match.Index + Prefix.Length, match.Length - Prefix.Length - 1);

            (int x, int y) = digits.AsNumberPair<int>();

            sum += x * y;
        }

        return sum;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        string memory = await ReadResourceAsStringAsync(cancellationToken);

        Sum = Scan(memory);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the multiplications is {0}", Sum);
        }

        return PuzzleResult.Create(Sum);
    }
}
