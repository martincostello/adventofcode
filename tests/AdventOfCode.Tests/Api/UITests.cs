// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Microsoft.Playwright;

namespace MartinCostello.AdventOfCode.Api;

public class UITests : IntegrationTest
{
    private static bool _playwrightInstalled;

    public UITests(HttpServerFixture fixture, ITestOutputHelper outputHelper)
        : base(fixture, outputHelper)
    {
        InstallPlaywright();
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
            await solver.SolutionsAsync().ShouldBe(new[] { "121", "RURUCEOEIL" });
            await solver.VisualizationsAsync().ShouldBe(1);
        });
    }

    private static async Task<string> GetPuzzleInputAsync(string year, string day)
    {
        var type = typeof(Puzzle);

        string name = FormattableString.Invariant(
            $"MartinCostello.{type.Assembly.GetName().Name}.Input.Y{year}.Day{(day.Length == 1 ? "0" : string.Empty)}{day}.input.txt");

        using var stream = type.Assembly.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(stream);

        return await reader.ReadToEndAsync();
    }

    private static void InstallPlaywright()
    {
        if (_playwrightInstalled)
        {
            return;
        }

        int exitCode = Microsoft.Playwright.Program.Main(new[] { "install" });

        if (exitCode != 0)
        {
            throw new InvalidOperationException($"Playwright exited with code {exitCode}.");
        }

        _playwrightInstalled = true;
    }

    private async Task<PuzzleSolver> LoadApplication(IPage page)
    {
        await page.GotoAsync(Fixture.ServerAddress.ToString());
        await page.WaitForLoadStateAsync();

        return new PuzzleSolver(page);
    }
}
