// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2017/day/8</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2017, 08, RequiresData = true)]
public sealed class Day08 : Puzzle
{
    /// <summary>
    /// Gets the highest value in any register once the program has run.
    /// </summary>
    public int HighestRegisterValueAtEnd { get; private set; }

    /// <summary>
    /// Gets the highest value in any register while the program was running.
    /// </summary>
    public int HighestRegisterValueDuring { get; private set; }

    /// <summary>
    /// Finds the highest value in any register once the specified program has run.
    /// </summary>
    /// <param name="instructions">The program instructions.</param>
    /// <returns>
    /// The highest value in any register once the program specified by <paramref name="instructions"/> has run.
    /// </returns>
    public static int FindHighestRegisterValueAtEnd(ICollection<string> instructions)
    {
        Cpu cpu = RunProgram(instructions);
        return cpu.Values.Max();
    }

    /// <summary>
    /// Finds the highest value in any register at any point when the specified program was run.
    /// </summary>
    /// <param name="instructions">The program instructions.</param>
    /// <returns>
    /// The highest value in any register at any point while the program specified by <paramref name="instructions"/> was run.
    /// </returns>
    public static int FindHighestRegisterValueDuring(ICollection<string> instructions)
    {
        Cpu cpu = RunProgram(instructions);
        return cpu.HighestValue;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> instructions = await ReadResourceAsLinesAsync();

        HighestRegisterValueAtEnd = FindHighestRegisterValueAtEnd(instructions);
        HighestRegisterValueDuring = FindHighestRegisterValueDuring(instructions);

        if (Verbose)
        {
            Logger.WriteLine($"The largest value in any register after executing the input is {HighestRegisterValueAtEnd:N0}.");
            Logger.WriteLine($"The largest value in any register at any point while executing the input was {HighestRegisterValueDuring:N0}.");
        }

        return PuzzleResult.Create(HighestRegisterValueAtEnd, HighestRegisterValueDuring);
    }

    /// <summary>
    /// Runs the specified program.
    /// </summary>
    /// <param name="instructions">The program instructions.</param>
    /// <returns>
    /// A <see cref="Cpu"/> containing the value of each CPU register at the end of the program.
    /// </returns>
    private static Cpu RunProgram(ICollection<string> instructions)
    {
        var program = instructions
            .Select((p) => new Instruction(p))
            .ToList();

        var cpu = new Cpu();

        foreach (Instruction instruction in program)
        {
            if (!cpu.TryGetValue(instruction.ConditionRegister, out int conditionRegisterValue))
            {
                conditionRegisterValue = cpu[instruction.ConditionRegister] = 0;
            }

            if (!Evaluate(conditionRegisterValue, instruction.ConditionComparand, instruction.ConditionOperator))
            {
                continue;
            }

            if (!cpu.ContainsKey(instruction.TargetRegister))
            {
                cpu[instruction.TargetRegister] = 0;
            }

            if (instruction.TargetOperator)
            {
                cpu[instruction.TargetRegister] += instruction.OperatorValue;
            }
            else
            {
                cpu[instruction.TargetRegister] -= instruction.OperatorValue;
            }

            cpu.HighestValue = Math.Max(cpu.HighestValue, cpu.Values.DefaultIfEmpty().Max());
        }

        return cpu;
    }

    /// <summary>
    /// Evaluates the specified logic condition.
    /// </summary>
    /// <param name="left">The left value.</param>
    /// <param name="right">The right value.</param>
    /// <param name="operation">The operation.</param>
    /// <returns>
    /// The result of evaluating the logic condition.
    /// </returns>
    private static bool Evaluate(int left, int right, string operation)
    {
        return operation switch
        {
            ">" => left > right,
            "<" => left < right,
            "!=" => left != right,
            "==" => left == right,
            ">=" => left >= right,
            "<=" => left <= right,
            _ => throw new PuzzleException($"Unknown operation '{operation}'."),
        };
    }

    /// <summary>
    /// A class representing the CPU. This class cannot be inherited.
    /// </summary>
    private sealed class Cpu : Dictionary<string, int>
    {
        /// <summary>
        /// Gets or sets the highest value held in any register at any time.
        /// </summary>
        internal int HighestValue { get; set; }
    }

    /// <summary>
    /// A class representing a CPU instruction. This class cannot be inherited.
    /// </summary>
    private sealed class Instruction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Instruction"/> class.
        /// </summary>
        /// <param name="instruction">The raw CPU instruction.</param>
        internal Instruction(string instruction)
        {
            string[] split = instruction.Split(' ');

            TargetRegister = split[0];
            TargetOperator = split[1] == "inc";
            OperatorValue = ParseInt32(split[2]);
            ConditionRegister = split[4];
            ConditionOperator = split[5];
            ConditionComparand = ParseInt32(split[6]);
        }

        /// <summary>
        /// Gets the name of the target register.
        /// </summary>
        public string TargetRegister { get; }

        /// <summary>
        /// Gets a value indicating whether the target operator is to be incremented.
        /// </summary>
        public bool TargetOperator { get; }

        /// <summary>
        /// Gets the value associated with the target operator.
        /// </summary>
        public int OperatorValue { get; }

        /// <summary>
        /// Gets the name of the register that is associated with the evaluation condition.
        /// </summary>
        public string ConditionRegister { get; }

        /// <summary>
        /// Gets the logic operator that is associated with the evaluation condition.
        /// </summary>
        public string ConditionOperator { get; }

        /// <summary>
        /// Gets the comparand value that is associated with the evaluation condition.
        /// </summary>
        public int ConditionComparand { get; }
    }
}
