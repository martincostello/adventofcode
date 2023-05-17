﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

#pragma warning disable CA1852

using BenchmarkDotNet.Running;
using MartinCostello.AdventOfCode.Benchmarks;

BenchmarkRunner.Run<PuzzleBenchmarks>(args: args);
