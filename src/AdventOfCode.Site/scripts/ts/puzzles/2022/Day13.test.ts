// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day13 } from './index';

describe('2022', () => {
    describe('Day 13', () => {
        test('returns correct values', () => {
            // Arrange
            const packets = [
                '[1,1,3,1,1]',
                '[1,1,5,1,1]',
                '',
                '[[1],[2,3,4]]',
                '[[1],4]',
                '',
                '[9]',
                '[[8,7,6]]',
                '',
                '[[4,4],4,4]',
                '[[4,4],4,4,4]',
                '',
                '[7,7,7,7]',
                '[7,7,7]',
                '',
                '[]',
                '[3]',
                '',
                '[[[]]]',
                '[[]]',
                '',
                '[1,[2,[3,[4,[5,6,7]]]],8,9]',
                '[1,[2,[3,[4,[5,6,0]]]],8,9]',
            ];

            // Act
            const [actualSum, actualDecoderKey] = Day13.decodePackets(packets);

            // Assert
            expect(actualSum).toBe(13);
            expect(actualDecoderKey).toBe(140);
        });
    });
});
