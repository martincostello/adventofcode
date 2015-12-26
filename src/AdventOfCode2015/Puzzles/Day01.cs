// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.IO;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/day/1</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day01 : Puzzle
    {
        /// <summary>
        /// Gets the final floor reached by the instructions.
        /// </summary>
        internal int FinalFloor { get; private set; }

        /// <summary>
        /// Gets the instruction number that first causes the basement to first be entered.
        /// </summary>
        internal int FirstBasementInstruction { get; private set; }

        /// <inheritdoc />
        protected override bool IsFirstArgumentFilePath => true;

        /// <inheritdoc />
        protected override int MinimumArguments => 1;

        /// <summary>
        /// Gets the final floor reached by following the specified set of instructions
        /// and the number of the instruction that first enters the basement.
        /// </summary>
        /// <param name="value">A string containing the instructions to follow.</param>
        /// <returns>
        /// A <see cref="Tuple{T1, T2}"/> that returns the floor Santa is on when the instructions
        /// are followed and the number of the instruction that first causes the basement to be entered.
        /// </returns>
        internal static Tuple<int, int> GetFinalFloorAndFirstInstructionBasementReached(string value)
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

            return Tuple.Create(floor, instructionThatEntersBasement);
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string value = File.ReadAllText(args[0]);

            Tuple<int, int> result = GetFinalFloorAndFirstInstructionBasementReached(value);

            FinalFloor = result.Item1;
            FirstBasementInstruction = result.Item2;

            Console.WriteLine("Santa should go to floor {0}.", FinalFloor);
            Console.WriteLine("Santa first enters the basement after following instruction {0:N0}.", FirstBasementInstruction);

            return 0;
        }
    }
}
