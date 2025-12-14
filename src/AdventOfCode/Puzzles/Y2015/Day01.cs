// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/1</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2015, 01, "Not Quite Lisp", RequiresData = true)]
public sealed class Day01 : Puzzle<int, int>
{
    /// <summary>
    /// Gets the final floor reached by following the specified set of instructions
    /// and the number of the instruction that first enters the basement.
    /// </summary>
    /// <param name="value">A <see cref="ReadOnlySpan{T}"/> containing the instructions to follow.</param>
    /// <returns>
    /// A named tuple that returns the floor Santa is on when the instructions
    /// are followed and the number of the instruction that first causes the basement to be entered.
    /// </returns>
    internal static (int Floor, int InstructionThatEntersBasement) GetFinalFloorAndFirstInstructionBasementReached(ReadOnlySpan<char> value)
    {
        int floor = 0;
        int instructionThatEntersBasement = -1;

        bool hasVisitedBasement = false;

        for (int i = 0; i < value.Length; i++)
        {
            switch (value[i])
            {
                case '(':
                    floor++;
                    break;

                case ')':
                    floor--;
                    break;

                default:
                    break;
            }

            if (!hasVisitedBasement && floor is -1)
            {
                instructionThatEntersBasement = i + 1;
                hasVisitedBasement = true;
            }
        }

        return (floor, instructionThatEntersBasement);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithStringAsync(
            static (value, logger, _) =>
            {
                (int finalFloor, int firstBasementInstruction) = GetFinalFloorAndFirstInstructionBasementReached(value);

                if (logger is { })
                {
                    logger.WriteLine("Santa should go to floor {0}.", finalFloor);
                    logger.WriteLine("Santa first enters the basement after following instruction {0:N0}.", firstBasementInstruction);
                }

                return (finalFloor, firstBasementInstruction);
            },
            cancellationToken);
    }
}
