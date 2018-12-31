// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2016/day/10</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day10 : Puzzle2016
    {
        /// <summary>
        /// Gets the number of the bot that compares value-61 and value-17 microchips.
        /// </summary>
        public int BotThatCompares61And17Microchips { get; private set; }

        /// <summary>
        /// Gets the product of the value of the microchips in output bins 0, 1 and 2.
        /// </summary>
        public int ProductOfMicrochipsInBins012 { get; private set; }

        /// <summary>
        /// Returns the value of the bot that compares microchips with the specified values.
        /// </summary>
        /// <param name="instructions">The instructions to process.</param>
        /// <param name="a">The first number to find the bot that compares it.</param>
        /// <param name="b">The second number to find the bot that compares it.</param>
        /// <param name="binsOfInterest">The numbers of the bins of interest for which to find the product of the microchip values.</param>
        /// <returns>
        /// A <see cref="Tuple{T1, T2}"/> that returns the number of the bot that compares microchips
        /// with the values specified by <paramref name="a"/> and <paramref name="b"/> and the product
        /// of the values of the microchips in the output bins with the numbers specified by <paramref name="binsOfInterest"/>.
        /// </returns>
        internal static Tuple<int, int> GetBotNumber(IEnumerable<string> instructions, int a, int b, IEnumerable<int> binsOfInterest)
        {
            int max = Math.Max(a, b);
            int min = Math.Min(a, b);

            int botOfInterest = -1;
            int productOfBinsOfInterest = 1;

            var processor = new InstructionProcessor();

            processor.Process(
                instructions,
                (bot, low, high) =>
                {
                    if (low == min && high == max)
                    {
                        botOfInterest = bot;
                    }
                });

            foreach (var bin in processor.Bins)
            {
                if (binsOfInterest.Contains(bin.Key))
                {
                    productOfBinsOfInterest *= bin.Value.Microchip.Value;
                }
            }

            return Tuple.Create(botOfInterest, productOfBinsOfInterest);
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> instructions = ReadResourceAsLines();
            IList<int> binsOfInterest = new[] { 0, 1, 2 };

            var result = GetBotNumber(instructions, 61, 17, binsOfInterest);

            BotThatCompares61And17Microchips = result.Item1;
            ProductOfMicrochipsInBins012 = result.Item2;

            if (Verbose)
            {
                Logger.WriteLine($"The number of the bot that compares value-61 and value-17 microchips is {BotThatCompares61And17Microchips:N0}.");
                Logger.WriteLine($"The product of the microchips in output bins 0, 1 and 2 is {ProductOfMicrochipsInBins012:N0}.");
            }

            return 0;
        }

        /// <summary>
        /// A class representing a chip output bin. This class cannot be inherited.
        /// </summary>
        private sealed class Bin : ChipDestination
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Bin"/> class.
            /// </summary>
            /// <param name="number">The bin's number.</param>
            internal Bin(int number)
                : base()
            {
                Number = number;
            }

            /// <summary>
            /// Gets the number of the bin.
            /// </summary>
            public int Number { get; }

            /// <summary>
            /// Accepts a microchip.
            /// </summary>
            /// <param name="chip">The microchip to accept.</param>
            public override void Accept(Chip chip)
            {
                Microchip = chip;
            }
        }

        /// <summary>
        /// A class representing a bot. This class cannot be inherited.
        /// </summary>
        private sealed class Bot : ChipDestination
        {
            /// <summary>
            /// A delegate to a method to invoke when to microchips are compared. This field is read-only.
            /// </summary>
            private readonly Action<int, int, int> _onCompare;

            /// <summary>
            /// Initializes a new instance of the <see cref="Bot"/> class.
            /// </summary>
            /// <param name="number">The bot's number.</param>
            /// <param name="onCompare">A delegate to a method to invoke when to microchips are compared by the bot.</param>
            internal Bot(int number, Action<int, int, int> onCompare)
                : base()
            {
                Number = number;
                _onCompare = onCompare;
            }

            /// <summary>
            /// Gets the number of the bot.
            /// </summary>
            public int Number { get; }

            /// <summary>
            /// Gets or sets the destination the high value microchip is passed to.
            /// </summary>
            internal ChipDestination HighDestination { get; set; }

            /// <summary>
            /// Gets or sets the destination the low value microchip is passed to.
            /// </summary>
            internal ChipDestination LowDestination { get; set; }

            /// <summary>
            /// Accepts a microchip.
            /// </summary>
            /// <param name="chip">The microchip to accept.</param>
            public override void Accept(Chip chip)
            {
                if (Microchip == null)
                {
                    Microchip = chip;
                }
                else
                {
                    Chip high = chip.Value > Microchip.Value ? chip : Microchip;
                    Chip low = chip.Value < Microchip.Value ? chip : Microchip;

                    _onCompare(Number, low.Value, high.Value);

                    LowDestination.Accept(low);
                    HighDestination.Accept(high);

                    Microchip = null;
                }
            }
        }

        /// <summary>
        /// A class representing a microchip. This class cannot be inherited.
        /// </summary>
        private sealed class Chip
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Chip"/> class.
            /// </summary>
            /// <param name="value">The value of the microchip.</param>
            internal Chip(int value)
            {
                Value = value;
            }

            /// <summary>
            /// Gets the value of the microchip.
            /// </summary>
            public int Value { get; }
        }

        /// <summary>
        /// Defines an object that holds a chip.
        /// </summary>
        private abstract class ChipDestination
        {
            /// <summary>
            /// Gets or sets the microchip currently held by the holder, if any.
            /// </summary>
            public Chip Microchip { get; set; }

            /// <summary>
            /// Accepts a microchip.
            /// </summary>
            /// <param name="chip">The microchip to accept.</param>
            public abstract void Accept(Chip chip);
        }

        /// <summary>
        /// A class that processes instructions. This class cannot be inherited.
        /// </summary>
        private sealed class InstructionProcessor
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="InstructionProcessor"/> class.
            /// </summary>
            internal InstructionProcessor()
            {
                Bins = new Dictionary<int, Bin>();
                Bots = new Dictionary<int, Bot>();
            }

            /// <summary>
            /// Gets the output bins, keyed by their number.
            /// </summary>
            internal IDictionary<int, Bin> Bins { get; }

            /// <summary>
            /// Gets the bots, keyed by their number.
            /// </summary>
            internal IDictionary<int, Bot> Bots { get; }

            /// <summary>
            /// Gets or sets a delegate to a method to invoke when to microchips are compared by a bot.
            /// </summary>
            private Action<int, int, int> OnCompare { get; set; }

            /// <summary>
            /// Processes the specified instructions.
            /// </summary>
            /// <param name="instructions">The instructions to process.</param>
            /// <param name="onCompare">A delegate to a method to invoke when to microchips are compared by a bot.</param>
            internal void Process(IEnumerable<string> instructions, Action<int, int, int> onCompare)
            {
                OnCompare = onCompare;

                // Process instructions that set up the connections
                foreach (string instruction in instructions.Where((p) => p.StartsWith("bot", StringComparison.Ordinal)))
                {
                    Process(instruction);
                }

                // Process instructions that assign the microchips
                foreach (string instruction in instructions.Where((p) => p.StartsWith("value", StringComparison.Ordinal)))
                {
                    Process(instruction);
                }
            }

            /// <summary>
            /// Processes the specified instruction.
            /// </summary>
            /// <param name="instruction">The instruction to process.</param>
            private void Process(string instruction)
            {
                string[] split = instruction.Split(Arrays.Space, StringSplitOptions.None);

                if (string.Equals(split[0], "value", StringComparison.Ordinal))
                {
                    int chipValue = ParseInt32(split[1]);
                    int botNumber = ParseInt32(split[5]);

                    GetBot(botNumber).Accept(new Chip(chipValue));
                }
                else
                {
                    bool isHighBot = string.Equals("bot", split[10], StringComparison.Ordinal);
                    int highValue = ParseInt32(split[11]);

                    bool isLowBot = string.Equals("bot", split[5], StringComparison.Ordinal);
                    int lowValue = ParseInt32(split[6]);

                    int botNumber = ParseInt32(split[1]);

                    Bot bot = GetBot(botNumber);

                    bot.HighDestination = isHighBot ? GetBot(highValue) : GetBin(highValue) as ChipDestination;
                    bot.LowDestination = isLowBot ? GetBot(lowValue) : GetBin(lowValue) as ChipDestination;
                }
            }

            /// <summary>
            /// Gets the bot with the specified number.
            /// </summary>
            /// <param name="number">The number of the bot to return.</param>
            /// <returns>
            /// The <see cref="Bot"/> with the specified number.
            /// </returns>
            private Bot GetBot(int number)
            {
                Bot bot;

                if (!Bots.TryGetValue(number, out bot))
                {
                    Bots[number] = bot = new Bot(number, OnCompare);
                }

                return bot;
            }

            /// <summary>
            /// Gets the bin with the specified number.
            /// </summary>
            /// <param name="number">The number of the bin to return.</param>
            /// <returns>
            /// The <see cref="Bin"/> with the specified number.
            /// </returns>
            private Bin GetBin(int number)
            {
                if (!Bins.TryGetValue(number, out Bin bin))
                {
                    Bins[number] = bin = new Bin(number);
                }

                return bin;
            }
        }
    }
}
