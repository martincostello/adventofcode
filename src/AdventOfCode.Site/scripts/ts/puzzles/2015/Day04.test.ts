// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day04 } from './Day04';

describe('2015', () => {
    describe('Day 04', () => {
        test.each([
            ['abcdef', 609043],
            ['pqrstuv', 1048970],
        ])('returns correct value for key %s', (secretKey: string, expected: number) => {
            // Act
            const actual = Day04.getLowestPositiveNumberWithStartingZeroesAsync(secretKey, 5);

            // Assert
            expect(actual).toEqual(expected);
        });
    });
});
