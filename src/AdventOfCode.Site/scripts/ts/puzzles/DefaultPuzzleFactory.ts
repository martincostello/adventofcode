// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { PuzzleFactory2022 } from './2022/PuzzleFactory2022';
import { Puzzle } from './Puzzle';
import { PuzzleFactory } from './PuzzleFactory';

export class DefaultPuzzleFactory implements PuzzleFactory {
    create(year: number, day: number): Puzzle | null {
        let factory: PuzzleFactory;

        switch (year) {
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
