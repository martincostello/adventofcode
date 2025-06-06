// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day09 } from './index';

describe('2024', () => {
    describe.skip('Day 09', () => {
        test.each([[[], -1]])('returns correct value for %s', (inputs: string[], expected: number) => {
            // Act
            const actual = Day09.solve(inputs);

            // Assert
            expect(actual).toBe(expected);
        });
    });
});
