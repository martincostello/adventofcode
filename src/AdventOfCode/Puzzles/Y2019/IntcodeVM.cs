// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Channels;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing an Intcode Virtual Machine. This class cannot be inherited.
    /// </summary>
    internal sealed class IntcodeVM
    {
        /// <summary>
        /// The virtual machine's memory. This field is read-only.
        /// </summary>
        private readonly long[] _memory;

        /// <summary>
        /// The virtual machine's output channel. This field is read-only.
        /// </summary>
        private readonly Channel<long> _output;

        /// <summary>
        /// The instruction pointer.
        /// </summary>
        private long _instruction;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntcodeVM"/> class.
        /// </summary>
        /// <param name="instructions">The instructions of the program to run in the VM.</param>
        /// <param name="size">The optional amount of memory to allocate.</param>
        internal IntcodeVM(IEnumerable<long> instructions, int? size = null)
        {
            _memory = instructions.ToArray();

            if (size.HasValue)
            {
                Array.Resize(ref _memory, size.Value);
            }

            Input = Channel.CreateUnbounded<long>().Reader;
            _output = Channel.CreateUnbounded<long>();
        }

        /// <summary>
        /// Occurs when the a value is output.
        /// </summary>
        internal event EventHandler<long>? OnOutput;

        /// <summary>
        /// Gets or sets the input channel for the VM.
        /// </summary>
        internal ChannelReader<long> Input { get; set; }

        /// <summary>
        /// Gets the output reader for the VM.
        /// </summary>
        internal ChannelReader<long> Output => _output.Reader;

        /// <summary>
        /// Parses the specified program.
        /// </summary>
        /// <param name="program">The Intcode program to parse.</param>
        /// <returns>
        /// The instructions of the program to run.
        /// </returns>
        internal static long[] ParseProgram(string program)
        {
            return program
                .Split(',')
                .Select((p) => Puzzle.ParseInt64(p))
                .ToArray();
        }

        /// <summary>
        /// Runs the specified Intcode program as an asynchronous operation.
        /// </summary>
        /// <param name="program">The Intcode program to run.</param>
        /// <param name="input">The input to the program.</param>
        /// <returns>
        /// The output of the program once run.
        /// </returns>
        internal static async Task<IReadOnlyList<long>> RunAsync(IEnumerable<long> program, params long[] input)
        {
            var vm = new IntcodeVM(program)
            {
                Input = await ChannelHelpers.CreateReaderAsync(input),
            };

            if (!await vm.RunAsync())
            {
                throw new InvalidProgramException();
            }

            return await vm.Output.ToListAsync();
        }

        /// <summary>
        /// Gets the current memory of the virtual machine.
        /// </summary>
        /// <returns>
        /// A read-only view of the current memory of the virtual machine.
        /// </returns>
        internal ReadOnlySpan<long> Memory() => _memory;

        /// <summary>
        /// Runs the virtual machine's program as an asynchronous operation.
        /// </summary>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that completes when the program exits
        /// or there is not yet any input available to read.
        /// </returns>
        internal async Task<bool> RunAsync()
        {
            long Read(long index, long offset, int mode)
            {
                return mode switch
                {
                    0 => _memory[index],
                    1 => index,
                    2 => _memory[index + offset],
                    _ => throw new InvalidProgramException(),
                };
            }

            void Write(long index, long offset, int mode, long value)
            {
                long address = mode switch
                {
                    0 => index,
                    2 => index + offset,
                    _ => throw new InvalidProgramException(),
                };

                _memory[address] = value;
            }

            void Add(long current, long offset, int[] modes)
            {
                long x = _memory[current + 1];
                long y = _memory[current + 2];
                long z = _memory[current + 3];

                long left = Read(x, offset, modes[0]);
                long right = Read(y, offset, modes[1]);

                Write(z, offset, modes[2], left + right);
            }

            void Multiply(long counter, long offset, int[] modes)
            {
                long x = _memory[counter + 1];
                long y = _memory[counter + 2];
                long z = _memory[counter + 3];

                long left = Read(x, offset, modes[0]);
                long right = Read(y, offset, modes[1]);

                Write(z, offset, modes[2], left * right);
            }

            void WriteInput(long current, long offset, long value, int[] modes)
            {
                long x = _memory[current + 1];
                Write(x, offset, modes[0], value);
            }

            long ReadOutput(long current, long offset, int[] modes)
            {
                long x = _memory[current + 1];
                return Read(x, offset, modes[0]);
            }

            void JumpIfTrue(ref long current, long offset, int[] modes)
            {
                long x = _memory[current + 1];
                long value = Read(x, offset, modes[0]);

                if (value != 0)
                {
                    long y = _memory[current + 2];
                    current = Read(y, offset, modes[1]);
                }
                else
                {
                    current += 3;
                }
            }

            void JumpIfFalse(ref long current, long offset, int[] modes)
            {
                long x = _memory[current + 1];
                long value = Read(x, offset, modes[0]);

                if (value == 0)
                {
                    long y = _memory[current + 2];
                    current = Read(y, offset, modes[1]);
                }
                else
                {
                    current += 3;
                }
            }

            void LessThan(long current, long offset, int[] modes)
            {
                long x = _memory[current + 1];
                long y = _memory[current + 2];
                long z = _memory[current + 3];

                long left = Read(x, offset, modes[0]);
                long right = Read(y, offset, modes[1]);

                Write(z, offset, modes[2], left < right ? 1 : 0);
            }

            void Equals(long current, long offset, int[] modes)
            {
                long x = _memory[current + 1];
                long y = _memory[current + 2];
                long z = _memory[current + 3];

                long left = Read(x, offset, modes[0]);
                long right = Read(y, offset, modes[1]);

                Write(z, offset, modes[2], left == right ? 1 : 0);
            }

            void Offset(long current, ref long offset, int[] modes)
            {
                long x = _memory[current + 1];
                offset += Read(x, offset, modes[0]);
            }

            static (int opcode, int[] modes, int length) Decode(long instruction)
            {
                if (instruction == 99)
                {
                    return (99, Array.Empty<int>(), 1);
                }

                string digits = instruction.ToString(CultureInfo.InvariantCulture);

                int opcode = digits[^1] - '0';
                int parameters = opcode switch
                {
                    1 => 3,
                    2 => 3,
                    3 => 1,
                    4 => 1,
                    5 => 3,
                    6 => 3,
                    7 => 3,
                    8 => 3,
                    9 => 1,
                    _ => throw new InvalidOperationException($"{opcode} is not a supported opcode."),
                };

                int[] modes = new int[parameters];
                int j = 0;

                for (int i = digits.Length - 3; i >= 0; i--, j++)
                {
                    modes[j] = digits[i] - '0';
                }

                int length = opcode switch
                {
                    5 => 0,
                    6 => 0,
                    _ => parameters + 1,
                };

                return (opcode, modes, length);
            }

            long offset = 0;

            for (; _instruction < _memory.Length;)
            {
                (int opcode, int[] modes, int length) = Decode(_memory[_instruction]);

                if (opcode == 99)
                {
                    break;
                }

                switch (opcode)
                {
                    case 1:
                        Add(_instruction, offset, modes);
                        break;

                    case 2:
                        Multiply(_instruction, offset, modes);
                        break;

                    case 3:
                        if (!Input.TryRead(out long input))
                        {
                            return false;
                        }

                        WriteInput(_instruction, offset, input, modes);
                        break;

                    case 4:
                        long output = ReadOutput(_instruction, offset, modes);
                        await _output.Writer.WriteAsync(output);
                        OnOutput?.Invoke(this, output);
                        break;

                    case 5:
                        JumpIfTrue(ref _instruction, offset, modes);
                        break;

                    case 6:
                        JumpIfFalse(ref _instruction, offset, modes);
                        break;

                    case 7:
                        LessThan(_instruction, offset, modes);
                        break;

                    case 8:
                        Equals(_instruction, offset, modes);
                        break;

                    case 9:
                        Offset(_instruction, ref offset, modes);
                        break;

                    default:
                        throw new InvalidOperationException($"{opcode} is not a supported opcode.");
                }

                _instruction += length;
            }

            _output.Writer.Complete();
            return true;
        }
    }
}
