// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

public sealed class Day21Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2020_Day21_GetIngredientsWithNoAllergens_Returns_Correct_Value()
    {
        // Arrange
        string[] foods =
        [
            "mxmxvkd kfcds sqjhc nhms (contains dairy, fish)",
            "trh fvjkl sbzzf mxmxvkd (contains dairy)",
            "sqjhc fvjkl (contains soy)",
            "sqjhc mxmxvkd sbzzf (contains fish)",
        ];

        // Act
        (int actualOccurrences, string actualAllergens) = Day21.GetIngredientsWithNoAllergens(foods);

        // Assert
        actualOccurrences.ShouldBe(5);
        actualAllergens.ShouldBe("mxmxvkd,sqjhc,fvjkl");
    }

    [Fact]
    public async Task Y2020_Day21_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day21>();

        // Assert
        puzzle.IngredientsWithNoAllergens.ShouldBe(2098);
        puzzle.CanonicalAllergens.ShouldBe("ppdplc,gkcplx,ktlh,msfmt,dqsbql,mvqkdj,ggsz,hbhsx");
    }
}
