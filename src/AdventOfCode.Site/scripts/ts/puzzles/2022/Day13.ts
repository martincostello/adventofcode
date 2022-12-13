// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { from } from 'linq-to-typescript';
import { Puzzle2022 } from './Puzzle2022';

export class Day13 extends Puzzle2022 {
    sumOfPresortedIndicies: number;
    decoderKey: number;

    override get name() {
        return 'Distress Signal';
    }

    override get day() {
        return 13;
    }

    static decodePackets(packets: string[]): [sumOfPresortedIndicies: number, decoderKey: number] {
        const parse = (values: string[]) => {
            const parseValue = (packet: Packet, value: string) => {
                let read = 0;

                for (let i = 0; i < value.length; i++) {
                    const c = value[i];

                    switch (c) {
                        case '[':
                            const child = new Packet([]);
                            i += parseValue(child, value.slice(i + 1));
                            if (packet.value instanceof Array) {
                                packet.value.push(child);
                            }
                            break;

                        case ']':
                            return i + 1;

                        case ',':
                            break;

                        default:
                            const zeroChar = '0'.charCodeAt(0);
                            let packetValue = c.charCodeAt(0) - zeroChar;

                            if (i < value.length - 1 && value[i + 1] === '0') {
                                packetValue += 10;
                                i++;
                            }

                            if (packet.value instanceof Array) {
                                packet.value.push(new Packet(packetValue));
                            }
                            break;
                    }
                }

                return read;
            };

            const result: [left: Packet, right: Packet][] = [];

            for (let i = 0; i < values.length; i += 3) {
                const left = new Packet([]);
                const right = new Packet([]);

                parseValue(left, values[i].slice(1, -1));
                parseValue(right, values[i + 1].slice(1, -1));

                result.push([left, right]);
            }

            return result;
        };

        const pairs = parse(packets);

        let sum = 0;

        for (let i = 0; i < pairs.length; i++) {
            const [left, right] = pairs[i];

            const sortedCorrectly = left.compareTo(right) < 0;

            if (sortedCorrectly) {
                sum += i + 1;
            }
        }

        const divider1 = new Packet([new Packet(2)]);
        const divider2 = new Packet([new Packet(6)]);

        const sorted: Packet[] = from(pairs)
            .selectMany((p: [Packet, Packet]) => [p[0], p[1]])
            .concatenate(from([divider1, divider2]))
            .toArray();

        sorted.sort((x, y) => x.compareTo(y));

        const dividerIndex1 = 1 + sorted.findIndex((p) => p === divider1);
        const dividerIndex2 = 1 + sorted.findIndex((p) => p === divider2);

        const decoderKey = dividerIndex1 * dividerIndex2;

        return [sum, decoderKey];
    }

    override solveCore(_: string[]) {
        const packets = this.readResourceAsLines();

        [this.sumOfPresortedIndicies, this.decoderKey] = Day13.decodePackets(packets);

        console.info(`The sum of the indices of the pairs of packets already in the right order is ${this.sumOfPresortedIndicies}.`);
        console.info(`The decoder key for the distress signal is ${this.decoderKey}.`);

        return this.createResult([this.sumOfPresortedIndicies, this.decoderKey]);
    }
}

class Packet extends Object {
    readonly value: number | Packet[];

    constructor(value: number);
    constructor(values: Packet[]);
    constructor(value: number | Packet[] = []) {
        super();
        this.value = value;
    }

    compareTo(other: Packet): number {
        if (!(this.value instanceof Array) && !(other.value instanceof Array)) {
            return this.value - other.value;
        } else if (this.value instanceof Array && other.value instanceof Array) {
            for (let i = 0; i < this.value.length && i < other.value.length; i++) {
                const comparison = this.value[i].compareTo(other.value[i]);
                if (comparison !== 0) {
                    return comparison;
                }
            }
            return this.value.length - other.value.length;
        } else {
            return Packet.expandToArray(this).compareTo(Packet.expandToArray(other));
        }
    }

    private static expandToArray(packet: Packet): Packet {
        if (packet.value instanceof Array) {
            return packet;
        } else {
            return new Packet([new Packet(packet.value)]);
        }
    }
}
