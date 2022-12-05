// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day02 } from './index';

describe('2015', () => {
    describe('Day 02', () => {
        test.each([
            ['2x3x4', 58, 34],
            ['1x1x10', 43, 14],
        ])('returns correct length and area for "%s"', (dimensions: string, expectedArea: number, expectedLength: number) => {
            // Act
            const [actualArea, actualLength] = Day02.getTotalWrappingPaperAreaAndRibbonLength([dimensions]);

            // Assert
            expect(actualArea).toEqual(expectedArea);
            expect(actualLength).toEqual(expectedLength);
        });
    });
});
