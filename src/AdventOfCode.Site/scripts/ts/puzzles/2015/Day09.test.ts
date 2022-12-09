// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day09 } from './index';

describe('2015', () => {
    describe('Day 09', () => {
        test.each([
            [['London to Dublin = 464'], 464],
            [['London to Dublin = 464', 'London to Belfast = 518', 'Dublin to Belfast = 141'], 605],
        ])('returns correct distance %s', (collection: string[], expected: number) => {
            // Act
            const actual = Day09.getDistanceBetweenPoints(collection, false);

            // Assert
            expect(actual).toEqual(expected);
        });
    });
});
