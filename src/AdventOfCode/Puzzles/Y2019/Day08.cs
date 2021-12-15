// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2019/day/8</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2019, 08, "Space Image Format", RequiresData = true)]
public sealed class Day08 : Puzzle
{
    /// <summary>
    /// Gets the checksum of the image.
    /// </summary>
    public int Checksum { get; private set; }

    /// <summary>
    /// Gets the message.
    /// </summary>
    public string Message { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the checksum for the specified image.
    /// </summary>
    /// <param name="image">The image data to get the checksum for.</param>
    /// <param name="height">The height of the image.</param>
    /// <param name="width">The width of the image.</param>
    /// <param name="logger">The optional logger to use.</param>
    /// <returns>
    /// The checksum of the image data, the message and a visualization.
    /// </returns>
    public static (int Checksum, string Message, string Visualization) GetImageChecksum(
        string image,
        int height,
        int width,
        ILogger? logger = null)
    {
        var layers = new List<int[,]>();
        int[,] current = new int[0, 0];

        int pixelsPerLayer = height * width;

        int i = 0;
        int x = 0;
        int y = 0;

        for (; i < image.Length; i++)
        {
            if (i % pixelsPerLayer == 0)
            {
                current = new int[width, height];
                layers.Add(current);
                y = 0;
            }

            int value = image[i] - '0';

            current[x, y] = value;

            if (x == width - 1)
            {
                x = 0;
                y++;
            }
            else
            {
                x++;
            }
        }

        int countOfZeroesInLastLayer = int.MaxValue;
        int indexOfLayerWithLeastZeros = -1;

        for (i = 0; i < layers.Count; i++)
        {
            int countOfZeroes = GetCountOfDigit(layers[i], 0);

            if (countOfZeroes < countOfZeroesInLastLayer)
            {
                countOfZeroesInLastLayer = countOfZeroes;
                indexOfLayerWithLeastZeros = i;
            }
        }

        int[,] layerWithLeastZeroes = layers[indexOfLayerWithLeastZeros];
        int ones = GetCountOfDigit(layerWithLeastZeroes, 1);
        int twos = GetCountOfDigit(layerWithLeastZeroes, 2);

        int checksum = ones * twos;

        int[,] finalLayer = (int[,])layers[^1].Clone();

        for (i = layers.Count - 2; i >= 0; i--)
        {
            int[,] layer = layers[i];

            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    int background = finalLayer[x, y];
                    int foreground = layer[x, y];

                    if (foreground != 2)
                    {
                        finalLayer[x, y] = foreground;
                    }
                }
            }
        }

        string visualization = WriteMessage(finalLayer, logger);
        string message = CharacterRecognition.Read(finalLayer);

        return (checksum, message, visualization);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        string image = (await ReadResourceAsStringAsync()).TrimEnd('\n');

        (Checksum, Message, string visualization) = GetImageChecksum(image, 6, 25, Logger);

        if (Verbose)
        {
            Logger.WriteLine("The checksum of the image data is {0}.", Checksum);
            Logger.WriteLine("The message in the image data is {0}.", Message);
        }

        var result = new PuzzleResult();

        result.Solutions.Add(Checksum);
        result.Solutions.Add(Message);
        result.Visualizations.Add(visualization);

        return result;
    }

    /// <summary>
    /// Gets the number of occurences of the specified digit in the array.
    /// </summary>
    /// <param name="layer">The array representing the layer to count the digits in.</param>
    /// <param name="digit">The digit to count in the layer.</param>
    /// <returns>
    /// The number of occurences of the digit specified by <paramref name="digit"/> in the array.
    /// </returns>
    private static int GetCountOfDigit(int[,] layer, int digit)
    {
        int count = 0;
        int width = layer.GetLength(0);
        int height = layer.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (layer[x, y] == digit)
                {
                    count++;
                }
            }
        }

        return count;
    }

    /// <summary>
    /// Writes the specified message.
    /// </summary>
    /// <param name="message">The message to write.</param>
    /// <param name="logger">The logger to write the message to.</param>
    /// <returns>
    /// The visualization of the data.
    /// </returns>
    private static string WriteMessage(int[,] message, ILogger? logger)
    {
        int width = message.GetLength(0);
        int height = message.GetLength(1);

        var builder = new StringBuilder((width + Environment.NewLine.Length) * height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                builder.Append(message[x, y] == 1 ? 'x' : ' ');
            }

            builder.AppendLine();
        }

        string visualization = builder.ToString();

        logger?.WriteLine(visualization);

        return visualization;
    }
}
