// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Api
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
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
        [InlineData(2015, 01, null, true, new object[] { 232, 1783 }, 0)]
        [InlineData(2015, 02, null, true, new object[] { 1598415, 3812909 }, 0)]
        [InlineData(2015, 03, null, true, new object[] { 2565, 2639 }, 0)]
        [InlineData(2015, 04, new string[] { "iwrupvqb", "5" }, false, new object[] { 346386 }, 0)]
        [InlineData(2015, 04, new string[] { "iwrupvqb", "6" }, false, new object[] { 9958218 }, 0)]
        [InlineData(2015, 05, new string[] { "1" }, true, new object[] { 236 }, 0)]
        [InlineData(2015, 05, new string[] { "2" }, true, new object[] { 51 }, 0)]
        [InlineData(2015, 06, new string[] { "1" }, true, new object[] { 543903 }, 0)]
        [InlineData(2015, 06, new string[] { "2" }, true, new object[] { 14687245 }, 0)]
        [InlineData(2015, 07, null, true, new object[] { 3176, 14710 }, 0)]
        [InlineData(2015, 08, null, true, new object[] { 1342, 2074 }, 0)]
        [InlineData(2015, 09, null, true, new object[] { 207 }, 0)]
        [InlineData(2015, 09, new string[] { "true" }, true, new object[] { 804 }, 0)]
        [InlineData(2015, 10, new string[] { "1321131112", "40" }, false, new object[] { 492982 }, 0)]
        [InlineData(2015, 10, new string[] { "1321131112", "50" }, false, new object[] { 6989950 }, 0)]
        [InlineData(2015, 11, new string[] { "cqjxjnds" }, false, new object[] { "cqjxxyzz" }, 0)]
        [InlineData(2015, 11, new string[] { "cqjxxyzz" }, false, new object[] { "cqkaabcc" }, 0)]
        [InlineData(2015, 12, null, true, new object[] { 191164 }, 0)]
        [InlineData(2015, 12, new[] { "red" }, true, new object[] { 87842 }, 0)]
        [InlineData(2015, 13, null, true, new object[] { 618, 601 }, 0)]
        [InlineData(2015, 14, new string[] { "2503" }, true, new object[] { 2655, 1059 }, 0)]
        [InlineData(2015, 15, null, true, new object[] { 222870, 117936 }, 0)]
        [InlineData(2015, 16, null, true, new object[] { 373, 260 }, 0)]
        [InlineData(2015, 17, new string[] { "150" }, true, new object[] { 1304, 18 }, 0)]
        [InlineData(2015, 18, new[] { "100", "false" }, true, new object[] { 814 }, 0)]
        [InlineData(2015, 18, new[] { "100", "true" }, true, new object[] { 924 }, 0)]
        [InlineData(2015, 19, new[] { "calibrate" }, false, new object[] { 576 }, 0, Skip = "Too slow.")]
        [InlineData(2015, 19, new[] { "fabricate" }, false, new object[] { 207 }, 0, Skip = "Too slow.")]
        [InlineData(2015, 20, new[] { "34000000" }, false, new object[] { 786240 }, 0)]
        [InlineData(2015, 20, new[] { "34000000", "50" }, false, new object[] { 831600 }, 0)]
        [InlineData(2015, 21, null, false, new object[] { 148, 78 }, 0)]
        [InlineData(2015, 22, new string[] { "easy" }, false, new object[] { 953 }, 0)]
        [InlineData(2015, 22, new string[] { "hard" }, false, new object[] { 1289 }, 0)]
        [InlineData(2015, 23, null, true, new object[] { 1u, 170u }, 0)]
        [InlineData(2015, 23, new string[] { "1" }, true, new object[] { 1u, 247u }, 0)]
        [InlineData(2015, 24, null, true, new object[] { 11266889531 }, 0, Skip = "Too slow.")]
        [InlineData(2015, 24, new string[] { "4" }, true, new object[] { 77387711 }, 0, Skip = "Too slow.")]
        [InlineData(2015, 25, new string[] { "2947", "3029" }, false, new object[] { 19980801u }, 0)]
        [InlineData(2016, 01, null, true, new object[] { 287, 133 }, 0)]
        [InlineData(2016, 02, null, true, new object[] { "14894", "26B96" }, 0)]
        [InlineData(2016, 03, null, true, new object[] { 983, 1836 }, 0)]
        [InlineData(2016, 04, null, true, new object[] { 137896, 501 }, 0)]
        [InlineData(2016, 05, new[] { "wtnhxymk" }, false, new object[] { "2414bc77", "437e60fc" }, 0)]
        [InlineData(2016, 06, null, true, new object[] { "qzedlxso", "ucmifjae" }, 0)]
        [InlineData(2016, 07, null, true, new object[] { 118, 260 }, 0)]
        [InlineData(2016, 08, null, true, new object[] { 121 }, 1)]
        [InlineData(2016, 09, null, true, new object[] { 98135, 10964557606 }, 0)]
        [InlineData(2016, 10, null, true, new object[] { 141, 1209 }, 0)]
        [InlineData(2016, 12, null, true, new object[] { 318020, 9227674 }, 0)]
        [InlineData(2016, 14, new[] { "ihaygndm" }, false, new object[] { 15035, 19968 }, 0, Skip = "Too slow.")]
        [InlineData(2016, 15, null, true, new object[] { 16824, 3543984 }, 0)]
        [InlineData(2016, 16, new[] { "10010000000110000", "272" }, false, new object[] { "10010110010011110" }, 0)]
        [InlineData(2016, 16, new[] { "10010000000110000", "35651584" }, false, new object[] { "01101011101100011" }, 0)]
        [InlineData(2016, 17, new[] { "pvhmgsws" }, false, new object[] { "DRRDRLDURD" }, 0)]
        [InlineData(2016, 18, new[] { "40" }, true, new object[] { 1987 }, 1)]
        [InlineData(2016, 18, new[] { "400000" }, true, new object[] { 19984714 }, 1)]
        [InlineData(2016, 19, new[] { "5", "1" }, false, new object[] { 3 }, 0)]
        [InlineData(2016, 19, new[] { "5", "2" }, false, new object[] { 2 }, 0)]
        [InlineData(2016, 19, new[] { "3014387", "1" }, false, new object[] { 1834471 }, 0)]
        [InlineData(2016, 19, new[] { "3014387", "2" }, false, new object[] { 1420064 }, 0)]
        [InlineData(2016, 20, null, true, new object[] { 22887907u, 109u }, 0)]
        [InlineData(2016, 21, new[] { "abcdefgh" }, true, new object[] { "gcedfahb" }, 0)]
        [InlineData(2016, 21, new[] { "fbgdceah", "true" }, true, new object[] { "hegbdcfa" }, 0)]
        [InlineData(2016, 22, null, true, new object[] { 985 }, 0)]
        [InlineData(2016, 23, null, true, new object[] { 14346, 479010906 }, 0, Skip = "Too slow.")]
        [InlineData(2016, 25, null, true, new object[] { 198 }, 0)]
        [InlineData(2017, 01, null, true, new object[] { 1034, 1356 }, 0)]
        [InlineData(2017, 02, null, true, new object[] { 36174, 244 }, 0)]
        [InlineData(2017, 03, new string[] { "312051" }, false, new object[] { 430, 312453 }, 0)]
        [InlineData(2017, 04, null, true, new object[] { 383, 265 }, 0)]
        [InlineData(2017, 05, null, true, new object[] { 373543, 27502966 }, 0)]
        [InlineData(2017, 06, null, true, new object[] { 3156, 1610 }, 0)]
        [InlineData(2017, 07, null, true, new object[] { "fbgguv", 1864 }, 0)]
        [InlineData(2017, 08, null, true, new object[] { 7296, 8186 }, 0)]
        [InlineData(2017, 09, null, true, new object[] { 11898, 5601 }, 0)]
        [InlineData(2017, 10, null, true, new object[] { 11413, "7adfd64c2a03a4968cf708d1b7fd418d" }, 0)]
        [InlineData(2017, 11, null, true, new object[] { 796, 1585 }, 0)]
        [InlineData(2017, 12, null, true, new object[] { 113, 202 }, 0)]
        [InlineData(2017, 13, null, true, new object[] { 1612, 3907994 }, 0)]
        [InlineData(2017, 14, new[] { "hwlqcszp" }, false, new object[] { 8304 }, 0)]
        [InlineData(2018, 01, null, true, new object[] { 543, 621 }, 0)]
        [InlineData(2018, 02, null, true, new object[] { 5880, "tiwcdpbseqhxryfmgkvjujvza" }, 0)]
        [InlineData(2018, 03, null, true, new object[] { 100595, "415" }, 0)]
        [InlineData(2018, 04, null, true, new object[] { 4716, 117061 }, 0)]
        [InlineData(2018, 05, null, true, new object[] { 10638, 4944 }, 0)]
        [InlineData(2019, 01, null, true, new object[] { 3226407, 4836738 }, 0)]
        [InlineData(2019, 02, null, true, new object[] { 9581917 }, 0)]
        [InlineData(2019, 03, null, true, new object[] { 855, 11238 }, 0)]
        [InlineData(2019, 04, new[] { "138241-674034" }, false, new object[] { 1890, 1277 }, 0)]
        [InlineData(2019, 05, new[] { "1" }, true, new object[] { 6745903 }, 0)]
        [InlineData(2019, 05, new[] { "5" }, true, new object[] { 9168267 }, 0)]
        [InlineData(2019, 07, null, true, new object[] { 77500, 22476942 }, 0)]
        [InlineData(2019, 08, null, true, new object[] { 2080 }, 1)]
        [InlineData(2019, 09, new[] { "1" }, true, new object[] { 2494485073 }, 0)]
        [InlineData(2019, 09, new[] { "2" }, true, new object[] { 44997 }, 0)]
        [InlineData(2019, 13, null, true, new object[] { 315 }, 0)]
        [InlineData(2020, 01, null, true, new object[] { 63616, 67877784 }, 0)]
        [InlineData(2020, 02, null, true, new object[] { 542, 360 }, 0)]
        [InlineData(2020, 03, null, true, new object[] { 216, 6708199680L }, 0)]
        [InlineData(2020, 04, null, true, new object[] { 226, 160 }, 0)]
        [InlineData(2020, 05, null, true, new object[] { 878, 504 }, 0)]
        [InlineData(2020, 06, null, true, new object[] { 6542, 3299 }, 0)]
        [InlineData(2020, 07, new[] { "shiny gold" }, true, new object[] { 179, 18925 }, 0)]
        [InlineData(2020, 08, null, true, new object[] { 1137, 1125 }, 0)]
        [InlineData(2020, 09, null, true, new object[] { 22406676, 2942387 }, 0)]
        [InlineData(2020, 10, null, true, new object[] { 2775, 518344341716992L }, 0)]
        [InlineData(2020, 11, null, true, new object[] { 2108, 1897 }, 2)]
        [InlineData(2020, 12, null, true, new object[] { 439, 12385 }, 0)]
        [InlineData(2020, 13, null, true, new object[] { 2935, 836024966345345L }, 0)]
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
            client.Timeout = TimeSpan.FromMinutes(1);

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

                if (expected is string valueAsString)
                {
                    actual.GetString().ShouldBe(valueAsString);
                }
                else
                {
                    actual.GetRawText().ShouldBe(expected.ToString());
                }
            }
        }

        [Fact]
        public async Task Can_Get_Puzzle_Metadata()
        {
            // Arrange
            using var client = Fixture.CreateClient();

            // Act
            using var response = await client.GetAsync($"/api/puzzles");

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            response.Content.ShouldNotBeNull();
            response.Content!.Headers.ContentType.ShouldNotBeNull();
            response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

            using var puzzles = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());

            puzzles.RootElement.GetArrayLength().ShouldBeGreaterThan(0);

            foreach (var puzzle in puzzles.RootElement.EnumerateArray())
            {
                puzzle.TryGetProperty("day", out _).ShouldBeTrue();
                puzzle.TryGetProperty("location", out _).ShouldBeTrue();
                puzzle.TryGetProperty("minimumArguments", out _).ShouldBeTrue();
                puzzle.TryGetProperty("requiresData", out _).ShouldBeTrue();
                puzzle.TryGetProperty("year", out _).ShouldBeTrue();
            }
        }

        [Fact]
        public async Task Api_Returns_404_If_Puzzle_Not_Found()
        {
            // Arrange
            using var client = Fixture.CreateClient();

            // Act
            using var content = new MultipartFormDataContent();
            using var response = await client.PostAsync($"/api/puzzles/2014/1/solve", content);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            response.Content.ShouldNotBeNull();
            response.Content!.Headers.ContentType.ShouldNotBeNull();
            response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");
        }

        [Fact]
        public async Task Api_Returns_415_If_Puzzle_Content_Incorrect()
        {
            // Arrange
            using var client = Fixture.CreateClient();

            // Act
            using var content = new StringContent("{}", Encoding.UTF8, "application/json");
            using var response = await client.PostAsync($"/api/puzzles/2015/1/solve", content);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.UnsupportedMediaType);
            response.Content.ShouldNotBeNull();
            response.Content!.Headers.ContentType.ShouldNotBeNull();
            response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");
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
