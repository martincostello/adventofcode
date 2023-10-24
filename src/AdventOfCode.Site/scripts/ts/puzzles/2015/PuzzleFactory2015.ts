// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Puzzle, PuzzleFactory } from '../index';
import * as thisYear from './index';

export class PuzzleFactory2015 implements PuzzleFactory {
    create(year: number, day: number): Puzzle | null {
        if (year !== 2015) {
            return null;
        }

        switch (day) {
            case 1:
                return new thisYear.Day01();
            case 2:
                return new thisYear.Day02();
            case 3:
                return new thisYear.Day03();
            case 4:
                return new thisYear.Day04();
            case 5:
                return new thisYear.Day05();
            case 6:
                return new thisYear.Day06();
            case 7:
                return new thisYear.Day07();
            case 8:
                return new thisYear.Day08();
            case 9:
                return new thisYear.Day09();
            default:
                return null;
        }
    }
}
