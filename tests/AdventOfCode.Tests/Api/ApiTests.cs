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
/// <param name="fixture">The fixture to use.</param>
/// <param name="outputHelper">The test output helper to use.</param>
public class ApiTests(HttpServerFixture fixture, ITestOutputHelper outputHelper)
    : IntegrationTest(fixture, outputHelper)
{
    [Theory]
    [PuzzleData(2015, 01, 232, 1783)]
    [PuzzleData(2015, 02, 1598415, 3812909)]
    [PuzzleData(2015, 03, 2565, 2639)]
    [PuzzleData(2015, 04, ["iwrupvqb"], 346386, 9958218)]
    [PuzzleData(2015, 05, 236, 51)]
    [PuzzleData(2015, 06, 543903, 14687245)]
    [PuzzleData(2015, 07, 3176, 14710)]
    [PuzzleData(2015, 08, 1342, 2074)]
    [PuzzleData(2015, 09, 207, 804)]
    [PuzzleData(2015, 10, ["1321131112"], 492982, 6989950)]
    [PuzzleData(2015, 11, ["cqjxjnds"], "cqjxxyzz", "cqkaabcc")]
    [PuzzleData(2015, 12, 191164, 87842)]
    [PuzzleData(2015, 13, 618, 601)]
    [PuzzleData(2015, 14, ["2503"], 2655, 1059, SendResource = true)]
    [PuzzleData(2015, 15, 222870, 117936)]
    [PuzzleData(2015, 16, 373, 260)]
    [PuzzleData(2015, 17, ["150"], 1304, 18, SendResource = true)]
    [PuzzleData(2015, 18, 814, 924)]
    [PuzzleData(2015, 19, 576, 207, Skip = "Too slow.")]
    [PuzzleData(2015, 20, ["34000000"], 786240, 831600)]
    [PuzzleData(2015, 21, 148, 78)]
    [PuzzleData(2015, 22, ["easy"], 953)]
    [PuzzleData(2015, 22, ["hard"], 1289)]
    [PuzzleData(2015, 23, 1u, 170u)]
    [PuzzleData(2015, 23, ["1"], 1u, 247u, SendResource = true)]
    [PuzzleData(2015, 24, 11266889531, Skip = "Too slow.")]
    [PuzzleData(2015, 24, ["4"], 77387711, SendResource = true, Skip = "Too slow.")]
    [PuzzleData(2015, 25, ["2947", "3029"], 19980801u)]
    [PuzzleData(2016, 01, 287, 133)]
    [PuzzleData(2016, 02, "14894", "26B96")]
    [PuzzleData(2016, 03, 983, 1836)]
    [PuzzleData(2016, 04, 137896, 501)]
    [PuzzleData(2016, 05, ["wtnhxymk"], "2414bc77", "437e60fc")]
    [PuzzleData(2016, 06, "qzedlxso", "ucmifjae")]
    [PuzzleData(2016, 07, 118, 260)]
    [PuzzleData(2016, 08, 121, "RURUCEOEIL", ExpectedVisualizations = 1)]
    [PuzzleData(2016, 09, 98135, 10964557606)]
    [PuzzleData(2016, 10, 141, 1209)]
    [PuzzleData(2016, 11, 47, 71, Skip = "Not implemented.")]
    [PuzzleData(2016, 12, 318020, 9227674)]
    [PuzzleData(2016, 13, ["1362"], 82, 138)]
    [PuzzleData(2016, 14, ["ihaygndm"], 15035, 19968, Skip = "Too slow.")]
    [PuzzleData(2016, 15, 16824, 3543984)]
    [PuzzleData(2016, 16, ["10010000000110000", "272"], "10010110010011110")]
    [PuzzleData(2016, 16, ["10010000000110000", "35651584"], "01101011101100011")]
    [PuzzleData(2016, 17, ["pvhmgsws"], "DRRDRLDURD", 618)]
    [PuzzleData(2016, 18, 1987, 19984714, ExpectedVisualizations = 1)]
    [PuzzleData(2016, 19, ["5"], 3, 2)]
    [PuzzleData(2016, 19, ["3014387"], 1834471, 1420064)]
    [PuzzleData(2016, 20, 22887907u, 109u)]
    [PuzzleData(2016, 21, ["abcdefgh"], "gcedfahb", SendResource = true)]
    [PuzzleData(2016, 21, ["fbgdceah", "true"], "hegbdcfa", SendResource = true)]
    [PuzzleData(2016, 22, 985, 179)]
    [PuzzleData(2016, 23, 14346, 479010906, Skip = "Too slow.")]
    [PuzzleData(2016, 24, 502, 724)]
    [PuzzleData(2016, 25, 198)]
    [PuzzleData(2017, 01, 1034, 1356)]
    [PuzzleData(2017, 02, 36174, 244)]
    [PuzzleData(2017, 03, ["312051"], 430, 312453)]
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
    [PuzzleData(2017, 14, ["hwlqcszp"], 8304)]
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
    [PuzzleData(2019, 04, ["138241-674034"], 1890, 1277)]
    [PuzzleData(2019, 05, 6745903, 9168267)]
    [PuzzleData(2019, 07, 77500, 22476942)]
    [PuzzleData(2019, 08, 2080, "AURCY", ExpectedVisualizations = 1)]
    [PuzzleData(2019, 09, 2494485073, 44997)]
    [PuzzleData(2019, 13, 315)]
    [PuzzleData(2020, 01, 63616, 67877784)]
    [PuzzleData(2020, 02, 542, 360)]
    [PuzzleData(2020, 03, 216, 6708199680L)]
    [PuzzleData(2020, 04, 226, 160)]
    [PuzzleData(2020, 05, 878, 504)]
    [PuzzleData(2020, 06, 6542, 3299)]
    [PuzzleData(2020, 07, ["shiny gold"], 179, 18925, SendResource = true)]
    [PuzzleData(2020, 08, 1137, 1125)]
    [PuzzleData(2020, 09, 22406676, 2942387)]
    [PuzzleData(2020, 10, 2775, 518344341716992L)]
    [PuzzleData(2020, 11, 2108, 1897, ExpectedVisualizations = 2)]
    [PuzzleData(2020, 12, 439, 12385)]
    [PuzzleData(2020, 13, 2935, 836024966345345L)]
    [PuzzleData(2020, 14, 9967721333886L, 4355897790573)]
    [PuzzleData(2020, 15, ["0,5,4,1,10,14,7"], 203, 9007186)]
    [PuzzleData(2020, 16, 21071, 3429967441937L)]
    [PuzzleData(2020, 17, 388, 2280, ExpectedVisualizations = 2)]
    [PuzzleData(2020, 18, 2743012121210L, 65658760783597L)]
    [PuzzleData(2020, 19, 195, 309)]
    [PuzzleData(2020, 20, 17712468069479L, 2173, ExpectedVisualizations = 1)]
    [PuzzleData(2020, 21, 2098, "ppdplc,gkcplx,ktlh,msfmt,dqsbql,mvqkdj,ggsz,hbhsx")]
    [PuzzleData(2020, 22, 33694, 31835)]
    [PuzzleData(2020, 23, ["583976241"], "24987653", 442938711161)]
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
    [PuzzleData(2021, 10, 323613, 3103006161L)]
    [PuzzleData(2021, 11, 1739, 324)]
    [PuzzleData(2021, 12, 4413, 118803)]
    [PuzzleData(2021, 13, 647, "HEJHJRCJ", ExpectedVisualizations = 1)]
    [PuzzleData(2021, 14, 3587, 3906445077999)]
    [PuzzleData(2021, 15, 487, 2821)]
    [PuzzleData(2021, 16, 974, 180616437720)]
    [PuzzleData(2021, 17, 8646, 5945)]
    [PuzzleData(2021, 18, 4323, 4749)]
    [PuzzleData(2021, 19, 308, 12124)]
    [PuzzleData(2021, 20, 5437, 19340)]
    [PuzzleData(2021, 21, 713328, 92399285032143)]
    [PuzzleData(2021, 22, 537042, 1304385553084863)]
    [PuzzleData(2021, 23, 12240, 44618)]
    [PuzzleData(2021, 24, 92928914999991, 91811211611981)]
    [PuzzleData(2021, 25, 532)]
    [PuzzleData(2022, 01, 68775, 202585)]
    [PuzzleData(2022, 02, 13675, 14184)]
    [PuzzleData(2022, 03, 7568, 2780)]
    [PuzzleData(2022, 04, 526, 886)]
    [PuzzleData(2022, 05, "TGWSMRBPN", "TZLTLWRNF")]
    [PuzzleData(2022, 06, 1850, 2823)]
    [PuzzleData(2022, 07, 1642503, 6999588)]
    [PuzzleData(2022, 08, 1763, 671160)]
    [PuzzleData(2022, 09, 5683, 2372)]
    [PuzzleData(2022, 10, 12740, "RBPARAGF", ExpectedVisualizations = 1)]
    [PuzzleData(2022, 11, 56120, 24389045529)]
    [PuzzleData(2022, 12, 408, 399)]
    [PuzzleData(2022, 13, 5252, 20592)]
    [PuzzleData(2022, 14, 832, 27601, ExpectedVisualizations = 2)]
    [PuzzleData(2022, 15, -1, Skip = "Not implemented.")]
    [PuzzleData(2022, 16, -1, Skip = "Not implemented.")]
    [PuzzleData(2022, 17, 3135, Skip = "Not implemented.")]
    [PuzzleData(2022, 18, 4340, 2468)]
    [PuzzleData(2022, 19, -1, Skip = "Not implemented.")]
    [PuzzleData(2022, 20, -1, Skip = "Not implemented.")]
    [PuzzleData(2022, 21, 10037517593724, 3272260914328)]
    [PuzzleData(2022, 22, -1, Skip = "Not implemented.")]
    [PuzzleData(2022, 23, -1, Skip = "Not implemented.")]
    [PuzzleData(2022, 24, -1, Skip = "Not implemented.")]
    [PuzzleData(2022, 25, -1, Skip = "Not implemented.")]
    [PuzzleData(2023, 01, 54697, 54885)]
    [PuzzleData(2023, 02, 2156, 66909)]
    [PuzzleData(2023, 03, 535351, 87287096)]
    [PuzzleData(2023, 04, 21138, 7185540)]
    [PuzzleData(2023, 05, 535088217, 51399228)]
    [PuzzleData(2023, 06, -1, Skip = "Not implemented.")]
    [PuzzleData(2023, 07, -1, Skip = "Not implemented.")]
    [PuzzleData(2023, 08, -1, Skip = "Not implemented.")]
    [PuzzleData(2023, 09, -1, Skip = "Not implemented.")]
    [PuzzleData(2023, 10, -1, Skip = "Not implemented.")]
    [PuzzleData(2023, 11, -1, Skip = "Not implemented.")]
    [PuzzleData(2023, 12, -1, Skip = "Not implemented.")]
    [PuzzleData(2023, 13, -1, Skip = "Not implemented.")]
    [PuzzleData(2023, 14, -1, Skip = "Not implemented.")]
    [PuzzleData(2023, 15, -1, Skip = "Not implemented.")]
    [PuzzleData(2023, 16, -1, Skip = "Not implemented.")]
    [PuzzleData(2023, 17, -1, Skip = "Not implemented.")]
    [PuzzleData(2023, 18, -1, Skip = "Not implemented.")]
    [PuzzleData(2023, 19, -1, Skip = "Not implemented.")]
    [PuzzleData(2023, 20, -1, Skip = "Not implemented.")]
    [PuzzleData(2023, 21, -1, Skip = "Not implemented.")]
    [PuzzleData(2023, 22, -1, Skip = "Not implemented.")]
    [PuzzleData(2023, 23, -1, Skip = "Not implemented.")]
    [PuzzleData(2023, 24, -1, Skip = "Not implemented.")]
    [PuzzleData(2023, 25, -1, Skip = "Not implemented.")]
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
            content.Add(new StringContent(GetPuzzleInput(year, day)), "resource", "input.txt");
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
            puzzle.TryGetProperty("name", out _).ShouldBeTrue();
            puzzle.TryGetProperty("requiresData", out _).ShouldBeTrue();
            puzzle.TryGetProperty("year", out _).ShouldBeTrue();
        }
    }

    [Fact]
    public async Task Api_Returns_404_If_Puzzle_Not_Found()
    {
        // Arrange
        using var client = Fixture.CreateClient();

        using var content = new MultipartFormDataContent()
        {
#pragma warning disable CA2000
            { new StringContent(GetPuzzleInput(2015, 1)), "resource", "input.txt" },
#pragma warning restore CA2000
        };

        // Act
        using var response = await client.PostAsync("/api/puzzles/2014/1/solve", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        response.Content.ShouldNotBeNull();
        response.Content!.Headers.ContentType.ShouldNotBeNull();
        response.Content.Headers.ContentType.MediaType.ShouldBe("application/problem+json");
    }

    [Fact]
    public async Task Api_Returns_400_If_Puzzle_Content_Incorrect()
    {
        // Arrange
        using var client = Fixture.CreateClient();
        using var content = new StringContent("{}", Encoding.UTF8, "application/json");

        // Act
        using var response = await client.PostAsync("/api/puzzles/2015/1/solve", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        response.Content.ShouldNotBeNull();
        response.Content!.Headers.ContentLength.ShouldBe(0);
    }

    private static string GetPuzzleInput(int year, int day)
    {
        using var stream = InputProvider.Get(year, day)!;
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
            => new object[1][] { [Year, Day, this] };

        public override string ToString() => string.Join(", ", ExpectedSolutions);
    }
}
