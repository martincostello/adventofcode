// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/17</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 17, "Chronospatial Computer", RequiresData = true)]
public sealed class Day17 : Puzzle
{
    /// <summary>
    /// Gets the output of the program.
    /// </summary>
    public string Output { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the lowest positive initial value for register A
    /// that causes the program to output a copy of itself.
    /// </summary>
    public int RegisterA { get; private set; }

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
    public static (string Output, int A) Run(IList<string> values, bool fix, CancellationToken cancellationToken)
    {
        var program = new List<int>();
        (int a, int b, int c) = (0, 0, 0);

        for (int i = 0; i < values.Count; i++)
        {
            const string Program = "Program: ";
            const string Register = "Register ";

            ReadOnlySpan<char> line = values[i];

            if (line.StartsWith(Register))
            {
                char register = line[Register.Length];
                int value = Parse<int>(line[(Register.Length + 3)..]);

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

        var output = Run(program, (a, b, c));

        if (fix)
        {
            for (a = 0; a < int.MaxValue && !cancellationToken.IsCancellationRequested; a++)
            {
                output = Run(program, (a, b, c));

                if (program.SequenceEqual(output))
                {
                    break;
                }
            }
        }

        cancellationToken.ThrowIfCancellationRequested();

        return (string.Join(',', output), a);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        (Output, _) = Run(values, fix: false, cancellationToken);
        (_, RegisterA) = Run(values, fix: true, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("The output of the program is {0}.", Output);
            Logger.WriteLine("The lowest positive initial value for register A that causes the program to output a copy of itself is {0}.", RegisterA);
        }

        return PuzzleResult.Create(Output, RegisterA);
    }

    private static List<int> Run(List<int> program, (int A, int B, int C) registers)
    {
        var output = new List<int>();

        for (int ip = 0; ip < program.Count; ip += 2)
        {
            int opcode = program[ip];
            int operand = program[ip + 1];

            switch (opcode)
            {
                case 0:
                    Adv(operand);
                    break;

                case 1:
                    Bxl(operand);
                    break;

                case 2:
                    Bst(operand);
                    break;

                case 3:
                    if (Jnz())
                    {
                        ip = operand - 2;
                    }

                    break;

                case 4:
                    Bxc();
                    break;

                case 5:
                    Out(operand);
                    break;

                case 6:
                    Bdv(operand);
                    break;

                case 7:
                    Cdv(operand);
                    break;

                default:
                    throw new UnreachableException();
            }
        }

        return output;

        void Adv(int operand) => registers.A = Divide(operand);

        void Bxl(int operand) => registers.B ^= operand;

        void Bst(int operand) => registers.B = Combo(operand) % 8;

        bool Jnz() => registers.A is not 0;

        void Bxc() => registers.B ^= registers.C;

        void Out(int operand) => output.Add(Combo(operand) % 8);

        void Bdv(int operand) => registers.B = Divide(operand);

        void Cdv(int operand) => registers.C = Divide(operand);

        int Combo(int value)
        {
            return value switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                3 => 3,
                4 => registers.A,
                5 => registers.B,
                6 => registers.C,
                _ => throw new UnreachableException(),
            };
        }

        int Divide(int operand)
        {
            int numerator = registers.A;
            int denominator = (int)Math.Pow(2, Combo(operand));

            return numerator / denominator;
        }
    }
}
