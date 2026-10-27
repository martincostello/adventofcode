// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/12</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2016, 12, "Leonardo's Monorail", RequiresData = true)]
public sealed class Day12 : Puzzle<int, int>
{
    /// <summary>
    /// An enumeration of the operations that can be performed by an instruction.
    /// </summary>
    private enum Operation
    {
        /// <summary>
        /// An unknown operation that does nothing.
        /// </summary>
        None,

        /// <summary>
        /// Copies a value into a register.
        /// </summary>
        Copy,

        /// <summary>
        /// Increments the value of a register.
        /// </summary>
        Increment,

        /// <summary>
        /// Decrements the value of a register.
        /// </summary>
        Decrement,

        /// <summary>
        /// Toggles another instruction.
        /// </summary>
        Toggle,

        /// <summary>
        /// Jumps to another instruction if a value is not zero.
        /// </summary>
        JumpIfNotZero,

        /// <summary>
        /// Outputs a value as a clock signal.
        /// </summary>
        Output,
    }

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
        // Parse the instructions once before possible modification
        var program = new Instruction[instructions.Count];

        for (int i = 0; i < program.Length; i++)
        {
            program[i] = Instruction.Parse(instructions[i]);
        }

        int[] registers = [initialValueOfA, 0, initialValueOfC, 0];

        for (int i = 0; i < program.Length; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var instruction = program[i];

            switch (instruction.Operation)
            {
                case Operation.Copy:
                    if (instruction.IsRegisterY)
                    {
                        registers[instruction.Y] = instruction.ValueOfX(registers);
                    }

                    break;

                case Operation.Increment:
                    if (instruction.IsRegisterX)
                    {
                        registers[instruction.X]++;
                    }

                    break;

                case Operation.Decrement:
                    if (instruction.IsRegisterX)
                    {
                        registers[instruction.X]--;
                    }

                    break;

                case Operation.Toggle:
                    int target = i + instruction.ValueOfX(registers);

                    if (target >= 0 && target < program.Length)
                    {
                        program[target] = program[target].Toggle();
                    }

                    break;

                case Operation.JumpIfNotZero:
                    if (instruction.ValueOfX(registers) != 0)
                    {
                        i += instruction.ValueOfY(registers) - 1;
                    }

                    break;

                case Operation.Output:
                    if (signal?.Invoke(instruction.ValueOfX(registers)) == true)
                    {
                        // Force the signal to stop being repeated
                        i = program.Length;
                    }

                    break;

                case Operation.None:
                default:
                    break;
            }
        }

        return new Dictionary<char, int>(4)
        {
            ['a'] = registers[0],
            ['b'] = registers[1],
            ['c'] = registers[2],
            ['d'] = registers[3],
        };
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

    /// <summary>
    /// A structure representing a parsed instruction.
    /// </summary>
    /// <param name="Operation">The operation to perform.</param>
    /// <param name="X">The first operand, which is either a register index or a literal value.</param>
    /// <param name="IsRegisterX">Whether the first operand is a register.</param>
    /// <param name="Y">The second operand, which is either a register index or a literal value.</param>
    /// <param name="IsRegisterY">Whether the second operand is a register.</param>
    private readonly record struct Instruction(Operation Operation, int X, bool IsRegisterX, int Y, bool IsRegisterY)
    {
        /// <summary>
        /// Parses the specified instruction.
        /// </summary>
        /// <param name="value">The instruction to parse.</param>
        /// <returns>
        /// The <see cref="Instruction"/> parsed from <paramref name="value"/>.
        /// </returns>
        public static Instruction Parse(string value)
        {
            string[] split = value.Split(' ');

            var operation = split[0] switch
            {
                "cpy" => Operation.Copy,
                "inc" => Operation.Increment,
                "dec" => Operation.Decrement,
                "tgl" => Operation.Toggle,
                "jnz" => Operation.JumpIfNotZero,
                "out" => Operation.Output,
                _ => Operation.None,
            };

            (int x, bool isRegisterX) = split.Length > 1 ? ParseOperand(split[1]) : (0, false);
            (int y, bool isRegisterY) = split.Length > 2 ? ParseOperand(split[2]) : (0, false);

            return new(operation, x, isRegisterX, y, isRegisterY);

            static (int Value, bool IsRegister) ParseOperand(string operand)
            {
                if (TryParse(operand, out int value))
                {
                    return (value, false);
                }

                return (operand[0] - 'a', true);
            }
        }

        /// <summary>
        /// Gets the value of the first operand.
        /// </summary>
        /// <param name="registers">The current values of the registers.</param>
        /// <returns>
        /// The value of the first operand.
        /// </returns>
        public int ValueOfX(ReadOnlySpan<int> registers)
            => IsRegisterX ? registers[X] : X;

        /// <summary>
        /// Gets the value of the second operand.
        /// </summary>
        /// <param name="registers">The current values of the registers.</param>
        /// <returns>
        /// The value of the second operand.
        /// </returns>
        public int ValueOfY(ReadOnlySpan<int> registers)
            => IsRegisterY ? registers[Y] : Y;

        /// <summary>
        /// Toggles the instruction.
        /// </summary>
        /// <returns>
        /// The toggled instruction.
        /// </returns>
        public Instruction Toggle() => Operation switch
        {
            Operation.Increment => this with { Operation = Operation.Decrement },
            Operation.Decrement or Operation.Toggle or Operation.Output => this with { Operation = Operation.Increment },
            Operation.JumpIfNotZero => this with { Operation = Operation.Copy },
            Operation.Copy => this with { Operation = Operation.JumpIfNotZero },
            _ => this,
        };
    }
}
