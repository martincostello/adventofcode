// Copyright (c) Martin Costello, 2015. All rights reserved.

export class Maths {
    static getPermutations<T>(collection: T[]): T[][] {
        return Maths.getPermutationsWithCount(collection, collection.length);
    }

    static getPermutationsWithCount<T>(collection: T[], count: number): T[][] {
        if (count === 1) {
            return collection.map((p) => [p]);
        }

        const permutations = Maths.getPermutationsWithCount(collection, count - 1);

        const result: T[][] = [];

        for (const permutation of permutations) {
            for (const item of collection) {
                if (!permutation.includes(item)) {
                    result.push([...permutation, item]);
                }
            }
        }

        return result;
    }
}
