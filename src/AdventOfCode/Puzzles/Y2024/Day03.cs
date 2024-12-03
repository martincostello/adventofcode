// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Text.RegularExpressions;

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/3</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 03, "Mull It Over", RequiresData = true)]
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

    /// <summary>
    /// A regular expression that finds <c>mul</c> instructions.
    /// </summary>
    [GeneratedRegex(@"mul\([0-9]+\,[0-9]+\)")]
    private static partial Regex Mul { get; }

    /// <summary>
    /// A regular expression that finds <c>do</c> or <c>don't</c> instructions.
    /// </summary>
    [GeneratedRegex(@"do(n\'t)?\(\)")]
    private static partial Regex DoOrDont { get; }

    /// <summary>
    /// Scans the specified list of instructions and returns the sum of the multiplications.
    /// </summary>
    /// <param name="memory">The memory to scan.</param>
    /// <param name="enhancedAccuracy">Whether to enable enhanced accuracy.</param>
    /// <param name="cancellationToken">The optional <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The sum of the multiplications.
    /// </returns>
    public static int Scan(ReadOnlySpan<char> memory, bool enhancedAccuracy, CancellationToken cancellationToken = default)
    {
        const string DoInstruction = "do()";
        const string MulPrefix = "mul(";

        if (enhancedAccuracy)
        {
            var remaining = memory;
            var simplified = new StringBuilder(memory.Length);

            bool enabled = true;

            while (!cancellationToken.IsCancellationRequested)
            {
                var enumerator = DoOrDont.EnumerateMatches(remaining);

                if (enumerator.MoveNext())
                {
                    var match = enumerator.Current;

                    if (enabled)
                    {
                        simplified.Append(remaining[..match.Index]);
                    }

                    remaining = remaining[(match.Index + match.Length)..];
                    enabled = match.Length == DoInstruction.Length;
                }
                else
                {
                    break;
                }
            }

            cancellationToken.ThrowIfCancellationRequested();

            if (enabled)
            {
                simplified.Append(remaining);
            }

            memory = simplified.ToString().AsSpan();
        }

        int sum = 0;

        foreach (var match in Mul.EnumerateMatches(memory))
        {
            var digits = memory.Slice(match.Index + MulPrefix.Length, match.Length - MulPrefix.Length - 1);

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

        Sum = Scan(memory, enhancedAccuracy: false, cancellationToken);
        AccurateSum = Scan(memory, enhancedAccuracy: true, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the multiplications is {0}", Sum);
            Logger.WriteLine("The sum of the multiplications with more accuracy is {0}", AccurateSum);
        }

        return PuzzleResult.Create(Sum, AccurateSum);
    }
}
