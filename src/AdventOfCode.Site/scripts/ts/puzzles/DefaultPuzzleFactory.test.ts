// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { existsSync, readFileSync } from 'fs';
import { join, resolve } from 'path';
import { DefaultPuzzleFactory } from './index';

const factory = new DefaultPuzzleFactory();
const solve = async (year: number, day: number, inputs: string[] = []): Promise<any[]> => {
    const toNumberWithDigits = (value: number, digits: number): string => {
        return value.toLocaleString('en-US', {
            minimumIntegerDigits: digits,
            useGrouping: false,
        });
    };

    const puzzle = factory.create(year, day);

    expect(puzzle).not.toBeNull();
    expect(puzzle.year).toBe(year);
    expect(puzzle.day).toBe(day);

    const yearString = toNumberWithDigits(puzzle.year, 4);
    const dayString = toNumberWithDigits(puzzle.day, 2);

    let input = join(__dirname, `../../../../AdventOfCode.Resources/Input/Y${yearString}/Day${dayString}/input.txt`);
    input = resolve(input);

    if (existsSync(input)) {
        puzzle.resource = readFileSync(input, 'utf-8');
    }

    // Act
    const result = await puzzle.solve(inputs);

    // Assert
    expect(result).not.toBeNull();
    expect(result.solutions).not.toBeNull();
    expect(result.solutions.length).toBeGreaterThan(0);
    expect(puzzle.name).not.toBeNull();
    expect(puzzle.name).not.toBe('');

    return result.solutions;
};

describe('Puzzles', () => {
    test.each([
        [2015, 1, [], [232, 1783]],
        [2015, 2, [], [1598415, 3812909]],
        [2015, 3, [], [2565, 2639]],
        [2015, 4, ['iwrupvqb', '5'], [346386]],
        [2015, 4, ['iwrupvqb', '6'], [9958218]],
        [2015, 5, ['1'], [236]],
        [2015, 5, ['2'], [51]],
        [2015, 6, ['1'], [543903]],
        [2015, 6, ['2'], [14687245]],
        [2015, 7, [], [3176, 14710]],
        [2015, 8, [], [1342, 2074]],
        [2015, 9, [], [207, 804]],
        /*
        [2015, 10, ['1321131112', '40'], [492982]],
        [2015, 10, ['1321131112', '50'], [6989950]],
        [2015, 11, ['cqjxjnds'], ['cqjxxyzz']],
        [2015, 11, ['cqjxxyzz'], ['cqkaabcc']],
        [2015, 12, [], [191164]],
        [2015, 12, ['red'], [87842]],
        [2015, 13, [], [618, 601]],
        [2015, 14, ['2503'], [2655, 1059]],
        [2015, 15, [], [222870, 117936]],
        [2015, 16, [], [373, 260]],
        [2015, 17, ['150'], [1304, 18]],
        [2015, 18, ['100', 'false'], [814]],
        [2015, 18, ['100', 'true'], [924]],
        [2015, 19, ['calibrate'], [576]],
        [2015, 19, ['fabricate'], [207]],
        [2015, 20, ['34000000'], [786240]],
        [2015, 20, ['34000000', '50'], [831600]],
        [2015, 21, [], [148, 78]],
        [2015, 22, ['easy'], [953]],
        [2015, 22, ['hard'], [1289]],
        [2015, 23, [], [1, 170]],
        [2015, 23, ['1'], [1, 247]],
        [2015, 24, [], [11266889531]],
        [2015, 24, ['4'], [77387711]],
        [2015, 25, ['2947', '3029'], [19980801]],
        */
        [2016, 1, [], [287, 133]],
        [2017, 1, [], [1034, 1356]],
        [2018, 1, [], [543, 621]],
        [2019, 1, [], [3226407, 4836738]],
        [2020, 1, [], [63616, 67877784]],
        [2021, 1, [], [1532, 1571]],
        [2022, 1, [], [68775, 202585]],
        [2022, 2, [], [13675, 14184]],
        [2022, 3, [], [7568, 2780]],
        [2022, 4, [], [526, 886]],
        [2022, 5, [], ['TGWSMRBPN', 'TZLTLWRNF']],
        [2022, 6, [], [1850, 2823]],
        [2022, 7, [], [1642503, 6999588]],
        [2022, 8, [], [1763, 671160]],
        [2022, 9, [], [5683, 2372]],
        [2022, 10, [], [12740, 'RBPARAGF']],
        [2022, 11, [], [56120, 24389045529]],
        /*
        [2022, 12, [], [408, 399]],
        */
        [2022, 13, [], [5252, 20592]],
        /*
        [2022, 14, [], [832, 27601]],
        [2022, 15, [], [-1, -1]],
        [2022, 16, [], [-1, -1]],
        [2022, 17, [], [3135, -1]],
        [2022, 18, [], [4340, 2468]],
        [2022, 19, [], [-1, -1]],
        [2022, 20, [], [-1, -1]],
        [2022, 21, [], [10037517593724, 3272260914328]],
        [2022, 22, [], [-1, -1]],
        [2022, 23, [], [-1, -1]],
        [2022, 24, [], [-1, -1]],
        [2022, 25, [], [-1, -1]],
        [2023, 1, [], [54697, 54885]],
        */
        [2023, 2, [], [2156, 66909]],
        /*
        [2023, 3, [], [535351, 87287096]],
        [2023, 4, [], [21138, 7185540]],
        [2023, 5, [], [535088217, 51399228]],
        [2023, 6, [], [741000, 38220708]],
        [2023, 7, [], [249726565, 251135960]],
        [2023, 8, [], [17621, 20685524831999]],
        [2023, 9, [], [1882395907, 1005]],
        [2023, 9, [], [-1, -1]],
        [2023, 10, [], [-1, -1]],
        [2023, 11, [], [-1, -1]],
        [2023, 12, [], [-1, -1]],
        [2023, 13, [], [-1, -1]],
        [2023, 14, [], [-1, -1]],
        [2023, 15, [], [-1, -1]],
        [2023, 16, [], [-1, -1]],
        [2023, 17, [], [-1, -1]],
        [2023, 18, [], [-1, -1]],
        [2023, 19, [], [-1, -1]],
        [2023, 20, [], [-1, -1]],
        [2023, 21, [], [-1, -1]],
        [2023, 22, [], [-1, -1]],
        [2023, 23, [], [-1, -1]],
        [2023, 24, [], [-1, -1]],
        [2023, 25, [], [-1, -1]],
        */
    ])('%i day %i returns the correct solution for %s', async (year: number, day: number, inputs: string[], expected: any[]) => {
        // Act
        const actual = await solve(year, day, inputs);

        // Assert
        expect(actual).toEqual(expected);
    });
});
