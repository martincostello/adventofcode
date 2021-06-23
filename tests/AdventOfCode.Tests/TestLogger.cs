// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    /// <summary>
    /// A class representing an implementation of <see cref="ILogger"/> that
    /// logs to the XUnit output. This class cannot be inherited.
    /// </summary>
    internal sealed class TestLogger : ILogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestLogger"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        internal TestLogger(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        /// <summary>
        /// Gets the <see cref="ITestOutputHelper"/> to use.
        /// </summary>
        private ITestOutputHelper OutputHelper { get; }

        /// <inheritdoc />
        public string WriteGrid(bool[,] array, char falseChar, char trueChar)
        {
            var builder = new StringBuilder().AppendLine();

            int lengthX = array.GetLength(0);
            int lengthY = array.GetLength(1);

            for (int x = 0; x < lengthX; x++)
            {
                for (int y = 0; y < lengthY; y++)
                {
                    builder.Append(array[x, y] ? trueChar : falseChar);
                }

                builder.AppendLine();
            }

            builder.AppendLine();

            string result = builder.ToString();

            OutputHelper.WriteLine(result);

            return result;
        }

        /// <inheritdoc />
        public void WriteLine(string format, params object[] args)
            => OutputHelper.WriteLine(format, args);
    }
}
