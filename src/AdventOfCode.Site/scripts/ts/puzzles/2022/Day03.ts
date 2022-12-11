// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { from } from 'linq-to-typescript';
import { Puzzle2022 } from './Puzzle2022';

export class Day03 extends Puzzle2022 {
    sumOfPriorities: number;
    sumOfPrioritiesOfGroups: number;

    override get name() {
        return 'Rucksack Reorganization';
    }

    override get day() {
        return 3;
    }

    static getSumOfCommonItemTypes(inventories: string[], useGroups: boolean) {
        const getPriority = (item: string) => {
            if (item.toUpperCase() === item) {
                return item.charCodeAt(0) - 'A'.charCodeAt(0) + 27;
            } else {
                return item.charCodeAt(0) - 'a'.charCodeAt(0) + 1;
            }
        };

        const getCommonItemType = (...inventories: string[]) => {
            const enumerable = from(inventories);
            let intersection = from(enumerable.first());

            for (const inventory of enumerable.skip(1)) {
                intersection = intersection.intersect(from([...inventory]));
            }

            return intersection.single();
        };

        if (useGroups) {
            let sum = 0;

            for (let i = 0; i < inventories.length; i += 3) {
                const first = inventories[i];
                const second = inventories[i + 1];
                const third = inventories[i + 2];

                const common = getCommonItemType(first, second, third);
                sum += getPriority(common);
            }

            return sum;
        } else {
            return from(inventories)
                .select((inventory: string) => {
                    const first = inventory.slice(0, inventory.length / 2);
                    const second = inventory.slice(inventory.length / 2);
                    return getCommonItemType(first, second);
                })
                .select((p: string) => getPriority(p))
                .sum();
        }
    }

    override solveCore(_: string[]) {
        const moves = this.readResourceAsLines();

        this.sumOfPriorities = Day03.getSumOfCommonItemTypes(moves, false);
        this.sumOfPrioritiesOfGroups = Day03.getSumOfCommonItemTypes(moves, true);

        console.info(`The sum of the priorities of the item types which appear in both compartments is ${this.sumOfPriorities}.`);
        console.info(
            `The sum of the priorities of the item types which appear in all three rucksacks of each group of elves is ${this.sumOfPrioritiesOfGroups}.`
        );

        return this.createResult([this.sumOfPriorities, this.sumOfPrioritiesOfGroups]);
    }
}
