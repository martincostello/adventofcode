// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

public sealed class Day25Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2020_Day25_GetPrivateKey_Returns_Correct_Value()
    {
        // Arrange
        int cardPublicKey = 5764801;
        int doorPublicKey = 17807724;

        // Act
        long actual = Day25.GetPrivateKey(cardPublicKey, doorPublicKey);

        // Assert
        actual.ShouldBe(14897079);
    }

    [Fact]
    public async Task Y2020_Day25_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day25>();

        // Assert
        puzzle.PrivateKey.ShouldBe(296776);
    }
}
