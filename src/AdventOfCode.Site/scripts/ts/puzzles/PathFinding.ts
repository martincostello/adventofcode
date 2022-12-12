// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Graph } from './Graph';
import { WeightedGraph } from './WeightedGraph';

export class PathFinding {
    static astar<T>(graph: WeightedGraph<T>, start: T, goal: T, heuristic: ((x: T, y: T) => number) | null = null) {
        heuristic ??= graph.cost;

        const frontier = new PriorityQueue<T>();
        frontier.enqueue(start, 0);

        const costSoFar = new Map<T, number>();
        costSoFar.set(start, 0);

        while (frontier.count > 0) {
            const current = frontier.dequeue();

            if (current === goal) {
                break;
            }

            for (const next of graph.neighbors(current)) {
                const newCost = costSoFar.get(current) + heuristic(current, next);

                if (!costSoFar.has(next) || newCost < costSoFar.get(next)) {
                    costSoFar.set(next, newCost);

                    const goalCost = heuristic(next, goal);
                    const priority = newCost + goalCost;

                    frontier.enqueue(next, priority);
                }
            }
        }

        if (!costSoFar.has(goal)) {
            return -1;
        }

        return costSoFar.get(goal);
    }

    static breadthFirst<T>(graph: Graph<T>, start: T) {
        const frontier: T[] = [];
        frontier.push(start);

        const reached = new Set<T>();
        reached.add(start);

        while (frontier.length > 0) {
            const current = frontier.shift();
            for (const next of graph.neighbors(current)) {
                if (!reached.has(next)) {
                    frontier.push(next);
                    reached.add(next);
                }
            }
        }

        return reached;
    }
}

class PriorityQueue<T> {
    private readonly elements: [item: T, priority: number][] = [];

    get count() {
        return this.elements.length;
    }

    enqueue(item: T, priority: number) {
        this.elements.push([item, priority]);
    }

    dequeue() {
        let bestIndex = 0;
        let [bestItem, bestPriority] = this.elements[bestIndex];

        for (let i = 0; i < this.elements.length; i++) {
            const [item, priority] = this.elements[i];
            if (priority < bestPriority) {
                bestIndex = i;
                [bestItem, bestPriority] = [item, priority];
            }
        }

        this.elements.splice(bestIndex, 1);

        return bestItem;
    }
}
