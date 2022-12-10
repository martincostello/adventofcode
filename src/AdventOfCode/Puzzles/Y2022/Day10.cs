// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/10</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 10, "Cathode-Ray Tube", RequiresData = true)]
public sealed class Day10 : Puzzle
{
    /// <summary>
    /// Gets the sum of the signal strengths for the 20th,
    /// 60th, 100th, 140th, 180th and 220th cycles.
    /// </summary>
    public int SumOfSignalStrengths { get; private set; }

    /// <summary>
    /// Gets the message output by the CRT display.
    /// </summary>
    public string Message { get; private set; } = string.Empty;

    /// <summary>
    /// Gets displayed message from executing the specified instructions.
    /// </summary>
    /// <param name="program">The program instructions to execute.</param>
    /// <returns>
    /// The message output by executing the program specified by <paramref name="program"/>,
    /// a visualization of the pixels of the display, and the sum of the signal strengths
    /// for the cycles of the 20th, 60th, 100th, 140th, 180th and 220th cycles.
    /// </returns>
    public static (string Message, string Visualization, int SumOfSignalStrengths) GetMessage(IList<string> program)
    {
        const int DisplayHeight = 6;
        const int DisplayWidth = 40;

        char[,] crt = new char[DisplayWidth, DisplayHeight];
        var registers = new Dictionary<int, int>(program.Count);

        int cycle = 0;
        int register = 1;
        int sprite = 1;

        foreach (string instruction in program)
        {
            switch (instruction[..4])
            {
                case "noop":
                    Tick();
                    break;

                case "addx":
                    Tick();
                    Tick(instruction[5..]);
                    break;
            }
        }

        void Tick(string? operand = null)
        {
            Draw();

            if (operand is { })
            {
                register += Parse<int>(operand);
            }

            registers[++cycle] = register;
            sprite = cycle % DisplayWidth;
        }

        void Draw()
        {
            (int y, int x) = Math.DivRem(cycle, DisplayWidth);

            char ch;

            if (register == sprite - 1 ||
                register == sprite ||
                register == sprite + 1)
            {
                ch = '#';
            }
            else
            {
                ch = '.';
            }

            crt[x, y] = ch;
        }

        string message = CharacterRecognition.Read(crt, '#');
        string visualization = Visualize(crt);

        int sum = 0;

        foreach (int r in new[] { 20, 60, 100, 140, 180, 220 })
        {
            sum += registers[r - 1] * r;
        }

        return (message, visualization, sum);

        static string Visualize(char[,] crt)
        {
            var output = new StringBuilder((DisplayHeight * DisplayWidth) + DisplayHeight);

            for (int y = 0; y < DisplayHeight; y++)
            {
                for (int x = 0; x < DisplayWidth; x++)
                {
                    output.Append(crt[x, y]);
                }

                output.Append('\n');
            }

            return output.Remove(output.Length - 1, 1).ToString();
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var program = await ReadResourceAsLinesAsync();

        (Message, string visualization, SumOfSignalStrengths) = GetMessage(program);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the six signal strengths is {0}.", SumOfSignalStrengths);
            Logger.WriteLine("The message output to the CRT is '{0}'.", Message);
        }

        var result = new PuzzleResult();

        result.Solutions.Add(SumOfSignalStrengths);
        result.Solutions.Add(Message);
        result.Visualizations.Add(visualization);

        return result;
    }
}
