// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Puzzle, PuzzleFactory } from '../index';
import * as Y2022 from './index';

export class PuzzleFactory2022 implements PuzzleFactory {
    create(year: number, day: number): Puzzle | null {
        if (year !== 2022) {
            return null;
        }

        switch (day) {
            case 1:
                return new Y2022.Day01();
            case 2:
                return new Y2022.Day02();
            case 3:
                return new Y2022.Day03();
            case 4:
                return new Y2022.Day04();
            case 5:
                return new Y2022.Day05();
            default:
                return null;
        }
    }
}
