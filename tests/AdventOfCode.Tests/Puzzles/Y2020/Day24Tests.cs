// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class containing tests for the <see cref="Day24"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day24Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day24Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day24Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1, 15)]
    [InlineData(2, 12)]
    [InlineData(3, 25)]
    [InlineData(4, 14)]
    [InlineData(5, 23)]
    [InlineData(6, 28)]
    [InlineData(7, 41)]
    [InlineData(8, 37)]
    [InlineData(9, 49)]
    [InlineData(10, 37)]
    [InlineData(20, 132)]
    [InlineData(30, 259)]
    [InlineData(40, 406)]
    [InlineData(50, 566)]
    [InlineData(60, 788)]
    [InlineData(70, 1106)]
    [InlineData(80, 1373)]
    [InlineData(90, 1844)]
    [InlineData(100, 2208)]
    public void Y2020_Day24_TileFloor_Returns_Correct_Value(int days, int expected)
    {
        // Arrange
        string[] instructions =
        {
            "sesenwnenenewseeswwswswwnenewsewsw",
            "neeenesenwnwwswnenewnwwsewnenwseswesw",
            "seswneswswsenwwnwse",
            "nwnwneseeswswnenewneswwnewseswneseene",
            "swweswneswnenwsewnwneneseenw",
            "eesenwseswswnenwswnwnwsewwnwsene",
            "sewnenenenesenwsewnenwwwse",
            "wenwwweseeeweswwwnwwe",
            "wsweesenenewnwwnwsenewsenwwsesesenwne",
            "neeswseenwwswnwswswnw",
            "nenwswwsewswnenenewsenwsenwnesesenew",
            "enewnwewneswsewnwswenweswnenwsenwsw",
            "sweneswneswneneenwnewenewwneswswnese",
            "swwesenesewenwneswnwwneseswwne",
            "enesenwswwswneneswsenwnewswseenwsese",
            "wnwnesenesenenwwnenwsewesewsesesew",
            "nenewswnwewswnenesenwnesewesw",
            "eneswnwswnwsenenwnwnwwseeswneewsenese",
            "neswnwewnwnwseenwseesewsenwsweewe",
            "wseweeenwnesenwwwswnew",
        };

        // Act
        int actual = Day24.TileFloor(instructions, days);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2020_Day24_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day24>();

        // Assert
        puzzle.BlackTilesDay0.ShouldBe(289);
        puzzle.BlackTilesDay100.ShouldBe(3551);
    }
}
