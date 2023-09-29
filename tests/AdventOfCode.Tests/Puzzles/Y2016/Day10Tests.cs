// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

#pragma warning disable SA1010

public sealed class Day10Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public static void Y2016_Day10_GetBotNumber_Returns_Correct_Solution()
    {
        // Arrange
        string[] instructions =
        [
            "value 5 goes to bot 2",
            "bot 2 gives low to bot 1 and high to bot 0",
            "value 3 goes to bot 1",
            "bot 1 gives low to output 1 and high to bot 0",
            "bot 0 gives low to output 2 and high to output 0",
            "value 2 goes to bot 2",
        ];

        int[] binsOfInterest = [0, 1, 2];

        // Act
        (int bot, int product) = Day10.GetBotNumber(instructions, 5, 2, binsOfInterest);

        // Assert
        bot.ShouldBe(2);
        product.ShouldBe(30);
    }

    [Fact]
    public async Task Y2016_Day10_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day10>();

        // Assert
        puzzle.BotThatCompares61And17Microchips.ShouldBe(141);
        puzzle.ProductOfMicrochipsInBins012.ShouldBe(1209);
    }
}
