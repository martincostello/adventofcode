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

        for (int x = 0; x < width - 1; x += 5)
        {
            var letter = new HashSet<Point>(15);

            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < 5; z++)
                {
                    if (array[x + z, y])
                    {
                        letter.Add(new(z, y));
                    }
                }
            }

            builder.Append(Alphabet.Get(letter));
        }

        return builder.ToString();
    }

    private static class Alphabet
    {
        private static readonly Dictionary<char, HashSet<Point>> _alphabet = CreateAlphabet();

        public static char Get(HashSet<Point> points)
        {
            foreach ((char letter, var glyph) in _alphabet)
            {
                if (points.SequenceEqual(glyph))
                {
                    return letter;
                }
            }

            return '?';
        }

        private static Dictionary<char, HashSet<Point>> CreateAlphabet()
        {
            Dictionary<char, HashSet<Point>> alphabet = new(26)
            {
                ['A'] = new(14)
                {
                    new(1, 0),
                    new(2, 0),
                    new(0, 1),
                    new(3, 1),
                    new(0, 2),
                    new(3, 2),
                    new(0, 3),
                    new(1, 3),
                    new(2, 3),
                    new(3, 3),
                    new(0, 4),
                    new(3, 4),
                    new(0, 5),
                    new(3, 5),
                },
                ['B'] = new(15)
                {
                    new(0, 0),
                    new(1, 0),
                    new(2, 0),
                    new(0, 1),
                    new(3, 1),
                    new(0, 2),
                    new(1, 2),
                    new(2, 2),
                    new(0, 3),
                    new(3, 3),
                    new(0, 4),
                    new(3, 4),
                    new(0, 5),
                    new(1, 5),
                    new(2, 5),
                },
                ['C'] = new(10)
                {
                    new(1, 0),
                    new(2, 0),
                    new(0, 1),
                    new(3, 1),
                    new(0, 2),
                    new(0, 3),
                    new(0, 4),
                    new(3, 4),
                    new(1, 5),
                    new(2, 5),
                },
                ['E'] = new(14)
                {
                    new(0, 0),
                    new(1, 0),
                    new(2, 0),
                    new(3, 0),
                    new(0, 1),
                    new(0, 2),
                    new(1, 2),
                    new(2, 2),
                    new(0, 3),
                    new(0, 4),
                    new(0, 5),
                    new(1, 5),
                    new(2, 5),
                    new(3, 5),
                },
                ['F'] = new(11)
                {
                    new(0, 0),
                    new(1, 0),
                    new(2, 0),
                    new(3, 0),
                    new(0, 1),
                    new(0, 2),
                    new(1, 2),
                    new(2, 2),
                    new(0, 3),
                    new(0, 4),
                    new(0, 5),
                },
                ['G'] = new(13)
                {
                    new(1, 0),
                    new(2, 0),
                    new(0, 1),
                    new(3, 1),
                    new(0, 2),
                    new(0, 3),
                    new(2, 3),
                    new(3, 3),
                    new(0, 4),
                    new(3, 4),
                    new(1, 5),
                    new(2, 5),
                    new(3, 5),
                },
                ['H'] = new(14)
                {
                    new(0, 0),
                    new(3, 0),
                    new(0, 1),
                    new(3, 1),
                    new(0, 2),
                    new(1, 2),
                    new(2, 2),
                    new(3, 2),
                    new(0, 3),
                    new(3, 3),
                    new(0, 4),
                    new(3, 4),
                    new(0, 5),
                    new(3, 5),
                },
                ['I'] = new(10)
                {
                    new(1, 0),
                    new(2, 0),
                    new(3, 0),
                    new(2, 1),
                    new(2, 2),
                    new(2, 3),
                    new(2, 4),
                    new(1, 5),
                    new(2, 5),
                    new(3, 5),
                },
                ['J'] = new(9)
                {
                    new(2, 0),
                    new(3, 0),
                    new(3, 1),
                    new(3, 2),
                    new(3, 3),
                    new(0, 4),
                    new(3, 4),
                    new(1, 5),
                    new(2, 5),
                },
                ['K'] = new(12)
                {
                    new(0, 0),
                    new(3, 0),
                    new(0, 1),
                    new(2, 1),
                    new(0, 2),
                    new(1, 2),
                    new(0, 3),
                    new(2, 3),
                    new(0, 4),
                    new(2, 4),
                    new(0, 5),
                    new(3, 5),
                },
                ['L'] = new(9)
                {
                    new(0, 0),
                    new(0, 1),
                    new(0, 2),
                    new(0, 3),
                    new(0, 4),
                    new(0, 5),
                    new(1, 5),
                    new(2, 5),
                    new(3, 5),
                },
                ['O'] = new(12)
                {
                    new(1, 0),
                    new(2, 0),
                    new(0, 1),
                    new(3, 1),
                    new(0, 2),
                    new(3, 2),
                    new(0, 3),
                    new(3, 3),
                    new(0, 4),
                    new(3, 4),
                    new(1, 5),
                    new(2, 5),
                },
                ['P'] = new(12)
                {
                    new(0, 0),
                    new(1, 0),
                    new(2, 0),
                    new(0, 1),
                    new(3, 1),
                    new(0, 2),
                    new(3, 2),
                    new(0, 3),
                    new(1, 3),
                    new(2, 3),
                    new(0, 4),
                    new(0, 5),
                },
                ['R'] = new(14)
                {
                    new(0, 0),
                    new(1, 0),
                    new(2, 0),
                    new(0, 1),
                    new(3, 1),
                    new(0, 2),
                    new(3, 2),
                    new(0, 3),
                    new(1, 3),
                    new(2, 3),
                    new(0, 4),
                    new(2, 4),
                    new(0, 5),
                    new(3, 5),
                },
                ['U'] = new(12)
                {
                    new(0, 0),
                    new(3, 0),
                    new(0, 1),
                    new(3, 1),
                    new(0, 2),
                    new(3, 2),
                    new(0, 3),
                    new(3, 3),
                    new(0, 4),
                    new(3, 4),
                    new(1, 5),
                    new(2, 5),
                },
                ['Y'] = new(9)
                {
                    new(0, 0),
                    new(4, 0),
                    new(0, 1),
                    new(4, 1),
                    new(1, 2),
                    new(3, 2),
                    new(2, 3),
                    new(2, 4),
                    new(2, 5),
                },
                ['Z'] = new(12)
                {
                    new(0, 0),
                    new(1, 0),
                    new(2, 0),
                    new(3, 0),
                    new(3, 1),
                    new(2, 2),
                    new(1, 3),
                    new(0, 4),
                    new(0, 5),
                    new(1, 5),
                    new(2, 5),
                    new(3, 5),
                },
            };

            return alphabet;
        }
    }
}
