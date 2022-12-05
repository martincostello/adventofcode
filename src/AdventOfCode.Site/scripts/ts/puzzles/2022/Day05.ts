// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { from } from 'linq-to-typescript';
import { Puzzle } from '../Puzzle';
import { Puzzle2022 } from './Puzzle2022';

export class Day05 extends Puzzle2022 {
    topCratesOfStacks9000: string;
    topCratesOfStacks9001: string;

    override get name() {
        return 'Supply Stacks';
    }

    override get day() {
        return 5;
    }

    protected override get requiresData() {
        return true;
    }

    static rearrangeCrates(instructions: string[], canMoveMultipleCrates: boolean) {
        const getStackCount = (instructions: string[]) => {
            for (let i = 0; i < instructions.length; i++) {
                if (instructions[i].length === 0) {
                    const trimmed = instructions[i - 1].trim();
                    return Puzzle.parse(trimmed[trimmed.length - 1]);
                }
            }

            return -1;
        };

        type Stack = Array<string>;
        type Step = [count: number, from: number, to: number];

        const getSteps = (instructions: string[]) => {
            const result: Step[] = [];

            const moves: string[] = from(instructions)
                .skipWhile((p: string) => p.length > 0)
                .skip(1)
                .toArray();

            for (const move of moves) {
                const split = move.split(' ');
                result.push([Puzzle.parse(split[1]), Puzzle.parse(split[3]), Puzzle.parse(split[5])]);
            }

            return result;
        };

        const getStacks = (instructions: string[], count: number) => {
            const stacks: Stack[] = [];

            for (let i = 0; i < count; i++) {
                stacks.push([]);
            }

            const lines: string[] = from(instructions).take(count).reverse().toArray();

            for (const line of lines) {
                for (let i = 1, j = 0; i < line.length; i += 4, j++) {
                    const container = line[i];

                    if (container === ' ') {
                        continue;
                    }

                    stacks[j].push(container);
                }
            }

            return stacks;
        };

        const rearrange = (steps: Step[], stacks: Stack[], canMoveMultipleCrates: boolean) => {
            let swap: string[] = [];

            for (const [count, from, to] of steps) {
                const source = stacks[from - 1];
                const destination = stacks[to - 1];

                if (canMoveMultipleCrates && count > 1) {
                    for (let i = 0; i < count; i++) {
                        swap.push(source.pop());
                    }

                    for (let i = 0; i < count; i++) {
                        destination.push(swap.pop());
                    }
                } else {
                    for (let i = 0; i < count; i++) {
                        destination.push(source.pop());
                    }
                }
            }
        };

        const getTopCrates = (stacks: Stack[]) => {
            let result = '';

            for (const stack of stacks) {
                result += stack.pop();
            }

            return result;
        };

        const count = getStackCount(instructions);
        const stacks = getStacks(instructions, count);
        const steps = getSteps(instructions);

        rearrange(steps, stacks, canMoveMultipleCrates);

        return getTopCrates(stacks);
    }

    override solveCore(_: string[]) {
        const moves = this.readResourceAsLines();

        this.topCratesOfStacks9000 = Day05.rearrangeCrates(moves, false);
        this.topCratesOfStacks9001 = Day05.rearrangeCrates(moves, true);

        console.info(`The crates on the top of each stack with the CraneMover 9000 are: ${this.topCratesOfStacks9000}.`);
        console.info(`The crates on the top of each stack with the CraneMover 9001 are: ${this.topCratesOfStacks9001}.`);

        return this.createResult([this.topCratesOfStacks9000, this.topCratesOfStacks9001]);
    }
}
