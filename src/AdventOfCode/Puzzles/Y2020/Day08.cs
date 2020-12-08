// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/8</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2020, 08, RequiresData = true)]
    public sealed class Day08 : Puzzle
    {
        /// <summary>
        /// An enumeration of CPU operations.
        /// </summary>
        private enum Operation
        {
            /// <summary>
            /// No-op.
            /// </summary>
            NoOp = 0,

            /// <summary>
            /// Updates the value of the accumulator.
            /// </summary>
            Accumulate,

            /// <summary>
            /// Jumps to another instruction.
            /// </summary>
            Jump,
        }

        /// <summary>
        /// Gets the value of the accumulator after one iteration of the program has been completed.
        /// </summary>
        public int Accumulator { get; private set; }

        /// <summary>
        /// Runs the specified program for one iteration.
        /// </summary>
        /// <param name="program">The program to run.</param>
        /// <returns>
        /// The value of the accumulator after the program has run one iteration.
        /// </returns>
        public static int RunProgram(IList<string> program)
        {
            IList<Instruction> instructions = ParseProgram(program);

            var visited = new HashSet<int>();

            int accumulator = 0;

            for (int i = 0; i < instructions.Count; i++)
            {
                if (visited.Contains(i))
                {
                    break;
                }

                visited.Add(i);
                Instruction instruction = instructions[i];

                switch (instruction.Operation)
                {
                    case Operation.Accumulate:
                        accumulator += instruction.Argument;
                        break;

                    case Operation.Jump:
                        i += instruction.Argument - 1;
                        break;

                    default:
                        break;
                }
            }

            return accumulator;
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> program = await ReadResourceAsLinesAsync();

            Accumulator = RunProgram(program);

            if (Verbose)
            {
                Logger.WriteLine("The value of the accumulator after one iteration is {0}.", Accumulator);
            }

            return PuzzleResult.Create(Accumulator);
        }

        /// <summary>
        /// Parses the specified program.
        /// </summary>
        /// <param name="program">The instructions of the program to parse.</param>
        /// <returns>
        /// The parsed CPU instructions.
        /// </returns>
        private static IList<Instruction> ParseProgram(ICollection<string> program)
        {
            var result = new List<Instruction>();

            foreach (string operation in program)
            {
                Operation op = operation.Substring(0, 3) switch
                {
                    "acc" => Operation.Accumulate,
                    "jmp" => Operation.Jump,
                    _ => Operation.NoOp,
                };

                int argument = 0;

                if (op != Operation.NoOp)
                {
                    argument = ParseInt32(operation.AsSpan(4), NumberStyles.Number);
                }

                result.Add(new Instruction() with { Operation = op, Argument = argument });
            }

            return result;
        }

        /// <summary>
        /// Represents a CPU instruction.
        /// </summary>
        private record Instruction
        {
            /// <summary>
            /// Gets the operation to perform.
            /// </summary>
            public Operation Operation { get; init; }

            /// <summary>
            /// Gets the argument of the operation.
            /// </summary>
            public int Argument { get; init; }
        }
    }
}
