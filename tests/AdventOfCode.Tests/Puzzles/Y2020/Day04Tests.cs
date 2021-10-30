// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class containing tests for the <see cref="Day04"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day04Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day04Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day04Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void Y2020_Day04_VerifyPassports_Returns_Correct_Valid_Value()
    {
        // Arrange
        string[] batch = new[]
        {
            "ecl:gry pid:860033327 eyr:2020 hcl:#fffffd",
            "byr:1937 iyr:2017 cid:147 hgt:183cm",
            string.Empty,
            "iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884",
            "hcl:#cfa07d byr:1929",
            string.Empty,
            "hcl:#ae17e1 iyr:2013",
            "eyr:2024",
            "ecl:brn pid:760753108 byr:1931",
            "hgt:179cm",
            string.Empty,
            "hcl:#cfa07d eyr:2025 pid:166559648",
            "iyr:2011 ecl:brn hgt:59in",
        };

        // Act
        (int actual, _) = Day04.VerifyPassports(batch);

        // Assert
        actual.ShouldBe(2);
    }

    [Fact]
    public void Y2020_Day04_VerifyPassports_Returns_Correct_Verified_Value_For_Invalid_Passports()
    {
        // Arrange
        string[] batch = new[]
        {
            "eyr:1972 cid:100",
            "hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926",
            string.Empty,
            "iyr:2019",
            "hcl:#602927 eyr:1967 hgt:170cm",
            "ecl:grn pid:012533040 byr:1946",
            string.Empty,
            "hcl:dab227 iyr:2012",
            "ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277",
            string.Empty,
            "hgt:59cm ecl:zzz",
            "eyr:2038 hcl:74454a iyr:2023",
            "pid:3556412378 byr:2007",
        };

        // Act
        (_, int actual) = Day04.VerifyPassports(batch);

        // Assert
        actual.ShouldBe(0);
    }

    [Fact]
    public void Y2020_Day04_VerifyPassports_Returns_Correct_Verified_Value_For_Valid_Passports()
    {
        // Arrange
        string[] batch = new[]
        {
            "pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980",
            "hcl:#623a2f",
            string.Empty,
            "eyr:2029 ecl:blu cid:129 byr:1989",
            "iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm",
            string.Empty,
            "hcl:#888785",
            "hgt:164cm byr:2001 iyr:2015 cid:88",
            "pid:545766238 ecl:hzl",
            "eyr:2022",
            string.Empty,
            "iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719",
        };

        // Act
        (_, int actual) = Day04.VerifyPassports(batch);

        // Assert
        actual.ShouldBe(4);
    }

    [Fact]
    public async Task Y2020_Day04_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day04>();

        // Assert
        puzzle.ValidPassports.ShouldBe(226);
        puzzle.VerifiedPassports.ShouldBe(160);
    }
}
