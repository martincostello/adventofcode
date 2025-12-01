// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from 'vitest';
import { Day01 } from './index';

describe('2025', () => {
    describe('Day 01', () => {
        test.each([
            [['L1000'], true, 10],
            [['L1001'], true, 10],
            [['R1000'], true, 10],
            [['R1001'], true, 10],
            [['L68', 'L30', 'R48', 'L5', 'R60', 'L55', 'L1', 'L99', 'R14', 'L82'], false, 3],
            [['L68', 'L30', 'R48', 'L5', 'R60', 'L55', 'L1', 'L99', 'R14', 'L82'], true, 6],
        ])('returns correct value for %s', (inputs: string[], useMethod0x434C49434B: boolean, expected: number) => {
            // Act
            const actual = Day01.solve(inputs, useMethod0x434C49434B);

            // Assert
            expect(actual).toBe(expected);
        });
    });
});
