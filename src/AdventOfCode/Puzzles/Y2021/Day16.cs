// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Collections;

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/16</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 16, "Packet Decoder", RequiresData = true)]
public sealed class Day16 : Puzzle
{
    /// <summary>
    /// Gets the sum of the packet version numbers.
    /// </summary>
    public int VersionNumberSum { get; private set; }

    /// <summary>
    /// Gets the evaluated value of the transmission.
    /// </summary>
    public long Value { get; private set; }

    /// <summary>
    /// Decodes the packets in the specified transmission.
    /// </summary>
    /// <param name="transmission">The transmission to decode.</param>
    /// <returns>
    /// The sum of the packet version numbers and the value of the evaluated transmission.
    /// </returns>
    public static (int VersionNumberSum, long Value) Decode(string transmission)
    {
        bool[] bits = new bool[transmission.Length * 4];

        for (int i = 0; i < transmission.Length; i++)
        {
            int value = Parse<int>(transmission[i].ToString(), NumberStyles.HexNumber);

            byte[] bytes = BitConverter.GetBytes(value);
            var array = new BitArray(bytes);

            int offset = i * 4;

            bits[offset] = array[3];
            bits[offset + 1] = array[2];
            bits[offset + 2] = array[1];
            bits[offset + 3] = array[0];
        }

        long result = ReadPacket(bits, out _, out int versionSum);

        return (versionSum, result);

        static long ReadPacket(ReadOnlySpan<bool> bits, out int bitsRead, out int versionSum)
        {
            bitsRead = 0;
            versionSum = 0;

            int version = ReadInteger(bits[..3]);
            bitsRead += 3;

            int typeId = ReadInteger(bits.Slice(bitsRead, 3));
            bitsRead += 3;

            versionSum += version;

            if (typeId == 4)
            {
                int value = ReadLiteral(bits[bitsRead..], out int literalBitsRead);
                bitsRead += literalBitsRead;

                return value;
            }
            else
            {
                int lengthTypeId = ReadInteger(bits.Slice(bitsRead, 1));
                bitsRead++;

                var values = new List<long>();

                if (lengthTypeId == 0)
                {
                    const int TotalLengthBits = 15;

                    int length = ReadInteger(bits.Slice(bitsRead, TotalLengthBits));
                    bitsRead += TotalLengthBits;

                    int index = bitsRead;
                    int toRead = length;

                    while (toRead > 0)
                    {
                        long childValue = ReadPacket(bits[index..], out int childBitsRead, out int childVersion);
                        bitsRead += childBitsRead;

                        versionSum += childVersion;

                        values.Add(childValue);

                        index += childBitsRead;
                        toRead -= childBitsRead;
                    }
                }
                else
                {
                    const int SubpacketCountBits = 11;

                    int subpackets = ReadInteger(bits.Slice(bitsRead, SubpacketCountBits));
                    bitsRead += SubpacketCountBits;

                    int index = bitsRead;

                    for (int i = 0; i < subpackets; i++)
                    {
                        long childValue = ReadPacket(bits[index..], out int childBitsRead, out int childVersion);
                        bitsRead += childBitsRead;

                        values.Add(childValue);

                        versionSum += childVersion;
                        index += childBitsRead;
                    }
                }

                return typeId switch
                {
                    0 => values.Sum(),
                    1 => values.Aggregate(1L, (x, y) => x * y),
                    2 => values.Min(),
                    3 => values.Max(),
                    5 => values[0] > values[1] ? 1 : 0,
                    6 => values[0] < values[1] ? 1 : 0,
                    7 => values[0] == values[1] ? 1 : 0,
                    _ => throw new PuzzleException($"The type ID {typeId} is invalid."),
                };
            }
        }

        static int ReadLiteral(ReadOnlySpan<bool> bits, out int bitsRead)
        {
            // TODO Just allocate an array, then slice at the end to avoid the copy
            var chunks = new List<bool>();

            for (int i = 0; i < bits.Length; i += 5)
            {
                bool isLast = !bits[i];

                chunks.Add(bits[i + 1]);
                chunks.Add(bits[i + 2]);
                chunks.Add(bits[i + 3]);
                chunks.Add(bits[i + 4]);

                if (isLast)
                {
                    break;
                }
            }

            bitsRead = chunks.Count + (chunks.Count / 4);
            return ReadInteger(chunks.ToArray());
        }

        static int ReadInteger(ReadOnlySpan<bool> bits)
        {
            int value = 0;

            for (int i = 0; i < bits.Length; i++)
            {
                SetBit(ref value, i, bits[^(i + 1)] ? 1 : 0);
            }

            return value;
        }

        static void SetBit(ref int reference, int bit, int value)
            => reference |= value << bit;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        string transmission = (await ReadResourceAsStringAsync()).Trim();

        (VersionNumberSum, Value) = Decode(transmission);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the version numbers in all packets is {0:N0}.", VersionNumberSum);
            Logger.WriteLine("The result of evaluating the transmission is {0:N0}.", Value);
        }

        return PuzzleResult.Create(VersionNumberSum);
    }
}
