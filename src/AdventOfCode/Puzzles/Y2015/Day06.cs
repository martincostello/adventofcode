// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/6</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2015, 06, "Probably a Fire Hazard", RequiresData = true)]
public sealed class Day06 : Puzzle<int, int>
{
    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (lines, logger, _) =>
            {
                var instructionsV1 = lines.Select(InstructionV1.Parse).ToArray();
                var instructionsV2 = lines.Select(InstructionV2.Parse).ToArray();

                const int Length = 1_000;

                var gridV1 = new LightGrid(Length, Length);
                var gridV2 = new LightGrid(Length, Length);

                foreach (var instruction in instructionsV1)
                {
                    instruction.Act(gridV1);
                }

                foreach (var instruction in instructionsV2)
                {
                    instruction.Act(gridV2);
                }

                int lightsIlluminated = gridV1.Count;
                int totalBrightness = gridV2.Brightness;

                if (logger is { })
                {
                    logger.WriteLine("{0:N0} lights are illuminated with the version 1 grid.", lightsIlluminated);
                    logger.WriteLine("The total brightness of the version 2 grid is {0:N0}.", totalBrightness);
                }

                return (lightsIlluminated, totalBrightness);
            },
            cancellationToken);
    }

    /// <summary>
    /// The base class for instructions.
    /// </summary>
    internal abstract class Instruction
    {
        /// <summary>
        /// Performs the instruction on the specified <see cref="LightGrid"/>.
        /// </summary>
        /// <param name="grid">The grid to perform the instruction on.</param>
        public abstract void Act(LightGrid grid);

        /// <summary>
        /// Parses the specified origin and termination points, as strings, to a bounding rectangle.
        /// </summary>
        /// <param name="origin">The origin point of the bounding rectangle, as a <see cref="string"/>.</param>
        /// <param name="termination">The termination point of the bounding rectangle, as a <see cref="string"/>.</param>
        /// <returns>
        /// A bounding <see cref="Rectangle"/> parsed from <paramref name="origin"/> and <paramref name="termination"/>.
        /// </returns>
        protected static Rectangle ParseBounds(string origin, string termination)
        {
            // Determine the termination and origin points of the bounds of the lights to operate on
            (int left, int bottom) = origin.AsNumberPair<int>();
            (int right, int top) = termination.AsNumberPair<int>();

            // Add one to the termination point so that the grid always has a width of at least one light
            return Rectangle.FromLTRB(left, bottom, right + 1, top + 1);
        }
    }

    /// <summary>
    /// A class representing a version 1 instruction for manipulating the light grid. This class cannot be inherited.
    /// </summary>
    internal sealed class InstructionV1 : Instruction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstructionV1"/> class.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="bounds">The bounds of the lights to perform the action on.</param>
        private InstructionV1(string action, Rectangle bounds)
        {
            Action = action;
            Bounds = bounds;
        }

        /// <summary>
        /// Gets the action to perform.
        /// </summary>
        internal string Action { get; }

        /// <summary>
        /// Gets the bounds of the lights to perform the action on.
        /// </summary>
        internal Rectangle Bounds { get; }

        /// <inheritdoc />
        public override void Act(LightGrid grid)
        {
            switch (Action)
            {
                case "OFF":
                    grid.TurnOff(Bounds);
                    break;

                case "ON":
                    grid.TurnOn(Bounds);
                    break;

                case "TOGGLE":
                    grid.Toggle(Bounds);
                    break;

                default:
                    throw new PuzzleException($"The current instruction '{Action}' is invalid.");
            }
        }

        /// <summary>
        /// Parses the specified instruction.
        /// </summary>
        /// <param name="value">A <see cref="string"/> representing the instruction.</param>
        /// <returns>The <see cref="InstructionV1"/> parsed from <paramref name="value"/>.</returns>
        internal static InstructionV1 Parse(string value)
        {
            // Split the instructions into 'words'
            string[] words = value.Split(' ');

            string? firstWord = words.ElementAtOrDefault(0);

            string? action = null;
            string? origin = null;
            string? termination = null;

            // Determine the action to perform for this instruction (OFF, ON or TOGGLE)
            if (string.Equals(firstWord, "turn", StringComparison.OrdinalIgnoreCase))
            {
                string? secondWord = words.ElementAtOrDefault(1);

                if (string.Equals(secondWord, "off", StringComparison.OrdinalIgnoreCase))
                {
                    action = "OFF";
                }
                else if (string.Equals(secondWord, "on", StringComparison.OrdinalIgnoreCase))
                {
                    action = "ON";
                }

                origin = words.ElementAtOrDefault(2);
                termination = words.ElementAtOrDefault(4);
            }
            else if (string.Equals(firstWord, "toggle", StringComparison.OrdinalIgnoreCase))
            {
                action = "TOGGLE";
                origin = words.ElementAtOrDefault(1);
                termination = words.ElementAtOrDefault(3);
            }

            if (action is null || origin is null || termination is null)
            {
                throw new PuzzleException("The specified instruction is invalid.");
            }

            Rectangle bounds = ParseBounds(origin, termination);

            return new(action, bounds);
        }
    }

    /// <summary>
    /// A class representing a version 2 instruction for manipulating the light grid. This class cannot be inherited.
    /// </summary>
    internal sealed class InstructionV2 : Instruction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstructionV2"/> class.
        /// </summary>
        /// <param name="delta">The delta to apply to the brightness of the light.</param>
        /// <param name="bounds">The bounds of the lights to perform the action on.</param>
        private InstructionV2(int delta, Rectangle bounds)
        {
            Delta = delta;
            Bounds = bounds;
        }

        /// <summary>
        /// Gets the bounds of the lights to perform the action on.
        /// </summary>
        internal Rectangle Bounds { get; }

        /// <summary>
        /// Gets the delta to apply to the brightness of the light.
        /// </summary>
        internal int Delta { get; }

        /// <inheritdoc />
        public override void Act(LightGrid grid)
            => grid.IncrementBrightness(Bounds, Delta);

        /// <summary>
        /// Parses the specified instruction.
        /// </summary>
        /// <param name="value">A <see cref="string"/> representing the instruction.</param>
        /// <returns>The <see cref="InstructionV2"/> parsed from <paramref name="value"/>.</returns>
        internal static InstructionV2 Parse(string value)
        {
            // Split the instructions into 'words'
            string[] words = value.Split(' ');

            string? firstWord = words.ElementAtOrDefault(0);

            int? delta = null;
            string? origin = null;
            string? termination = null;

            // Determine the action to perform for this instruction (OFF, ON or TOGGLE)
            if (string.Equals(firstWord, "turn", StringComparison.OrdinalIgnoreCase))
            {
                string? secondWord = words.ElementAtOrDefault(1);

                if (string.Equals(secondWord, "off", StringComparison.OrdinalIgnoreCase))
                {
                    delta = -1;
                }
                else if (string.Equals(secondWord, "on", StringComparison.OrdinalIgnoreCase))
                {
                    delta = 1;
                }

                origin = words.ElementAtOrDefault(2);
                termination = words.ElementAtOrDefault(4);
            }
            else if (string.Equals(firstWord, "toggle", StringComparison.OrdinalIgnoreCase))
            {
                delta = 2;
                origin = words.ElementAtOrDefault(1);
                termination = words.ElementAtOrDefault(3);
            }

            if (delta is null || origin is null || termination is null)
            {
                throw new PuzzleException("The specified instruction is invalid.");
            }

            Rectangle bounds = ParseBounds(origin, termination);

            return new(delta.Value, bounds);
        }
    }

    /// <summary>
    /// A class representing a grid of Christmas lights. This class cannot be inherited.
    /// </summary>
    internal sealed class LightGrid
    {
        /// <summary>
        /// The brightnesses of lights, indexed by their position in row-major order.
        /// </summary>
        private readonly int[] _lightBrightnesses;

        /// <summary>
        /// The bounds of the grid. This field is read-only.
        /// </summary>
        private readonly Rectangle _bounds;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightGrid"/> class.
        /// </summary>
        /// <param name="width">The width of the light grid.</param>
        /// <param name="height">The height of the light grid.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="width"/> or <paramref name="height"/> is less than one.
        /// </exception>
        /// <remarks>
        /// The initial state of each light in the grid is that it is off.
        /// </remarks>
        internal LightGrid(int width, int height)
        {
            if (width < 1)
            {
                throw new PuzzleException("Width cannot be less than zero.");
            }

            if (height < 1)
            {
                throw new PuzzleException("Height cannot be less than zero.");
            }

            _bounds = new(0, 0, width, height);
            _lightBrightnesses = new int[width * height];
        }

        /// <summary>
        /// Gets the total brightness of the grid.
        /// </summary>
        internal int Brightness => _lightBrightnesses.Sum();

        /// <summary>
        /// Gets the number of lights in the grid that have a brightness of at least one.
        /// </summary>
        internal int Count => _lightBrightnesses.Length - _lightBrightnesses.AsSpan().Count(0);

        /// <summary>
        /// Gets the brightness of the light at the specified position.
        /// </summary>
        /// <param name="position">The position of the light to get the state of.</param>
        /// <returns>The current brightness of the light at <paramref name="position"/>.</returns>
        internal int this[Point position] => _bounds.Contains(position) ? _lightBrightnesses[Index(position)] : 0;

        /// <inheritdoc />
        public override string ToString()
        {
            var builder = new StringBuilder((_bounds.Width + Environment.NewLine.Length) * _bounds.Height);

            for (int x = 0; x < _bounds.Width; x++)
            {
                for (int y = 0; y < _bounds.Height; y++)
                {
                    builder.Append(_lightBrightnesses[Index(new(x, y))]);
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }

        /// <summary>
        /// Increments the brightness of the lights within the specified bounds by the specified amount.
        /// </summary>
        /// <param name="bounds">The bounds of the lights to toggle.</param>
        /// <param name="delta">The brightness to increase (or decrease) the brightness of the lights by.</param>
        internal void IncrementBrightness(Rectangle bounds, int delta)
        {
            for (int y = 0; y < bounds.Height; y++)
            {
                var row = Row(bounds, y);

                for (int x = 0; x < row.Length; x++)
                {
                    row[x] = Math.Max(row[x] + delta, 0);
                }
            }
        }

        /// <summary>
        /// Increments the brightness of the light at the specified position by the specified amount.
        /// </summary>
        /// <param name="position">The position of the light to increment the brightness for.</param>
        /// <param name="delta">The brightness to increase (or decrease) the brightness of the lights by.</param>
        /// <returns>The new brightness of the light at <paramref name="position"/>.</returns>
        internal int IncrementBrightness(Point position, int delta) => IncrementOrSetBrightness(position, delta, false);

        /// <summary>
        /// Toggles the lights within the specified bounds.
        /// </summary>
        /// <param name="bounds">The bounds of the lights to toggle.</param>
        internal void Toggle(Rectangle bounds)
        {
            for (int y = 0; y < bounds.Height; y++)
            {
                var row = Row(bounds, y);

                for (int x = 0; x < row.Length; x++)
                {
                    int current = row[x];
                    row[x] = current == 0 ? 1 : current - 1;
                }
            }
        }

        /// <summary>
        /// Toggles the state of the light at the specified position.
        /// </summary>
        /// <param name="position">The position of the light to toggle.</param>
        /// <returns>
        /// <see langword="true"/> if the light at <paramref name="position"/> is now on; otherwise <see langword="false"/>.
        /// </returns>
        internal bool Toggle(Point position)
        {
            bool isOff = this[position] == 0;
            int delta = isOff ? 1 : -1;

            IncrementBrightness(position, delta);

            return isOff;
        }

        /// <summary>
        /// Turns off the lights within the specified bounds.
        /// </summary>
        /// <param name="bounds">The bounds of the lights to turn off.</param>
        internal void TurnOff(Rectangle bounds)
        {
            for (int y = 0; y < bounds.Height; y++)
            {
                Row(bounds, y).Clear();
            }
        }

        /// <summary>
        /// Turns off the light at the specified position.
        /// </summary>
        /// <param name="position">The position of the light to turn off.</param>
        internal void TurnOff(Point position) => IncrementOrSetBrightness(position, 0, true);

        /// <summary>
        /// Turns on the lights within the specified bounds.
        /// </summary>
        /// <param name="bounds">The bounds of the lights to turn on.</param>
        internal void TurnOn(Rectangle bounds)
        {
            for (int y = 0; y < bounds.Height; y++)
            {
                Row(bounds, y).Fill(1);
            }
        }

        /// <summary>
        /// Turns on the light at the specified position.
        /// </summary>
        /// <param name="position">The position of the light to turn on.</param>
        internal void TurnOn(Point position) => IncrementOrSetBrightness(position, 1, true);

        /// <summary>
        /// Gets the index of the light at the specified position.
        /// </summary>
        /// <param name="position">The position of the light.</param>
        /// <returns>The index of the light at <paramref name="position"/>.</returns>
        private int Index(Point position) => (position.Y * _bounds.Width) + position.X;

        /// <summary>
        /// Gets the lights for the specified row within the specified bounds.
        /// </summary>
        /// <param name="bounds">The bounds of the lights.</param>
        /// <param name="y">The row within the bounds to get the lights for.</param>
        /// <returns>The lights for the specified row of <paramref name="bounds"/>.</returns>
        private Span<int> Row(Rectangle bounds, int y)
            => _lightBrightnesses.AsSpan(Index(new(bounds.X, bounds.Y + y)), bounds.Width);

        /// <summary>
        /// Increments or sets the brightness of the light at the specified position by the specified amount.
        /// </summary>
        /// <param name="position">The position of the light to increment the brightness for.</param>
        /// <param name="delta">The brightness to increase (or decrease) the brightness of the lights by .</param>
        /// <param name="set">Whether to set the value rather than increment it.</param>
        /// <returns>The new brightness of the light at <paramref name="position"/>.</returns>
        private int IncrementOrSetBrightness(Point position, int delta, bool set = false)
        {
            ref int light = ref _lightBrightnesses[Index(position)];

            int current = set ? delta : light + delta;

            light = Math.Max(current, 0);

            return light;
        }
    }
}
