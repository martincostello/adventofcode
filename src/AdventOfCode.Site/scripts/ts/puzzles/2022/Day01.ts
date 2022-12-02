// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Solution } from '../../models/Solution';
import { Puzzle } from '../Puzzle';
import { Puzzle2022 } from './Puzzle2022';

export class Day01 extends Puzzle2022 {
    maximumCalories: number;
    maximumCaloriesForTop3: number;

    override get name(): string {
        return 'Calorie Counting';
    }

    override get day(): number {
        return 1;
    }

    protected override get requiresData(): boolean {
        return true;
    }

    static getCalorieInventories(inventories: string[]): number[] {
        const calories: number[] = [];
        let current = 0;

        for (const line of inventories) {
            if (line === '') {
                calories.push(current);
                current = 0;
            } else {
                current += Puzzle.parse(line);
            }
        }

        if (current > 0) {
            calories.push(current);
        }

        return calories;
    }

    override solveCore(_: string[]): Promise<Solution> {
        const inventories = this.readResourceAsLines();
        const calories = Day01.getCalorieInventories(inventories);

        this.maximumCalories = Math.max(...calories);
        this.maximumCaloriesForTop3 = calories
            .sort((x, y) => y - x)
            .slice(0, 3)
            .reduce((x, y) => x + y, 0);

        console.info(`The elf carrying the largest inventory has ${this.maximumCalories} Calories.`);
        console.info(`The elves carrying the largest three inventories have ${this.maximumCaloriesForTop3} Calories.`);

        return this.createResult([this.maximumCalories, this.maximumCaloriesForTop3]);
    }
}
