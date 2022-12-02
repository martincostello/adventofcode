// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Solution } from '../../models/Solution';
import { Puzzle2019 } from './Puzzle2019';

export class Day01 extends Puzzle2019 {
    totalFuelRequiredForModules: number;
    totalFuelRequiredForRocket: number;

    override get name(): string {
        return 'The Tyranny of the Rocket Equation';
    }

    override get day(): number {
        return 1;
    }

    protected override get requiresData(): boolean {
        return true;
    }

    static getFuelRequirementsForMass(mass: number): number {
        let requirement = mass / 3;

        requirement -= 2;

        return Math.floor(Math.max(requirement, 0));
    }

    static getFuelRequirementsForMassWithFuel(mass: number): number {
        let fuel = Day01.getFuelRequirementsForMass(mass);

        if (fuel > 0) {
            fuel += Day01.getFuelRequirementsForMassWithFuel(fuel);
        }

        return Math.floor(fuel);
    }

    override solveCore(_: string[]): Promise<Solution> {
        const masses = this.readResourceAsNumbers();

        this.totalFuelRequiredForModules = Day01.getFuelRequirementsForMasses(masses);
        this.totalFuelRequiredForRocket = Day01.getFuelRequirementsForRocket(masses);

        return this.createResult([this.totalFuelRequiredForModules, this.totalFuelRequiredForRocket]);
    }

    private static getFuelRequirementsForMasses(masses: number[]): number {
        let total = 0;

        for (const mass of masses) {
            total += Day01.getFuelRequirementsForMass(mass);
        }

        return total;
    }

    private static getFuelRequirementsForRocket(masses: number[]): number {
        let total = 0;

        for (const mass of masses) {
            total += Day01.getFuelRequirementsForMassWithFuel(mass);
        }

        return total;
    }
}
