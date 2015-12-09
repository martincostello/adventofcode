// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgramTests.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   ProgramTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day9
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Program"/> class. This class cannot be inherited.
    /// </summary>
    public static class ProgramTests
    {
        [Fact]
        public static void Shortest_Distance_To_Visit_All_Points_Once_Is_Correct()
        {
            // Arrange
            var distances = new[]
            {
                "London to Dublin = 464",
                "London to Belfast = 518",
                "Dublin to Belfast = 141",
            };

            // Act
            int actual = Program.GetShortestDistanceBetweenPoints(distances);

            // Assert
            Assert.Equal(605, actual);
        }

        [Fact]
        public static void Shortest_Distance_To_Visit_All_Points_Once_Is_Correct_If_Only_One_Point()
        {
            // Arrange
            var distances = new[]
            {
                "London to Dublin = 464",
            };

            // Act
            int actual = Program.GetShortestDistanceBetweenPoints(distances);

            // Assert
            Assert.Equal(464, actual);
        }
    }
}
