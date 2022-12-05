// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Puzzle } from '../index';
import { Puzzle2022 } from './Puzzle2022';

export class Day04 extends Puzzle2022 {
    fullyOverlappingAssignments: number;
    partiallyOverlappingAssignments: number;

    override get name() {
        return 'Camp Cleanup';
    }

    override get day() {
        return 4;
    }

    protected override get requiresData() {
        return true;
    }

    static getOverlappingAssignments(assignments: string[], partial: boolean) {
        type Range = [start: number, end: number];

        const asRange = (value: string): Range => {
            const numbers = value.split('-');
            return [Puzzle.parse(numbers[0]), Puzzle.parse(numbers[1])];
        };

        const isWithinRange = (range: Range, other: Range): boolean => {
            return range[0] >= other[0] && range[1] <= other[1];
        };

        const overlaps = (range: Range, other: Range): boolean => {
            return (range[0] <= other[0] && range[1] >= other[0]) || (range[0] <= other[1] && range[1] >= other[1]);
        };

        let count = 0;

        for (const assignment of assignments) {
            const split = assignment.split(',');
            const first = asRange(split[0]);
            const second = asRange(split[1]);

            if (partial) {
                if (overlaps(first, second) || overlaps(second, first)) {
                    count++;
                }
            } else {
                if (isWithinRange(first, second) || isWithinRange(second, first)) {
                    count++;
                }
            }
        }

        return count;
    }

    override solveCore(_: string[]) {
        const moves = this.readResourceAsLines();

        this.fullyOverlappingAssignments = Day04.getOverlappingAssignments(moves, false);
        this.partiallyOverlappingAssignments = Day04.getOverlappingAssignments(moves, true);

        console.info(
            `There are ${this.fullyOverlappingAssignments} assignment pairs where one range is entirely contained within the other.`
        );
        console.info(`There are ${this.partiallyOverlappingAssignments} assignment pairs where one range overlaps with the other.`);

        return this.createResult([this.fullyOverlappingAssignments, this.partiallyOverlappingAssignments]);
    }
}
