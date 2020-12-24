// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System.Threading.Tasks;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day21"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day21Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day21Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day21Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public void Y2020_Day21_GetIngredientsWithNoAllergens_Returns_Correct_Value()
        {
            // Arrange
            string[] foods = new[]
            {
                "mxmxvkd kfcds sqjhc nhms (contains dairy, fish)",
                "trh fvjkl sbzzf mxmxvkd (contains dairy)",
                "sqjhc fvjkl (contains soy)",
                "sqjhc mxmxvkd sbzzf (contains fish)",
            };

            // Act
            int actual = Day21.GetIngredientsWithNoAllergens(foods);

            // Assert
            actual.ShouldBe(5);
        }

        [Fact]
        public async Task Y2020_Day21_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day21>();

            // Assert
            puzzle.IngredientsWithNoAllergens.ShouldBe(2098);
        }
    }
}
