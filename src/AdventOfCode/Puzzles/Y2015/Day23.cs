﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/23</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2015, 23, "Opening the Turing Lock", RequiresData = true)]
public sealed class Day23 : Puzzle
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
    /// <param name="logger">The logger to use.</param>
    /// <returns>
    /// A named tuple that contains the values of the a and b registers.
    /// </returns>
    internal static (uint A, uint B) ProcessInstructions(
        IList<string> instructions,
        uint initialValue,
        ILogger logger)
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

            string? operation = split.ElementAtOrDefault(0);
            string? registerOrOffset = split.ElementAtOrDefault(1);
            string? offset = split.ElementAtOrDefault(2);

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
                    next = i + Parse<int>(registerOrOffset!);
                    break;

                case "jie":
                    if ((registerOrOffset!.Split(',')[0].Trim() == "a" ? a : b).Value % 2 == 0)
                    {
                        next = i + Parse<int>(offset!);
                    }

                    break;

                case "jio":
                    if ((registerOrOffset!.Split(',')[0].Trim() == "a" ? a : b).Value == 1)
                    {
                        next = i + Parse<int>(offset!);
                    }

                    break;

                default:
                    logger.WriteLine($"Instruction '{operation}' is not defined.");
                    return (uint.MaxValue, uint.MaxValue);
            }

            if (next == i)
            {
                logger.WriteLine($"Instruction at line {i + 1:N0} creates an infinite loop.");
                break;
            }

            i = next;
        }

        return (a.Value, b.Value);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var instructions = await ReadResourceAsLinesAsync(cancellationToken);
        uint initialValue = args.Length == 1 ? Parse<uint>(args[0]) : 0;

        (A, B) = ProcessInstructions(instructions, initialValue, Logger);

        if (Verbose)
        {
            Logger.WriteLine(
                "After processing {0:N0} instructions, the value of a is {1:N0} and the value of b is {2:N0}.",
                instructions.Count,
                A,
                B);
        }

        return PuzzleResult.Create(A, B);
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
