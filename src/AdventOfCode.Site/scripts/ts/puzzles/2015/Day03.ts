// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { CardinalDirection, Point, Size } from '../index';
import { Puzzle2015 } from './Puzzle2015';

export class Day03 extends Puzzle2015 {
    housesWithPresentsFromSanta: number;
    housesWithPresentsFromSantaAndRoboSanta: number;

    override get name() {
        return 'Perfectly Spherical Houses in a Vacuum';
    }

    override get day() {
        return 3;
    }

    static getUniqueHousesVisitedBySanta(instructions: string) {
        const directions = Day03.getDirections(instructions);

        const santa = new SantaGps();
        const coordinates = new Set<string>();

        for (const direction of directions) {
            coordinates.add(santa.location.toString());
            santa.move(direction);
        }

        return coordinates.size;
    }

    static getUniqueHousesVisitedBySantaAndRoboSanta(instructions: string) {
        const directions = Day03.getDirections(instructions);

        const santa = new SantaGps();
        const roboSanta = new SantaGps();

        let current = santa;
        let previous = roboSanta;

        const coordinates = new Set<string>();
        coordinates.add(Point.empty.toString());

        for (const direction of directions) {
            current.move(direction);
            coordinates.add(current.location.toString());
            [current, previous] = [previous, current];
        }

        return coordinates.size;
    }

    override solveCore(_: string[]) {
        const instructions = this.readResourceAsString();

        this.housesWithPresentsFromSanta = Day03.getUniqueHousesVisitedBySanta(instructions);
        this.housesWithPresentsFromSantaAndRoboSanta = Day03.getUniqueHousesVisitedBySantaAndRoboSanta(instructions);

        console.info(`In 2015, Santa delivered presents to ${this.housesWithPresentsFromSanta} houses.`);
        console.info(`In 2016, Santa and Robo-Santa delivered presents to ${this.housesWithPresentsFromSantaAndRoboSanta} houses.`);

        return this.createResult([this.housesWithPresentsFromSanta, this.housesWithPresentsFromSantaAndRoboSanta]);
    }

    private static getDirections(instructions: string) {
        const directions: CardinalDirection[] = [];

        for (let i = 0; i < instructions.length; i++) {
            let direction: CardinalDirection;

            switch (instructions[i]) {
                case '^':
                    direction = CardinalDirection.north;
                    break;
                case 'v':
                    direction = CardinalDirection.south;
                    break;
                case '>':
                    direction = CardinalDirection.east;
                    break;
                case '<':
                    direction = CardinalDirection.west;
                    break;
                default:
                    throw new Error(`Invalid direction: '${instructions[i]}'.`);
            }

            directions.push(direction);
        }

        return directions;
    }
}

class SantaGps {
    constructor(private current: Point = Point.empty) {}

    get location() {
        return this.current;
    }

    move(direction: CardinalDirection) {
        let delta: Size;
        switch (direction) {
            case CardinalDirection.east:
                delta = new Size(1, 0);
                break;
            case CardinalDirection.north:
                delta = new Size(0, 1);
                break;
            case CardinalDirection.south:
                delta = new Size(0, -1);
                break;
            case CardinalDirection.west:
                delta = new Size(-1, 0);
                break;
            default:
                throw new Error(`The specified direction '${direction}' is invalid.`);
        }

        this.current = Point.add(this.current, delta);
    }
}
