// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2015/day/1</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2015, 01, RequiresData = true)]
    public sealed class Day01 : Puzzle
    {
        /// <summary>
        /// Gets the final floor reached by the instructions.
        /// </summary>
        internal int FinalFloor { get; private set; }

        /// <summary>
        /// Gets the instruction number that first causes the basement to first be entered.
        /// </summary>
        internal int FirstBasementInstruction { get; private set; }

        /// <summary>
        /// Gets the final floor reached by following the specified set of instructions
        /// and the number of the instruction that first enters the basement.
        /// </summary>
        /// <param name="value">A <see cref="ReadOnlySpan{T}"/> containing the instructions to follow.</param>
        /// <returns>
        /// A named tuple that returns the floor Santa is on when the instructions
        /// are followed and the number of the instruction that first causes the basement to be entered.
        /// </returns>
        internal static (int floor, int instructionThatEntersBasement) GetFinalFloorAndFirstInstructionBasementReached(ReadOnlySpan<char> value)
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

                if (!hasVisitedBasement)
                {
                    if (floor == -1)
                    {
                        instructionThatEntersBasement = i + 1;
                        hasVisitedBasement = true;
                    }
                }
            }

            return (floor, instructionThatEntersBasement);
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            string value = await ReadResourceAsStringAsync();

            (FinalFloor, FirstBasementInstruction) = GetFinalFloorAndFirstInstructionBasementReached(value);

            if (Verbose)
            {
                Logger.WriteLine("Santa should go to floor {0}.", FinalFloor);
                Logger.WriteLine("Santa first enters the basement after following instruction {0:N0}.", FirstBasementInstruction);
            }

            return PuzzleResult.Create(FinalFloor, FirstBasementInstruction);
        }
    }
}
