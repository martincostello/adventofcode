// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Net;
using System.Reflection;
using System.Text.Json;
using Xunit.Sdk;

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

    [Theory]
    [PuzzleData(2015, 01, 232, 1783)]
    [PuzzleData(2015, 02, 1598415, 3812909)]
    [PuzzleData(2015, 03, 2565, 2639)]
    [PuzzleData(2015, 04, new[] { "iwrupvqb", "5" }, 346386)]
    [PuzzleData(2015, 04, new[] { "iwrupvqb", "6" }, 9958218)]
    [PuzzleData(2015, 05, new[] { "1" }, 236, SendResource = true)]
    [PuzzleData(2015, 05, new[] { "2" }, 51, SendResource = true)]
    [PuzzleData(2015, 06, new[] { "1" }, 543903, SendResource = true)]
    [PuzzleData(2015, 06, new[] { "2" }, 14687245, SendResource = true)]
    [PuzzleData(2015, 07, 3176, 14710)]
    [PuzzleData(2015, 08, 1342, 2074)]
    [PuzzleData(2015, 09, 207)]
    [PuzzleData(2015, 09, 804, Arguments = new[] { "true" })]
    [PuzzleData(2015, 10, new[] { "1321131112", "40" }, 492982)]
    [PuzzleData(2015, 10, new[] { "1321131112", "50" }, 6989950)]
    [PuzzleData(2015, 11, new[] { "cqjxjnds" }, "cqjxxyzz")]
    [PuzzleData(2015, 11, new[] { "cqjxxyzz" }, "cqkaabcc")]
    [PuzzleData(2015, 12, 191164)]
    [PuzzleData(2015, 12, new[] { "red" }, 87842, SendResource = true)]
    [PuzzleData(2015, 13, 618, 601)]
    [PuzzleData(2015, 14, new[] { "2503" }, 2655, 1059, SendResource = true)]
    [PuzzleData(2015, 15, 222870, 117936)]
    [PuzzleData(2015, 16, 373, 260)]
    [PuzzleData(2015, 17, new[] { "150" }, 1304, 18, SendResource = true)]
    [PuzzleData(2015, 18, new[] { "100", "false" }, 814, SendResource = true)]
    [PuzzleData(2015, 18, new[] { "100", "true" }, 924, SendResource = true)]
    [PuzzleData(2015, 19, new[] { "calibrate" }, 576, Skip = "Too slow.")]
    [PuzzleData(2015, 19, new[] { "fabricate" }, 207, Skip = "Too slow.")]
    [PuzzleData(2015, 20, new[] { "34000000" }, 786240)]
    [PuzzleData(2015, 20, new[] { "34000000", "50" }, 831600)]
    [PuzzleData(2015, 21, 148, 78, SendResource = false)]
    [PuzzleData(2015, 22, new[] { "easy" }, 953)]
    [PuzzleData(2015, 22, new[] { "hard" }, 1289)]
    [PuzzleData(2015, 23, 1u, 170u, SendResource = true)]
    [PuzzleData(2015, 23, new[] { "1" }, 1u, 247u, SendResource = true)]
    [PuzzleData(2015, 24, 11266889531, Skip = "Too slow.")]
    [PuzzleData(2015, 24, new[] { "4" }, 77387711, SendResource = true, Skip = "Too slow.")]
    [PuzzleData(2015, 25, new[] { "2947", "3029" }, 19980801u)]
    [PuzzleData(2016, 01, 287, 133)]
    [PuzzleData(2016, 02, "14894", "26B96")]
    [PuzzleData(2016, 03, 983, 1836)]
    [PuzzleData(2016, 04, 137896, 501)]
    [PuzzleData(2016, 05, new[] { "wtnhxymk" }, "2414bc77", "437e60fc")]
    [PuzzleData(2016, 06, "qzedlxso", "ucmifjae")]
    [PuzzleData(2016, 07, 118, 260)]
    [PuzzleData(2016, 08, 121, ExpectedVisualizations = 1)]
    [PuzzleData(2016, 09, 98135, 10964557606)]
    [PuzzleData(2016, 10, 141, 1209)]
    [PuzzleData(2016, 12, 318020, 9227674)]
    [PuzzleData(2016, 13, new[] { "1362" }, 82, 138)]
    [PuzzleData(2016, 14, new[] { "ihaygndm" }, 15035, 19968, Skip = "Too slow.")]
    [PuzzleData(2016, 15, 16824, 3543984)]
    [PuzzleData(2016, 16, new[] { "10010000000110000", "272" }, "10010110010011110")]
    [PuzzleData(2016, 16, new[] { "10010000000110000", "35651584" }, "01101011101100011")]
    [PuzzleData(2016, 17, new[] { "pvhmgsws" }, "DRRDRLDURD", 618)]
    [PuzzleData(2016, 18, new[] { "40" }, 1987, ExpectedVisualizations = 1, SendResource = true)]
    [PuzzleData(2016, 18, new[] { "400000" }, 19984714, ExpectedVisualizations = 1, SendResource = true)]
    [PuzzleData(2016, 19, new[] { "5", "1" }, 3)]
    [PuzzleData(2016, 19, new[] { "5", "2" }, 2)]
    [PuzzleData(2016, 19, new[] { "3014387", "1" }, 1834471)]
    [PuzzleData(2016, 19, new[] { "3014387", "2" }, 1420064)]
    [PuzzleData(2016, 20, 22887907u, 109u)]
    [PuzzleData(2016, 21, new[] { "abcdefgh" }, "gcedfahb", SendResource = true)]
    [PuzzleData(2016, 21, new[] { "fbgdceah", "true" }, "hegbdcfa", SendResource = true)]
    [PuzzleData(2016, 22, 985, 179)]
    [PuzzleData(2016, 23, 14346, 479010906, Skip = "Too slow.")]
    [PuzzleData(2016, 24, 502, 724)]
    [PuzzleData(2016, 25, 198)]
    [PuzzleData(2017, 01, 1034, 1356)]
    [PuzzleData(2017, 02, 36174, 244)]
    [PuzzleData(2017, 03, new[] { "312051" }, 430, 312453)]
    [PuzzleData(2017, 04, 383, 265)]
    [PuzzleData(2017, 05, 373543, 27502966)]
    [PuzzleData(2017, 06, 3156, 1610)]
    [PuzzleData(2017, 07, "fbgguv", 1864)]
    [PuzzleData(2017, 08, 7296, 8186)]
    [PuzzleData(2017, 09, 11898, 5601)]
    [PuzzleData(2017, 10, 11413, "7adfd64c2a03a4968cf708d1b7fd418d")]
    [PuzzleData(2017, 11, 796, 1585)]
    [PuzzleData(2017, 12, 113, 202)]
    [PuzzleData(2017, 13, 1612, 3907994)]
    [PuzzleData(2017, 14, new[] { "hwlqcszp" }, 8304)]
    [PuzzleData(2017, 15, 594, 328)]
    [PuzzleData(2018, 01, 543, 621)]
    [PuzzleData(2018, 02, 5880, "tiwcdpbseqhxryfmgkvjujvza")]
    [PuzzleData(2018, 03, 100595, "415")]
    [PuzzleData(2018, 04, 4716, 117061)]
    [PuzzleData(2018, 05, 10638, 4944)]
    [PuzzleData(2018, 06, 5626, 46554)]
    [PuzzleData(2018, 07, "BGJCNLQUYIFMOEZTADKSPVXRHW", 1017)]
    [PuzzleData(2018, 08, 45210, 22793)]
    [PuzzleData(2019, 01, 3226407, 4836738)]
    [PuzzleData(2019, 02, 9581917)]
    [PuzzleData(2019, 03, 855, 11238)]
    [PuzzleData(2019, 04, new[] { "138241-674034" }, 1890, 1277)]
    [PuzzleData(2019, 05, new[] { "1" }, 6745903, SendResource = true)]
    [PuzzleData(2019, 05, new[] { "5" }, 9168267, SendResource = true)]
    [PuzzleData(2019, 07, 77500, 22476942)]
    [PuzzleData(2019, 08, 2080, ExpectedVisualizations = 1)]
    [PuzzleData(2019, 09, new[] { "1" }, 2494485073, SendResource = true)]
    [PuzzleData(2019, 09, new[] { "2" }, 44997, SendResource = true)]
    [PuzzleData(2019, 13, 315)]
    [PuzzleData(2020, 01, 63616, 67877784)]
    [PuzzleData(2020, 02, 542, 360)]
    [PuzzleData(2020, 03, 216, 6708199680L)]
    [PuzzleData(2020, 04, 226, 160)]
    [PuzzleData(2020, 05, 878, 504)]
    [PuzzleData(2020, 06, 6542, 3299)]
    [PuzzleData(2020, 07, new[] { "shiny gold" }, 179, 18925, SendResource = true)]
    [PuzzleData(2020, 08, 1137, 1125)]
    [PuzzleData(2020, 09, 22406676, 2942387)]
    [PuzzleData(2020, 10, 2775, 518344341716992L)]
    [PuzzleData(2020, 11, 2108, 1897, ExpectedVisualizations = 2)]
    [PuzzleData(2020, 12, 439, 12385)]
    [PuzzleData(2020, 13, 2935, 836024966345345L)]
    [PuzzleData(2020, 14, 9967721333886L, 4355897790573)]
    [PuzzleData(2020, 15, new[] { "0,5,4,1,10,14,7" }, 203, 9007186)]
    [PuzzleData(2020, 16, 21071, 3429967441937L)]
    [PuzzleData(2020, 17, 388, 2280, ExpectedVisualizations = 2)]
    [PuzzleData(2020, 18, 2743012121210L, 65658760783597L)]
    [PuzzleData(2020, 19, 195, 309)]
    [PuzzleData(2020, 20, 17712468069479L, 2173, ExpectedVisualizations = 1)]
    [PuzzleData(2020, 21, 2098, "ppdplc,gkcplx,ktlh,msfmt,dqsbql,mvqkdj,ggsz,hbhsx")]
    [PuzzleData(2020, 22, 33694, 31835, SendResource = true)]
    [PuzzleData(2020, 23, new[] { "583976241" }, "24987653", 442938711161)]
    [PuzzleData(2020, 24, 289, 3551)]
    [PuzzleData(2020, 25, 296776)]
    [PuzzleData(2021, 01, 1532, 1571)]
    [PuzzleData(2021, 02, 2150351, 1842742223)]
    [PuzzleData(2021, 03, 3633500, 4550283)]
    [PuzzleData(2021, 04, 41668, 10478)]
    [PuzzleData(2021, 05, 5690, 17741)]
    [PuzzleData(2021, 06, 377263L, 1695929023803L)]
    [PuzzleData(2021, 07, 335271, 95851339)]
    [PuzzleData(2021, 08, 294, 973292)]
    [PuzzleData(2021, 09, 607, 900864)]
    public async Task Can_Solve_Puzzle(int year, int day, PuzzleDataAttribute testCase)
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

#pragma warning disable CA1019
#pragma warning disable CA1034
    public sealed class PuzzleDataAttribute : DataAttribute
#pragma warning restore CA1034
    {
        public PuzzleDataAttribute(int year, int day, params object[] solutions)
        {
            Year = year;
            Day = day;
            SendResource = true;
            ExpectedSolutions = solutions;
        }

        public PuzzleDataAttribute(int year, int day, string[] arguments, params object[] solutions)
        {
            Year = year;
            Day = day;
            Arguments = arguments;
            ExpectedSolutions = solutions;
        }

        public string[]? Arguments { get; init; }

        public int ExpectedVisualizations { get; init; } = default!;

        public bool SendResource { get; init; }

        internal int Year { get; }

        internal int Day { get; }

        internal object[] ExpectedSolutions { get; }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
            => new object[1][] { new object[] { Year, Day, this } };

        public override string ToString() => string.Join(", ", ExpectedSolutions);
    }
}
