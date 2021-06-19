// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// A class containing metadata for the assembly.
    /// </summary>
    public static class GitMetadata
    {
        /// <summary>
        /// Gets the branch name.
        /// </summary>
        public static string Branch { get; } = GetMetadataValue("CommitBranch", "Unknown");

        /// <summary>
        /// Gets the commit name.
        /// </summary>
        public static string Commit { get; } = GetMetadataValue("CommitHash", "HEAD");

        /// <summary>
        /// Gets the build Id.
        /// </summary>
        public static string BuildId { get; } = GetMetadataValue("BuildId", "Unknown");

        /// <summary>
        /// Gets the build timestamp.
        /// </summary>
        public static DateTime Timestamp { get; } = DateTime.Parse(GetMetadataValue("BuildTimestamp", DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

        /// <summary>
        /// Gets the assembly metadata value with the specified name.
        /// </summary>
        /// <param name="name">The name of the metadata value to return.</param>
        /// <param name="defaultValue">The default value to use.</param>
        /// <returns>
        /// The value of the metadata with the name specified by <paramref name="name"/>,
        /// if found, otherwise the value of <paramref name="defaultValue"/>.
        /// </returns>
        private static string GetMetadataValue(string name, string defaultValue)
        {
            return typeof(GitMetadata).Assembly
                .GetCustomAttributes<AssemblyMetadataAttribute>()
                .Where((p) => string.Equals(p.Key, name, StringComparison.Ordinal))
                .Select((p) => p.Value)
                .FirstOrDefault() ?? defaultValue;
        }
    }
}
