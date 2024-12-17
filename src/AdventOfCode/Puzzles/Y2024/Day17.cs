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
    /// Runs the specified 7-bit program.
    /// </summary>
    /// <param name="values">The program to run.</param>
    /// <returns>
    /// The output from executing the instructions in the program.
    /// </returns>
    public static string Run(IList<string> values)
    {
        var program = new List<int>();
        var registers = new Dictionary<char, int>();

        for (int i = 0; i < values.Count; i++)
        {
            const string Program = "Program: ";
            const string Register = "Register ";

            ReadOnlySpan<char> line = values[i];

            if (line.StartsWith(Register))
            {
                char register = line[Register.Length];
                int value = Parse<int>(line[(Register.Length + 3)..]);
                registers[register] = value;
            }
            else if (line.StartsWith(Program))
            {
                program = line[Program.Length..].AsNumbers<int>();
            }
        }

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

        return string.Join(',', output);

        void Adv(int operand) => Divide(operand, 'A');

        void Bxl(int operand) => registers['B'] ^= operand;

        void Bst(int operand) => registers['B'] = Combo(operand) % 8;

        bool Jnz() => registers['A'] is not 0;

        void Bxc() => registers['B'] ^= registers['C'];

        void Out(int operand) => output.Add(Combo(operand) % 8);

        void Bdv(int operand) => Divide(operand, 'B');

        void Cdv(int operand) => Divide(operand, 'C');

        int Combo(int value)
        {
            return value switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                3 => 3,
                4 => registers['A'],
                5 => registers['B'],
                6 => registers['C'],
                _ => throw new UnreachableException(),
            };
        }

        void Divide(int operand, char register)
        {
            int numerator = registers['A'];
            int denominator = (int)Math.Pow(2, Combo(operand));

            int result = numerator / denominator;

            registers[register] = result;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        Output = Run(values);

        if (Verbose)
        {
            Logger.WriteLine("The output of the program is {0}.", Output);
        }

        return PuzzleResult.Create(Output);
    }
}
