// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day01 } from './index';

describe('2015', () => {
    describe('Day 01', () => {
        test.each([
            ['(())', 0, -1],
            ['()()', 0, -1],
            ['(((', 3, -1],
            ['(()(()(', 3, -1],
            ['))(((((', 3, 1],
            ['())', -1, 3],
            ['))(', -1, 1],
            [')))', -3, 1],
            [')())())', -3, 1],
            [')', -1, 1],
            ['()())', -1, 5],
        ])('returns correct instructions for "%s"', (value: string, expectedFloor: number, expectedInstruction: number) => {
            // Act
            const [actualFloor, actualInstruction] = Day01.getFinalFloorAndFirstInstructionBasementReached(value);

            // Assert
            expect(actualFloor).toEqual(expectedFloor);
            expect(actualInstruction).toEqual(expectedInstruction);
        });
    });
});
