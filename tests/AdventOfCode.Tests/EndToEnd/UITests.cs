// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MartinCostello.AdventOfCode.EndToEnd
{
    public class UITests : EndToEndTest
    {
        public UITests(SiteFixture fixture, ITestOutputHelper outputHelper)
            : base(fixture)
        {
            OutputHelper = outputHelper;
        }

        private ITestOutputHelper OutputHelper { get; }

        [SkippableTheory]
        [InlineData("2015", "25", new[] { "2947", "3029" }, new[] { "19980801" })]
        public async Task Can_Solve_Puzzle_With_Input(
            string year,
            string day,
            string[] arguments,
            string[] expected)
        {
            // Arrange
            var browser = new BrowserFixture(OutputHelper);

            await browser.WithPageAsync(async (page) =>
            {
                PuzzleSolver solver = await LoadApplication(page);

                await solver.SelectYearAsync(year);
                await solver.SelectDayAsync(day);

                await solver.InputArgumentsAsync(arguments);

                // Act
                await solver.SolveAsync();

                // Assert
                await solver.SolutionsAsync().ShouldBe(expected);
            });
        }

        [SkippableTheory]
        [InlineData("2016", "25", new[] { "198" })]
        public async Task Can_Solve_Puzzle_With_Input_File(
            string year,
            string day,
            string[] expected)
        {
            // Arrange
            var browser = new BrowserFixture(OutputHelper);

            // Act
            await browser.WithPageAsync(async (page) =>
            {
                PuzzleSolver solver = await LoadApplication(page);

                await solver.SelectYearAsync(year);
                await solver.SelectDayAsync(day);

                string input = await GetPuzzleInputAsync(year, day);
                await solver.SelectInputAsync(input);

                // Act
                await solver.SolveAsync();

                // Assert
                await solver.SolutionsAsync().ShouldBe(expected);
            });
        }

        private static async Task<string> GetPuzzleInputAsync(string year, string day)
        {
            var type = typeof(Puzzle);

            string name = FormattableString.Invariant(
                $"MartinCostello.{type.Assembly.GetName().Name}.Input.Y{year}.Day{(day.Length == 1 ? "0" : string.Empty)}{day}.input.txt");

            using var stream = type.Assembly.GetManifestResourceStream(name) !;
            using var reader = new StreamReader(stream);

            return await reader.ReadToEndAsync();
        }

        private async Task<PuzzleSolver> LoadApplication(IPage page)
        {
            await page.GotoAsync(Fixture.ServerAddress.ToString());
            await page.WaitForLoadStateAsync();

            return new PuzzleSolver(page);
        }
    }
}
