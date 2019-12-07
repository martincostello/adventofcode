// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2015/day/6</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day06 : Puzzle2015
    {
        /// <summary>
        /// Gets the number of lights illuminated.
        /// </summary>
        internal int LightsIlluminated { get; private set; }

        /// <summary>
        /// Gets the total brightness of the grid.
        /// </summary>
        internal int TotalBrightness { get; private set; }

        /// <inheritdoc />
        protected override int MinimumArguments => 1;

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            int version = -1;

            switch (args[0])
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
                Logger.WriteLine("The instruction set specified is invalid.");
                return -1;
            }

            var lines = ReadResourceAsLines();
            var instructions = new List<Instruction>(lines.Count);

            foreach (string line in lines)
            {
                if (version == 1)
                {
                    instructions.Add(InstructionV1.Parse(line));
                }
                else
                {
                    instructions.Add(InstructionV2.Parse(line));
                }
            }

            if (Verbose)
            {
                Logger.WriteLine("Processing instructions using set {0}...", version);
            }

            var grid = new LightGrid(1000, 1000);

            foreach (Instruction instruction in instructions)
            {
                instruction.Act(grid);
            }

            if (version == 1)
            {
                LightsIlluminated = grid.Count;

                if (Verbose)
                {
                    Logger.WriteLine("{0:N0} lights are illuminated.", LightsIlluminated);
                }
            }
            else
            {
                TotalBrightness = grid.Brightness;

                if (Verbose)
                {
                    Logger.WriteLine("The total brightness of the grid is {0:N0}.", TotalBrightness);
                }
            }

            return 0;
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
                string[] originPoints = origin.Split(',');
                string[] terminationPoints = termination.Split(',');

                int left = ParseInt32(originPoints[0]);
                int bottom = ParseInt32(originPoints[1]);

                int right = ParseInt32(terminationPoints[0]);
                int top = ParseInt32(terminationPoints[1]);

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
                        throw new InvalidOperationException("The current instruction is invalid.");
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

                string firstWord = words.ElementAtOrDefault(0);

                string? action = null;
                string? origin = null;
                string? termination = null;

                // Determine the action to perform for this instruction (OFF, ON or TOGGLE)
                if (string.Equals(firstWord, "turn", StringComparison.OrdinalIgnoreCase))
                {
                    string secondWord = words.ElementAtOrDefault(1);

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

                if (action == null || origin == null || termination == null)
                {
                    throw new ArgumentException("The specified instruction is invalid.", nameof(value));
                }

                Rectangle bounds = ParseBounds(origin, termination);

                return new InstructionV1(action, bounds);
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

                string firstWord = words.ElementAtOrDefault(0);

                int? delta = null;
                string? origin = null;
                string? termination = null;

                // Determine the action to perform for this instruction (OFF, ON or TOGGLE)
                if (string.Equals(firstWord, "turn", StringComparison.OrdinalIgnoreCase))
                {
                    string secondWord = words.ElementAtOrDefault(1);

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

                if (delta == null || origin == null || termination == null)
                {
                    throw new ArgumentException("The specified instruction is invalid.", nameof(value));
                }

                Rectangle bounds = ParseBounds(origin, termination);

                return new InstructionV2(delta.Value, bounds);
            }
        }

        /// <summary>
        /// A class representing a grid of Christmas lights. This class cannot be inherited.
        /// </summary>
        internal sealed class LightGrid
        {
            /// <summary>
            /// The brightnesses of lights by their position.
            /// </summary>
            private readonly int[,] _lightBrightnesses;

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
                    throw new ArgumentOutOfRangeException(
                        nameof(width),
                        width,
                        FormattableString.Invariant($"{nameof(width)} cannot be less than zero."));
                }

                if (height < 1)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(height),
                        height,
                        FormattableString.Invariant($"{nameof(height)} cannot be less than zero."));
                }

                _bounds = new Rectangle(0, 0, width, height);
                _lightBrightnesses = new int[width, height];
            }

            /// <summary>
            /// Gets the total brightness of the grid.
            /// </summary>
            internal int Brightness
            {
                get
                {
                    int result = 0;

                    for (int x = 0; x < _bounds.Width; x++)
                    {
                        for (int y = 0; y < _bounds.Height; y++)
                        {
                            result += _lightBrightnesses[x, y];
                        }
                    }

                    return result;
                }
            }

            /// <summary>
            /// Gets the number of lights in the grid that have a brightness of at least one.
            /// </summary>
            internal int Count
            {
                get
                {
                    int result = 0;

                    for (int x = 0; x < _bounds.Width; x++)
                    {
                        for (int y = 0; y < _bounds.Height; y++)
                        {
                            result += _lightBrightnesses[x, y] > 0 ? 1 : 0;
                        }
                    }

                    return result;
                }
            }

            /// <summary>
            /// Gets the brightness of the light at the specified position.
            /// </summary>
            /// <param name="position">The position of the light to get the state of.</param>
            /// <returns>The current brightness of the light at <paramref name="position"/>.</returns>
            internal int this[Point position] => _lightBrightnesses[position.X, position.Y];

            /// <inheritdoc />
            public override string ToString()
            {
                var builder = new StringBuilder((_bounds.Width * _bounds.Height) + (_bounds.Width * Environment.NewLine.Length));

                for (int x = 0; x < _bounds.Width; x++)
                {
                    for (int y = 0; y < _bounds.Height; y++)
                    {
                        builder.AppendFormat(CultureInfo.InvariantCulture, "{0}", _lightBrightnesses[x, y]);
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
                for (int x = 0; x < bounds.Width; x++)
                {
                    for (int y = 0; y < bounds.Height; y++)
                    {
                        IncrementBrightness(new Point(bounds.X + x, bounds.Y + y), delta);
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
                for (int x = 0; x < bounds.Width; x++)
                {
                    for (int y = 0; y < bounds.Height; y++)
                    {
                        Toggle(new Point(bounds.X + x, bounds.Y + y));
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

                if (isOff)
                {
                    IncrementBrightness(position, 1);
                    return true;
                }
                else
                {
                    IncrementBrightness(position, -1);
                    return false;
                }
            }

            /// <summary>
            /// Turns off the lights within the specified bounds.
            /// </summary>
            /// <param name="bounds">The bounds of the lights to turn off.</param>
            internal void TurnOff(Rectangle bounds)
            {
                for (int x = 0; x < bounds.Width; x++)
                {
                    for (int y = 0; y < bounds.Height; y++)
                    {
                        TurnOff(new Point(bounds.X + x, bounds.Y + y));
                    }
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
                for (int x = 0; x < bounds.Width; x++)
                {
                    for (int y = 0; y < bounds.Height; y++)
                    {
                        TurnOn(new Point(bounds.X + x, bounds.Y + y));
                    }
                }
            }

            /// <summary>
            /// Turns on the light at the specified position.
            /// </summary>
            /// <param name="position">The position of the light to turn on.</param>
            internal void TurnOn(Point position) => IncrementOrSetBrightness(position, 1, true);

            /// <summary>
            /// Increments or sets the brightness of the light at the specified position by the specified amount.
            /// </summary>
            /// <param name="position">The position of the light to increment the brightness for.</param>
            /// <param name="delta">The brightness to increase (or decrease) the brightness of the lights by .</param>
            /// <param name="set">Whether to set the value rather than increment it.</param>
            /// <returns>The new brightness of the light at <paramref name="position"/>.</returns>
            private int IncrementOrSetBrightness(Point position, int delta, bool set = false)
            {
                int current = _lightBrightnesses[position.X, position.Y];

                if (set)
                {
                    current = delta;
                }
                else
                {
                    current += delta;
                }

                if (current < 0)
                {
                    current = 0;
                }

                return _lightBrightnesses[position.X, position.Y] = current;
            }
        }
    }
}
