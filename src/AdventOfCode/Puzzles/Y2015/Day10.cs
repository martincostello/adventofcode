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
    /// Gets the 'look-and-say' representation of a <see cref="string"/>.
    /// </summary>
    /// <param name="value">The value to get the 'look-and-say' result for.</param>
    /// <returns>The 'look-and-say' representation of <paramref name="value"/>.</returns>
    public static string AsLookAndSay(string value)
    {
        var input = new Queue<char>(value);
        var output = new StringBuilder(input.Count);

        while (input.Count > 0)
        {
            char current = input.Dequeue();
            int count = 1;

            while (input.Count > 0 && input.Peek() == current)
            {
                input.Dequeue();
                count++;
            }

            output.Append(count);
            output.Append(current);
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
