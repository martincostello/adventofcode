// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from 'vitest';
import { Day01 } from './index';

describe('2025', () => {
    describe.skip('Day 01', () => {
        test.each([[[], 1530215]])('returns correct value for %s', (inputs: string[], expected: number) => {
            // Act
            const actual = Day01.solve(inputs);

            // Assert
            expect(actual).toBe(expected);
        });
    });
});
