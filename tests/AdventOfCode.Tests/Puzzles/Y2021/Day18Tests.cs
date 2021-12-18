// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day18Tests : PuzzleTest
{
    public Day18Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData("[1,2]")]
    [InlineData("[[1,2],3]")]
    [InlineData("[9,[8,7]]")]
    [InlineData("[[1,9],[8,5]]")]
    [InlineData("[[[[1,2],[3,4]],[[5,6],[7,8]]],9]")]
    [InlineData("[[[9,[3,8]],[[0,9],6]],[[[3,7],[4,9]],3]]")]
    [InlineData("[[[[1,3],[5,3]],[[1,3],[8,7]]],[[[4,9],[6,9]],[[8,2],[7,3]]]]")]
    public void Y2021_Day18_Parse_Returns_Correct_Value(string number)
    {
        // Act
        string actual = Day18.Parse(number);

        // Assert
        actual.ShouldBe(number);
    }

    [Theory]
    [InlineData("[[[[[9,8],1],2],3],4]", "[[[[0,9],2],3],4]")]
    [InlineData("[7,[6,[5,[4,[3,2]]]]]", "[7,[6,[5,[7,0]]]]")]
    [InlineData("[[6,[5,[4,[3,2]]]],1]", "[[6,[5,[7,0]]],3]")]
    [InlineData("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]")]
    [InlineData("[[[[[4,3],4],4],[7,[[8,4],9]]],[1,1]]", "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]")]
    public void Y2021_Day18_Reduce_Returns_Correct_Value(string number, string expected)
    {
        // Act
        string actual = Day18.Reduce(number);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData(new[] { "[1,1]", "[2,2]", "[3,3]", "[4,4]" }, "[[[[1,1],[2,2]],[3,3]],[4,4]]")]
    [InlineData(new[] { "[1,1]", "[2,2]", "[3,3]", "[4,4]", "[5,5]" }, "[[[[3,0],[5,3]],[4,4]],[5,5]]")]
    [InlineData(new[] { "[1,1]", "[2,2]", "[3,3]", "[4,4]", "[5,5]", "[6,6]" }, "[[[[5,0],[7,4]],[5,5]],[6,6]]")]
    public void Y2021_Day18_SumValues_Returns_Correct_Value(string[] numbers, string expected)
    {
        // Act
        string actual = Day18.SumValues(numbers);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData("[[1,2],[[3,4],5]]", 143)]
    [InlineData("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]", 1384)]
    [InlineData("[[[[1,1],[2,2]],[3,3]],[4,4]]", 445)]
    [InlineData("[[[[3,0],[5,3]],[4,4]],[5,5]]", 791)]
    [InlineData("[[[[5,0],[7,4]],[5,5]],[6,6]]", 1137)]
    [InlineData("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]", 3488)]
    public void Y2021_Day18_Magnitude_Returns_Correct_Value(string number, int expected)
    {
        // Act
        int actual = Day18.Magnitude(number);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public void Y2021_Day18_SumValues_Returns_Correct_Reduced_Value()
    {
        // Arrange
        string x = "[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]";
        string y = "[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]";

        // Act
        string actual = Day18.SumValues(x, y);

        // Assert
        actual.ShouldBe("[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]");
    }

    [Fact]
    public void Y2021_Day18_Sum_Returns_Correct_Value()
    {
        // Arrange
        string[] numbers =
        {
            "[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]",
            "[[[5,[2,8]],4],[5,[[9,9],0]]]",
            "[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]",
            "[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]",
            "[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]",
            "[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]",
            "[[[[5,4],[7,7]],8],[[8,3],8]]",
            "[[9,3],[[9,9],[6,[4,9]]]]",
            "[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]",
            "[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]",
        };

        // Act
        int actual = Day18.Sum(numbers);

        // Assert
        actual.ShouldBe(4140);
    }

    [Fact]
    public async Task Y2021_Day18_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day18>();

        // Assert
        puzzle.MagnitudeOfSum.ShouldBe(-1); // x < 5028
    }
}
