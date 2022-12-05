// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Puzzle2015 } from './Puzzle2015';

export class Day07 extends Puzzle2015 {
    firstSignal: number;
    secondSignal: number;

    override get name() {
        return 'Some Assembly Required';
    }

    override get day() {
        return 7;
    }

    protected override get requiresData() {
        return true;
    }

    static getWireValues(instructions: string[]) {
        const toUint16 = (value: number) => value & 0xffff;
        const trySolveValueForOperation = (operation: string, firstValue: number, secondValue: number): number | null => {
            let value;
            switch (operation) {
                case 'AND':
                    value = firstValue & secondValue; // "x AND y -> z"
                    break;
                case 'OR':
                    value = firstValue | secondValue; // "i OR j => k"
                    break;
                case 'LSHIFT':
                    value = firstValue << secondValue; // "p LSHIFT 2"
                    break;
                case 'RSHIFT':
                    value = firstValue >> secondValue; // "q RSHIFT 3"
                    break;
                default:
                    return null;
            }

            return toUint16(value);
        };

        const instructionMap = new Map<string, string[]>();

        for (const instruction of instructions) {
            const [action, wire] = instruction.split(' -> ');
            instructionMap.set(wire, action.split(' '));
        }

        const result = new Map<string, number>();

        const tryParse = (value: string): number | null => {
            let parsed = parseInt(value, 10);

            if (!isNaN(parsed)) {
                return parsed;
            }

            if (!result.has(value)) {
                return null;
            }

            return result.get(value);
        };

        // Loop through the instructions until we have reduced each instruction to a value
        while (result.size !== instructionMap.size) {
            for (const [wireId, words] of instructionMap) {
                if (result.has(wireId)) {
                    // We already have the value for this wire
                    continue;
                }

                let solvedValue: number | null = null;

                let firstOperand: string | null = words.length === 0 ? null : words[0];
                let secondOperand: string | null = null;

                if (words.length === 1) {
                    // "123 -> x" or " -> "lx -> a"
                    // Is the instruction a value or a previously solved value?
                    if (firstOperand !== null) {
                        const value = tryParse(firstOperand);
                        if (value !== null) {
                            result.set(wireId, value);
                        }
                    }
                } else if (words.length === 2 && firstOperand === 'NOT') {
                    // "NOT e -> f" or "NOT 1 -> g"
                    secondOperand = words.length > 1 ? words[1] : null;

                    // Is the second operand a value or a previously solved value?
                    const value = tryParse(secondOperand);
                    if (value !== null) {
                        result.set(wireId, toUint16(~value));
                    }
                } else if (words.length === 3) {
                    secondOperand = words.length > 2 ? words[2] : null;

                    const firstValue = tryParse(firstOperand);
                    const secondValue = tryParse(secondOperand);

                    // Are both operands a value or a previously solved value?
                    if (firstValue !== null && secondValue !== null) {
                        const operation = words[1];
                        solvedValue = trySolveValueForOperation(operation, firstValue, secondValue);
                    }
                }

                // The value for this wire Id has been solved
                if (solvedValue !== null) {
                    result.set(wireId, solvedValue);
                }
            }
        }

        return result;
    }

    override solveCore(_: string[]) {
        const instructions = this.readResourceAsLines();

        let values = Day07.getWireValues(instructions);

        this.firstSignal = values.get('a');

        console.info(`The signal for wire a is ${this.firstSignal}.`);

        const indexForB = instructions.indexOf('44430 -> b');
        instructions[indexForB] = `${this.firstSignal} -> b`;

        values = Day07.getWireValues(instructions);

        this.secondSignal = values.get('a');

        console.info(`The new signal for wire a is ${this.secondSignal}.`);

        return this.createResult([this.firstSignal, this.secondSignal]);
    }
}
