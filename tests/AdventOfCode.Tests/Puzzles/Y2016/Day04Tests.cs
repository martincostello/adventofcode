// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class containing tests for the <see cref="Day04"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day04Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day04Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day04Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData("qzmt-zixmtkozy-ivhz-343[zimth]", "very encrypted name")]
    public static void Y2016_Day04_DecryptRoomName_Returns_Correct_Solution(string name, string expected)
    {
        // Act
        string actual = Day04.DecryptRoomName(name);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData("aaaaa-bbb-z-y-x-123[abxyz]", true)]
    [InlineData("a-b-c-d-e-f-g-h-987[abcde]", true)]
    [InlineData("not-a-real-room-404[oarel]", true)]
    [InlineData("totally-real-room-200[decoy]", false)]
    public static void Y2016_Day04_IsRoomReal_Returns_Correct_Solution(string name, bool expected)
    {
        // Act
        bool actual = Day04.IsRoomReal(name);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData(new[] { "aaaaa-bbb-z-y-x-123[abxyz]", "a-b-c-d-e-f-g-h-987[abcde]", "not-a-real-room-404[oarel]", "totally-real-room-200[decoy]" }, 1514)]
    public static void Y2016_Day04_SumOfRealRoomSectorIds_Returns_Correct_Solution(string[] names, int expected)
    {
        // Act
        int actual = Day04.SumOfRealRoomSectorIds(names);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2016_Day04_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day04>();

        // Assert
        puzzle.SumOfSectorIdsOfRealRooms.ShouldBe(137896);
        puzzle.SectorIdOfNorthPoleObjectsRoom.ShouldBe(501);
    }
}
