// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day13"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day13Tests
    {
        [Fact]
        public static void Day13_GetMaximumTotalChangeInHappiness()
        {
            // Arrange
            string[] potentialHappiness = new[]
            {
                "Alice would gain 54 happiness units by sitting next to Bob.",
                "Alice would lose 79 happiness units by sitting next to Carol.",
                "Alice would lose 2 happiness units by sitting next to David.",
                "Bob would gain 83 happiness units by sitting next to Alice.",
                "Bob would lose 7 happiness units by sitting next to Carol.",
                "Bob would lose 63 happiness units by sitting next to David.",
                "Carol would lose 62 happiness units by sitting next to Alice.",
                "Carol would gain 60 happiness units by sitting next to Bob.",
                "Carol would gain 55 happiness units by sitting next to David.",
                "David would gain 46 happiness units by sitting next to Alice.",
                "David would lose 7 happiness units by sitting next to Bob.",
                "David would gain 41 happiness units by sitting next to Carol.",
            };

            // Act
            int actual = Day13.GetMaximumTotalChangeInHappiness(potentialHappiness);

            // Assert
            Assert.Equal(330, actual);
        }

        [Fact]
        public static void Day13_Solve_Returns_Correct_Solution()
        {
            // Arrange
            string[] args = new[] { @".\Input\Day13\input.txt" };
            Day13 target = new Day13();

            // Act
            int actual = target.Solve(args);

            // Assert
            Assert.Equal(0, actual);
            Assert.Equal(618, target.MaximumTotalChangeInHappiness);
            Assert.Equal(601, target.MaximumTotalChangeInHappinessWithCurrentUser);
        }
    }
}
