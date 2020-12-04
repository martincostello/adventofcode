// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

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

        [Fact]
        public void Y2020_Day04_GetValidPassportCount_Returns_Correct_Value()
        {
            // Arrange
            string[] batch = new[]
            {
                "ecl:gry pid:860033327 eyr:2020 hcl:#fffffd",
                "byr:1937 iyr:2017 cid:147 hgt:183cm",
                string.Empty,
                "iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884",
                "hcl:#cfa07d byr:1929",
                string.Empty,
                "hcl:#ae17e1 iyr:2013",
                "eyr:2024",
                "ecl:brn pid:760753108 byr:1931",
                "hgt:179cm",
                string.Empty,
                "hcl:#cfa07d eyr:2025 pid:166559648",
                "iyr:2011 ecl:brn hgt:59in",
            };

            // Act
            int actual = Day04.GetValidPassportCount(batch);

            // Assert
            actual.ShouldBe(2);
        }

        [Fact]
        public void Y2020_Day04_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = SolvePuzzle<Day04>();

            // Assert
            puzzle.ValidPassports.ShouldBe(226);
        }
    }
}
