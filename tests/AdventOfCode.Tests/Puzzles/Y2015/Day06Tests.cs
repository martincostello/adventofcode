// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class containing tests for the <see cref="Day06"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day06Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day06Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day06Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData("turn on 0,0 through 999,999", "ON", 0, 0, 1000, 1000)]
    [InlineData("toggle 0,0 through 999,0", "TOGGLE", 0, 0, 1000, 1)]
    [InlineData("turn off 499,499 through 500,500", "OFF", 499, 499, 2, 2)]
    public static void Y2015_Day06_V1InstructionsCanBeParsed(string value, string action, int x, int y, int width, int height)
    {
        var actual = Day06.InstructionV1.Parse(value);

        actual.Action.ShouldBe(action);
        actual.Bounds.X.ShouldBe(x);
        actual.Bounds.Y.ShouldBe(y);
        actual.Bounds.Height.ShouldBe(height);
        actual.Bounds.Width.ShouldBe(width);
    }

    [Theory]
    [InlineData("turn on 0,0 through 999,999", 1, 0, 0, 1000, 1000)]
    [InlineData("toggle 0,0 through 999,0", 2, 0, 0, 1000, 1)]
    [InlineData("turn off 499,499 through 500,500", -1, 499, 499, 2, 2)]
    public static void Y2015_Day06_V2InstructionsCanBeParsed(string value, int delta, int x, int y, int width, int height)
    {
        var actual = Day06.InstructionV2.Parse(value);

        actual.Delta.ShouldBe(delta);
        actual.Bounds.X.ShouldBe(x);
        actual.Bounds.Y.ShouldBe(y);
        actual.Bounds.Height.ShouldBe(height);
        actual.Bounds.Width.ShouldBe(width);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 1)]
    [InlineData(2, 2)]
    [InlineData(100, 100)]
    public static void Y2015_Day06_LightGridExhibitsCorrectBehavior(int width, int height)
    {
        var target = new Day06.LightGrid(width, height);

        var minimum = new Point(0, 0);
        var maximum = new Point(width - 1, height - 1);

        target.Brightness.ShouldBe(0);
        target.Count.ShouldBe(0);
        target[minimum].ShouldBe(0);
        target[maximum].ShouldBe(0);

        target.Toggle(minimum).ShouldBeTrue();
        target[minimum].ShouldBe(1);
        target.Brightness.ShouldBe(1);
        target.Count.ShouldBe(1);

        target.Toggle(maximum).ShouldBeTrue();
        target[maximum].ShouldBe(1);
        target.Brightness.ShouldBe(2);
        target.Count.ShouldBe(2);

        target.TurnOn(minimum);
        target[minimum].ShouldBe(1);
        target.Brightness.ShouldBe(2);
        target.Count.ShouldBe(2);

        target.TurnOff(minimum);
        target[minimum].ShouldBe(0);
        target.Brightness.ShouldBe(1);
        target.Count.ShouldBe(1);

        target.TurnOff(minimum);
        target[minimum].ShouldBe(0);
        target.Brightness.ShouldBe(1);
        target.Count.ShouldBe(1);

        target.TurnOff(maximum);
        target[maximum].ShouldBe(0);
        target.Brightness.ShouldBe(0);
        target.Count.ShouldBe(0);

        target.TurnOn(minimum);
        target[minimum].ShouldBe(1);
        target.Brightness.ShouldBe(1);
        target.Count.ShouldBe(1);

        target.Toggle(minimum).ShouldBeFalse();
        target[minimum].ShouldBe(0);
        target.Brightness.ShouldBe(0);
        target.Count.ShouldBe(0);

        target.Toggle(maximum).ShouldBeTrue();
        target[maximum].ShouldBe(1);
        target.Brightness.ShouldBe(1);
        target.Count.ShouldBe(1);

        var bounds = new Rectangle(0, 0, width, height);

        target.TurnOff(bounds);
        target[minimum].ShouldBe(0);
        target[maximum].ShouldBe(0);
        target.Brightness.ShouldBe(0);
        target.Count.ShouldBe(0);

        target.TurnOff(bounds);
        target[minimum].ShouldBe(0);
        target[maximum].ShouldBe(0);
        target.Brightness.ShouldBe(0);
        target.Count.ShouldBe(0);

        target.TurnOn(bounds);
        target[minimum].ShouldBe(1);
        target[maximum].ShouldBe(1);
        target.Brightness.ShouldBe(width * height);
        target.Count.ShouldBe(width * height);

        target.TurnOn(bounds);
        target[minimum].ShouldBe(1);
        target[maximum].ShouldBe(1);
        target.Brightness.ShouldBe(width * height);
        target.Count.ShouldBe(width * height);

        target.Toggle(bounds);
        target[minimum].ShouldBe(0);
        target[maximum].ShouldBe(0);
        target.Brightness.ShouldBe(0);
        target.Count.ShouldBe(0);

        target.IncrementBrightness(minimum, 1);
        target.IncrementBrightness(minimum, 2);
        target.IncrementBrightness(minimum, 3);
        target.IncrementBrightness(minimum, -4);

        target[minimum].ShouldBe(2);
        target.Brightness.ShouldBe(2);

        target.IncrementBrightness(maximum, 10);

        target[minimum].ShouldBe(2);
        target[maximum].ShouldBe(10);
        target.Brightness.ShouldBe(12);

        target.IncrementBrightness(minimum, -3);

        target[minimum].ShouldBe(0);
        target[maximum].ShouldBe(10);
        target.Brightness.ShouldBe(10);
    }

    [Fact]
    public static void Y2015_Day06_LightGridDrawsCorrectly3By3()
    {
        // Arrange
        var target = new Day06.LightGrid(3, 3);

        target.TurnOn(new Point(0, 0));
        target.TurnOn(new Point(0, 2));
        target.TurnOn(new Point(1, 1));
        target.TurnOn(new Point(2, 0));
        target.TurnOn(new Point(2, 2));

        string expected = string.Format(CultureInfo.InvariantCulture, "101{0}010{0}101{0}", Environment.NewLine);

        // Act
        string actual = target.ToString();

        // Assert
        actual.ShouldBe(expected);

        // Arrange
        target.TurnOff(new Rectangle(0, 0, 3, 3));
        target.TurnOn(new Rectangle(0, 0, 2, 2));

        expected = string.Format(CultureInfo.InvariantCulture, "110{0}110{0}000{0}", Environment.NewLine);

        // Act
        actual = target.ToString();

        // Assert
        actual.ShouldBe(expected);

        // Arrange
        target.TurnOff(new Rectangle(0, 0, 3, 3));
        target.TurnOn(new Rectangle(1, 1, 2, 2));

        expected = string.Format(CultureInfo.InvariantCulture, "000{0}011{0}011{0}", Environment.NewLine);

        // Act
        actual = target.ToString();

        // Assert
        actual.ShouldBe(expected);

        // Arrange
        target.IncrementBrightness(new Rectangle(1, 1, 2, 2), 2);

        expected = string.Format(CultureInfo.InvariantCulture, "000{0}033{0}033{0}", Environment.NewLine);

        // Act
        actual = target.ToString();

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2015_Day06_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day06>();

        // Assert
        puzzle.LightsIlluminated.ShouldBe(543903);
        puzzle.TotalBrightness.ShouldBe(14687245);
    }
}
