// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day08Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    public static IEnumerable<object[]> WalkNetworkCases
    {
        get
        {
            yield return new object[]
            {
                new[]
                {
                    "RL",
                    string.Empty,
                    "AAA = (BBB, CCC)",
                    "BBB = (DDD, EEE)",
                    "CCC = (ZZZ, GGG)",
                    "DDD = (DDD, DDD)",
                    "EEE = (EEE, EEE)",
                    "GGG = (GGG, GGG)",
                    "ZZZ = (ZZZ, ZZZ)",
                },
                2,
            };

            yield return new object[]
            {
                new[]
                {
                    "LLR",
                    string.Empty,
                    "AAA = (BBB, BBB)",
                    "BBB = (AAA, ZZZ)",
                    "ZZZ = (ZZZ, ZZZ)",
                },
                6,
            };
        }
    }

    [Theory]
    [MemberData(nameof(WalkNetworkCases))]
    public void Y2023_Day08_WalkNetwork_Returns_Correct_Value(string[] network, int expected)
    {
        // Act
        int actual = Day08.WalkNetwork(network, CancellationToken.None);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2023_Day08_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day08>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.Steps.ShouldBe(17621);
    }
}
