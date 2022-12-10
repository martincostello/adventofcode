// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/10</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 10, "Cathode-Ray Tube", RequiresData = true)]
public sealed class Day10 : Puzzle
{
    /// <summary>
    /// Gets the sum of the signal strengths for the 20th,
    /// 60th, 100th, 140th, 180th and 220th cycles.
    /// </summary>
    public int SumOfSignalStrengths { get; private set; }

    /// <summary>
    /// Gets the sum of the signal strength of executing the specified instructions for the specified cycles.
    /// </summary>
    /// <param name="instructions">The program instructions to execute.</param>
    /// <param name="cycles">The values to the cycles to sum the signal strengths for.</param>
    /// <returns>
    /// The sum of the signal strengths for the specified cycles from executing the specified program.
    /// </returns>
    public static int GetSignalStrengths(
        IList<string> instructions,
        IList<int> cycles)
    {
        int counter = 1;
        int x = 1;

        var values = new Dictionary<int, int>()
        {
            [0] = 0,
            [1] = x,
        };

        foreach (string instruction in instructions)
        {
            switch (instruction[..4])
            {
                case "noop":
                    values[++counter] = x;
                    break;

                case "addx":
                    values[++counter] = x;
                    values[++counter] = x += Parse<int>(instruction[5..]);
                    break;
            }
        }

        int sum = 0;

        foreach (int cycle in cycles)
        {
            sum += values[cycle] * cycle;
        }

        return sum;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync();

        SumOfSignalStrengths = GetSignalStrengths(values, new[] { 20, 60, 100, 140, 180, 220 });

        if (Verbose)
        {
            Logger.WriteLine("The sum of six signal strengths is {0}.", SumOfSignalStrengths);
        }

        return PuzzleResult.Create(SumOfSignalStrengths);
    }
}
