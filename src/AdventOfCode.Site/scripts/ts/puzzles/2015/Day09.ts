// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { from } from 'linq-to-typescript';
import { Puzzle } from '../Puzzle';
import { Puzzle2015 } from './Puzzle2015';

export class Day09 extends Puzzle2015 {
    shortestDistance: number;
    longestDistance: number;

    override get name() {
        return 'All in a Single Night';
    }

    override get day() {
        return 9;
    }

    protected override get requiresData() {
        return true;
    }

    static getDistanceBetweenPoints(collection: string[], findLongest: boolean) {
        const getRouteDistancesFromLocation = (map: LocationMap, start: string) => {
            const visited: string[] = [];
            return getRouteDistances(map, start, visited);
        };

        const getRouteDistances = (map: LocationMap, location: string, visited: string[]) => {
            visited.push(location);

            if (visited.length === map.edges.size) {
                let current = visited[0];
                let distance = 0;

                for (let i = 1; i < visited.length; i++) {
                    const next = visited[i];

                    distance += map.cost(current, next);

                    current = next;
                }

                visited.splice(visited.indexOf(location), 1);

                return [distance];
            }

            let distances: number[] = [];

            for (const next of map.neighbors(location)) {
                if (visited.indexOf(next) < 0) {
                    distances = distances.concat(getRouteDistances(map, next, visited));
                }
            }

            visited.splice(visited.indexOf(location), 1);

            return distances;
        };

        type Vector = [start: string, end: string];

        const distances = new Map<string, number>();
        const vectors: Vector[] = [];

        for (let i = 0; i < collection.length; i++) {
            const split = collection[i].split(' = ');
            const locations = split[0].split(' to ');

            const start = locations[0];
            const end = locations[1];

            vectors[i] = [start, end];

            const distance = Puzzle.parse(split[1]);

            distances.set(`${start} to ${end}`, distance);
            distances.set(`${end} to ${start}`, distance);
        }

        const getOrAdd = <T>(values: Map<T, T[]>, key: T) => {
            if (values.has(key)) {
                return values.get(key);
            } else {
                const value: T[] = [];
                values.set(key, value);
                return value;
            }
        };

        const map = new LocationMap(distances);

        for (const [start, end] of vectors) {
            getOrAdd(map.edges, start).push(end);
            getOrAdd(map.edges, end).push(start);
        }

        let routeDistances: number[] = [];

        for (const [location, _] of map.edges) {
            routeDistances = routeDistances.concat(getRouteDistancesFromLocation(map, location));
        }

        return findLongest ? from(routeDistances).max() : from(routeDistances).min();
    }

    override solveCore(_: string[]) {
        const distances = this.readResourceAsLines();

        this.shortestDistance = Day09.getDistanceBetweenPoints(distances, false);
        this.longestDistance = Day09.getDistanceBetweenPoints(distances, true);

        console.info(`The distance of the shortest route is ${this.shortestDistance}.`);
        console.info(`The distance of the longest route is ${this.longestDistance}.`);

        return this.createResult([this.shortestDistance, this.longestDistance]);
    }
}

class Graph<T> {
    readonly edges = new Map<T, T[]>();

    neighbors(id: T) {
        return this.edges.get(id);
    }
}

class LocationMap extends Graph<string> {
    constructor(private readonly distances: Map<string, number>) {
        super();
    }

    cost(a: string, b: string) {
        return this.distances.get(`${b} to ${a}`);
    }
}
