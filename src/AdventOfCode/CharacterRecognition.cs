// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode;

/// <summary>
/// A class containing methods for character recognition. This class cannot be inherited.
/// </summary>
internal static class CharacterRecognition
{
    /// <summary>
    /// Returns the characters encoded in the specified string.
    /// </summary>
    /// <param name="text">The string to read.</param>
    /// <param name="ink">The char that signifies a value is part of a written character.</param>
    /// <returns>
    /// A string containing the characters encoded in <paramref name="text"/>.
    /// </returns>
    /// <remarks>
    /// Each encoded character is assumed to be a 5x6 grid.
    /// </remarks>
    public static string Read(string text, char ink = '*')
    {
        string[] lines = text.Split(Environment.NewLine);

        int width = lines[0].Length;
        int height = lines.Length;

        char[,] chars = new char[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                chars[i, j] = lines[j][i];
            }
        }

        return Read(chars, ink);
    }

    /// <summary>
    /// Returns the string encoded in the specified array.
    /// </summary>
    /// <param name="array">The array to read.</param>
    /// <param name="ink">The char that signifies a value is part of a written character.</param>
    /// <returns>
    /// A string containing the characters encoded in <paramref name="array"/>.
    /// </returns>
    /// <remarks>
    /// Each encoded character is assumed to be a 5x6 grid.
    /// </remarks>
    public static string Read(char[,] array, char ink = '*')
    {
        int width = array.GetLength(0);
        int height = array.GetLength(1);

        bool[,] bits = new bool[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                bits[i, j] = array[i, j] == ink;
            }
        }

        return Read(bits);
    }

    /// <summary>
    /// Returns the string encoded in the specified array.
    /// </summary>
    /// <param name="array">The array to read.</param>
    /// <param name="ink">The integer that signifies a value is part of a written character.</param>
    /// <returns>
    /// A string containing the characters encoded in <paramref name="array"/>.
    /// </returns>
    /// <remarks>
    /// Each encoded character is assumed to be a 5x6 grid.
    /// </remarks>
    public static string Read(int[,] array, int ink = 1)
    {
        int width = array.GetLength(0);
        int height = array.GetLength(1);

        bool[,] bits = new bool[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                bits[i, j] = array[i, j] == ink;
            }
        }

        return Read(bits);
    }

    /// <summary>
    /// Returns the string encoded in the specified array.
    /// </summary>
    /// <param name="array">The array to read.</param>
    /// <returns>
    /// A string containing the characters encoded in <paramref name="array"/>.
    /// </returns>
    /// <remarks>
    /// Each encoded character is assumed to be a 5x6 grid.
    /// </remarks>
    public static string Read(bool[,] array)
    {
        int width = array.GetLength(0);
        int height = array.GetLength(1);

        if (width % 5 != 0 || height % 6 != 0)
        {
            return string.Empty;
        }

        var builder = new StringBuilder(width);

        for (uint offset = 0; offset < width - 1; offset += 5)
        {
            uint letter = 0;
            int bit = 31;

            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (array[offset + x, y])
                    {
                        letter |= 1u << bit;
                    }

                    bit--;
                }
            }

            builder.Append(Alphabet.Get(letter));
        }

        return builder.ToString();
    }

    private static class Alphabet
    {
        private static readonly Dictionary<uint, char> _alphabet = CreateAlphabet();

        public static char Get(uint value)
        {
            if (!_alphabet.TryGetValue(value, out char letter))
            {
                letter = '?';
            }

            return letter;
        }

        private static Dictionary<uint, char> CreateAlphabet()
        {
            // Letters are encoded as a 32-bit unsigned integer
            // where the top-left pixel is the 32nd bit and the
            // bottom-right pixel is the 3rd bit, then two zeroes.
            // The bits are encoded top-to-bottom, left-to-right.
            return new Dictionary<uint, char>(26)
            {
                [0b01111110010010010001111100000000] = 'A',
                [0b11111110100110100101011000000000] = 'B',
                [0b01111010000110000101001000000000] = 'C',
                [0b11111110100110100110000100000000] = 'E',
                [0b11111110100010100010000000000000] = 'F',
                [0b01111010000110010101011100000000] = 'G',
                [0b11111100100000100011111100000000] = 'H',
                [0b00000010000111111110000100000000] = 'I',
                [0b00001000000110000111111000000000] = 'J',
                [0b11111100100001011010000100000000] = 'K',
                [0b11111100000100000100000100000000] = 'L',
                [0b01111010000110000101111000000000] = 'O',
                [0b11111110010010010001100000000000] = 'P',
                [0b11111110010010011001100100000000] = 'R',
                [0b11111000000100000111111000000000] = 'U',
                [0b11000000100000011100100011000000] = 'Y',
                [0b10001110010110100111000100000000] = 'Z',
            };
        }
    }
}
