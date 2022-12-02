// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Puzzle } from '../Puzzle';
import { PuzzleFactory } from '../PuzzleFactory';
import { Day01 } from './Day01';

export class PuzzleFactory2018 implements PuzzleFactory {
    create(year: number, day: number): Puzzle | null {
        if (year !== 2018) {
            return null;
        }

        switch (day) {
            case 1:
                return new Day01();
            default:
                return null;
        }
    }
}
