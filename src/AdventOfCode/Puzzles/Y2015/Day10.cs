// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/10</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2015, 10, "Elves Look, Elves Say", MinimumArguments = 1, IsSlow = true)]
public sealed class Day10 : Puzzle
{
    /// <summary>
    /// Gets the solution to the puzzle for 40 iterations.
    /// </summary>
    public int Solution40 { get; private set; }

    /// <summary>
    /// Gets the solution to the puzzle for 50 iterations.
    /// </summary>
    public int Solution50 { get; private set; }

    /// <summary>
    /// Gets the 'look-and-say' representation of a span of characters.
    /// </summary>
    /// <param name="value">The value to get the 'look-and-say' result for.</param>
    /// <returns>The 'look-and-say' representation of <paramref name="value"/>.</returns>
    public static string AsLookAndSay(ReadOnlySpan<char> value)
    {
        var output = new StringBuilder(value.Length);

        while (value.Length > 0)
        {
            char current = value[0];
            int count = 1;

            for (int i = 1; i < value.Length; i++)
            {
                if (value[i] != current)
                {
                    break;
                }

                count++;
            }

            output.Append(count);
            output.Append(current);

            value = value[count..];
        }

        return output.ToString();
    }

    /// <inheritdoc />
    protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        string value = args[0];
        string result = value;

        for (int i = 0; i < 40; i++)
        {
            result = AsLookAndSay(result);
        }

        Solution40 = result.Length;

        for (int i = 0; i < 10; i++)
        {
            result = AsLookAndSay(result);
        }

        Solution50 = result.Length;

        if (Verbose)
        {
            Logger.WriteLine("The length of the result for input '{0}' after 40 iterations is {1:N0}.", value, Solution40);
            Logger.WriteLine("The length of the result for input '{0}' after 50 iterations is {1:N0}.", value, Solution50);
        }

        return PuzzleResult.Create(Solution40, Solution50);
    }
}
