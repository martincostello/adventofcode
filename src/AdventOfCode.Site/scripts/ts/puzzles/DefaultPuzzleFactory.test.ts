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

    let input = join(__dirname, `../../../../AdventOfCode/Input/Y${yearString}/Day${dayString}/input.txt`);
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
    ])('%i day %i returns the correct solution for %s', async (year: number, day: number, inputs: string[], expected: any[]) => {
        // Act
        const actual = await solve(year, day, inputs);

        // Assert
        expect(actual).toEqual(expected);
    });
});
