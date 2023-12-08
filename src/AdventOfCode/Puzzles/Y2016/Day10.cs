// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/10</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2016, 10, "Balance Bots", RequiresData = true)]
public sealed class Day10 : Puzzle
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
    /// A named tuple that returns the number of the bot that compares microchips
    /// with the values specified by <paramref name="a"/> and <paramref name="b"/> and the product
    /// of the values of the microchips in the output bins with the numbers specified by <paramref name="binsOfInterest"/>.
    /// </returns>
    internal static (int Bot, int Product) GetBotNumber(ICollection<string> instructions, int a, int b, IEnumerable<int> binsOfInterest)
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

        var foundBins = new HashSet<int>();

        foreach (var bin in processor.Bins)
        {
            bool found = false;

            if (foundBins.Contains(bin.Key))
            {
                found = true;
            }
            else if (binsOfInterest.Contains(bin.Key))
            {
                foundBins.Add(bin.Key);
                found = true;
            }

            if (found)
            {
                productOfBinsOfInterest *= bin.Value.Microchip!.Value;
            }
        }

        return (botOfInterest, productOfBinsOfInterest);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var instructions = await ReadResourceAsLinesAsync(cancellationToken);
        int[] binsOfInterest = [0, 1, 2];

        (BotThatCompares61And17Microchips, ProductOfMicrochipsInBins012) = GetBotNumber(instructions, 61, 17, binsOfInterest);

        if (Verbose)
        {
            Logger.WriteLine($"The number of the bot that compares value-61 and value-17 microchips is {BotThatCompares61And17Microchips:N0}.");
            Logger.WriteLine($"The product of the microchips in output bins 0, 1 and 2 is {ProductOfMicrochipsInBins012:N0}.");
        }

        return PuzzleResult.Create(BotThatCompares61And17Microchips, ProductOfMicrochipsInBins012);
    }

    /// <summary>
    /// A class representing a chip output bin. This class cannot be inherited.
    /// </summary>
    private sealed class Bin(int number) : ChipDestination
    {
        /// <summary>
        /// Gets the number of the bin.
        /// </summary>
        public int Number { get; } = number;

        /// <summary>
        /// Accepts a microchip.
        /// </summary>
        /// <param name="chip">The microchip to accept.</param>
        public override void Accept(Chip chip)
            => Microchip = chip;
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
        internal ChipDestination? HighDestination { get; set; }

        /// <summary>
        /// Gets or sets the destination the low value microchip is passed to.
        /// </summary>
        internal ChipDestination? LowDestination { get; set; }

        /// <summary>
        /// Accepts a microchip.
        /// </summary>
        /// <param name="chip">The microchip to accept.</param>
        public override void Accept(Chip chip)
        {
            if (Microchip is null)
            {
                Microchip = chip;
            }
            else
            {
                Chip high = chip.Value > Microchip.Value ? chip : Microchip;
                Chip low = chip.Value < Microchip.Value ? chip : Microchip;

                _onCompare(Number, low.Value, high.Value);

                LowDestination!.Accept(low);
                HighDestination!.Accept(high);

                Microchip = null;
            }
        }
    }

    /// <summary>
    /// A class representing a microchip. This class cannot be inherited.
    /// </summary>
    /// <param name="value">The value of the microchip.</param>
    private sealed class Chip(int value)
    {
        /// <summary>
        /// Gets the value of the microchip.
        /// </summary>
        public int Value { get; } = value;
    }

    /// <summary>
    /// Defines an object that holds a chip.
    /// </summary>
    private abstract class ChipDestination
    {
        /// <summary>
        /// Gets or sets the microchip currently held by the holder, if any.
        /// </summary>
        public Chip? Microchip { get; set; }

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
        /// Gets the output bins, keyed by their number.
        /// </summary>
        internal Dictionary<int, Bin> Bins { get; } = [];

        /// <summary>
        /// Gets the bots, keyed by their number.
        /// </summary>
        internal Dictionary<int, Bot> Bots { get; } = [];

        /// <summary>
        /// Gets or sets a delegate to a method to invoke when to microchips are compared by a bot.
        /// </summary>
        private Action<int, int, int>? OnCompare { get; set; }

        /// <summary>
        /// Processes the specified instructions.
        /// </summary>
        /// <param name="instructions">The instructions to process.</param>
        /// <param name="onCompare">A delegate to a method to invoke when to microchips are compared by a bot.</param>
        internal void Process(ICollection<string> instructions, Action<int, int, int> onCompare)
        {
            OnCompare = onCompare;

            // Process instructions that set up the connections
            foreach (string instruction in instructions.Where((p) => p.AsSpan().StartsWith("bot")))
            {
                Process(instruction);
            }

            // Process instructions that assign the microchips
            foreach (string instruction in instructions.Where((p) => p.AsSpan().StartsWith("value")))
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
            var split = instruction.Split(' ').AsSpan();

            if (split[0] is "value")
            {
                int chipValue = Parse<int>(split[1]);
                int botNumber = Parse<int>(split[5]);

                GetBot(botNumber).Accept(new Chip(chipValue));
            }
            else
            {
                bool isHighBot = split[10] is "bot";
                int highValue = Parse<int>(split[11]);

                bool isLowBot = split[5] is "bot";
                int lowValue = Parse<int>(split[6]);

                int botNumber = Parse<int>(split[1]);

                Bot bot = GetBot(botNumber);

                bot.HighDestination = isHighBot ? GetBot(highValue) : GetBin(highValue);
                bot.LowDestination = isLowBot ? GetBot(lowValue) : GetBin(lowValue);
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
            => Bots.GetOrAdd(number, () => new(number, OnCompare!));

        /// <summary>
        /// Gets the bin with the specified number.
        /// </summary>
        /// <param name="number">The number of the bin to return.</param>
        /// <returns>
        /// The <see cref="Bin"/> with the specified number.
        /// </returns>
        private Bin GetBin(int number)
            => Bins.GetOrAdd(number, () => new(number));
    }
}
