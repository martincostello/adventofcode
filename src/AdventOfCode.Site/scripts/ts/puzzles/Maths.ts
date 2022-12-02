// Copyright (c) Martin Costello, 2015. All rights reserved.

export class Maths {
    static getPermutations<T>(collection: T[]): T[][] {
        return Maths.getPermutationsWithCount(collection, collection.length);
    }

    static getPermutationsWithCount<T>(collection: T[], count: number): T[][] {
        if (count === 1) {
            return collection.map((p) => [p]);
        }

        const first = new Set<T>(collection);

        return Maths.getPermutationsWithCount(collection, count - 1)
            .map((p) => Maths.except(first, new Set<T>(p)))
            .reduce((set, value) => set.concat(value), []);
    }

    private static except<T>(a: Set<T>, b: Set<T>): Set<T> {
        // See https://bobbyhadz.com/blog/javascript-get-difference-between-two-sets
        const difference = (a: Set<T>, b: Set<T>): Set<T> => {
            return new Set([...a].filter((element) => !b.has(element)));
        };

        return new Set([...difference(a, b), ...difference(b, a)]);
    }
}
