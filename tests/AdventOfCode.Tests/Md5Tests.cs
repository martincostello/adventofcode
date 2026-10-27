// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Security.Cryptography;

namespace MartinCostello.AdventOfCode;

public static class Md5Tests
{
    [Theory]
    [InlineData("", "d41d8cd98f00b204e9800998ecf8427e")]
    [InlineData("a", "0cc175b9c0f1b6a831c399e269772661")]
    [InlineData("abc", "900150983cd24fb0d6963f7d28e17f72")]
    [InlineData("message digest", "f96b697d7cb7938d525a2f31aaf161d0")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "c3fcd3d76192e4007dfb496cca67e13b")]
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", "d174ab98d277d9f5a5611c2c9f419d9f")]
    [InlineData("12345678901234567890123456789012345678901234567890123456789012345678901234567890", "57edf4a22be3c955ac49da2e2107b67a")]
    [InlineData("abcdef609043", "000001dbbfa3a5c83a2d506429c7b00e")]
    [InlineData("pqrstuv1048970", "000006136ef2ff3b291c85725f17325c")]
    public static void Md5_HashData_Returns_Correct_Hash(string value, string expected)
    {
        // Arrange
        byte[] source = Encoding.ASCII.GetBytes(value);
        byte[] actual = new byte[Md5.HashSizeInBytes];

        // Act
        Md5.HashData(source, actual);

        // Assert
        Convert.ToHexStringLower(actual).ShouldBe(expected);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(55)]
    [InlineData(56)]
    [InlineData(63)]
    [InlineData(64)]
    [InlineData(65)]
    [InlineData(119)]
    [InlineData(120)]
    [InlineData(128)]
    [InlineData(1000)]
    public static void Md5_HashData_Matches_Runtime_Implementation(int length)
    {
        // Arrange
        byte[] source = new byte[length];

        for (int i = 0; i < source.Length; i++)
        {
            source[i] = (byte)((i * 31) + length);
        }

#pragma warning disable CA5351
        byte[] expected = MD5.HashData(source);
#pragma warning restore CA5351
        byte[] actual = new byte[Md5.HashSizeInBytes];

        // Act
        Md5.HashData(source, actual);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public static void Md5_HashData_Throws_If_Destination_Too_Small()
    {
        // Arrange
        byte[] source = [1, 2, 3];
        byte[] destination = new byte[Md5.HashSizeInBytes - 1];

        // Act and Assert
        Should.Throw<ArgumentOutOfRangeException>(() => Md5.HashData(source, destination));
    }
}
