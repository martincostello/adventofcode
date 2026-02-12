// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/24</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 24, "Arithmetic Logic Unit", RequiresData = true)]
public sealed class Day24 : Puzzle<long, long>
{
    /// <summary>
    /// Executes the specified MONAD instructions using the Arithmetic Logic Unit (ALU)
    /// to find the largest or smallest valid 14 digit model number accepted by MONAD.
    /// </summary>
    /// <param name="instructions">The instructions to execute using the ALU.</param>
    /// <param name="maximumValue">Whether to return the maximum ALU value instead of the minimum.</param>
    /// <returns>
    /// The largest or smallest valid 14 digit model number accepted by MONAD.
    /// </returns>
    public static long Execute(IList<string> instructions, bool maximumValue)
    {
        const int Sections = 18;
        const int Instructions = 14;

        var constants = new (int A, int B)[Instructions];

        for (int i = 0; i < constants.Length; i++)
        {
            int section = i * Sections;

            int a = Parse<int>(instructions[section + 5].AsSpan()[6..]);
            int b = Parse<int>(instructions[section + 15].AsSpan()[6..]);

            constants[i] = (a, b);
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

        var output = new Dictionary<int, int>(Instructions);

        foreach ((int key, (int x, int y)) in keys)
        {
            output[key] = maximumValue ? Math.Min(9, 9 + y) : Math.Max(1, 1 + y);
            output[x] = maximumValue ? Math.Min(9, 9 - y) : Math.Max(1, 1 - y);
        }

        int[] digits = [.. output.OrderBy((p) => p.Key).Select((p) => p.Value)];

        return Maths.FromDigits<long>(digits);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (instructions, logger, _) =>
            {
                long maximumValidModelNumber = Execute(instructions, maximumValue: true);
                long minimumValidModelNumber = Execute(instructions, maximumValue: false);

                if (logger is { })
                {
                    logger.WriteLine("The largest model number accepted by MONAD is {0:N0}.", maximumValidModelNumber);
                    logger.WriteLine("The smallest model number accepted by MONAD is {0:N0}.", minimumValidModelNumber);
                }

                return Task.FromResult<(long Solution1, long Solution2)>((maximumValidModelNumber, minimumValidModelNumber));
            },
            cancellationToken);
    }
}
