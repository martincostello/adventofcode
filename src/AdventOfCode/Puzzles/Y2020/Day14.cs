// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/14</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2020, 14, RequiresData = true)]
    public sealed class Day14 : Puzzle
    {
        /// <summary>
        /// Gets the sum of all values left in memory after processing completes.
        /// </summary>
        public long SumOfRemainingValues { get; private set; }

        /// <summary>
        /// Runs the specified program.
        /// </summary>
        /// <param name="program">The instructions of the program to run.</param>
        /// <returns>
        /// The sum of the values in memory once the program has been run.
        /// </returns>
        public static long RunProgram(IList<string> program)
        {
            const int Bits = 36;
            const string MaskPrefix = "mask = ";

            char[] splitChars = { '[', ']', '=' };
            StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

            bool?[] mask = new bool?[Bits];
            var memory = new Dictionary<long, BitArray>(program.Count);

            foreach (string instruction in program)
            {
                if (instruction.StartsWith(MaskPrefix, StringComparison.Ordinal))
                {
                    string maskValue = instruction[MaskPrefix.Length..];

                    // Flip the mask to match the endianness of BitArray
                    for (int i = 0; i < maskValue.Length; i++)
                    {
                        mask[i] = maskValue[mask.Length - i - 1] switch
                        {
                            '0' => false,
                            '1' => true,
                            _ => null,
                        };
                    }
                }
                else
                {
                    (long address, long value36) = DecodeInstruction(instruction);

                    byte[] valueBytes = BitConverter.GetBytes(value36);
                    var value = new BitArray(valueBytes);

                    for (int i = 0; i < mask.Length; i++)
                    {
                        bool? maskBit = mask[i];

                        if (maskBit.HasValue)
                        {
                            value[i] = maskBit.Value;
                        }
                    }

                    memory[address] = value;
                }
            }

            byte[] buffer = new byte[8];
            long sum = 0;

            foreach (BitArray value in memory.Values)
            {
                value.CopyTo(buffer, 0);

                sum += BitConverter.ToInt64(buffer, 0);

                Array.Clear(buffer, 0, buffer.Length);
            }

            return sum;

            (long address, long value) DecodeInstruction(string instruction)
            {
                string[] split = instruction.Split(splitChars, splitOptions);
                return (ParseInt64(split[1]), ParseInt64(split[2]));
            }
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> values = await ReadResourceAsLinesAsync();

            SumOfRemainingValues = RunProgram(values);

            if (Verbose)
            {
                Logger.WriteLine("The sum of all values left in memory is {0}.", SumOfRemainingValues);
            }

            return PuzzleResult.Create(SumOfRemainingValues);
        }
    }
}
