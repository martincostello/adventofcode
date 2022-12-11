// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { CardinalDirection, Point, Puzzle, Size } from '../index';
import { Puzzle2016 } from './Puzzle2016';

export class Day01 extends Puzzle2016 {
    blocksToEasterBunnyHQ: number;
    blocksToEasterBunnyHQIgnoringDuplicates: number;

    override get name() {
        return 'No Time for a Taxicab';
    }

    override get day() {
        return 1;
    }

    static calculateDistance(input: string, ignoreDuplicates: boolean) {
        let bearing = CardinalDirection.north;
        let position = Point.empty;

        const instructions = this.parseDirections(input);
        const positions: Point[] = [];

        for (const instruction of instructions) {
            bearing = this.turn(bearing, instruction.direction);

            for (let i = 0; i < instruction.distance; i++) {
                if (!ignoreDuplicates) {
                    if (positions.find((other) => other.equals(position))) {
                        break;
                    }

                    positions.push(position);
                }

                let delta: Size;

                switch (bearing) {
                    case CardinalDirection.north:
                        delta = new Size(0, 1);
                        break;
                    case CardinalDirection.south:
                        delta = new Size(0, -1);
                        break;
                    case CardinalDirection.east:
                        delta = new Size(1, 0);
                        break;
                    case CardinalDirection.west:
                        delta = new Size(-1, 0);
                        break;
                    default:
                        throw new Error(`The bearing ${bearing} is not known.`);
                }

                position = Point.add(position, delta);
            }
        }

        return Point.manhattanDistance(position, Point.empty);
    }

    override solveCore(_: string[]) {
        const instructions = this.readResourceAsString();

        this.blocksToEasterBunnyHQIgnoringDuplicates = Day01.calculateDistance(instructions, true);
        this.blocksToEasterBunnyHQ = Day01.calculateDistance(instructions, false);

        console.info(`The Easter Bunny's headquarters are ${this.blocksToEasterBunnyHQIgnoringDuplicates} blocks away.`);
        console.info(
            `The Easter Bunny's headquarters are ${this.blocksToEasterBunnyHQ} blocks away if it is the first location visited twice.`
        );

        return this.createResult([this.blocksToEasterBunnyHQIgnoringDuplicates, this.blocksToEasterBunnyHQ]);
    }

    private static parseDirections(input: string) {
        const instructions = input.split(/[,\s]+/);

        const result: Instruction[] = [];

        for (let i = 0; i < instructions.length; i++) {
            const rawInstruction = instructions[i];

            result[i] = {
                direction: rawInstruction[0] === 'L' ? Direction.left : Direction.right,
                distance: Puzzle.parse(rawInstruction.slice(1)),
            };
        }

        return result;
    }

    private static turn(bearing: CardinalDirection, direction: Direction) {
        switch (bearing) {
            case CardinalDirection.east:
                return direction === Direction.left ? CardinalDirection.north : CardinalDirection.south;
            case CardinalDirection.north:
                return direction === Direction.left ? CardinalDirection.west : CardinalDirection.east;
            case CardinalDirection.south:
                return direction === Direction.left ? CardinalDirection.east : CardinalDirection.west;
            case CardinalDirection.west:
                return direction === Direction.left ? CardinalDirection.south : CardinalDirection.north;
            default:
                throw new Error(`Invalid bearing '${bearing}'.`);
        }
    }
}

enum Direction {
    left,
    right,
}

class Instruction {
    direction: Direction;
    distance: number;
}
