// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { from } from 'linq-to-typescript';
import { Puzzle2015 } from './Puzzle2015';

export class Day05 extends Puzzle2015 {
    niceStringCount = 0;

    override get name() {
        return "Doesn't He Have Intern-Elves For This?";
    }

    override get day() {
        return 5;
    }

    protected override get minimumArguments() {
        return 1;
    }

    static isNice(value: string, version: '1' | '2') {
        switch (version) {
            case '1':
                return Day05.isNiceV1(value);
            case '2':
                return Day05.isNiceV2(value);
        }
    }

    override solveCore(inputs: string[]) {
        const version = inputs[0];

        if (version !== '1' && version !== '2') {
            throw new Error('The rules version specified is invalid.');
        }

        this.niceStringCount = this.readResourceAsLines().filter((value) => Day05.isNice(value, version)).length;

        console.info(`${this.niceStringCount} strings are nice using version ${version} of the rules.`);

        return this.createResult([this.niceStringCount]);
    }

    private static isNiceV1(value: string) {
        const notNice = from(['ab', 'cd', 'pq', 'xy']);
        if (notNice.any((p: string) => value.includes(p))) {
            return false;
        }

        let vowels = 0;
        let hasAnyConsecutiveLetters = false;

        const isNice = () => hasAnyConsecutiveLetters && vowels > 2;
        const isVowel = (letter: string) => {
            switch (letter) {
                case 'a':
                case 'e':
                case 'i':
                case 'o':
                case 'u':
                    return true;
                default:
                    return false;
            }
        };

        for (let i = 0; i < value.length; i++) {
            const current = value[i];

            if (isVowel(current)) {
                vowels++;
            }

            if (i > 0 && !hasAnyConsecutiveLetters) {
                hasAnyConsecutiveLetters = current === value[i - 1];
            }

            if (isNice()) {
                return true;
            }
        }

        return isNice();
    }

    private static isNiceV2(value: string) {
        const hasPairOfLettersWithMoreThanOneOccurence = (value: string) => {
            const letterPairs = new Map<string, number[]>();

            for (let i = 0; i < value.length - 1; i++) {
                const pair = value.slice(i, i + 2);

                let indexes = letterPairs.get(pair);

                if (!indexes) {
                    indexes = [];
                    letterPairs.set(pair, indexes);
                }

                if (!indexes.includes(i - 1)) {
                    indexes.push(i);
                }
            }

            for (const indexes of letterPairs.values()) {
                if (indexes.length > 1) {
                    return true;
                }
            }

            return false;
        };

        const hasLetterThatIsTheBreadOfALetterSandwich = (value: string) => {
            if (value.length < 3) {
                return false;
            }

            for (let i = 1; i < value.length - 1; i++) {
                if (value[i - 1] === value[i + 1]) {
                    return true;
                }
            }

            return false;
        };

        return hasLetterThatIsTheBreadOfALetterSandwich(value) && hasPairOfLettersWithMoreThanOneOccurence(value);
    }
}
