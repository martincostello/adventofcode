// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2015/day/23</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day23 : Puzzle2015
    {
        /// <summary>
        /// Gets the final value of the <c>a</c> register.
        /// </summary>
        internal uint A { get; private set; }

        /// <summary>
        /// Gets the final value of the <c>b</c> register.
        /// </summary>
        internal uint B { get; private set; }

        /// <summary>
        /// Processes the specified instructions and returns the values of registers a and b.
        /// </summary>
        /// <param name="instructions">The instructions to process.</param>
        /// <param name="initialValue">The initial value to use for register a.</param>
        /// <returns>
        /// A <see cref="Tuple{T1, T2}"/> that contains the values of the a and b registers.
        /// </returns>
        internal static Tuple<uint, uint> ProcessInstructions(IList<string> instructions, uint initialValue)
        {
            var a = new Register()
            {
                Value = initialValue,
            };

            var b = new Register();

            for (int i = 0; i >= 0 && i < instructions.Count;)
            {
                string instruction = instructions[i];
                string[] split = instruction.Split(' ');

                string operation = split.ElementAtOrDefault(0);
                string registerOrOffset = split.ElementAtOrDefault(1);
                string offset = split.ElementAtOrDefault(2);

                int next = i + 1;

                switch (operation)
                {
                    case "hlf":
                        (registerOrOffset == "a" ? a : b).Value /= 2;
                        break;

                    case "tpl":
                        (registerOrOffset == "a" ? a : b).Value *= 3;
                        break;

                    case "inc":
                        (registerOrOffset == "a" ? a : b).Value++;
                        break;

                    case "jmp":
                        next = i + ParseInt32(registerOrOffset);
                        break;

                    case "jie":
                        if ((registerOrOffset.Split(',')[0].Trim() == "a" ? a : b).Value % 2 == 0)
                        {
                            next = i + ParseInt32(offset);
                        }

                        break;

                    case "jio":
                        if ((registerOrOffset.Split(',')[0].Trim() == "a" ? a : b).Value == 1)
                        {
                            next = i + ParseInt32(offset);
                        }

                        break;

                    default:
                        Console.Error.WriteLine("Instruction '{0}' is not defined.", operation);
                        return Tuple.Create(uint.MaxValue, uint.MaxValue);
                }

                if (next == i)
                {
                    Console.Error.WriteLine("Instruction at line {0:N0} creates an infinite loop.", i + 1);
                    break;
                }

                i = next;
            }

            return Tuple.Create(a.Value, b.Value);
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> instructions = ReadResourceAsLines();
            uint initialValue = args.Length == 1 ? ParseUInt32(args[0]) : 0;

            Tuple<uint, uint> result = ProcessInstructions(instructions, initialValue);

            A = result.Item1;
            B = result.Item2;

            if (Verbose)
            {
                Console.WriteLine(
                    "After processing {0:N0} instructions, the value of a is {1:N0} and the value of b is {2:N0}.",
                    instructions.Count,
                    A,
                    B);
            }

            return 0;
        }

        /// <summary>
        /// A class representing a processor register. This class cannot be inherited.
        /// </summary>
        private sealed class Register
        {
            /// <summary>
            /// Gets or sets the value of the register.
            /// </summary>
            internal uint Value { get; set; }
        }
    }
}
