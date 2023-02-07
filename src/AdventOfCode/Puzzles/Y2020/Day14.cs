// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2020/day/14</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2020, 14, "Docking Data", RequiresData = true)]
public sealed class Day14 : Puzzle
{
    /// <summary>
    /// Gets the sum of all values left in memory after processing completes using version 1.
    /// </summary>
    public long SumOfRemainingValuesV1 { get; private set; }

    /// <summary>
    /// Gets the sum of all values left in memory after processing completes using version 2.
    /// </summary>
    public long SumOfRemainingValuesV2 { get; private set; }

    /// <summary>
    /// Runs the specified program.
    /// </summary>
    /// <param name="program">The instructions of the program to run.</param>
    /// <param name="version">The emulator version to use.</param>
    /// <returns>
    /// The sum of the values in memory once the program has been run.
    /// </returns>
    public static long RunProgram(IList<string> program, int version)
    {
        const string MaskPrefix = "mask = ";

        char[] splitChars = { '[', ']', '=' };
        StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

        int floatingBits = 0;
        string mask = string.Empty;

        var memory = new Dictionary<long, long>(program.Count);

        foreach (string instruction in program)
        {
            if (instruction.StartsWith(MaskPrefix, StringComparison.Ordinal))
            {
                // Flip the mask to match the endianness of BitArray
                string maskValue = instruction[MaskPrefix.Length..];
                mask = maskValue.Mirror();
                floatingBits = mask.Count('X');
            }
            else
            {
                (long address, long value) = DecodeInstruction(instruction);

                if (version == 1)
                {
                    long maskedValue = value;

                    for (int i = 0; i < mask.Length; i++)
                    {
                        switch (mask[i])
                        {
                            case '0':
                                maskedValue &= ~(1L << i);
                                break;

                            case '1':
                                maskedValue |= 1L << i;
                                break;
                        }
                    }

                    memory[address] = maskedValue;
                }
                else
                {
                    long limit = (long)Math.Pow(2, floatingBits);

                    for (long i = 0; i < limit; i++)
                    {
                        long baseAddress = address;
                        int offset = 0;

                        for (int bit = 0; bit < mask.Length; bit++)
                        {
                            char ch = mask[bit];

                            if (ch == '1')
                            {
                                baseAddress |= 1L << bit;
                            }
                            else if (ch == 'X')
                            {
                                if (((i >> offset) & 1) == 0)
                                {
                                    baseAddress &= ~(1L << bit);
                                }
                                else
                                {
                                    baseAddress |= 1L << bit;
                                }

                                offset++;
                            }
                        }

                        memory[baseAddress] = value;
                    }
                }
            }
        }

        return memory.Values.Sum();

        (long Address, long Value) DecodeInstruction(string instruction)
        {
            string[] split = instruction.Split(splitChars, splitOptions);
            return (Parse<long>(split[1]), Parse<long>(split[2]));
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> values = await ReadResourceAsLinesAsync(cancellationToken);

        SumOfRemainingValuesV1 = RunProgram(values, version: 1);
        SumOfRemainingValuesV2 = RunProgram(values, version: 2);

        if (Verbose)
        {
            Logger.WriteLine("The sum of all values left in memory using version 1 is {0}.", SumOfRemainingValuesV1);
            Logger.WriteLine("The sum of all values left in memory using version 2 is {0}.", SumOfRemainingValuesV2);
        }

        return PuzzleResult.Create(SumOfRemainingValuesV1, SumOfRemainingValuesV2);
    }
}
