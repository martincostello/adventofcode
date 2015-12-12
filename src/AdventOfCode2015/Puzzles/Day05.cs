// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.IO;
    using Impl;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/day/5</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day05 : IPuzzle
    {
        /// <inheritdoc />
        public int Solve(string[] args)
        {
            if (args.Length != 2)
            {
                Console.Error.WriteLine("No input file path and rule version specified.");
                return -1;
            }

            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine("The input file path specified cannot be found.");
                return -1;
            }

            int version = -1;

            switch (args[1])
            {
                case "1":
                    version = 1;
                    break;

                case "2":
                    version = 2;
                    break;

                default:
                    break;
            }

            if (version == -1)
            {
                Console.Error.WriteLine("The rules version specified is invalid.");
                return -1;
            }

            int niceStrings = 0;
            INicenessRule rule;

            if (version == 1)
            {
                rule = new NicenessRuleV1();
            }
            else
            {
                rule = new NicenessRuleV2();
            }

            foreach (string value in File.ReadLines(args[0]))
            {
                if (rule.IsNice(value))
                {
                    niceStrings++;
                }
            }

            Console.WriteLine("{0:N0} strings are nice using version {1} of the rules.", niceStrings, version);

            return 0;
        }
    }
}
