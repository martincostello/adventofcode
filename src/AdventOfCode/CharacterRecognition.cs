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
                [0b_01111110_01001001_00011111_00000000] = 'A',
                [0b_11111110_10011010_01010110_00000000] = 'B',
                [0b_01111010_00011000_01010010_00000000] = 'C',
                [0b_11111110_10011010_01100001_00000000] = 'E',
                [0b_11111110_10001010_00100000_00000000] = 'F',
                [0b_01111010_00011001_01010111_00000000] = 'G',
                [0b_11111100_10000010_00111111_00000000] = 'H',
                [0b_00000010_00011111_11100001_00000000] = 'I',
                [0b_00001000_00011000_01111110_00000000] = 'J',
                [0b_11111100_10000101_10100001_00000000] = 'K',
                [0b_11111100_00010000_01000001_00000000] = 'L',
                [0b_01111010_00011000_01011110_00000000] = 'O',
                [0b_11111110_01001001_00011000_00000000] = 'P',
                [0b_11111110_01001001_10011001_00000000] = 'R',
                [0b_11111000_00010000_01111110_00000000] = 'U',
                [0b_11000000_10000001_11001000_11000000] = 'Y',
                [0b_10001110_01011010_01110001_00000000] = 'Z',
            };
        }
    }
}
