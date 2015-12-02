// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   Program.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day2
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    internal static class Program
    {
        internal static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("No input file path specified.");
                return -1;
            }

            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine("The input file path specified cannot be found.");
                return -1;
            }

            List<Present> presents = new List<Present>();

            foreach (string line in File.ReadLines(args[0]))
            {
                presents.Add(Present.Parse(line));
            }

            int totalArea = presents.Sum(GetWrappingPaperSurfaceArea);
            int length = presents.Sum(GetRibbonLength);

            Console.Write(
                "The elves should order {0:N0} square feet of wrapping paper.{1}They also need {2:N0} feet of ribbon.",
                totalArea,
                Environment.NewLine,
                length);

            Console.Read();

            return 0;
        }

        private static int GetRibbonLength(Present present)
        {
            int smallestPerimiter = new[] { (present.Length + present.Width) * 2, (present.Width + present.Height) * 2, (present.Height + present.Length) * 2 }.Min();
            int lengthForBow = present.Height * present.Length * present.Width;

            return smallestPerimiter + lengthForBow;
        }

        private static int GetWrappingPaperSurfaceArea(Present present)
        {
            int surfaceArea = (2 * present.Length * present.Width) + (2 * present.Width * present.Height) + (2 * present.Height * present.Length);
            int extra = new[] { present.Length * present.Width, present.Width * present.Height, present.Height * present.Length }.Min();

            return surfaceArea + extra;
        }

        private sealed class Present
        {
            internal Present(int length, int width, int height)
            {
                Length = length;
                Width = width;
                Height = height;
            }

            internal int Length { get; }

            internal int Width { get; }

            internal int Height { get; }

            internal static Present Parse(string value)
            {
                string[] split = value.Split('x');

                int length = int.Parse(split[0], NumberStyles.Integer, CultureInfo.InvariantCulture);
                int width = int.Parse(split[1], NumberStyles.Integer, CultureInfo.InvariantCulture);
                int height = int.Parse(split[2], NumberStyles.Integer, CultureInfo.InvariantCulture);

                return new Present(length, width, height);
            }
        }
    }
}
