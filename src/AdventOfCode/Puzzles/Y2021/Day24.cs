// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/24</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 24, "Arithmetic Logic Unit", RequiresData = true)]
public sealed class Day24 : Puzzle
{
    /// <summary>
    /// Gets the largest valid 14 digit model number accepted by MONAD.
    /// </summary>
    public long MaximumValidModelNumber { get; private set; }

    /// <summary>
    /// Executes the specified MONAD instructions using the Arithmetic Logic Unit (ALU)
    /// to find the largest valid 14 digit model number accepted by MONAD.
    /// </summary>
    /// <param name="instructions">The instructions to execute using the ALU.</param>
    /// <returns>
    /// The largest valid 14 digit model number accepted by MONAD.
    /// </returns>
    public static long Execute(IList<string> instructions)
    {
        var constants = new List<(int A, int B)>();

        const int Sections = 18;

        foreach (int i in Enumerable.Range(0, 14))
        {
            int section = i * Sections;

            int a = Parse<int>(instructions[section + 5][6..]);
            int b = Parse<int>(instructions[section + 15][6..]);

            constants.Add((a, b));
        }

        var stack = new Stack<(int, int)>();
        var keys = new Dictionary<int, (int X, int Y)>();

        foreach (((int a, int b), int i) in constants.Select((pair, index) => (pair, index)))
        {
            if (a > 0)
            {
                stack.Push((i, b));
            }
            else
            {
                (int j, int address) = stack.Pop();
                keys[i] = (j, address + a);
            }
        }

        var output = new Dictionary<int, int>(14);

        foreach ((int key, (int x, int y)) in keys)
        {
            output[key] = Math.Min(9, 9 + y);
            output[x] = Math.Min(9, 9 - y);
        }

        int[] digits = output
            .OrderBy((p) => p.Key)
            .Select((p) => p.Value)
            .ToArray();

        return Maths.FromDigits<long>(digits);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> instructions = await ReadResourceAsLinesAsync();

        MaximumValidModelNumber = Execute(instructions);

        if (Verbose)
        {
            Logger.WriteLine("The largest model number accepted by MONAD is {0:N0}.", MaximumValidModelNumber);
        }

        return PuzzleResult.Create(MaximumValidModelNumber);
    }
}
