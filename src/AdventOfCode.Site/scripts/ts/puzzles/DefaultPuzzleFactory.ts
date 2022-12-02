// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { PuzzleFactory2015 } from './2015/PuzzleFactory2015';
import { PuzzleFactory2016 } from './2016/PuzzleFactory2016';
import { PuzzleFactory2017 } from './2017/PuzzleFactory2017';
import { PuzzleFactory2018 } from './2018/PuzzleFactory2018';
import { PuzzleFactory2019 } from './2019/PuzzleFactory2019';
import { PuzzleFactory2020 } from './2020/PuzzleFactory2020';
import { PuzzleFactory2021 } from './2021/PuzzleFactory2021';
import { PuzzleFactory2022 } from './2022/PuzzleFactory2022';
import { Puzzle } from './Puzzle';
import { PuzzleFactory } from './PuzzleFactory';

export class DefaultPuzzleFactory implements PuzzleFactory {
    create(year: number, day: number): Puzzle | null {
        let factory: PuzzleFactory;

        switch (year) {
            case 2015:
                factory = new PuzzleFactory2015();
                break;
            case 2016:
                factory = new PuzzleFactory2016();
                break;
            case 2017:
                factory = new PuzzleFactory2017();
                break;
            case 2018:
                factory = new PuzzleFactory2018();
                break;
            case 2019:
                factory = new PuzzleFactory2019();
                break;
            case 2020:
                factory = new PuzzleFactory2020();
                break;
            case 2021:
                factory = new PuzzleFactory2021();
                break;
            case 2022:
                factory = new PuzzleFactory2022();
                break;
            default:
                factory = null;
                break;
        }

        if (!factory) {
            return null;
        }

        return factory.create(year, day);
    }
}
