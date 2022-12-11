// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { from } from 'linq-to-typescript';
import { Point, Rectangle } from '../index';
import { Puzzle2015 } from './Puzzle2015';

export class Day06 extends Puzzle2015 {
    lightsIlluminated: number;
    totalBrightness: number;

    override get name() {
        return 'Probably a Fire Hazard';
    }

    override get day() {
        return 6;
    }

    protected override get minimumArguments() {
        return 1;
    }

    override solveCore(inputs: string[]) {
        const version = inputs[0];

        if (version !== '1' && version !== '2') {
            throw new Error('The rules version specified is invalid.');
        }

        const lines = this.readResourceAsLines();
        const instructions: Instruction[] = [];
        const parser = version === '1' ? InstructionV1.parse : InstructionV2.parse;

        for (const line of lines) {
            instructions.push(parser(line));
        }

        console.info(`Processing instructions using set ${version}...`);

        const grid = new LightGrid();

        for (const instruction of instructions) {
            instruction.act(grid);
        }

        if (version === '1') {
            this.lightsIlluminated = grid.count;

            console.info(`${this.lightsIlluminated} lights are illuminated.`);

            return this.createResult([this.lightsIlluminated]);
        } else {
            this.totalBrightness = grid.brightness;

            console.info(`The total brightness of the grid is ${this.totalBrightness}.`);

            return this.createResult([this.totalBrightness]);
        }
    }
}

abstract class Instruction {
    abstract act(grid: LightGrid): void;

    protected static parseBounds(origin: string, termination: string): Rectangle {
        // Determine the termination and origin points of the bounds of the lights to operate on
        const [left, bottom] = origin.split(',');
        const [right, top] = termination.split(',');

        // Add one to the termination point so that the grid always has a width of at least one light
        return Rectangle.fromLTRB(parseInt(left, 10), parseInt(bottom, 10), parseInt(right, 10) + 1, parseInt(top, 10) + 1);
    }
}

class InstructionV1 extends Instruction {
    constructor(public readonly action: string, public readonly bounds: Rectangle) {
        super();
    }

    override act(grid: LightGrid) {
        switch (this.action) {
            case 'OFF':
                grid.turnOff(this.bounds);
                break;

            case 'ON':
                grid.turnOn(this.bounds);
                break;

            case 'TOGGLE':
                grid.toggle(this.bounds);
                break;

            default:
                throw new Error(`The current instruction ${this.action} is invalid.`);
        }
    }

    static parse(value: string): InstructionV1 {
        // Split the instructions into 'words'
        const words = value.split(' ');

        const firstWord = words[0];

        let action: string | null = null;
        let origin: string | null = null;
        let termination: string | null = null;

        // Determine the action to perform for this instruction (OFF, ON or TOGGLE)
        if (firstWord === 'turn') {
            const secondWord = words[1];

            if (secondWord === 'off') {
                action = 'OFF';
            } else if (secondWord === 'on') {
                action = 'ON';
            }

            origin = words[2];
            termination = words[4];
        } else if (firstWord === 'toggle') {
            action = 'TOGGLE';
            origin = words[1];
            termination = words[3];
        }

        if (action === null || origin === null || termination === null) {
            throw new Error('The specified instruction is invalid.');
        }

        const bounds = Instruction.parseBounds(origin, termination);

        return new InstructionV1(action, bounds);
    }
}

class InstructionV2 extends Instruction {
    constructor(public readonly delta: number, public readonly bounds: Rectangle) {
        super();
    }

    override act(grid: LightGrid) {
        grid.incrementBrightness(this.bounds, this.delta);
    }

    static parse(value: string): InstructionV2 {
        // Split the instructions into 'words'
        const words = value.split(' ');

        const firstWord = words[0];

        let delta: number | null = null;
        let origin: string | null = null;
        let termination: string | null = null;

        // Determine the action to perform for this instruction (OFF, ON or TOGGLE)
        if (firstWord === 'turn') {
            const secondWord = words[1];

            if (secondWord === 'off') {
                delta = -1;
            } else if (secondWord === 'on') {
                delta = 1;
            }

            origin = words[2];
            termination = words[4];
        } else if (firstWord === 'toggle') {
            delta = 2;
            origin = words[1];
            termination = words[3];
        }

        if (delta === null || origin === null || termination === null) {
            throw new Error('The specified instruction is invalid.');
        }

        const bounds = Instruction.parseBounds(origin, termination);

        return new InstructionV2(delta, bounds);
    }
}

class LightGrid {
    private readonly lightBrightnesses = new Map<string, number>();

    get brightness() {
        return from(this.lightBrightnesses.values()).sum();
    }

    get count() {
        return from(this.lightBrightnesses.values())
            .where((p: number) => p > 0)
            .count();
    }

    get(position: Point) {
        return this.lightBrightnesses.get(position.toString()) || 0;
    }

    incrementBrightness(item: Point | Rectangle, delta: number) {
        if (item instanceof Point) {
            this.incrementOrSetBrightness(item, delta, false);
        } else {
            for (let x = 0; x < item.width; x++) {
                for (let y = 0; y < item.height; y++) {
                    this.incrementBrightness(new Point(item.x + x, item.y + y), delta);
                }
            }
        }
    }

    toggle(item: Point | Rectangle) {
        if (item instanceof Point) {
            const isOff = this.get(item) === 0;
            const delta = isOff ? 1 : -1;

            this.incrementBrightness(item, delta);

            return isOff;
        } else {
            for (let x = 0; x < item.width; x++) {
                for (let y = 0; y < item.height; y++) {
                    this.toggle(new Point(item.x + x, item.y + y));
                }
            }
        }
    }

    turnOff(item: Point | Rectangle) {
        if (item instanceof Point) {
            this.incrementOrSetBrightness(item, 0, true);
        } else {
            for (let x = 0; x < item.width; x++) {
                for (let y = 0; y < item.height; y++) {
                    this.turnOff(new Point(item.x + x, item.y + y));
                }
            }
        }
    }

    turnOn(item: Point | Rectangle) {
        if (item instanceof Point) {
            this.incrementOrSetBrightness(item, 1, true);
        } else {
            for (let x = 0; x < item.width; x++) {
                for (let y = 0; y < item.height; y++) {
                    this.turnOn(new Point(item.x + x, item.y + y));
                }
            }
        }
    }

    private incrementOrSetBrightness(position: Point, delta: number, set: boolean = false) {
        let current;

        if (set) {
            current = delta;
        } else {
            current = this.get(position) + delta;
        }

        const value = Math.max(current, 0);
        const key = position.toString();

        if (value === 0) {
            this.lightBrightnesses.delete(key);
        } else {
            this.lightBrightnesses.set(key, value);
        }

        return value;
    }
}
