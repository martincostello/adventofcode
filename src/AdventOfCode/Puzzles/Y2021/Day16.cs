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
    public long VersionNumberSum { get; private set; }

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
    public static (long VersionNumberSum, long Value) Decode(string transmission)
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

        long result = ReadPacket(bits, out _, out long versionSum);

        return (versionSum, result);

        static long ReadPacket(ReadOnlySpan<bool> bits, out int bitsRead, out long versionSum)
        {
            bitsRead = 0;
            versionSum = 0;

            long version = ReadInteger(bits[..3]);
            bitsRead += 3;

            long typeId = ReadInteger(bits.Slice(bitsRead, 3));
            bitsRead += 3;

            versionSum += version;

            if (typeId == 4)
            {
                long value = ReadLiteral(bits[bitsRead..], out int literalBitsRead);
                bitsRead += literalBitsRead;

                return value;
            }
            else
            {
                long lengthTypeId = ReadInteger(bits.Slice(bitsRead, 1));
                bitsRead++;

                var values = new List<long>();

                if (lengthTypeId == 0)
                {
                    const int TotalLengthBits = 15;

                    long length = ReadInteger(bits.Slice(bitsRead, TotalLengthBits));
                    bitsRead += TotalLengthBits;

                    int index = bitsRead;
                    long toRead = length;

                    while (toRead > 0)
                    {
                        long childValue = ReadPacket(bits[index..], out int childBitsRead, out long childVersion);
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

                    long subpackets = ReadInteger(bits.Slice(bitsRead, SubpacketCountBits));
                    bitsRead += SubpacketCountBits;

                    int index = bitsRead;

                    for (int i = 0; i < subpackets; i++)
                    {
                        long childValue = ReadPacket(bits[index..], out int childBitsRead, out long childVersion);
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
                    5 => values[0] > values[1] ? 1L : 0,
                    6 => values[0] < values[1] ? 1L : 0,
                    7 => values[0] == values[1] ? 1L : 0,
                    _ => throw new PuzzleException($"The type ID {typeId} is invalid."),
                };
            }
        }

        static long ReadLiteral(ReadOnlySpan<bool> bits, out int bitsRead)
        {
            int length = 0;

            for (int i = 0; i < bits.Length; i += 5)
            {
                length += 4;

                if (!bits[i])
                {
                    break;
                }
            }

            bool[] valueBits = new bool[length];

            int index = 0;

            for (int i = 0; i < bits.Length; i += 5)
            {
                bool isLast = !bits[i];

                valueBits[index++] = bits[i + 1];
                valueBits[index++] = bits[i + 2];
                valueBits[index++] = bits[i + 3];
                valueBits[index++] = bits[i + 4];

                if (isLast)
                {
                    break;
                }
            }

            bitsRead = length + (length / 4);
            return ReadInteger(valueBits);
        }

        static long ReadInteger(ReadOnlySpan<bool> bits)
        {
            long value = 0;

            for (int i = 0; i < bits.Length; i++)
            {
                SetBit(ref value, i, bits[^(i + 1)] ? 1 : 0);
            }

            return value;
        }

        static void SetBit(ref long reference, int bit, long value)
            => reference |= value << bit;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        string transmission = (await ReadResourceAsStringAsync(cancellationToken)).Trim();

        (VersionNumberSum, Value) = Decode(transmission);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the version numbers in all packets is {0:N0}.", VersionNumberSum);
            Logger.WriteLine("The result of evaluating the transmission is {0:N0}.", Value);
        }

        return PuzzleResult.Create(VersionNumberSum, Value);
    }
}
