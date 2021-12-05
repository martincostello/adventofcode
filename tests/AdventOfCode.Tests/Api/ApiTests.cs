// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Net;
using System.Text.Json;

namespace MartinCostello.AdventOfCode.Api;

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

    public static IEnumerable<object[]> Puzzles()
    {
        var testCases = new List<TestCase>()
        {
            new(2015, 01, 232, 1783),
            new(2015, 02, 1598415, 3812909),
            new(2015, 03, 2565, 2639),
            new(2015, 04, new[] { "iwrupvqb", "5" }, 346386),
            new(2015, 04, new[] { "iwrupvqb", "6" }, 9958218),
            new(2015, 05, new[] { "1" }, 236) { SendResource = true },
            new(2015, 05, new[] { "2" }, 51) { SendResource = true },
            new(2015, 06, new[] { "1" }, 543903) { SendResource = true },
            new(2015, 06, new[] { "2" }, 14687245) { SendResource = true },
            new(2015, 07, 3176, 14710),
            new(2015, 08, 1342, 2074),
            new(2015, 09, 207),
            new(2015, 09, 804) { Arguments = new string[] { "true" } },
            new(2015, 10, new[] { "1321131112", "40" }, 492982),
            new(2015, 10, new[] { "1321131112", "50" }, 6989950),
            new(2015, 11, new[] { "cqjxjnds" }, "cqjxxyzz"),
            new(2015, 11, new[] { "cqjxxyzz" }, "cqkaabcc"),
            new(2015, 12, 191164),
            new(2015, 12, new[] { "red" }, 87842) { SendResource = true },
            new(2015, 13, 618, 601),
            new(2015, 14, new string[] { "2503" }, 2655, 1059) { SendResource = true },
            new(2015, 15, 222870, 117936),
            new(2015, 16, 373, 260),
            new(2015, 17, new[] { "150" }, 1304, 18) { SendResource = true },
            new(2015, 18, new[] { "100", "false" }, 814) { SendResource = true },
            new(2015, 18, new[] { "100", "true" }, 924) { SendResource = true },
            ////new(2015, 19, new[] { "calibrate" }, 576),
            ////new(2015, 19, new[] { "fabricate" }, 207),
            new(2015, 20, new[] { "34000000" }, 786240),
            new(2015, 20, new[] { "34000000", "50" }, 831600),
            new(2015, 21, 148, 78) { SendResource = false },
            new(2015, 22, new[] { "easy" }, 953),
            new(2015, 22, new[] { "hard" }, 1289),
            new(2015, 23, 1u, 170u) { SendResource = true },
            new(2015, 23, new[] { "1" }, 1u, 247u) { SendResource = true },
            ////new(2015, 24, 11266889531),
            ////new(2015, 24, new[] { "4" }, 77387711) { SendResource = true },
            new(2015, 25, new[] { "2947", "3029" }, 19980801u),
            new(2016, 01, 287, 133),
            new(2016, 02, "14894", "26B96"),
            new(2016, 03, 983, 1836),
            new(2016, 04, 137896, 501),
            new(2016, 05, new[] { "wtnhxymk" }, "2414bc77", "437e60fc"),
            new(2016, 06, "qzedlxso", "ucmifjae"),
            new(2016, 07, 118, 260),
            new(2016, 08, 121) { ExpectedVisualizations = 1 },
            new(2016, 09, 98135, 10964557606),
            new(2016, 10, 141, 1209),
            new(2016, 12, 318020, 9227674),
            new(2016, 13, new[] { "1362" }, 82, 138),
            ////new(2016, 14, new[] { "ihaygndm" }, 15035, 19968),
            new(2016, 15, 16824, 3543984),
            new(2016, 16, new[] { "10010000000110000", "272" }, "10010110010011110"),
            new(2016, 16, new[] { "10010000000110000", "35651584" }, "01101011101100011"),
            new(2016, 17, new[] { "pvhmgsws" }, "DRRDRLDURD", 618),
            new(2016, 18, new[] { "40" }, 1987) { SendResource = true, ExpectedVisualizations = 1 },
            new(2016, 18, new[] { "400000" }, 19984714) { SendResource = true, ExpectedVisualizations = 1 },
            new(2016, 19, new[] { "5", "1" }, 3),
            new(2016, 19, new[] { "5", "2" }, 2),
            new(2016, 19, new[] { "3014387", "1" }, 1834471),
            new(2016, 19, new[] { "3014387", "2" }, 1420064),
            new(2016, 20, 22887907u, 109u),
            new(2016, 21, new[] { "abcdefgh" }, "gcedfahb") { SendResource = true },
            new(2016, 21, new[] { "fbgdceah", "true" }, "hegbdcfa") { SendResource = true },
            new(2016, 22, 985, 179),
            ////new(2016, 23, 14346, 479010906),
            new(2016, 24, 502, 724),
            new(2016, 25, 198),
            new(2017, 01, 1034, 1356),
            new(2017, 02, 36174, 244),
            new(2017, 03, new[] { "312051" }, 430, 312453),
            new(2017, 04, 383, 265),
            new(2017, 05, 373543, 27502966),
            new(2017, 06, 3156, 1610),
            new(2017, 07, "fbgguv", 1864),
            new(2017, 08, 7296, 8186),
            new(2017, 09, 11898, 5601),
            new(2017, 10, 11413, "7adfd64c2a03a4968cf708d1b7fd418d"),
            new(2017, 11, 796, 1585),
            new(2017, 12, 113, 202),
            new(2017, 13, 1612, 3907994),
            new(2017, 14, new[] { "hwlqcszp" }, 8304),
            new(2017, 15, 594, 328),
            new(2018, 01, 543, 621),
            new(2018, 02, 5880, "tiwcdpbseqhxryfmgkvjujvza"),
            new(2018, 03, 100595, "415"),
            new(2018, 04, 4716, 117061),
            new(2018, 05, 10638, 4944),
            new(2018, 06, 5626, 46554),
            new(2019, 01, 3226407, 4836738),
            new(2019, 02, 9581917),
            new(2019, 03, 855, 11238),
            new(2019, 04, new[] { "138241-674034" }, 1890, 1277),
            new(2019, 05, new[] { "1" }, 6745903) { SendResource = true },
            new(2019, 05, new[] { "5" }, 9168267) { SendResource = true },
            new(2019, 07, 77500, 22476942),
            new(2019, 08, 2080) { ExpectedVisualizations = 1 },
            new(2019, 09, new[] { "1" }, 2494485073) { SendResource = true },
            new(2019, 09, new[] { "2" }, 44997) { SendResource = true },
            new(2019, 13, 315),
            new(2020, 01, 63616, 67877784),
            new(2020, 02, 542, 360),
            new(2020, 03, 216, 6708199680L),
            new(2020, 04, 226, 160),
            new(2020, 05, 878, 504),
            new(2020, 06, 6542, 3299),
            new(2020, 07, new[] { "shiny gold" }, 179, 18925) { SendResource = true },
            new(2020, 08, 1137, 1125),
            new(2020, 09, 22406676, 2942387),
            new(2020, 10, 2775, 518344341716992L),
            new(2020, 11, 2108, 1897) { ExpectedVisualizations = 2 },
            new(2020, 12, 439, 12385),
            new(2020, 13, 2935, 836024966345345L),
            new(2020, 14, 9967721333886L, 4355897790573),
            new(2020, 15, new[] { "0,5,4,1,10,14,7" }, 203, 9007186),
            new(2020, 16, 21071, 3429967441937L),
            new(2020, 17, 388, 2280) { ExpectedVisualizations = 2 },
            new(2020, 18, 2743012121210L, 65658760783597L),
            new(2020, 19, 195, 309),
            new(2020, 20, 17712468069479L, 2173) { ExpectedVisualizations = 1 },
            new(2020, 21, 2098, "ppdplc,gkcplx,ktlh,msfmt,dqsbql,mvqkdj,ggsz,hbhsx"),
            new(2020, 22, 33694, 31835) { SendResource = true },
            new(2020, 23, new[] { "583976241" }, "24987653", 442938711161),
            new(2020, 24, 289, 3551),
            new(2020, 25, 296776),
            new(2021, 01, 1532, 1571),
            new(2021, 02, 2150351, 1842742223),
            new(2021, 03, 3633500, 4550283),
            new(2021, 04, 41668, 10478),
            new(2021, 05, 5690, 17741),
        };

        return testCases
            .Select((p) => new object[] { p.Year, p.Day, p })
            .ToArray();
    }

    [Theory]
    [MemberData(nameof(Puzzles))]
    public async Task Can_Solve_Puzzle(int year, int day, TestCase testCase)
    {
        // Arrange
        using var client = Fixture.CreateClient();
        client.Timeout = TimeSpan.FromMinutes(1.5);

        using var content = new MultipartFormDataContent();

        if (testCase.Arguments is not null)
        {
            foreach (string argument in testCase.Arguments)
            {
#pragma warning disable CA2000
                content.Add(new StringContent(argument), "arguments");
#pragma warning restore CA2000
            }
        }

        if (testCase.SendResource)
        {
#pragma warning disable CA2000
            content.Add(new StringContent(GetPuzzleInput(year, day)), "resource");
#pragma warning restore CA2000
        }

        // Act
        using var response = await client.PostAsync($"/api/puzzles/{year}/{day}/solve", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK, await response.Content.ReadAsStringAsync());
        response.Content.ShouldNotBeNull();
        response.Content!.Headers.ContentType.ShouldNotBeNull();
        response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

        using var solution = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());

        solution.RootElement.GetProperty("year").GetInt32().ShouldBe(year);
        solution.RootElement.GetProperty("day").GetInt32().ShouldBe(day);
        solution.RootElement.GetProperty("timeToSolve").GetDouble().ShouldBeGreaterThan(0);
        solution.RootElement.GetProperty("visualizations").GetArrayLength().ShouldBe(testCase.ExpectedVisualizations);

        solution.RootElement.TryGetProperty("solutions", out var solutions).ShouldBeTrue();
        solutions.GetArrayLength().ShouldBe(testCase.ExpectedSolutions.Length);

        var actualSolutions = solutions.EnumerateArray().ToArray();

        for (int i = 0; i < testCase.ExpectedSolutions.Length; i++)
        {
            object? expected = testCase.ExpectedSolutions[i];
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
        using var response = await client.GetAsync("/api/puzzles");

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
        using var response = await client.PostAsync("/api/puzzles/2014/1/solve", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        response.Content.ShouldNotBeNull();
        response.Content!.Headers.ContentType.ShouldNotBeNull();
        response.Content.Headers.ContentType.MediaType.ShouldBe("application/problem+json");
    }

    [Fact]
    public async Task Api_Returns_415_If_Puzzle_Content_Incorrect()
    {
        // Arrange
        using var client = Fixture.CreateClient();

        // Act
        using var content = new StringContent("{}", Encoding.UTF8, "application/json");
        using var response = await client.PostAsync("/api/puzzles/2015/1/solve", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnsupportedMediaType);
        response.Content.ShouldNotBeNull();
        response.Content!.Headers.ContentType.ShouldNotBeNull();
        response.Content.Headers.ContentType.MediaType.ShouldBe("application/problem+json");
    }

    private static string GetPuzzleInput(int year, int day)
    {
        var type = typeof(Puzzle);

        string name = FormattableString.Invariant(
            $"MartinCostello.{type.Assembly.GetName().Name}.Input.Y{year}.Day{day:00}.input.txt");

        using var stream = type.Assembly.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }

#pragma warning disable CA1034
    public sealed class TestCase
#pragma warning restore CA1034
    {
        internal TestCase(int year, int day, params object[] solutions)
        {
            Year = year;
            Day = day;
            SendResource = true;
            ExpectedSolutions = solutions;
        }

        internal TestCase(int year, int day, string[] arguments, params object[] solutions)
        {
            Year = year;
            Day = day;
            Arguments = arguments;
            ExpectedSolutions = solutions;
        }

        internal int Year { get; init; }

        internal int Day { get; init; }

        internal string[]? Arguments { get; init; }

        internal bool SendResource { get; init; }

        internal object[] ExpectedSolutions { get; init; } = default!;

        internal int ExpectedVisualizations { get; init; }

        public override string ToString() => string.Join(", ", ExpectedSolutions);
    }
}
