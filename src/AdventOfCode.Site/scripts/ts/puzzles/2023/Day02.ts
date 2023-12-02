// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Puzzle2023 } from './Puzzle2023';

export class Day02 extends Puzzle2023 {
    sumOfPossibleSolutions: number;
    sumOfPowers: number;

    override get solved() {
        return true;
    }

    override get name() {
        return 'Cube Conundrum';
    }

    override get day() {
        return 2;
    }

    static play(values: string[]): [number, number] {
        let sumOfIds = 0;
        let sumOfPowers = 0;

        for (let game of values) {
            const split = game.split(':');
            const rounds = split[1].split(';');

            let blue = 0;
            let green = 0;
            let red = 0;

            for (let round of rounds) {
                const cubes = round.split(',');

                for (let cube of cubes) {
                    const [howMany, color] = cube.trimStart().split(' ');
                    const count = parseInt(howMany, 10);

                    switch (color) {
                        case 'blue':
                            blue = Math.max(count, blue);
                            break;

                        case 'green':
                            green = Math.max(count, green);
                            break;

                        case 'red':
                            red = Math.max(count, red);
                            break;

                        default:
                            throw new Error(`Unknown color '${color}'.`);
                    }
                }
            }

            sumOfPowers += blue * green * red;

            if (blue <= 14 && green <= 13 && red <= 12) {
                sumOfIds += parseInt(split[0].slice('Game '.length), 10);
            }
        }

        return [sumOfIds, sumOfPowers];
    }

    override solveCore(_: string[]) {
        const values = this.readResourceAsLines();

        [this.sumOfPossibleSolutions, this.sumOfPowers] = Day02.play(values);

        console.info(`The sum of the IDs of the possible games is ${this.sumOfPossibleSolutions}.`);
        console.info(`The sum of the powers of the cubes in the games is ${this.sumOfPowers}.`);

        return this.createResult([this.sumOfPossibleSolutions, this.sumOfPowers]);
    }
}
