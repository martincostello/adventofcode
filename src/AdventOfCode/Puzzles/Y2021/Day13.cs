// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/13</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 13, RequiresData = true)]
public sealed class Day13 : Puzzle
{
    /// <summary>
    /// Gets the number of dots that are visible after completing the first fold.
    /// </summary>
    public int DotCountAfterFold1 { get; private set; }

    /// <summary>
    /// Gets the activation code for the infrared thermal imaging camera system.
    /// </summary>
    public string? ActivationCode { get; private set; }

    /// <summary>
    /// Folds the transparent paper the specified number of times.
    /// </summary>
    /// <param name="instructions">The instructions to follow.</param>
    /// <param name="folds">The number of times to fold the paper, otherwise all folds are performed.</param>
    /// <param name="logger">The optional logger to use.</param>
    /// <returns>
    /// The number of dots that are visible after completing the specified number of folds
    /// and the activation code on the paper if <paramref name="folds"/> is <see langword="null"/>,
    /// and a visualization of the paper if <paramref name="folds"/> is <see langword="null"/>.
    /// </returns>
    public static (int DotCount, string? ActivationCode, string? Visualization) Fold(
        IList<string> instructions,
        int? folds,
        ILogger? logger = null)
    {
        var paper = new Dictionary<Point, bool>(instructions.Count - 1);
        var foldLines = new List<Point>();

        foreach (string instruction in instructions)
        {
            if (string.IsNullOrEmpty(instruction))
            {
                continue;
            }

            if (instruction[0] == 'f')
            {
                string fold = instruction.Split(' ')[2];
                string[] parts = fold.Split('=');
                int size = Parse<int>(parts[1]);

                if (parts[0][0] == 'x')
                {
                    foldLines.Add(new(size, 0));
                }
                else
                {
                    foldLines.Add(new(0, size));
                }

                continue;
            }

            int[] points = instruction.AsNumbers<int>().ToArray();

            paper[new(points[0], points[1])] = true;
        }

        int timesToFold = folds ?? int.MaxValue;

        for (int i = 0; i < timesToFold && i < foldLines.Count; i++)
        {
            Point fold = foldLines[i];

            foreach (Point dot in paper.Keys.ToArray())
            {
                if (!paper.GetValueOrDefault(dot))
                {
                    continue;
                }

                Point? transformed = null;

                if (fold.X > 0 && dot.X > fold.X)
                {
                    transformed = dot - new Size(Math.Abs(dot.X - fold.X) * 2, 0);
                }
                else if (fold.Y > 0 && dot.Y > fold.Y)
                {
                    transformed = dot - new Size(0, Math.Abs(dot.Y - fold.Y) * 2);
                }

                if (transformed.HasValue)
                {
                    paper[transformed.Value] = true;
                    paper.Remove(dot);
                }
            }
        }

        string? activationCode = null;
        string? visualization = null;

        if (folds is null)
        {
            var builder = new StringBuilder(6 * 5 * 8);

            int maxX = paper.Keys.MaxBy((p) => p.X).X + 2;
            int maxY = paper.Keys.MaxBy((p) => p.Y).Y + 1;

            char[,] array = new char[maxX, maxY];

            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    bool value = paper.GetValueOrDefault(new(x, y));
                    builder.Append(array[x, y] = value ? '*' : ' ');
                }

                builder.AppendLine();
            }

            visualization = logger?.WriteGrid(array);
            activationCode = CharacterRecognition.Read(array);
        }

        int dotCount = paper.Values.Count((p) => p);

        return (dotCount, activationCode, visualization);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> instructions = await ReadResourceAsLinesAsync();

        (DotCountAfterFold1, _, _) = Fold(instructions, folds: 1);
        (_, ActivationCode, string? visualization) = Fold(instructions, folds: null, Logger);

        if (Verbose)
        {
            Logger.WriteLine(
                "{0:N0} dots are visible after completing the first fold.",
                DotCountAfterFold1);

            Logger.WriteLine(
                "The code to activate the infrared thermal imaging camera system is {0}.",
                ActivationCode!);
        }

        var result = new PuzzleResult();

        result.Solutions.Add(DotCountAfterFold1);
        result.Solutions.Add(ActivationCode!);
        result.Visualizations.Add(visualization!);

        return result;
    }
}
