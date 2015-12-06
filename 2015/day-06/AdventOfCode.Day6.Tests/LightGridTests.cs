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

            Assert.Equal(0, target.Count);
            Assert.False(target[minimum]);
            Assert.False(target[maximum]);

            Assert.True(target.Toggle(minimum));
            Assert.True(target[minimum]);
            Assert.Equal(1, target.Count);

            Assert.True(target.Toggle(maximum));
            Assert.True(target[maximum]);
            Assert.Equal(2, target.Count);

            target.TurnOn(minimum);
            Assert.True(target[minimum]);
            Assert.Equal(2, target.Count);

            target.TurnOff(minimum);
            Assert.False(target[minimum]);
            Assert.Equal(1, target.Count);

            target.TurnOff(minimum);
            Assert.False(target[minimum]);
            Assert.Equal(1, target.Count);

            target.TurnOff(maximum);
            Assert.False(target[maximum]);
            Assert.Equal(0, target.Count);

            target.TurnOn(minimum);
            Assert.True(target[minimum]);
            Assert.Equal(1, target.Count);

            Assert.False(target.Toggle(minimum));
            Assert.False(target[minimum]);
            Assert.Equal(0, target.Count);

            Assert.True(target.Toggle(maximum));
            Assert.True(target[maximum]);
            Assert.Equal(1, target.Count);

            Rectangle bounds = new Rectangle(0, 0, width, height);

            target.TurnOff(bounds);
            Assert.False(target[minimum]);
            Assert.False(target[maximum]);
            Assert.Equal(0, target.Count);

            target.TurnOff(bounds);
            Assert.False(target[minimum]);
            Assert.False(target[maximum]);
            Assert.Equal(0, target.Count);

            target.TurnOn(bounds);
            Assert.True(target[minimum]);
            Assert.True(target[maximum]);
            Assert.Equal(width * height, target.Count);

            target.TurnOn(bounds);
            Assert.True(target[minimum]);
            Assert.True(target[maximum]);
            Assert.Equal(width * height, target.Count);

            target.Toggle(bounds);
            Assert.False(target[minimum]);
            Assert.False(target[maximum]);
            Assert.Equal(0, target.Count);
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

            string expected = @"x x
 x 
x x
";

            // Act
            string actual = target.ToString();

            // Assert
            Assert.Equal(expected, actual);

            // Arrange
            target.TurnOff(new Rectangle(0, 0, 3, 3));
            target.TurnOn(new Rectangle(0, 0, 2, 2));

            expected = @"xx 
xx 
   
";

            // Act
            actual = target.ToString();

            // Assert
            Assert.Equal(expected, actual);

            // Arrange
            target.TurnOff(new Rectangle(0, 0, 3, 3));
            target.TurnOn(new Rectangle(1, 1, 2, 2));

            expected = @"   
 xx
 xx
";

            // Act
            actual = target.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
