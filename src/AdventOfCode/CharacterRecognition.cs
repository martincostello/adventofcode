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

        var builder = new StringBuilder();

        for (int x = 0; x < width - 1; x += 5)
        {
            var letter = new HashSet<Point>();

            for (int y = 0; y < height; y++)
            {
                if (array[x, y])
                {
                    letter.Add(new(0, y));
                }

                if (array[x + 1, y])
                {
                    letter.Add(new(1, y));
                }

                if (array[x + 2, y])
                {
                    letter.Add(new(2, y));
                }

                if (array[x + 3, y])
                {
                    letter.Add(new(3, y));
                }

                if (array[x + 4, y])
                {
                    letter.Add(new(4, y));
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
            Dictionary<char, HashSet<Point>> alphabet = new(26);

            alphabet['A'] = new HashSet<Point>()
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
            };

            alphabet['B'] = new HashSet<Point>()
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
            };

            alphabet['C'] = new HashSet<Point>()
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
            };

            alphabet['E'] = new HashSet<Point>()
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
            };

            alphabet['F'] = new HashSet<Point>()
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
            };

            alphabet['G'] = new HashSet<Point>()
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
            };

            alphabet['H'] = new HashSet<Point>()
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
            };

            alphabet['I'] = new HashSet<Point>()
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
            };

            alphabet['J'] = new HashSet<Point>()
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
            };

            alphabet['K'] = new HashSet<Point>()
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
            };

            alphabet['L'] = new HashSet<Point>()
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
            };

            alphabet['O'] = new HashSet<Point>()
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
            };

            alphabet['P'] = new HashSet<Point>()
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
            };

            alphabet['R'] = new HashSet<Point>()
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
            };

            alphabet['U'] = new HashSet<Point>()
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
            };

            alphabet['Y'] = new HashSet<Point>()
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
            };

            alphabet['Z'] = new HashSet<Point>()
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
            };

            return alphabet;
        }
    }
}
