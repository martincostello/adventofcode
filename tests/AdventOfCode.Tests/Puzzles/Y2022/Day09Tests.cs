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
        var testCases = new List<(Size Direction, Point[] InitialKnots, Point[] FinalKnots)>()
        {
            (new(1, 0), new Point[] { new(1, 0), new(0, 0) }, new Point[] { new(2, 0), new(1, 0) }),
            (new(0, -1), new Point[] { new(0, -1), new(0, 0) }, new Point[] { new(0, -2), new(0, -1) }),
            (new(1, 0), new Point[] { new(1, 1), new(0, 0) }, new Point[] { new(2, 1), new(1, 1) }),
            (new(4, 0), new Point[] { new(0, 0), new(0, 0) }, new Point[] { new(4, 0), new(3, 0) }),
            (new(0, 4), new Point[] { new(4, 0), new(3, 0) }, new Point[] { new(4, 4), new(4, 3) }),
            (new(-3, 0), new Point[] { new(4, 4), new(4, 3) }, new Point[] { new(1, 4), new(2, 4) }),
            (new(0, -1), new Point[] { new(1, 4), new(2, 4) }, new Point[] { new(1, 3), new(2, 4) }),
            (new(4, 0), new Point[] { new(1, 3), new(2, 4) }, new Point[] { new(5, 3), new(4, 3) }),
            (new(0, -1), new Point[] { new(5, 3), new(4, 3) }, new Point[] { new(5, 2), new(4, 3) }),
            (new(-5, 0), new Point[] { new(5, 2), new(4, 3) }, new Point[] { new(0, 2), new(1, 2) }),
            (new(2, 0), new Point[] { new(0, 2), new(1, 2) }, new Point[] { new(2, 2), new(1, 2) }),
        };

        foreach (var (direction, initialKnots, finalKnots) in testCases)
        {
            yield return new object[] { direction, initialKnots, finalKnots };
        }
    }

    public static IEnumerable<object[]> TenKnots()
    {
        var testCases = new List<(Size Direction, Point[] InitialKnots, Point[] FinalKnots)>();

        // Right 4
        var move = new Size(4, 0);
        var initial = EmptyPoints(10);
        var final = Points(new(4, 0), new(3, 0), new(2, 0), new(1, 0)).Concat(EmptyPoints(6)).ToArray();

        testCases.Add((move, initial, final));

        // Up 4
        move = new(0, 4);
        (initial, final) = (final, Points(new(4, 4), new(4, 3), new(4, 2), new(3, 2), new(2, 2), new(1, 1)).Concat(EmptyPoints(4)).ToArray());

        testCases.Add((move, initial, final));

        // Left 3
        move = new(-3, 0);
        (initial, final) = (final, Points(new(1, 4), new(2, 4), new(3, 3), new(3, 2), new(2, 2), new(1, 1)).Concat(EmptyPoints(4)).ToArray());

        testCases.Add((move, initial, final));

        // Down 1
        move = new(0, -1);
        (initial, final) = (final, Points(new(1, 3), new(2, 4), new(3, 3), new(3, 2), new(2, 2), new(1, 1)).Concat(EmptyPoints(4)).ToArray());

        testCases.Add((move, initial, final));

        // Right 4
        move = new(4, 0);
        (initial, final) = (final, Points(new(5, 3), new(4, 3), new(3, 3), new(3, 2), new(2, 2), new(1, 1)).Concat(EmptyPoints(4)).ToArray());

        testCases.Add((move, initial, final));

        // Down 1
        move = new(0, -1);
        (initial, final) = (final, Points(new(5, 2), new(4, 3), new(3, 3), new(3, 2), new(2, 2), new(1, 1)).Concat(EmptyPoints(4)).ToArray());

        testCases.Add((move, initial, final));

        // Left 5
        move = new(-5, 0);
        (initial, final) = (final, Points(new(0, 2), new(1, 2), new(2, 2), new(3, 2), new(2, 2), new(1, 1)).Concat(EmptyPoints(4)).ToArray());

        testCases.Add((move, initial, final));

        // Right 2
        move = new(2, 0);
        (initial, final) = (final, Points(new(2, 2), new(1, 2), new(2, 2), new(3, 2), new(2, 2), new(1, 1)).Concat(EmptyPoints(4)).ToArray());

        testCases.Add((move, initial, final));

        foreach (var (direction, initialKnots, finalKnots) in testCases)
        {
            yield return new object[] { direction, initialKnots, finalKnots };
        }

        static Point[] EmptyPoints(int count) => Enumerable.Repeat(Point.Empty, count).ToArray();

        static IEnumerable<Point> Points(params Point[] points) => points;
    }

    [Theory]
    [MemberData(nameof(TwoKnots))]
    [MemberData(nameof(TenKnots))]
    public void Y2022_Day09_Move_Returns_Correct_Values(
        Size direction,
        Point[] initialKnots,
        Point[] expectedKnots)
    {
        // Arrange
        var rope = new Day09.Rope(initialKnots);

        // Act
        rope.Move(direction, (_) => { });

        // Assert
        rope.Head.ShouldBe(rope.Knots[0], "The head is incorrectly placed.");
        rope.Tail.ShouldBe(rope.Knots[^1], "The tail is incorrectly placed.");
        rope.Knots.ShouldBe(expectedKnots, "The knots are incorrectly placed.");
    }

    [Theory]
    [InlineData(new[] { "R 4", "U 4", "L 3", "D 1", "R 4", "D 1", "L 5", "R 2" }, 2, 13)]
    [InlineData(new[] { "R 5", "U 8", "L 8", "D 3", "R 17", "D 10", "L 25", "U 20" }, 10, 36)]
    public void Y2022_Day09_Move_Returns_Correct_Value(string[] moves, int knots, int expected)
    {
        // Act
        int actual = Day09.Move(moves, knots);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2022_Day09_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day09>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.PositionsVisited2.ShouldBe(5683);
        puzzle.PositionsVisited10.ShouldBe(2372);
    }
}
