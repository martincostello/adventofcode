// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Puzzle } from '../Puzzle';
import { PuzzleFactory } from '../PuzzleFactory';
import { Day01 } from './Day01';

export class PuzzleFactory2022 implements PuzzleFactory {
    create(year: number, day: number): Puzzle {
        if (year !== 2022) {
            throw new Error('The year specified is invalid.');
        }

        switch (day) {
            case 1:
                return new Day01();
            default:
                throw new Error('The day specified is invalid.');
        }
    }
}
