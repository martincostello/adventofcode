// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Puzzle, PuzzleFactory } from '../index';
import * as thisYear from './index';

export class PuzzleFactory2022 implements PuzzleFactory {
    create(year: number, day: number): Puzzle | null {
        if (year !== 2022) {
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
            case 10:
                return new thisYear.Day10();
            case 11:
                return new thisYear.Day11();
            case 12:
                return new thisYear.Day12();
            case 13:
                return new thisYear.Day13();
            case 14:
                return new thisYear.Day14();
            case 15:
                return new thisYear.Day15();
            case 16:
                return new thisYear.Day16();
            case 17:
                return new thisYear.Day17();
            case 18:
                return new thisYear.Day18();
            case 19:
                return new thisYear.Day19();
            case 20:
                return new thisYear.Day20();
            case 21:
                return new thisYear.Day21();
            case 22:
                return new thisYear.Day22();
            case 23:
                return new thisYear.Day23();
            case 24:
                return new thisYear.Day24();
            case 25:
                return new thisYear.Day25();
            default:
                return null;
        }
    }
}
