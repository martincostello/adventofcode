﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Api
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Playwright;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the User Interface.
    /// </summary>
    public class UITests : IntegrationTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UITests"/> class.
        /// </summary>
        /// <param name="fixture">The fixture to use.</param>
        /// <param name="outputHelper">The test output helper to use.</param>
        public UITests(HttpServerFixture fixture, ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Theory]
        [InlineData("2015", "25", new[] { "2947", "3029" }, new[] { "19980801" })]
        [InlineData("2016", "13", new[] { "1362" }, new[] { "82", "138" })]
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

        [Theory]
        [InlineData("2015", "1", new[] { "232", "1783" })]
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

        [Theory]
        [InlineData("2015", "5", new[] { "1" }, new[] { "236" })]
        [InlineData("2016", "21", new[] { "fbgdceah", "true" }, new[] { "hegbdcfa" })]
        public async Task Can_Solve_Puzzle_With_Input_And_Input_File(
            string year,
            string day,
            string[] arguments,
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

                await solver.InputArgumentsAsync(arguments);

                string input = await GetPuzzleInputAsync(year, day);
                await solver.SelectInputAsync(input);

                // Act
                await solver.SolveAsync();

                // Assert
                await solver.SolutionsAsync().ShouldBe(expected);
            });
        }

        [Fact]
        public async Task Can_Solve_Puzzle_With_Visualization()
        {
            // Arrange
            string year = "2016";
            string day = "8";

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
                await solver.SolutionsAsync().ShouldBe(new[] { "121" });
                await solver.VisualizationsAsync().ShouldBe(1);
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

        private sealed class PuzzleSolver
        {
            internal PuzzleSolver(IPage page)
            {
                Page = page;
            }

            private IPage Page { get; }

            public async Task InputArgumentsAsync(IReadOnlyList<string> arguments)
            {
                const string Selector = "id=arguments";

                for (int i = 0; i < arguments.Count; i++)
                {
                    if (i > 0)
                    {
                        await Page.FocusAsync(Selector);
                        await Page.Keyboard.PressAsync("Enter");
                    }

                    await Page.TypeAsync(Selector, arguments[i]);
                }
            }

            public async Task SelectDayAsync(string day)
                => await Page.SelectOptionAsync("id=day", day);

            public async Task SelectYearAsync(string year)
                => await Page.SelectOptionAsync("id=year", year);

            public async Task SelectInputAsync(string content)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(content);

                var files = new FilePayload()
                {
                    Buffer = buffer,
                    Name = "input.txt",
                    MimeType = "text/plain",
                };

                await Page.SetInputFilesAsync("id=resource", files);
            }

            public async Task SolveAsync()
                => await Page.ClickAsync("id=solve");

            public async Task<IList<string>> SolutionsAsync()
            {
                var container = await Page.QuerySelectorAsync("id=solution");
                await container!.WaitForElementStateAsync(ElementState.Visible);

                var solutions = await container.QuerySelectorAllAsync(".text-monospace");

                var actual = new List<string>();

                foreach (var solution in solutions)
                {
                    actual.Add(await solution.InnerTextAsync());
                }

                return actual;
            }

            public async Task<int> VisualizationsAsync()
            {
                var container = await Page.QuerySelectorAsync("id=solution");
                await container!.WaitForElementStateAsync(ElementState.Visible);

                var visualizations = await container.QuerySelectorAllAsync("pre");
                return visualizations.Count;
            }
        }
    }
}
