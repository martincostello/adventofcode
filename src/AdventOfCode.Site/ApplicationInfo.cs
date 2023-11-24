// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Site;

/// <summary>
/// A class representing information about the application.
/// </summary>
public sealed class ApplicationInfo
{
    /// <summary>
    /// Gets or sets the application version.
    /// </summary>
    public string ApplicationVersion { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the framework description.
    /// </summary>
    public string FrameworkDescription { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the operating system.
    /// </summary>
    public string OperatingSystem { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the operating system architecture.
    /// </summary>
    public string OperatingSystemArchitecture { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the process architecture.
    /// </summary>
    public string ProcessArchitecture { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the .NET runtime version.
    /// </summary>
    public string DotNetRuntimeVersion { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ASP.NET Core version.
    /// </summary>
    public string AspNetCoreVersion { get; set; } = string.Empty;
}
