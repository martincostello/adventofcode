// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { PuzzleSolver } from '../PuzzleSolver';
import { Day01 } from './Day01';

describe('2017', () => {
    describe('Day 01', () => {
        test.each([
            ['1122', false, 3],
            ['1111', false, 4],
            ['1234', false, 0],
            ['91212129', false, 9],
            ['1212', true, 6],
            ['1221', true, 0],
            ['123425', true, 4],
            ['123123', true, 12],
            ['12131415', true, 4],
        ])('returns correct sum', (digits: string, nextDigit: boolean, expected: number) => {
            // Act
            const actual = Day01.calculateSum(digits, nextDigit);

            // Assert
            expect(actual).toEqual(expected);
        });
        test('returns the correct solution', async () => {
            // Act
            const puzzle = await PuzzleSolver.solve(Day01);

            // Assert
            expect(puzzle).not.toBeNull();
            expect(puzzle.captchaSolutionNext).toBe(1034);
            expect(puzzle.captchaSolutionOpposite).toBe(1356);
        });
    });
});
