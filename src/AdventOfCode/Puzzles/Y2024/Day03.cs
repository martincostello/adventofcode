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

    /// <summary>
    /// Gets the sum of the multiplications with enhanced accuracy.
    /// </summary>
    public int AccurateSum { get; private set; }

    [GeneratedRegex(@"mul\([0-9]+\,[0-9]+\)")]
    private static partial Regex Instructions { get; }

    [GeneratedRegex(@"do(n\'t)?\(\)")]
    private static partial Regex Enabled { get; }

    /// <summary>
    /// Scans the specified list of instructions and returns the sum of the multiplications.
    /// </summary>
    /// <param name="memory">The memory to scan.</param>
    /// <param name="enhancedAccuracy">Whether to enable enhanced accuracy.</param>
    /// <returns>
    /// The sum of the multiplications.
    /// </returns>
    public static int Scan(ReadOnlySpan<char> memory, bool enhancedAccuracy)
    {
        const string Prefix = "mul(";

        int sum = 0;
        var ranges = new List<Range>();

        if (enhancedAccuracy)
        {
            int firstDisabled = -1;
            int lastEnabled = -1;

            foreach (var match in Enabled.EnumerateMatches(memory))
            {
                if (match.Length is 4)
                {
                    lastEnabled = match.Index;
                    ranges.Add(new(match.Index, match.Index + match.Length));
                }
                else if (firstDisabled is -1)
                {
                    firstDisabled = match.Index;
                }
            }

            ranges.Add(new(0, firstDisabled - 1));
            ranges.Add(new(lastEnabled, memory.Length));
        }
        else
        {
            ranges.Add(new(0, memory.Length));
        }

        foreach (var match in Instructions.EnumerateMatches(memory))
        {
            bool found = false;

            foreach (var r in ranges)
            {
                if (match.Index >= r.Start.Value &&
                    match.Index + match.Length <= r.End.Value - 1)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                continue;
            }

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

        Sum = Scan(memory, enhancedAccuracy: false);
        AccurateSum = Scan(memory, enhancedAccuracy: true);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the multiplications is {0}", Sum);
            Logger.WriteLine("The sum of the multiplications with more accuracy is {0}", AccurateSum);
        }

        return PuzzleResult.Create(Sum, AccurateSum);
    }
}
