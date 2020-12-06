// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Api
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the API.
    /// </summary>
    public class ApiTests : IntegrationTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTests"/> class.
        /// </summary>
        /// <param name="fixture">The fixture to use.</param>
        /// <param name="outputHelper">The test output helper to use.</param>
        public ApiTests(HttpServerFixture fixture, ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Theory]
        [InlineData(2020, 05, new string[0], true, new object[] { 878, 504 }, 0)]
        public async Task Can_Solve_Puzzle(
            int year,
            int day,
            string[] arguments,
            bool sendResource,
            object[] expectedSolutions,
            int expectedVisualizations)
        {
            // Arrange
            using var client = Fixture.CreateClient();

            using var content = new MultipartFormDataContent();

            if (arguments is not null)
            {
                foreach (string argument in arguments)
                {
#pragma warning disable CA2000
                    content.Add(new StringContent(argument), "arguments");
#pragma warning restore CA2000
                }
            }

            if (sendResource)
            {
#pragma warning disable CA2000
                content.Add(new StringContent(GetPuzzleInput(year, day)), "resource");
#pragma warning restore CA2000
            }

            // Act
            using var content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string?, string?>>)parameters);
            using var response = await client.PostAsync($"/api/puzzles/{year}/{day}/solve", content);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            response.Content.ShouldNotBeNull();
            response.Content!.Headers.ContentType.ShouldNotBeNull();
            response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

            using var solution = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());

            solution.RootElement.GetProperty("year").GetInt32().ShouldBe(year);
            solution.RootElement.GetProperty("day").GetInt32().ShouldBe(day);
            solution.RootElement.GetProperty("timeToSolve").GetDouble().ShouldBeGreaterThan(0);
            solution.RootElement.GetProperty("visualizations").GetArrayLength().ShouldBe(expectedVisualizations);

            solution.RootElement.TryGetProperty("solutions", out var solutions).ShouldBeTrue();
            solutions.GetArrayLength().ShouldBe(expectedSolutions.Length);

            var actualSolutions = solutions.EnumerateArray().ToArray();

            for (int i = 0; i < expectedSolutions.Length; i++)
            {
                object? expected = expectedSolutions[i];
                var actual = actualSolutions[i];

                actual.GetRawText().ShouldBe(expected.ToString());
            }
        }

        private static string GetPuzzleInput(int year, int day)
        {
            var type = typeof(Puzzle);

            string name = FormattableString.Invariant(
                $"MartinCostello.{type.Assembly.GetName().Name}.Input.Y{year}.Day{day:00}.input.txt");

            using var stream = type.Assembly.GetManifestResourceStream(name) !;
            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}
