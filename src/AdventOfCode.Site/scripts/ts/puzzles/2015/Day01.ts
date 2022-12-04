// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Puzzle2015 } from './Puzzle2015';

export class Day01 extends Puzzle2015 {
    finalFloor: number;
    firstBasementInstruction: number;

    override get name() {
        return 'Not Quite Lisp';
    }

    override get day() {
        return 1;
    }

    protected override get requiresData() {
        return true;
    }

    static getFinalFloorAndFirstInstructionBasementReached(value: string): [floor: number, instructionThatEntersBasement: number] {
        let floor = 0;
        let instructionThatEntersBasement = -1;

        let hasVisitedBasement = false;

        for (let i = 0; i < value.length; i++) {
            switch (value[i]) {
                case '(':
                    floor++;
                    break;

                case ')':
                    floor--;
                    break;

                default:
                    break;
            }

            if (!hasVisitedBasement) {
                if (floor === -1) {
                    instructionThatEntersBasement = i + 1;
                    hasVisitedBasement = true;
                }
            }
        }

        return [floor, instructionThatEntersBasement];
    }

    override solveCore(_: string[]) {
        const value = this.readResourceAsString();

        [this.finalFloor, this.firstBasementInstruction] = Day01.getFinalFloorAndFirstInstructionBasementReached(value);

        console.info(`Santa should go to floor ${this.finalFloor}.`);
        console.info(`Santa first enters the basement after following instruction ${this.firstBasementInstruction}.`);

        return this.createResult([this.finalFloor, this.firstBasementInstruction]);
    }
}
