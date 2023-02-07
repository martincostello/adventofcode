// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/2</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 02, "Dive!", RequiresData = true)]
public sealed class Day02 : Puzzle
{
    /// <summary>
    /// Gets the product of the depth and horizontal
    /// position of the submarine at the end of its course.
    /// </summary>
    public int ProductOfFinalPosition { get; private set; }

    /// <summary>
    /// Gets the product of the depth and horizontal position
    /// of the submarine at the end of its course when the aim
    /// of the submarine is accounted for.
    /// </summary>
    public int ProductOfFinalPositionWithAim { get; private set; }

    /// <summary>
    /// Navigates the submarine through the specified course.
    /// </summary>
    /// <param name="course">The course to navigate.</param>
    /// <param name="useAim">Whether to account for the aim of the submarine.</param>
    /// <returns>
    /// The product of the depth and horizontal position of the
    /// submarine at the end of the course specified by <paramref name="course"/>.
    /// </returns>
    public static int NavigateCourse(IEnumerable<string> course, bool useAim)
    {
        int aim = 0;
        var position = Point.Empty;

        foreach (string instruction in course)
        {
            Size adjustment = ParseInstruction(instruction);

            if (useAim)
            {
                aim += adjustment.Height;

                if (adjustment.Width > 0)
                {
                    position += adjustment;
                    position += new Size(0, adjustment.Width * aim);
                }
            }
            else
            {
                position += adjustment;
            }
        }

        return position.X * position.Y;

        static Size ParseInstruction(string text)
        {
            (string direction, string rawMagnitude) = text.Bifurcate(' ');
            int magnitude = Parse<int>(rawMagnitude);

            return direction switch
            {
                "forward" => new(magnitude, 0),
                "down" => new(0, magnitude),
                "up" => new(0, -magnitude),
                _ => throw new PuzzleException($"The direction '{direction}' is not supported."),
            };
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> course = await ReadResourceAsLinesAsync(cancellationToken);

        ProductOfFinalPosition = NavigateCourse(course, useAim: false);
        ProductOfFinalPositionWithAim = NavigateCourse(course, useAim: true);

        if (Verbose)
        {
            Logger.WriteLine(
                "The product of the submarine's final depth and forward position is {0:N0}.",
                ProductOfFinalPosition);

            Logger.WriteLine(
                "The product of the submarine's final depth and forward position is {0:N0} when accounting for aim.",
                ProductOfFinalPositionWithAim);
        }

        return PuzzleResult.Create(ProductOfFinalPosition, ProductOfFinalPositionWithAim);
    }
}
