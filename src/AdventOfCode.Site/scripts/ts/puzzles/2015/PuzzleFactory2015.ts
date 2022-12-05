// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Puzzle, PuzzleFactory } from '../index';
import { Day01, Day02, Day03, Day04, Day05, Day06 } from './index';

export class PuzzleFactory2015 implements PuzzleFactory {
    create(year: number, day: number): Puzzle | null {
        if (year !== 2015) {
            return null;
        }

        switch (day) {
            case 1:
                return new Day01();
            case 2:
                return new Day02();
            case 3:
                return new Day03();
            case 4:
                return new Day04();
            case 5:
                return new Day05();
            case 6:
                return new Day06();
            default:
                return null;
        }
    }
}
