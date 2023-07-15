// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode;

/// <summary>
/// A class representing an implementation of <see cref="ILogger"/> that
/// logs to the XUnit output. This class cannot be inherited.
/// </summary>
/// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
internal sealed class TestLogger(ITestOutputHelper outputHelper) : ILogger
{
    /// <inheritdoc />
    public string WriteGrid(bool[,] array, char falseChar, char trueChar)
    {
        int lengthX = array.GetLength(0);
        int lengthY = array.GetLength(1);

        var builder = new StringBuilder(((lengthX + 2) * lengthY) + 4)
            .AppendLine();

        for (int y = 0; y < lengthY; y++)
        {
            foreach (bool value in array.GetColumn(y))
            {
                builder.Append(value ? trueChar : falseChar);
            }

            builder.AppendLine();
        }

        builder.AppendLine();

        string result = builder.ToString();

        outputHelper.WriteLine(result);

        return result;
    }

    /// <inheritdoc />
    public void WriteLine(string format, params object[] args)
        => outputHelper.WriteLine(format, args);
}
