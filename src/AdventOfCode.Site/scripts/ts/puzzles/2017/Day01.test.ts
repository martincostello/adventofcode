// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day01 } from './index';

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
        ])('returns correct sum for %s with next digit %s', (digits: string, nextDigit: boolean, expected: number) => {
            // Act
            const actual = Day01.calculateSum(digits, nextDigit);

            // Assert
            expect(actual).toEqual(expected);
        });
    });
});
