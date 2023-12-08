// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day08Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    public static IEnumerable<object[]> TestCases
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
                false,
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
                false,
                6,
            };

            yield return new object[]
            {
                new[]
                {
                    "LR",
                    string.Empty,
                    "11A = (11B, XXX)",
                    "11B = (XXX, 11Z)",
                    "11Z = (11B, XXX)",
                    "22A = (22B, XXX)",
                    "22B = (22C, 22C)",
                    "22C = (22Z, 22Z)",
                    "22Z = (22B, 22B)",
                    "XXX = (XXX, XXX)",
                },
                true,
                6,
            };
        }
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Y2023_Day08_WalkNetwork_Returns_Correct_Value(string[] network, bool asGhost, long expected)
    {
        // Act
        long actual = Day08.WalkNetwork(network, asGhost, CancellationToken.None);

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
        puzzle.StepsAsGhost.ShouldBe(20685524831999);
    }
}
