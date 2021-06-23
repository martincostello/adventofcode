// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Threading.Channels;

namespace MartinCostello.AdventOfCode
{
    /// <summary>
    /// A class containing helper methods for use with channels. This class cannot be inherited.
    /// </summary>
    internal static class ChannelHelpers
    {
        /// <summary>
        /// Creates a channel reader for the specified values as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">The type of the values in the channel.</typeparam>
        /// <param name="values">The values to return from the reader.</param>
        /// <param name="cancellationToken">The cancellation token to use.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that returns a channel reader that returned the specified values.
        /// </returns>
        internal static async Task<ChannelReader<T>> CreateReaderAsync<T>(
            IEnumerable<T>? values,
            CancellationToken cancellationToken)
        {
            var channel = Channel.CreateUnbounded<T>();

            if (values != null)
            {
                foreach (T value in values)
                {
                    await channel.Writer.WriteAsync(value, cancellationToken);
                }
            }

            channel.Writer.Complete();

            return channel.Reader;
        }

        /// <summary>
        /// Returns a read-only list containing all the values in the channel as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">The type of the values in the channel.</typeparam>
        /// <param name="reader">The reader to read the values from .</param>
        /// <param name="cancellationToken">The cancellation token to use.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that returns a list containing the values read from the channel.
        /// </returns>
        internal static async Task<IReadOnlyList<T>> ToListAsync<T>(
            this ChannelReader<T> reader,
            CancellationToken cancellationToken)
        {
            var values = new List<T>();

            await foreach (T output in reader.ReadAllAsync(cancellationToken))
            {
                values.Add(output);
            }

            return values;
        }
    }
}
