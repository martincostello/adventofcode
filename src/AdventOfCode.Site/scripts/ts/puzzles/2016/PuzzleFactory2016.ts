// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Puzzle, PuzzleFactory } from '../index';
import * as thisYear from './index';

export class PuzzleFactory2016 implements PuzzleFactory {
    create(year: number, day: number): Puzzle | null {
        if (year !== 2016) {
            return null;
        }

        switch (day) {
            case 1:
                return new thisYear.Day01();
            default:
                return null;
        }
    }
}
