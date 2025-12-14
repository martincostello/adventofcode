// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/1</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 01, "Secret Entrance", RequiresData = true)]
public sealed class Day01 : Puzzle<int, int>
{
    /// <summary>
    /// Gets the password to open the door from the specified list of rotations.
    /// </summary>
    /// <param name="rotations">The rotations to perform on the dial to derive the password.</param>
    /// <param name="useMethod0x434C49434B">Whether to use password method <c>0x434C49434B</c> to derive the password.</param>
    /// <returns>
    /// The password to use to open the door.
    /// </returns>
    public static int GetPassword(IList<string> rotations, bool useMethod0x434C49434B)
    {
        const int Steps = 100;

        int previous;
        int current = 50;
        int zeroes = 0;

        foreach (string rotation in rotations)
        {
            int sign = rotation[0] is 'R' ? 1 : -1;
            int cycles = Math.DivRem(Parse<int>(rotation[1..]), Steps, out int distance);

            previous = current;
            current = Math.Abs(current + (sign * distance) + Steps) % Steps;

            if (useMethod0x434C49434B)
            {
                zeroes += cycles;

                if (previous != current && (current is 0 || (previous is not 0 && sign * (current - previous) < 0)))
                {
                    zeroes++;
                }
            }
            else if (current is 0)
            {
                zeroes++;
            }
        }

        return zeroes;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (values, logger, _) =>
            {
                int password1 = GetPassword(values, useMethod0x434C49434B: false);
                int password2 = GetPassword(values, useMethod0x434C49434B: true);

                if (logger is { })
                {
                    logger.WriteLine("The password to open the door is {0}.", password1);
                    logger.WriteLine("The password to open the door using method 0x434C49434B is {0}.", password2);
                }

                return (password1, password2);
            },
            cancellationToken);
    }
}
