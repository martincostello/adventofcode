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

    [Theory]
    [InlineData(1, 0, 1, 0, 0, 0, 2, 0, 1, 0)]
    [InlineData(0, -1, 0, -1, 0, 0, 0, -2, 0, -1)]
    [InlineData(1, 0, 1, 1, 0, 0, 2, 1, 1, 1)]
    [InlineData(4, 0, 0, 0, 0, 0, 4, 0, 3, 0)]
    [InlineData(0, 4, 4, 0, 3, 0, 4, 4, 4, 3)]
    [InlineData(-3, 0, 4, 4, 4, 3, 1, 4, 2, 4)]
    [InlineData(0, -1, 1, 4, 2, 4, 1, 3, 2, 4)]
    [InlineData(4, 0, 1, 3, 2, 4, 5, 3, 4, 3)]
    [InlineData(0, -1, 5, 3, 4, 3, 5, 2, 4, 3)]
    [InlineData(-5, 0, 5, 2, 4, 3, 0, 2, 1, 2)]
    [InlineData(2, 0, 0, 2, 1, 2, 2, 2, 1, 2)]
    public void Y2022_Day09_Move_For_Two_Knots_Returns_Correct_Values(
        int moveX,
        int moveY,
        int originHeadX,
        int originHeadY,
        int originTailX,
        int originTailY,
        int expectedHeadX,
        int expectedHeadY,
        int expectedTailX,
        int expectedTailY)
    {
        // Arrange
        var rope = new Day09.Rope(new(originHeadX, originHeadY), new(originTailX, originTailY));

        // Act
        rope.Move(new(moveX, moveY), (_) => { });

        // Assert
        rope.Head.ShouldBe(new(expectedHeadX, expectedHeadY), "The head is incorrectly placed.");
        rope.Tail.ShouldBe(new(expectedTailX, expectedTailY), "The tail is incorrectly placed.");
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
