// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day03 } from './index';

describe('2023', () => {
    describe.skip('Day 03', () => {
        test.each([[[], -1]])('returns correct value for %s', (inputs: string[], expected: number) => {
            // Act
            const actual = Day03.solve(inputs);

            // Assert
            expect(actual).toBe(expected);
        });
    });
});
