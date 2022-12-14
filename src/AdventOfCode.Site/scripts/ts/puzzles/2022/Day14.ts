// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Point } from '../Point';
import { Puzzle } from '../Puzzle';
import { Rectangle } from '../Rectangle';
import { Size } from '../Size';
import { Puzzle2022 } from './Puzzle2022';

export class Day14 extends Puzzle2022 {
    grainsOfSandWithVoid: number;
    grainsOfSandWithFloor: number;

    override get name() {
        return 'Regolith Reservoir';
    }

    override get day() {
        return 14;
    }

    static simulate(paths: string[], hasFloor: boolean): [grains: number, visualization: string] {
        const down = new Size(0, 1);
        const left = new Size(-1, 1);
        const right = new Size(1, 1);

        const parse = (paths: string[]) => {
            const cave = new PointMap<Content>();

            for (const path of paths) {
                const points = path.split(' -> ').map((p) => {
                    const [first, second] = p.split(',');
                    return new Point(Puzzle.parse(first), Puzzle.parse(second));
                });

                for (let i = 0; i < points.length - 1; i++) {
                    const start = points[i];
                    const end = points[i + 1];

                    cave.setKey(start, Content.rock);

                    const direction = new Size(end.x - start.x, end.y - start.y);
                    const magnitude = Math.max(Math.abs(direction.width), Math.abs(direction.height));
                    const unit = direction.divide(magnitude);

                    let current = start;

                    while (!current.equals(end)) {
                        current = Point.add(current, unit);
                        cave.setKey(current, Content.rock);
                    }
                }
            }

            return cave;
        };

        const nextLocation = (location: Point, cave: PointMap<Content>): Point | null => {
            let next = Point.add(location, down);

            if (!cave.hasKey(next)) {
                return next;
            }

            next = Point.add(location, left);

            if (!cave.hasKey(next)) {
                return next;
            }

            next = Point.add(location, right);

            if (!cave.hasKey(next)) {
                return next;
            }

            return null;
        };

        const simulate = (cave: PointMap<Content>, origin: Point, bounds: Rectangle, hasFloor: boolean) => {
            for (let i = 0; ; i++) {
                let current = origin;

                while (!bounds.contains(current)) {
                    const next = nextLocation(current, cave);

                    if (next === null) {
                        cave.setKey(current, Content.sand);
                        break;
                    }

                    current = next;

                    // Extend the bounds to account for the "infinite" floor so sand can still flow
                    if (hasFloor && (current.x === bounds.left + 1 || current.x === bounds.right - 1)) {
                        bounds = new Rectangle(bounds.x - 1, bounds.top, bounds.width + 2, bounds.height);
                        cave.setKey(new Point(bounds.left, bounds.bottom), Content.floor);
                        cave.setKey(new Point(bounds.right, bounds.bottom), Content.floor);
                    }
                }

                if (!bounds.contains(current)) {
                    return i - 1;
                } else if (hasFloor && current.equals(origin)) {
                    return i;
                }
            }
        };

        const visualize = (cave: PointMap<Content>, origin: Point) => {
            let maxX = -1;
            let minX = 1000000;
            let maxY = -1;
            const minY = 0;

            for (const item of cave.points()) {
                maxX = Math.max(maxX, item.x);
                minX = Math.min(minX, item.x);
                maxY = Math.max(maxY, item.y);
            }

            let result = '';

            for (let y = minY; y <= maxY; y++) {
                for (let x = minX; x <= maxX; x++) {
                    const location = new Point(x, y);

                    if (location.equals(origin)) {
                        result += '+';
                    } else if (cave.hasKey(location)) {
                        const content = cave.get(location.toString());
                        result += content === Content.sand ? 'o' : '#';
                    } else {
                        result += '.';
                    }
                }

                result += '\n';
            }

            return result;
        };

        const cave = parse(paths);

        let maxX = -1;
        let minX = 1000000;
        let maxY = -1;
        const minY = 0;

        for (const item of cave.points()) {
            maxX = Math.max(maxX, item.x);
            minX = Math.min(minX, item.x);
            maxY = Math.max(maxY, item.y);
        }

        if (hasFloor) {
            maxY += 2;
            for (let x = minX; x <= maxX; x++) {
                cave.setKey(new Point(x, maxY), Content.floor);
            }
        }

        const origin = new Point(500, 0);
        const bounds = new Rectangle(minX, minY, maxX - minX, maxY);

        const grains = simulate(cave, origin, bounds, hasFloor);
        const visualization = visualize(cave, origin);

        return [grains, visualization];
    }

    override solveCore(_: string[]) {
        const paths = this.readResourceAsLines();

        let visualization1 = '';
        let visualization2 = '';

        [this.grainsOfSandWithVoid, visualization1] = Day14.simulate(paths, false);
        [this.grainsOfSandWithFloor, visualization2] = Day14.simulate(paths, true);

        console.info(`${this.grainsOfSandWithVoid} grains of sand come to rest before sand starts flowing into the abyss below.`);
        console.info(`${this.grainsOfSandWithFloor} grains of sand come to rest before sand blocks the source.`);

        return this.createResult([this.grainsOfSandWithVoid, this.grainsOfSandWithFloor], [visualization1, visualization2]);
    }
}

enum Content {
    rock = 'Rock',
    sand = 'Sand',
    floor = 'Floor',
}

class PointMap<V> extends Map<string, V> {
    points() {
        function* generator(map: PointMap<V>) {
            for (const key of map.keys()) {
                const [x, y] = key.split(',');
                yield new Point(parseInt(x, 10), parseInt(y, 10));
            }
        }
        return generator(this);
    }

    getValue(key: Point) {
        return super.get(key.toString());
    }

    hasKey(key: Point) {
        return super.has(key.toString());
    }

    setKey(key: Point, value: V) {
        this.set(key.toString(), value);
    }
}
