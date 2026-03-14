# Coding Agent Instructions

This file provides guidance to coding agents when working with code in this repository.

## Build, test, and lint commands

- Use `./build.ps1` for the main .NET workflow. It builds the solution, runs the .NET tests, and publishes the site/Lambda package.
- Use `./build.ps1 -SkipPublish` when you only need the .NET build and test steps.
- Run the .NET test project directly with `dotnet test tests\AdventOfCode.Tests\AdventOfCode.Tests.csproj`.
- Run a single .NET test with a filter, for example:
  `dotnet test tests\AdventOfCode.Tests\AdventOfCode.Tests.csproj --filter "FullyQualifiedName~MartinCostello.AdventOfCode.Puzzles.Y2025.Day12Tests.Y2025_Day12_Arrange_Returns_Correct_Value"`
- The web assets and browser-side puzzle solvers live under `src\AdventOfCode.Site`. On a clean checkout, run `npm ci` in that directory before any npm scripts.
- Run the site asset pipeline with `cd src\AdventOfCode.Site && npm run all`. This runs `npm run publish` and `npm test`.
- Use `cd src\AdventOfCode.Site && npm run build`, `npm run lint`, and `npm test` for narrower TypeScript work.
- Run a single TypeScript test file with:
  `cd src\AdventOfCode.Site && npm test -- scripts/ts/puzzles/DefaultPuzzleFactory.test.ts`
- The site project copies `coverage\**` into build output. If a standalone `dotnet test` fails trying to copy `src\AdventOfCode.Site\coverage\coverage-final.json`, run `npm test` in `src\AdventOfCode.Site` once first.

## High-level architecture

- `src\AdventOfCode` is the core library. It contains the puzzle abstractions, shared algorithms/helpers, and all C# puzzle implementations under `Puzzles\Y####\Day##.cs`.
- `src\AdventOfCode.Resources` contains the embedded puzzle inputs under `Input\**\input.txt`. The core puzzle base classes read these resources by convention.
- `src\AdventOfCode.Console` is a Spectre.Console runner built on `PuzzleFactory`. It can solve one day or an entire year, and year-wide runs skip puzzles marked hidden, slow, unsolved, or requiring explicit arguments.
- `src\AdventOfCode.Site` is an ASP.NET Core minimal API with RazorSlices views. `Program.GetPuzzles()` reflects over the puzzle assembly, registers puzzle metadata, and exposes `/api/puzzles` plus `/api/puzzles/{year}/{day}/solve`.
- The site API executes C# puzzles through `PuzzleFactory`, accepts optional form arguments and an uploaded input file, and enforces a 30 second solve timeout in `PuzzlesApi`.
- The browser app is in `src\AdventOfCode.Site\scripts\ts`. `App.ts` loads puzzle metadata from the API, then chooses either a client-side TypeScript solver or the server API.
- Client-side solvers are registered through `scripts\ts\puzzles\DefaultPuzzleFactory.ts` and per-year TypeScript factories. If a puzzle is not wired there, the UI falls back to the server-side C# implementation.
- `tests\AdventOfCode.Tests` covers multiple layers: puzzle-specific tests by year/day, metadata/invariant tests, API integration tests through `WebApplicationFactory<Site.Program>`, and Playwright-based UI tests. The deployed-site end-to-end tests use `WEBSITE_URL` and skip when it is not set.

## Key conventions

- C# puzzle discovery is naming-based. Keep the namespace as `MartinCostello.AdventOfCode.Puzzles.Y{year}`, the type name as `Day{day:00}`, and the `[Puzzle(year, day, ...)]` metadata aligned. `PuzzleFactory` builds the type name from the year/day and resolves it via reflection.
- Default puzzle input loading is also convention-based. `Puzzle.ReadResource()` derives the year from the namespace and the day from the class name, so new puzzles must follow the existing namespace/class layout to pick up embedded inputs correctly.
- `PuzzleAttribute` flags drive behavior across the console app and the site. `IsHidden` and `Unsolved` remove puzzles from the site list, while `IsHidden`, `IsSlow`, `Unsolved`, and `MinimumArguments > 0` cause year-wide console runs to skip them.
- When adding a browser-side solver, update the matching year-specific TypeScript puzzle factory and `DefaultPuzzleFactory.ts`; adding only the C# puzzle is not enough for the browser to use a client solver.
- Shared puzzle test helpers live in `tests\AdventOfCode.Tests\Puzzles\PuzzleTest.cs`, and broad expected-answer coverage is centralized in `tests\AdventOfCode.Tests\Api\ApiTests.cs`.
- Repo-wide MSBuild settings come from `Directory.Build.props`, including global usings for `System.Collections.Frozen`, `System.Collections.Immutable`, `System.Numerics`, and `Microsoft.Toolkit.HighPerformance`.
- `build.ps1` is the safest entry point for .NET work because it honors `global.json` and will install the required SDK into `.dotnet` when the exact version is not already available.

## General guidelines

- Always ensure code compiles with no warnings or errors and tests pass locally before pushing changes.
- Do not change the public API unless specifically requested.
- Do not use APIs marked with `[Obsolete]`.
- Bug fixes should **always** include a test that would fail without the corresponding fix.
- Do not introduce new dependencies unless specifically requested.
- Do not update existing dependencies unless specifically requested.
