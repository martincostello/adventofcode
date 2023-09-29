// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode;

public static class CharacterRecognitionTests
{
    [Fact]
    public static void Can_Parse_Characters()
    {
        // Arrange
        string[] letters =
        [
            ".**..***...**..****.****..**..*..*..***...**.*..*.*.....**..***..***..*..*.*...*****.",
            "*..*.*..*.*..*.*....*....*..*.*..*...*.....*.*.*..*....*..*.*..*.*..*.*..*.*...*...*.",
            "*..*.***..*....***..***..*....****...*.....*.**...*....*..*.*..*.*..*.*..*..*.*...*..",
            "****.*..*.*....*....*....*.**.*..*...*.....*.*.*..*....*..*.***..***..*..*...*...*...",
            "*..*.*..*.*..*.*....*....*..*.*..*...*..*..*.*.*..*....*..*.*....*.*..*..*...*..*....",
            "*..*.***...**..****.*.....***.*..*..***..**..*..*.****..**..*....*..*..**....*..****.",
        ];

        // Act
        string actual = CharacterRecognition.Read(string.Join(Environment.NewLine, letters));

        // Assert
        actual.ShouldBe("ABCEFGHIJKLOPRUYZ");
    }
}
