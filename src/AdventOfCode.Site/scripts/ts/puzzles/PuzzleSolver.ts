// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { expect } from '@jest/globals';
import { readFileSync } from 'fs';
import { join, resolve } from 'path';
import { Puzzle } from './Puzzle';

export class PuzzleSolver {
    static async solve<T extends Puzzle>(type: { new (): T }, inputs: string[] = []): Promise<T> {
        const puzzle = new type();

        const year = PuzzleSolver.toNumberWithDigits(puzzle.year, 4);
        const day = PuzzleSolver.toNumberWithDigits(puzzle.day, 2);

        let input = join(__dirname, `../../../../AdventOfCode/Input/Y${year}/Day${day}/input.txt`);
        input = resolve(input);

        puzzle.resource = readFileSync(input, 'utf-8');

        // Act
        const result = await puzzle.solve(inputs);

        // Assert
        expect(result).not.toBeNull();
        expect(result.solutions).not.toBeNull();
        expect(result.solutions.length).toBeGreaterThan(0);

        return puzzle;
    }

    private static toNumberWithDigits(value: number, digits: number): string {
        return value.toLocaleString('en-US', {
            minimumIntegerDigits: digits,
            useGrouping: false,
        });
    }
}
