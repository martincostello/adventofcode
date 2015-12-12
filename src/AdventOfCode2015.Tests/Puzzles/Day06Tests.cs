// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using Impl;
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day06"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day06Tests
    {
        [Theory]
        [InlineData("turn on 0,0 through 999,999", "ON", 0, 0, 1000, 1000)]
        [InlineData("toggle 0,0 through 999,0", "TOGGLE", 0, 0, 1000, 1)]
        [InlineData("turn off 499,499 through 500,500", "OFF", 499, 499, 2, 2)]
        public static void Day06_V1InstructionsCanBeParsed(string value, string action, int x, int y, int width, int height)
        {
            InstructionV1 actual = InstructionV1.Parse(value);

            Assert.Equal(action, actual.Action);
            Assert.Equal(x, actual.Bounds.X);
            Assert.Equal(y, actual.Bounds.Y);
            Assert.Equal(height, actual.Bounds.Height);
            Assert.Equal(width, actual.Bounds.Width);
        }

        [Theory]
        [InlineData("turn on 0,0 through 999,999", 1, 0, 0, 1000, 1000)]
        [InlineData("toggle 0,0 through 999,0", 2, 0, 0, 1000, 1)]
        [InlineData("turn off 499,499 through 500,500", -1, 499, 499, 2, 2)]
        public static void Day06_V2InstructionsCanBeParsed(string value, int delta, int x, int y, int width, int height)
        {
            InstructionV2 actual = InstructionV2.Parse(value);

            Assert.Equal(delta, actual.Delta);
            Assert.Equal(x, actual.Bounds.X);
            Assert.Equal(y, actual.Bounds.Y);
            Assert.Equal(height, actual.Bounds.Height);
            Assert.Equal(width, actual.Bounds.Width);
        }

        [Fact]
        public static void Day06_LightGridValidatesBounds()
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
        public static void Day06_LightGridExhibitsCorrectBehavior(int width, int height)
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
        public static void Day06_LightGridDrawsCorrectly3x3()
        {
            // Arrange
            var target = new LightGrid(3, 3);

            target.TurnOn(new Point(0, 0));
            target.TurnOn(new Point(0, 2));
            target.TurnOn(new Point(1, 1));
            target.TurnOn(new Point(2, 0));
            target.TurnOn(new Point(2, 2));

            string expected = string.Format(CultureInfo.InvariantCulture, "101{0}010{0}101{0}", Environment.NewLine);

            // Act
            string actual = target.ToString();

            // Assert
            Assert.Equal(expected, actual);

            // Arrange
            target.TurnOff(new Rectangle(0, 0, 3, 3));
            target.TurnOn(new Rectangle(0, 0, 2, 2));

            expected = string.Format(CultureInfo.InvariantCulture, "110{0}110{0}000{0}", Environment.NewLine);

            // Act
            actual = target.ToString();

            // Assert
            Assert.Equal(expected, actual);

            // Arrange
            target.TurnOff(new Rectangle(0, 0, 3, 3));
            target.TurnOn(new Rectangle(1, 1, 2, 2));

            expected = string.Format(CultureInfo.InvariantCulture, "000{0}011{0}011{0}", Environment.NewLine);

            // Act
            actual = target.ToString();

            // Assert
            Assert.Equal(expected, actual);

            // Arrange
            target.IncrementBrightness(new Rectangle(1, 1, 2, 2), 2);

            expected = string.Format(CultureInfo.InvariantCulture, "000{0}033{0}033{0}", Environment.NewLine);

            // Act
            actual = target.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
