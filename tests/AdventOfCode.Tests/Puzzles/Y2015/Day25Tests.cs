// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

public sealed class Day25Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(20151125u, 31916031u)]
    [InlineData(31916031u, 18749137u)]
    [InlineData(18749137u, 16080970u)]
    [InlineData(16080970u, 21629792u)]
    [InlineData(21629792u, 17289845u)]
    [InlineData(17289845u, 24592653u)]
    [InlineData(24592653u, 8057251u)]
    [InlineData(8057251u, 16929656u)]
    [InlineData(16929656u, 30943339u)]
    [InlineData(30943339u, 77061u)]
    [InlineData(77061u, 32451966u)]
    [InlineData(32451966u, 1601130u)]
    [InlineData(1601130u, 7726640u)]
    [InlineData(7726640u, 10071777u)]
    [InlineData(10071777u, 33071741u)]
    [InlineData(33071741u, 17552253u)]
    [InlineData(17552253u, 21345942u)]
    [InlineData(21345942u, 7981243u)]
    [InlineData(7981243u, 15514188u)]
    [InlineData(15514188u, 33511524u)]
    public static void Y2015_Day25_GenerateCode(ulong value, ulong expected)
    {
        // Act
        ulong actual = Day25.GenerateCode(value);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData(1, 1, 20151125u)]
    [InlineData(2, 1, 31916031u)]
    [InlineData(1, 2, 18749137u)]
    [InlineData(3, 1, 16080970u)]
    [InlineData(2, 2, 21629792u)]
    [InlineData(1, 3, 17289845u)]
    [InlineData(4, 1, 24592653u)]
    [InlineData(3, 2, 8057251u)]
    [InlineData(2, 3, 16929656u)]
    [InlineData(5, 1, 77061u)]
    [InlineData(4, 2, 32451966u)]
    [InlineData(3, 3, 1601130u)]
    [InlineData(2, 4, 7726640u)]
    [InlineData(1, 5, 10071777u)]
    [InlineData(6, 1, 33071741u)]
    [InlineData(5, 2, 17552253u)]
    [InlineData(4, 3, 21345942u)]
    [InlineData(3, 4, 7981243u)]
    [InlineData(5, 5, 9250759u)]
    [InlineData(1, 6, 33511524u)]
    public static void Y2015_Day25_GetCodeForWeatherMachine(int row, int column, ulong expected)
    {
        // Act
        ulong actual = Day25.GetCodeForWeatherMachine(row, column);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2015_Day25_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day25>("2947", "3029");

        // Assert
        puzzle.Code.ShouldBe(19980801u);
    }
}
