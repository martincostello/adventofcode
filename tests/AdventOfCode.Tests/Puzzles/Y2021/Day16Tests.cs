// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day16Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("D2FE28", 6)]
    [InlineData("38006F45291200", 9)]
    [InlineData("EE00D40C823060", 14)]
    [InlineData("8A004A801A8002F478", 16)]
    [InlineData("620080001611562C8802118E34", 12)]
    [InlineData("C0015000016115A2E0802F182340", 23)]
    [InlineData("A0016C880162017C3686B18A3D4780", 31)]
    public void Y2021_Day16_Decode_Returns_Correct_Sum(string transmission, long expected)
    {
        // Act
        (long actual, _) = Day16.Decode(transmission);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData("C200B40A82", 3)]
    [InlineData("04005AC33890", 54)]
    [InlineData("880086C3E88112", 7)]
    [InlineData("CE00C43D881120", 9)]
    [InlineData("D8005AC2A8F0", 1)]
    [InlineData("F600BC2D8F", 0)]
    [InlineData("9C005AC2F8F0", 0)]
    [InlineData("9C0141080250320F1802104A08", 1)]
    public void Y2021_Day16_Decode_Returns_Correct_Value(string transmission, long expected)
    {
        // Act
        (_, long actual) = Day16.Decode(transmission);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2021_Day16_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day16>();

        // Assert
        puzzle.VersionNumberSum.ShouldBe(974);
        puzzle.Value.ShouldBe(180616437720);
    }
}
