// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class containing tests for the <see cref="Day09"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day09Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day09Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day09Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    public static IEnumerable<object[]> TwoKnots()
    {
        var testCases = new List<(Size Direction, Point[] Knots, Point ExpectedHead, Point ExpectedTail)>()
        {
            (new(1, 0), new Point[] { new(1, 0), new(0, 0) }, new(2, 0), new(1, 0)),
            (new(0, -1), new Point[] { new(0, -1), new(0, 0) }, new(0, -2), new(0, -1)),
            (new(1, 0), new Point[] { new(1, 1), new(0, 0) }, new(2, 1), new(1, 1)),
            (new(4, 0), new Point[] { new(0, 0), new(0, 0) }, new(4, 0), new(3, 0)),
            (new(0, 4), new Point[] { new(4, 0), new(3, 0) }, new(4, 4), new(4, 3)),
            (new(-3, 0), new Point[] { new(4, 4), new(4, 3) }, new(1, 4), new(2, 4)),
            (new(0, -1), new Point[] { new(1, 4), new(2, 4) }, new(1, 3), new(2, 4)),
            (new(4, 0), new Point[] { new(1, 3), new(2, 4) }, new(5, 3), new(4, 3)),
            (new(0, -1), new Point[] { new(5, 3), new(4, 3) }, new(5, 2), new(4, 3)),
            (new(-5, 0), new Point[] { new(5, 2), new(4, 3) }, new(0, 2), new(1, 2)),
            (new(2, 0), new Point[] { new(0, 2), new(1, 2) }, new(2, 2), new(1, 2)),
        };

        foreach (var (direction, knots, expectedHead, expectedTail) in testCases)
        {
            yield return new object[]
            {
                direction,
                knots,
                expectedHead,
                expectedTail,
            };
        }
    }

    [Theory]
    [MemberData(nameof(TwoKnots))]
    public void Y2022_Day09_Move_Returns_Correct_Values(
        Size direction,
        Point[] knots,
        Point expectedHead,
        Point expectedTail)
    {
        // Arrange
        var rope = new Day09.Rope(knots);

        // Act
        rope.Move(direction, (_) => { });

        // Assert
        rope.Head.ShouldBe(expectedHead, "The head is incorrectly placed.");
        rope.Tail.ShouldBe(expectedTail, "The tail is incorrectly placed.");
        rope.AllKnots.ShouldBe(new[] { expectedHead, expectedTail }, "The knots are incorrectly placed.");
    }

    [Theory]
    [InlineData(2)]
    public void Y2022_Day09_Move_Returns_Correct_Value(int knots)
    {
        // Arrange
        string[] instructions = new[]
        {
            "R 4",
            "U 4",
            "L 3",
            "D 1",
            "R 4",
            "D 1",
            "L 5",
            "R 2",
        };

        // Act
        int actual = Day09.Move(instructions, knots);

        // Assert
        actual.ShouldBe(13);
    }

    [Fact]
    public async Task Y2022_Day09_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day09>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.PositionsVisited2.ShouldBe(5683);
    }
}
