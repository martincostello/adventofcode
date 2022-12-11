// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Puzzle2021 } from './Puzzle2021';

export class Day01 extends Puzzle2021 {
    depthIncreases: number;
    depthIncreasesWithSlidingWindow: number;

    override get name() {
        return 'Sonar Sweep';
    }

    override get day() {
        return 1;
    }

    static getDepthMeasurementIncreases(depthMeasurements: number[], useSlidingWindow: boolean) {
        if (useSlidingWindow) {
            const aggregated: number[] = [];

            for (let i = 0; i < depthMeasurements.length - 2; i++) {
                aggregated.push(depthMeasurements[i] + depthMeasurements[i + 1] + depthMeasurements[i + 2]);
            }

            depthMeasurements = aggregated;
        }

        let result = 0;

        for (let i = 1; i < depthMeasurements.length; i++) {
            const last = depthMeasurements[i - 1];
            const current = depthMeasurements[i];

            if (current > last) {
                result++;
            }
        }

        return result;
    }

    override solveCore(_: string[]) {
        const values = this.readResourceAsNumbers();

        this.depthIncreases = Day01.getDepthMeasurementIncreases(values, false);
        this.depthIncreasesWithSlidingWindow = Day01.getDepthMeasurementIncreases(values, true);

        console.info(`The depth measurement increases ${this.depthIncreases} times.`);
        console.info(
            `The depth measurement increases ${this.depthIncreasesWithSlidingWindow} times when using a sliding window of 3 measurements.`
        );

        return this.createResult([this.depthIncreases, this.depthIncreasesWithSlidingWindow]);
    }
}
