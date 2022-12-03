// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day01 } from './Day01';

describe('2016', () => {
    describe('Day 01', () => {
        test.each([
            ['R2, L3', true, 5],
            ['R2, R2, R2', true, 2],
            ['R5, L5, R5, R3', true, 12],
            ['R8, R4, R4, R8', false, 4],
        ])(
            'returns correct distance for instructions "%s" with ignoreDuplicates %s',
            (instructions: string, ignoreDuplicates: boolean, expected: number) => {
                // Act
                const actual = Day01.calculateDistance(instructions, ignoreDuplicates);

                // Assert
                expect(actual).toEqual(expected);
            }
        );
    });
});
