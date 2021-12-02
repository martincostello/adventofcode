// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Drawing;

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/2</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 02, RequiresData = true)]
public sealed class Day02 : Puzzle
{
    /// <summary>
    /// Gets the product of the depth and horizontal
    /// position of the submarine at the end of its course.
    /// </summary>
    public int ProductOfFinalPosition { get; private set; }

    /// <summary>
    /// Navigates the submarine through the specified course.
    /// </summary>
    /// <param name="course">The course to navigate.</param>
    /// <returns>
    /// The product of the depth and horizontal position of the
    /// submarine at the end of the course specified by <paramref name="course"/>.
    /// </returns>
    public static int NavigateCourse(IEnumerable<string> course)
    {
        var position = Point.Empty;

        foreach (string instruction in course)
        {
            position += ParseInstruction(instruction);
        }

        return position.X * position.Y;

        static Size ParseInstruction(string text)
        {
            string[] split = text.Split(' ');
            int magnitude = ParseInt32(split[1]);

            return split[0] switch
            {
                "forward" => new Size(magnitude, 0),
                "down" => new Size(0, magnitude),
                "up" => new Size(0, -magnitude),
                _ => throw new NotImplementedException(),
            };
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> course = await ReadResourceAsLinesAsync();

        ProductOfFinalPosition = NavigateCourse(course);

        if (Verbose)
        {
            Logger.WriteLine(
                "The product of the submarine's final depth and forward position is {0:N0}.",
                ProductOfFinalPosition);
        }

        return PuzzleResult.Create(ProductOfFinalPosition);
    }
}
