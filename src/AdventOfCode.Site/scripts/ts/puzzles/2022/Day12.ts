// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { PathFinding } from '../PathFinding';
import { Point } from '../Point';
import { SquareGrid } from '../SquareGrid';
import { Puzzle2022 } from './Puzzle2022';

export class Day12 extends Puzzle2022 {
    minimumStepsFromStart: number;
    minimumStepsFromGroundLevel: number;

    override get name() {
        return 'Hill Climbing Algorithm';
    }

    override get day() {
        return 12;
    }

    static getMinimumSteps(heightmap: string[]): [actualFromStart: number, actualFromGround: number] {
        const buildMap = (heightmap: string[]) => {
            const width = heightmap[0].length;
            const height = heightmap.length;

            const map = new ElevationMap(width, height);

            for (let y = 0; y < map.height; y++) {
                for (let x = 0; x < map.width; x++) {
                    const location = new Point(x, y);
                    map.locations.add(location.toString());

                    const ch = heightmap[y][x];

                    map.elevations.set(location.toString(), ch === 'S' ? 0 : ch === 'E' ? 25 : ch.charCodeAt(0) - 'a'.charCodeAt(0));

                    if (ch === 'S') {
                        map.start = location;
                    } else if (ch === 'E') {
                        map.end = location;
                    }
                }
            }

            return map;
        };

        const map = buildMap(heightmap);

        const locationsAtGroundLevel: Point[] = [];

        for (const location of map.locations) {
            if (map.elevations.get(location.toString()) === 0) {
                locationsAtGroundLevel.push(new Point(parseInt(location.split(',')[0], 10), parseInt(location.split(',')[1], 10)));
            }
        }

        const distances = new Map<Point, number>();

        for (const start of locationsAtGroundLevel) {
            const distance = PathFinding.astar(map, start, map.end);

            // Ignore starting locations that cannot reach the end
            if (distance > 0) {
                distances.set(start, distance);
            }
        }

        let minimum = 1000000;

        for (const [_, distance] of distances) {
            minimum = Math.min(minimum, distance);
        }

        return [distances.get(map.start), minimum];
    }

    override solveCore(_: string[]) {
        const heightmap = this.readResourceAsLines();

        [this.minimumStepsFromStart, this.minimumStepsFromGroundLevel] = Day12.getMinimumSteps(heightmap);

        console.info(
            `The fewest steps required to move from your current position to the location that should get the best signal is ${this.minimumStepsFromStart}.`
        );
        console.info(
            `The fewest steps required to move from any position at ground level to the location that should get the best signal is ${this.minimumStepsFromGroundLevel}.`
        );

        return this.createResult([this.minimumStepsFromStart, this.minimumStepsFromGroundLevel]);
    }
}

class ElevationMap extends SquareGrid {
    readonly elevations: Map<string, number>;
    start: Point;
    end: Point;

    constructor(width: number, height: number) {
        super(width, height);
        this.elevations = new Map<string, number>();
    }

    override cost(a: Point, b: Point) {
        return Point.manhattanDistance(a, b);
    }

    override neighbors(id: Point): Iterable<Point> {
        function* generator(map: ElevationMap, neighbors: (id: Point) => Iterable<Point>) {
            const thisHeight = map.elevations.get(id.toString());
            for (const point of neighbors(id)) {
                const nextHeight = map.elevations.get(point.toString());
                const delta = nextHeight - thisHeight;
                if (delta < 2) {
                    yield point;
                }
            }
        }
        return generator(this, (x) => super.neighbors(x));
    }
}
