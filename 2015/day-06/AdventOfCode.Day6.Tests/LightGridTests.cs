// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LightGridTests.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   LightGridTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day6
{
    using System;
    using System.Drawing;
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="LightGrid"/> class. This class cannot be inherited.
    /// </summary>
    public static class LightGridTests
    {
        [Fact]
        public static void LightGridValidatesBounds()
        {
            Assert.Throws<ArgumentOutOfRangeException>("width", () => new LightGrid(0, 1));
            Assert.Throws<ArgumentOutOfRangeException>("height", () => new LightGrid(1, 0));

            int width = 1;
            int height = 1;

            var target = new LightGrid(width, height);

            Assert.Throws<ArgumentOutOfRangeException>("position", () => target[new Point(-1, 0)]);
            Assert.Throws<ArgumentOutOfRangeException>("position", () => target[new Point(0, -1)]);
            Assert.Throws<ArgumentOutOfRangeException>("position", () => target[new Point(0, 1)]);
            Assert.Throws<ArgumentOutOfRangeException>("position", () => target[new Point(1, 0)]);

            Assert.Throws<ArgumentOutOfRangeException>("position", () => target.Toggle(new Point(-1, 0)));
            Assert.Throws<ArgumentOutOfRangeException>("position", () => target.Toggle(new Point(0, -1)));
            Assert.Throws<ArgumentOutOfRangeException>("position", () => target.Toggle(new Point(0, 1)));
            Assert.Throws<ArgumentOutOfRangeException>("position", () => target.Toggle(new Point(1, 0)));

            Assert.Throws<ArgumentOutOfRangeException>("position", () => target.TurnOff(new Point(-1, 0)));
            Assert.Throws<ArgumentOutOfRangeException>("position", () => target.TurnOff(new Point(0, -1)));
            Assert.Throws<ArgumentOutOfRangeException>("position", () => target.TurnOff(new Point(0, 1)));
            Assert.Throws<ArgumentOutOfRangeException>("position", () => target.TurnOff(new Point(1, 0)));

            Assert.Throws<ArgumentOutOfRangeException>("position", () => target.TurnOn(new Point(-1, 0)));
            Assert.Throws<ArgumentOutOfRangeException>("position", () => target.TurnOn(new Point(0, -1)));
            Assert.Throws<ArgumentOutOfRangeException>("position", () => target.TurnOn(new Point(0, 1)));
            Assert.Throws<ArgumentOutOfRangeException>("position", () => target.TurnOn(new Point(1, 0)));
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        [InlineData(100, 100)]
        public static void LightGridExhibitsCorrectBehavior(int width, int height)
        {
            var target = new LightGrid(width, height);

            Point minimum = new Point(0, 0);
            Point maximum = new Point(width - 1, height - 1);

            Assert.Equal(0, target.Brightness);
            Assert.Equal(0, target.Count);
            Assert.Equal(0, target[minimum]);
            Assert.Equal(0, target[maximum]);

            Assert.True(target.Toggle(minimum));
            Assert.Equal(1, target[minimum]);
            Assert.Equal(1, target.Brightness);
            Assert.Equal(1, target.Count);

            Assert.True(target.Toggle(maximum));
            Assert.Equal(1, target[maximum]);
            Assert.Equal(2, target.Brightness);
            Assert.Equal(2, target.Count);

            target.TurnOn(minimum);
            Assert.Equal(1, target[minimum]);
            Assert.Equal(2, target.Brightness);
            Assert.Equal(2, target.Count);

            target.TurnOff(minimum);
            Assert.Equal(0, target[minimum]);
            Assert.Equal(1, target.Brightness);
            Assert.Equal(1, target.Count);

            target.TurnOff(minimum);
            Assert.Equal(0, target[minimum]);
            Assert.Equal(1, target.Brightness);
            Assert.Equal(1, target.Count);

            target.TurnOff(maximum);
            Assert.Equal(0, target[maximum]);
            Assert.Equal(0, target.Brightness);
            Assert.Equal(0, target.Count);

            target.TurnOn(minimum);
            Assert.Equal(1, target[minimum]);
            Assert.Equal(1, target.Brightness);
            Assert.Equal(1, target.Count);

            Assert.False(target.Toggle(minimum));
            Assert.Equal(0, target[minimum]);
            Assert.Equal(0, target.Brightness);
            Assert.Equal(0, target.Count);

            Assert.True(target.Toggle(maximum));
            Assert.Equal(1, target[maximum]);
            Assert.Equal(1, target.Brightness);
            Assert.Equal(1, target.Count);

            Rectangle bounds = new Rectangle(0, 0, width, height);

            target.TurnOff(bounds);
            Assert.Equal(0, target[minimum]);
            Assert.Equal(0, target[maximum]);
            Assert.Equal(0, target.Brightness);
            Assert.Equal(0, target.Count);

            target.TurnOff(bounds);
            Assert.Equal(0, target[minimum]);
            Assert.Equal(0, target[maximum]);
            Assert.Equal(0, target.Brightness);
            Assert.Equal(0, target.Count);

            target.TurnOn(bounds);
            Assert.Equal(1, target[minimum]);
            Assert.Equal(1, target[maximum]);
            Assert.Equal(width * height, target.Brightness);
            Assert.Equal(width * height, target.Count);

            target.TurnOn(bounds);
            Assert.Equal(1, target[minimum]);
            Assert.Equal(1, target[maximum]);
            Assert.Equal(width * height, target.Brightness);
            Assert.Equal(width * height, target.Count);

            target.Toggle(bounds);
            Assert.Equal(0, target[minimum]);
            Assert.Equal(0, target[maximum]);
            Assert.Equal(0, target.Brightness);
            Assert.Equal(0, target.Count);

            target.IncrementBrightness(minimum, 1);
            target.IncrementBrightness(minimum, 2);
            target.IncrementBrightness(minimum, 3);
            target.IncrementBrightness(minimum, -4);

            Assert.Equal(2, target[minimum]);
            Assert.Equal(2, target.Brightness);
            Assert.Equal(1, target.Count);

            target.IncrementBrightness(maximum, 10);

            Assert.Equal(2, target[minimum]);
            Assert.Equal(10, target[maximum]);
            Assert.Equal(12, target.Brightness);
            Assert.Equal(2, target.Count);

            target.IncrementBrightness(minimum, -3);

            Assert.Equal(0, target[minimum]);
            Assert.Equal(10, target[maximum]);
            Assert.Equal(10, target.Brightness);
            Assert.Equal(1, target.Count);
        }

        [Fact]
        public static void LightGridDrawsCorrectly3x3()
        {
            // Arrange
            var target = new LightGrid(3, 3);

            target.TurnOn(new Point(0, 0));
            target.TurnOn(new Point(0, 2));
            target.TurnOn(new Point(1, 1));
            target.TurnOn(new Point(2, 0));
            target.TurnOn(new Point(2, 2));

            string expected = @"101
010
101
";

            // Act
            string actual = target.ToString();

            // Assert
            Assert.Equal(expected, actual);

            // Arrange
            target.TurnOff(new Rectangle(0, 0, 3, 3));
            target.TurnOn(new Rectangle(0, 0, 2, 2));

            expected = @"110
110
000
";

            // Act
            actual = target.ToString();

            // Assert
            Assert.Equal(expected, actual);

            // Arrange
            target.TurnOff(new Rectangle(0, 0, 3, 3));
            target.TurnOn(new Rectangle(1, 1, 2, 2));

            expected = @"000
011
011
";

            // Act
            actual = target.ToString();

            // Assert
            Assert.Equal(expected, actual);

            // Arrange
            target.IncrementBrightness(new Rectangle(1, 1, 2, 2), 2);

            expected = @"000
033
033
";

            // Act
            actual = target.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
