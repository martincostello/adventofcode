// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/17</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 17, "Chronospatial Computer", RequiresData = true)]
public sealed class Day17 : Puzzle<string, long>
{
    /// <summary>
    /// Runs the specified 7-bit program.
    /// </summary>
    /// <param name="values">The program to run.</param>
    /// <param name="fix">Whether to fix the program so that it outputs itself.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The output from executing the instructions in the program
    /// and the the lowest positive initial value for register A
    /// that causes the program to output a copy of itself if
    /// <paramref name="fix"/> is <see langword="true"/>.
    /// </returns>
    public static (string Output, long A) Run(IList<string> values, bool fix, CancellationToken cancellationToken)
    {
        var program = new List<int>();
        long a = 0, b = 0, c = 0;

        for (int i = 0; i < values.Count; i++)
        {
            const string Program = "Program: ";
            const string Register = "Register ";

            ReadOnlySpan<char> line = values[i];

            if (line.StartsWith(Register))
            {
                char register = line[Register.Length];
                long value = Parse<long>(line[(Register.Length + 3)..]);

                switch (register)
                {
                    case 'A':
                        a = value;
                        break;

                    case 'B':
                        b = value;
                        break;

                    case 'C':
                        c = value;
                        break;

                    default:
                        throw new UnreachableException();
                }
            }
            else if (line.StartsWith(Program))
            {
                program = line[Program.Length..].AsNumbers<int>();
            }
        }

        IList<int> output = [];

        if (fix)
        {
            var queue = new Queue<(long A, int Digit)>(program.Count);
            queue.Enqueue((0, program.Count - 1));

            while (queue.Count > 0 && !cancellationToken.IsCancellationRequested)
            {
                (long floor, int digit) = queue.Dequeue();

                for (long i = 0; i < 8; i++)
                {
                    a = (floor << 3) + i;

                    output = Run(program, a, b, c);

                    if (output.SequenceEqual(program[digit..]))
                    {
                        if (digit is 0)
                        {
                            queue.Clear();
                            break;
                        }

                        queue.Enqueue((a, digit - 1));
                    }
                }
            }
        }
        else
        {
            output = Run(program, a, b, c);
        }

        cancellationToken.ThrowIfCancellationRequested();

        return (string.Join(',', output), a);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (values, logger, cancellationToken) =>
            {
                (string output, _) = Run(values, fix: false, cancellationToken);
                (_, long registerA) = Run(values, fix: true, cancellationToken);

                if (logger is { })
                {
                    logger.WriteLine("The output of the program is {0}.", output);
                    logger.WriteLine("The lowest positive initial value for register A that causes the program to output a copy of itself is {0}.", registerA);
                }

                return (output, registerA);
            },
            cancellationToken);
    }

    private static List<int> Run(List<int> program, long a, long b, long c)
    {
        var output = new List<int>(program.Count);

        for (int ip = 0; ip < program.Count; ip += 2)
        {
            int opcode = program[ip];
            int operand = program[ip + 1];

            switch (opcode)
            {
                case 0: // adv
                    a >>= (int)Combo(operand);
                    break;

                case 1: // bxl
                    b ^= operand;
                    break;

                case 2: // bst
                    b = Combo(operand) % 8;
                    break;

                case 3: // jnz
                    if (a is not 0)
                    {
                        ip = operand - 2;
                    }

                    break;

                case 4: // bxc
                    b ^= c;
                    break;

                case 5: // out
                    output.Add((int)(Combo(operand) % 8));
                    break;

                case 6: // bdv
                    b = a >> (int)Combo(operand);
                    break;

                case 7: // cdv
                    c = a >> (int)Combo(operand);
                    break;

                default:
                    throw new UnreachableException();
            }
        }

        return output;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        long Combo(long value)
        {
            return value switch
            {
                < 4 => value,
                4 => a,
                5 => b,
                6 => c,
                _ => throw new UnreachableException(),
            };
        }
    }
}
