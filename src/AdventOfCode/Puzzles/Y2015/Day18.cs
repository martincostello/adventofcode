// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/18</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2015, 18, "Like a GIF For Your Yard", RequiresData = true, IsSlow = true)]
public sealed class Day18 : Puzzle
{
    /// <summary>
    /// The character that signifies that a light is on.
    /// </summary>
    private const char Off = '.';

    /// <summary>
    /// The character that signifies that a light is on.
    /// </summary>
    private const char On = '#';

    /// <summary>
    /// Gets the number of lights that are illuminated without the stuck lights.
    /// </summary>
    public int LightsIlluminated { get; private set; }

    /// <summary>
    /// Gets the number of lights that are illuminated with the stuck lights.
    /// </summary>
    public int LightsIlluminatedWithStuckLights { get; private set; }

    /// <summary>
    /// Returns the light configuration for the specified initial state after the specified number of steps.
    /// </summary>
    /// <param name="initial">The initial light configuration.</param>
    /// <param name="steps">The number of steps to return the configuration for.</param>
    /// <param name="areCornerLightsBroken">Whether the corner lights are broken.</param>
    /// <returns>
    /// An <see cref="IList{T}"/> of <see cref="string"/> containing the light
    /// configuration after the number of steps specified by the value of <paramref name="steps"/>.
    /// </returns>
    internal static string[] GetGridConfigurationAfterSteps(IList<string> initial, int steps, bool areCornerLightsBroken)
    {
        bool[,] current = ParseInitialState(initial);

        for (int i = 0; i < steps; i++)
        {
            current = Animate(current, areCornerLightsBroken);
        }

        int width = current.GetLength(0);
        int height = current.GetLength(1);

        string[] result = new string[width];

        for (int x = 0; x < width; x++)
        {
            var builder = new StringBuilder(height);

            for (int y = 0; y < height; y++)
            {
                builder.Append(current[x, y] ? On : Off);
            }

            result[x] = builder.ToString();
        }

        return result;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var initial = await ReadResourceAsLinesAsync(cancellationToken);

        int steps = 100;

        string[] finalUnstuck = GetGridConfigurationAfterSteps(initial, steps, areCornerLightsBroken: false);
        string[] finalStuck = GetGridConfigurationAfterSteps(initial, steps, areCornerLightsBroken: true);

        LightsIlluminated = finalUnstuck.Sum((p) => p.Count(On));
        LightsIlluminatedWithStuckLights = finalStuck.Sum((p) => p.Count(On));

        if (Verbose)
        {
            Logger.WriteLine(
                "There are {0:N0} lights illuminated after {1:N0} steps.",
                LightsIlluminated,
                steps);

            Logger.WriteLine(
                "There are {0:N0} lights illuminated after {1:N0} steps with the corner lights stuck on.",
                LightsIlluminatedWithStuckLights,
                steps);
        }

        return PuzzleResult.Create(LightsIlluminated, LightsIlluminatedWithStuckLights);
    }

    /// <summary>
    /// Animates the specified input frame and returns the new output frame.
    /// </summary>
    /// <param name="input">The input frame.</param>
    /// <param name="areCornerLightsBroken">Whether the corner lights are broken.</param>
    /// <returns>
    /// An <see cref="Array"/> of <see cref="bool"/> containing the new frame.
    /// </returns>
    private static bool[,] Animate(bool[,] input, bool areCornerLightsBroken)
    {
        int width = input.GetLength(0);
        int height = input.GetLength(1);

        bool[,] output = new bool[width, height];

        Point[] cornerLights;

        if (areCornerLightsBroken)
        {
            cornerLights =
            [
                new(0, 0),
                new(width - 1, 0),
                new(0, height - 1),
                new(width - 1, height - 1),
            ];
        }
        else
        {
#pragma warning disable SA1010
            cornerLights = [];
#pragma warning restore SA1010
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bool newState;

                if (areCornerLightsBroken && cornerLights.Contains(new Point(x, y)))
                {
                    newState = true;
                }
                else
                {
                    Point[] neighbors =
                    [
                        new(x - 1, y - 1),
                        new(x, y - 1),
                        new(x + 1, y - 1),
                        new(x - 1, y),
                        new(x + 1, y),
                        new(x + 1, y + 1),
                        new(x, y + 1),
                        new(x - 1, y + 1),
                    ];

                    int neighborsOn = 0;

                    foreach (Point neighbor in neighbors)
                    {
                        if (neighbor.X >= 0 && neighbor.X < width && neighbor.Y >= 0 && neighbor.Y < height)
                        {
                            if (input[neighbor.X, neighbor.Y] || cornerLights.Contains(new Point(neighbor.X, neighbor.Y)))
                            {
                                neighborsOn++;
                            }
                        }
                    }

                    if (input[x, y])
                    {
                        newState = neighborsOn == 2 || neighborsOn == 3;
                    }
                    else
                    {
                        newState = neighborsOn == 3;
                    }
                }

                output[x, y] = newState;
            }
        }

        return output;
    }

    /// <summary>
    /// Parses the specified initial state as an <see cref="Array"/> of <see cref="bool"/>.
    /// </summary>
    /// <param name="initialState">The initial state as a collection of strings.</param>
    /// <returns>
    /// An <see cref="Array"/> of <see cref="bool"/> representing the light configuration
    /// specified by <paramref name="initialState"/>.
    /// </returns>
    private static bool[,] ParseInitialState(IList<string> initialState)
    {
        // Assume that the input configuration is a rectangle
        int width = initialState[0].Length;
        int height = initialState.Count;

        bool[,] state = new bool[width, height];

        for (int x = 0; x < height; x++)
        {
            string value = initialState[x];

            for (int y = 0; y < value.Length; y++)
            {
                // '#' means 'on'; '.' means 'off'
                state[x, y] = value[y] == On;
            }
        }

        return state;
    }
}
