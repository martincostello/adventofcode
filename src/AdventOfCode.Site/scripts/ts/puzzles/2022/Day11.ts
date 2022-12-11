// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { from } from 'linq-to-typescript';
import { Puzzle } from '../Puzzle';
import { Puzzle2022 } from './Puzzle2022';

export class Day11 extends Puzzle2022 {
    monkeyBusiness20: number;
    monkeyBusiness10000: number;

    override get name() {
        return 'Monkey in the Middle';
    }

    override get day() {
        return 11;
    }

    protected override get requiresData() {
        return true;
    }

    static getMonkeyBusiness(observations: string[], rounds: number, highAnxiety: boolean): number {
        const parse = (observations: string[], highAnxiety: boolean) => {
            const monkeyPrefix = 'Monkey ';
            const itemsPrefix = '  Starting items: ';
            const operationPrefix = '  Operation: new = old ';
            const testPrefix = '  Test: divisible by ';
            const truePrefix = '    If true: throw to monkey ';
            const falsePrefix = '    If false: throw to monkey ';

            const monkeys = new Map<number, [monkey: Monkey, divisor: number]>();
            let commonDivisor = 1;

            for (let i = 0; i < observations.length; i += 7) {
                const monkeyLine = observations[i].slice(monkeyPrefix.length, -1);
                const testValue = observations[i + 3].slice(testPrefix.length);

                const monkey = Puzzle.parse(monkeyLine);

                const items = observations[i + 1]
                    .slice(itemsPrefix.length)
                    .split(', ')
                    .map((p: string) => Puzzle.parse(p));

                const divisor = Puzzle.parse(testValue);
                commonDivisor *= divisor;

                monkeys.set(monkey, [new Monkey(monkey, items), divisor]);
            }

            const reducer = highAnxiety ? (p: number) => p % commonDivisor : (p: number) => Math.floor(p / 3);

            for (let i = 0; i < observations.length; i += 7) {
                const operation = observations[i + 2].slice(operationPrefix.length);
                const monkeyForTrue = observations[i + 4].slice(truePrefix.length);
                const monkeyForFalse = observations[i + 5].slice(falsePrefix.length);

                const [monkey, divisor] = monkeys.get(i / 7);

                monkey.reducer = reducer;

                const operationString = operation.slice(2);

                if (operation.startsWith('+')) {
                    if (operationString === 'old') {
                        monkey.inspector = (p) => p + p;
                    } else {
                        const operationValue = Puzzle.parse(operation.slice(2));
                        monkey.inspector = (p) => p + operationValue;
                    }
                } else if (operation.startsWith('*')) {
                    if (operationString === 'old') {
                        monkey.inspector = (p) => p * p;
                    } else {
                        const operationValue = Puzzle.parse(operation.slice(2));
                        monkey.inspector = (p) => p * operationValue;
                    }
                } else {
                    throw new Error(`Invalid operation '${operation[0]}'.`);
                }

                const [recipientForTrue] = monkeys.get(Puzzle.parse(monkeyForTrue));
                const [recipientForFalse] = monkeys.get(Puzzle.parse(monkeyForFalse));

                monkey.next = (p) => {
                    if (p % divisor === 0) {
                        return recipientForTrue;
                    } else {
                        return recipientForFalse;
                    }
                };
            }

            const result: Monkey[] = [];

            for (const [monkey] of monkeys.values()) {
                result.push(monkey);
            }

            return result;
        };

        const monkeys = parse(observations, highAnxiety);

        for (let round = 0; round < rounds; round++) {
            for (const monkey of monkeys) {
                monkey.inspect();
            }
        }

        return from(monkeys)
            .orderByDescending((p: Monkey) => p.inspections)
            .take(2)
            .aggregate(1, (x: number, y: Monkey) => x * y.inspections);
    }

    override solveCore(_: string[]) {
        const values = this.readResourceAsLines();

        this.monkeyBusiness20 = Day11.getMonkeyBusiness(values, 20, false);
        this.monkeyBusiness10000 = Day11.getMonkeyBusiness(values, 10000, true);

        console.info(`The level of monkey business after 20 rounds of stuff-slinging simian shenanigans is ${this.monkeyBusiness20}.`);
        console.info(
            `The level of monkey business after 10,000 rounds of stuff-slinging simian shenanigans is ${this.monkeyBusiness10000}.`
        );

        return this.createResult([this.monkeyBusiness20, this.monkeyBusiness10000]);
    }
}

class Monkey {
    next: (value: number) => Monkey;
    inspector: (value: number) => number;
    reducer: (value: number) => number;
    private inspectionsMade = 0;

    constructor(public readonly name: number, public readonly items: number[]) {}

    get inspections() {
        return this.inspectionsMade;
    }

    inspect() {
        while (this.items.length > 0) {
            let item = this.items.shift();

            item = this.inspector(item);
            item = this.reducer(item);

            this.inspectionsMade++;

            const recipient = this.next(item);
            recipient.throw(item);
        }
    }

    throw(item: number) {
        this.items.push(item);
    }
}
