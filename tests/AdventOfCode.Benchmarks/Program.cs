﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using BenchmarkDotNet.Running;
using MartinCostello.AdventOfCode.Benchmarks;

var summary = BenchmarkRunner.Run<PuzzleBenchmarks>(args: args);
return summary.Reports.Any((p) => !p.Success) ? 1 : 0;
