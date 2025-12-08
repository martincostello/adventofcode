// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/12</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2016, 12, "Leonardo's Monorail", RequiresData = true, IsSlow = true)]
public sealed class Day12 : Puzzle<int, int>
{
    /// <summary>
    /// Processes the specified instructions and returns the values of the CPU registers.
    /// </summary>
    /// <param name="instructions">The instructions to process.</param>
    /// <param name="initialValueOfA">The initial value of register A.</param>
    /// <param name="initialValueOfC">The initial value of register C.</param>
    /// <param name="signal">
    /// An optional delegate to receive any output clock signal; which causes signal
    /// processing to stop if it returns <see langword="true"/>.
    /// </param>
    /// <param name="cancellationToken">The optional cancellation token to use.</param>
    /// <returns>
    /// An <see cref="IDictionary{TKey, TValue}"/> containing the values of the CPU
    /// registers after processing the instructions specified by <paramref name="instructions"/>.
    /// </returns>
    internal static Dictionary<char, int> Process(
        IList<string> instructions,
        int initialValueOfA = 0,
        int initialValueOfC = 0,
        Func<int, bool>? signal = null,
        CancellationToken cancellationToken = default)
    {
        instructions = [.. instructions]; // Copy before possible modification

        var registers = new Dictionary<char, int>()
        {
            ['a'] = initialValueOfA,
            ['b'] = 0,
            ['c'] = initialValueOfC,
            ['d'] = 0,
        };

        for (int i = 0; i < instructions.Count; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string instruction = instructions[i];
            string[] split = instruction.Split(' ');
            int value;

            switch (split[0])
            {
                case "cpy":
                    registers[split[2][0]] = TryParse(split[1], out value) ? value : registers[split[1][0]];
                    break;

                case "inc":
                    registers[split[1][0]]++;
                    break;

                case "dec":
                    registers[split[1][0]]--;
                    break;

                case "tgl":

                    if (!TryParse(split[1], out value))
                    {
                        value = registers[split[1][0]];
                    }

                    int target = i + value;

                    if (target >= 0 && target < instructions.Count)
                    {
                        string otherInstruction = instructions[target];
                        split = otherInstruction.Split(' ');

                        instructions[target] = split.Length == 2 ?
                            string.Equals(split[0], "inc", StringComparison.OrdinalIgnoreCase) ?
                                $"dec {split[1]}" :
                                $"inc {split[1]}" :
                            string.Equals(split[0], "jnz", StringComparison.OrdinalIgnoreCase) ?
                                $"cpy {split[1]} {split[2]}" :
                                $"jnz {split[1]} {split[2]}";
                    }

                    break;

                case "jnz":

                    if (!TryParse(split[1], out value))
                    {
                        value = registers[split[1][0]];
                    }

                    int other;

                    if (!TryParse(split[2], out other))
                    {
                        other = registers[split[2][0]];
                    }

                    if (value != 0)
                    {
                        i += other - 1;
                    }

                    break;

                case "out":

                    if (!TryParse(split[1], out value))
                    {
                        value = registers[split[1][0]];
                    }

                    if (signal?.Invoke(value) == true)
                    {
                        // Force the signal to stop being repeated
                        i = int.MaxValue - 1;
                        break;
                    }

                    break;

                default:
                    break;
            }
        }

        return registers;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (instructions, logger, cancellationToken) =>
            {
                var registers = Process(instructions, initialValueOfC: 0, cancellationToken: cancellationToken);
                int valueInRegisterA = registers['a'];

                registers = Process(instructions, initialValueOfC: 1, cancellationToken: cancellationToken);
                int valueInRegisterAWhenInitializedWithIgnitionKey = registers['a'];

                if (logger is { })
                {
                    logger.WriteLine($"The value left in register a is {valueInRegisterA:N0}.");
                    logger.WriteLine($"The value left in register a if c is initialized to 1 is {valueInRegisterAWhenInitializedWithIgnitionKey:N0}.");
                }

                return (valueInRegisterA, valueInRegisterAWhenInitializedWithIgnitionKey);
            },
            cancellationToken);
    }
}
