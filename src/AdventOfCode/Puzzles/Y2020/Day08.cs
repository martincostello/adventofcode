// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2020/day/8</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2020, 08, "Handheld Halting", RequiresData = true)]
public sealed class Day08 : Puzzle
{
    /// <summary>
    /// An enumeration of CPU operations.
    /// </summary>
    private enum Operation
    {
        /// <summary>
        /// No-op.
        /// </summary>
        NoOp = 0,

        /// <summary>
        /// Updates the value of the accumulator.
        /// </summary>
        Accumulate,

        /// <summary>
        /// Jumps to another instruction.
        /// </summary>
        Jump,
    }

    /// <summary>
    /// Gets the value of the accumulator after one iteration of the program has been completed.
    /// </summary>
    public int Accumulator { get; private set; }

    /// <summary>
    /// Gets the value of the accumulator when the fixed program has terminated.
    /// </summary>
    public int AccumulatorWithFix { get; private set; }

    /// <summary>
    /// Runs the specified program and returns the value of the accumulator.
    /// </summary>
    /// <param name="program">The program to run.</param>
    /// <param name="fix">Whether to fix the program.</param>
    /// <returns>
    /// The value of the accumulator after the program has run.
    /// </returns>
    public static int RunProgram(IList<string> program, bool fix)
    {
        var instructions = ParseProgram(program);

        int result = 0;

        if (fix)
        {
            for (int i = 0; i < instructions.Count; i++)
            {
                Instruction instruction = instructions[i];

                var fixCandidate = new List<Instruction>(instructions)
                {
                    [i] = instruction.Operation switch
                    {
                        Operation.Jump => instruction with { Operation = Operation.NoOp },
                        Operation.NoOp => instruction with { Operation = Operation.Jump },
                        _ => instruction,
                    },
                };

                (int accumulator, bool completed) = RunProgram(fixCandidate);

                if (completed)
                {
                    result = accumulator;
                    break;
                }
            }
        }
        else
        {
            (result, _) = RunProgram(instructions);
        }

        return result;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> program = await ReadResourceAsLinesAsync();

        Accumulator = RunProgram(program, fix: false);
        AccumulatorWithFix = RunProgram(program, fix: true);

        if (Verbose)
        {
            Logger.WriteLine("The value of the accumulator after one iteration is {0}.", Accumulator);
            Logger.WriteLine("The value of the accumulator when the fixed program completes is {0}.", AccumulatorWithFix);
        }

        return PuzzleResult.Create(Accumulator, AccumulatorWithFix);
    }

    /// <summary>
    /// Parses the specified program.
    /// </summary>
    /// <param name="program">The instructions of the program to parse.</param>
    /// <returns>
    /// The parsed CPU instructions.
    /// </returns>
    private static List<Instruction> ParseProgram(ICollection<string> program)
    {
        var result = new List<Instruction>(program.Count);

        foreach (ReadOnlySpan<char> operation in program)
        {
            var op = new string(operation[..3]) switch
            {
                "acc" => Operation.Accumulate,
                "jmp" => Operation.Jump,
                _ => Operation.NoOp,
            };

            int argument = 0;

            if (op != Operation.NoOp)
            {
                argument = Parse<int>(operation[4..], NumberStyles.Number);
            }

            result.Add(new Instruction() with { Operation = op, Argument = argument });
        }

        return result;
    }

    /// <summary>
    /// Runs the specified program and returns the value of the accumulator.
    /// </summary>
    /// <param name="instructions">The program to run.</param>
    /// <returns>
    /// The value of the accumulator after the program has run and
    /// whether it completed without an infinite loop.
    /// </returns>
    private static (int Accumulator, bool Completed) RunProgram(IList<Instruction> instructions)
    {
        var visited = new HashSet<int>();

        int accumulator = 0;

        for (int i = 0; i < instructions.Count; i++)
        {
            if (visited.Contains(i))
            {
                return (accumulator, false);
            }

            visited.Add(i);
            Instruction instruction = instructions[i];

            switch (instruction.Operation)
            {
                case Operation.Accumulate:
                    accumulator += instruction.Argument;
                    break;

                case Operation.Jump:
                    i += instruction.Argument - 1;
                    break;

                default:
                    break;
            }
        }

        return (accumulator, true);
    }

    /// <summary>
    /// Represents a CPU instruction.
    /// </summary>
    private record struct Instruction
    {
        /// <summary>
        /// Gets the operation to perform.
        /// </summary>
        public Operation Operation { get; init; }

        /// <summary>
        /// Gets the argument of the operation.
        /// </summary>
        public int Argument { get; init; }
    }
}
