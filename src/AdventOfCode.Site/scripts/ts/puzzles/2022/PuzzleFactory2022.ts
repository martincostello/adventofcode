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
            case 6:
                return new Y2022.Day06();
            case 7:
                return new Y2022.Day07();
            case 8:
                return new Y2022.Day08();
            case 9:
                return new Y2022.Day09();
            case 10:
                return new Y2022.Day10();
            case 11:
                return new Y2022.Day11();
            case 12:
                return new Y2022.Day12();
            case 13:
                return new Y2022.Day13();
            case 14:
                return new Y2022.Day14();
            case 15:
                return new Y2022.Day15();
            case 16:
                return new Y2022.Day16();
            case 17:
                return new Y2022.Day17();
            case 18:
                return new Y2022.Day18();
            case 19:
                return new Y2022.Day19();
            case 20:
                return new Y2022.Day20();
            case 21:
                return new Y2022.Day21();
            case 22:
                return new Y2022.Day22();
            case 23:
                return new Y2022.Day23();
            case 24:
                return new Y2022.Day24();
            case 25:
                return new Y2022.Day25();
            default:
                return null;
        }
    }
}
