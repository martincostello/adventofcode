// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { CharacterRecognition } from '../CharacterRecognition';
import { Puzzle } from '../Puzzle';
import { Puzzle2022 } from './Puzzle2022';

export class Day10 extends Puzzle2022 {
    sumOfSignalStrengths: number;
    message: string;

    override get name() {
        return 'Cathode-Ray Tube';
    }

    override get day() {
        return 10;
    }

    static getMessage(program: string[]): [message: string, visualization: string, sumOfSignalStrengths: number] {
        const displayHeight = 6;
        const displayWidth = 40;

        const crt: Array<string>[] = [
            new Array<string>(displayWidth),
            new Array<string>(displayWidth),
            new Array<string>(displayWidth),
            new Array<string>(displayWidth),
            new Array<string>(displayWidth),
            new Array<string>(displayWidth),
        ];

        const registers = new Map<number, number>();

        let cycle = 0;
        let register = 1;
        let sprite = 1;

        const draw = () => {
            const x = cycle % displayWidth;
            const y = Math.floor(cycle / displayWidth);

            let ch: string;

            if (register === sprite - 1 || register === sprite || register === sprite + 1) {
                ch = '#';
            } else {
                ch = '.';
            }

            crt[y][x] = ch;
        };

        const tick = (operand: string | null = null) => {
            draw();

            if (operand !== null) {
                register += Puzzle.parse(operand);
            }

            registers.set(++cycle, register);
            sprite = cycle % displayWidth;
        };

        const visualize = (crt: Array<string>[]) => {
            let output = '';

            for (let y = 0; y < displayHeight; y++) {
                for (let x = 0; x < displayWidth; x++) {
                    output += crt[y][x];
                }

                output += '\n';
            }

            return output.slice(0, -1);
        };

        for (const instruction of program) {
            switch (instruction.slice(0, 4)) {
                case 'noop':
                    tick();
                    break;

                case 'addx':
                    tick();
                    tick(instruction.slice(5));
                    break;
            }
        }

        const message = CharacterRecognition.readCharacters(crt, '#');
        const visualization = visualize(crt);

        let sum = 0;

        for (const r of [20, 60, 100, 140, 180, 220]) {
            sum += registers.get(r - 1) * r;
        }

        return [message, visualization, sum];
    }

    override solveCore(_: string[]) {
        const program = this.readResourceAsLines();

        let visualization = '';
        [this.message, visualization, this.sumOfSignalStrengths] = Day10.getMessage(program);

        console.info(`The sum of the six signal strengths is ${this.sumOfSignalStrengths}.`);
        console.info(`The message output to the CRT is '${this.message}'.'`);

        return this.createResult([this.sumOfSignalStrengths, this.message], [visualization]);
    }
}
