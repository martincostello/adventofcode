// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/23</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2016, 23, "Safe Cracking", MinimumArguments = 1, RequiresData = true, IsHidden = true)]
public sealed class Day23 : Puzzle
{
    /// <summary>
    /// Gets the value to send to the safe.
    /// </summary>
    public int SafeValue { get; private set; }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        int input = Parse<int>(args[0]);

        var instructions = await ReadResourceAsLinesAsync(cancellationToken);

        var registers = Day12.Process(instructions, initialValueOfA: input, cancellationToken: cancellationToken);
        SafeValue = registers['a'];

        if (Verbose)
        {
            Logger.WriteLine($"The value to send to the safe for an input of {input:N0} is '{SafeValue}'.");
        }

        return PuzzleResult.Create(SafeValue);
    }
}
