// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// A class representing an Intcode Virtual Machine. This class cannot be inherited.
    /// </summary>
    internal static class IntcodeVM
    {
        /// <summary>
        /// Runs the specified Intcode program.
        /// </summary>
        /// <param name="program">The Intcode program to run.</param>
        /// <param name="input">The input to the program.</param>
        /// <param name="output">The output from the program.</param>
        /// <returns>
        /// The memory values of the program once run.
        /// </returns>
        internal static IReadOnlyList<int> Run(ReadOnlySpan<int> program, int input, out int output)
        {
            output = 0;

            int[] memory = program.ToArray();

            int Add(int counter, int[] modes)
            {
                int first = memory[counter + 1];
                int second = memory[counter + 2];
                int address = memory[counter + 3];
                memory[address] = (modes[0] == 0 ? memory[first] : first) + (modes[1] == 0 ? memory[second] : second);
                return 4;
            }

            int Multiply(int counter, int[] modes)
            {
                int first = memory[counter + 1];
                int second = memory[counter + 2];
                int address = memory[counter + 3];
                memory[address] = (modes[0] == 0 ? memory[first] : first) * (modes[1] == 0 ? memory[second] : second);
                return 4;
            }

            int Input(int counter)
            {
                int address = memory[counter + 1];
                memory[address] = input;
                return 2;
            }

            int Output(int counter, out int result)
            {
                int address = memory[counter + 1];
                result = memory[address];
                return 2;
            }

            static (int opcode, int[] modes, int length) Decode(int instruction)
            {
                if (instruction == 99)
                {
                    return (99, Array.Empty<int>(), 1);
                }

                string digits = instruction.ToString(CultureInfo.InvariantCulture);

                int opcode = digits[^1] - '0';
                int length = opcode switch
                {
                    1 => 4,
                    2 => 4,
                    3 => 2,
                    4 => 2,
                    _ => 1,
                };

                int[] modes = new int[length - 1];
                int j = 0;

                for (int i = digits.Length - 3; i >= 0; i--, j++)
                {
                    modes[j] = digits[i] - '0';
                }

                return (opcode, modes, length);
            }

            for (int i = 0; i < memory.Length;)
            {
                (int opcode, int[] modes, int length) = Decode(memory[i]);

                if (opcode == 99)
                {
                    break;
                }

                switch (opcode)
                {
                    case 1:
                        Add(i, modes);
                        break;

                    case 2:
                        Multiply(i, modes);
                        break;

                    case 3:
                        Input(i);
                        break;

                    case 4:
                        Output(i, out output);
                        break;

                    default:
                        throw new InvalidOperationException($"{opcode} is not a supported opcode.");
                }

                i += length;
            }

            return memory;
        }
    }
}
