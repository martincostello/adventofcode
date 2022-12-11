// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { from } from 'linq-to-typescript';
import { Puzzle2019 } from './Puzzle2019';

export class Day01 extends Puzzle2019 {
    totalFuelRequiredForModules: number;
    totalFuelRequiredForRocket: number;

    override get name() {
        return 'The Tyranny of the Rocket Equation';
    }

    override get day() {
        return 1;
    }

    static getFuelRequirementsForMass(mass: number, includeFuel: boolean) {
        let fuel = Day01.getRequiredFuel(mass);

        if (includeFuel && fuel > 0) {
            fuel += Day01.getFuelRequirementsForMass(fuel, includeFuel);
        }

        return Math.floor(fuel);
    }

    override solveCore(_: string[]) {
        const masses = from(this.readResourceAsNumbers());

        this.totalFuelRequiredForModules = masses.sum((p: number) => Day01.getFuelRequirementsForMass(p, false));
        this.totalFuelRequiredForRocket = masses.sum((p: number) => Day01.getFuelRequirementsForMass(p, true));

        console.info(`${this.totalFuelRequiredForModules} fuel is required for the modules.`);
        console.info(`${this.totalFuelRequiredForRocket} fuel is required for the fully-fuelled rocket.`);

        return this.createResult([this.totalFuelRequiredForModules, this.totalFuelRequiredForRocket]);
    }

    private static getRequiredFuel(mass: number) {
        return Math.floor(Math.max(mass / 3 - 2, 0));
    }
}
