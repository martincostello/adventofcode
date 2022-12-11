// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { from, NumberComparer } from 'linq-to-typescript';
import { Puzzle } from '../index';
import { Puzzle2022 } from './Puzzle2022';

export class Day01 extends Puzzle2022 {
    maximumCalories: number;
    maximumCaloriesForTop3: number;

    override get name() {
        return 'Calorie Counting';
    }

    override get day() {
        return 1;
    }

    static getCalorieInventories(inventories: string[]) {
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

    override solveCore(_: string[]) {
        const inventories = this.readResourceAsLines();
        const calories = from(Day01.getCalorieInventories(inventories));

        this.maximumCalories = calories.max();
        this.maximumCaloriesForTop3 = calories
            .orderByDescending((p: number) => p, NumberComparer)
            .take(3)
            .sum();

        console.info(`The elf carrying the largest inventory has ${this.maximumCalories} Calories.`);
        console.info(`The elves carrying the largest three inventories have ${this.maximumCaloriesForTop3} Calories.`);

        return this.createResult([this.maximumCalories, this.maximumCaloriesForTop3]);
    }
}
