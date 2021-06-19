// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Playwright;

    internal sealed class PuzzleSolver
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
