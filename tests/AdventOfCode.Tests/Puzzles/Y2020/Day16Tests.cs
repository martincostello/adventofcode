﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    /// <summary>
    /// A class containing tests for the <see cref="Day16"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day16Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day16Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day16Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public void Y2020_Day16_ScanTickets_Returns_Correct_Error_Rate()
        {
            // Arrange
            string[] notes = new[]
            {
                "class: 1-3 or 5-7",
                "row: 6 - 11 or 33 - 44",
                "seat: 13 - 40 or 45 - 50",
                string.Empty,
                "your ticket:",
                "7,1,14",
                string.Empty,
                "nearby tickets:",
                "7,3,47",
                "40,4,50",
                "55,2,20",
                "38,6,12",
            };

            // Act
            (int actual, _) = Day16.ScanTickets(notes);

            // Assert
            actual.ShouldBe(71);
        }

        [Fact]
        public void Y2020_Day16_ScanTickets_Returns_Correct_Ticket()
        {
            // Arrange
            string[] notes = new[]
            {
                "class: 0-1 or 4-19",
                "row: 0-5 or 8-19",
                "seat: 0-13 or 16-19",
                string.Empty,
                "your ticket:",
                "11,12,13",
                string.Empty,
                "nearby tickets:",
                "3,9,18",
                "15,1,5",
                "5,14,9",
            };

            // Act
            (_, IDictionary<string, int> actual) = Day16.ScanTickets(notes);

            // Assert
            actual.ShouldNotBeNull();
            actual.ShouldContainKeyAndValue("class", 12);
            actual.ShouldContainKeyAndValue("row", 11);
            actual.ShouldContainKeyAndValue("seat", 13);
        }

        [Fact]
        public async Task Y2020_Day16_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day16>();

            // Assert
            puzzle.ScanningErrorRate.ShouldBe(21071);
            puzzle.DepartureProduct.ShouldBe(3429967441937L);
        }
    }
}
