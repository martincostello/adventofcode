// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/1</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 01, "Secret Entrance", RequiresData = true)]
public sealed class Day01 : Puzzle
{
    /// <summary>
    /// Gets the password to open the door.
    /// </summary>
    public int Password { get; private set; }

    /// <summary>
    /// Gets the password to open the door from the specified list of rotations.
    /// </summary>
    /// <param name="rotations">The rotations to perform on the dial to derive the password.</param>
    /// <returns>
    /// The password to use to open the door.
    /// </returns>
    public static int GetPassword(IList<string> rotations)
    {
        const int Steps = 100;

        int current = 50;
        int zeroes = 0;

        foreach (string rotation in rotations)
        {
            int sign = rotation[0] is 'R' ? 1 : -1;
            int distance = Parse<int>(rotation[1..]) % Steps;

            current = Math.Abs(current + (sign * distance) + Steps) % Steps;

            if (current is 0)
            {
                zeroes++;
            }
        }

        return zeroes;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        Password = GetPassword(values);

        if (Verbose)
        {
            Logger.WriteLine("The password to open the door is {0}", Password);
        }

        return PuzzleResult.Create(Password);
    }
}
