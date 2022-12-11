// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { from } from 'linq-to-typescript';
import { Puzzle2022 } from './Puzzle2022';

export class Day06 extends Puzzle2022 {
    indexOfFirstStartOfPacketMarker: number;
    indexOfFirstStartOfMessageMarker: number;

    override get name() {
        return 'Tuning Trouble';
    }

    override get day() {
        return 6;
    }

    static findFirstPacket(datastream: string, distinctCharacters: number) {
        const queue: string[] = [];
        let index = 0;

        for (const item of datastream.split('')) {
            if (queue.length === distinctCharacters && from(queue).distinct().count() === distinctCharacters) {
                break;
            }

            index++;

            queue.push(item);

            if (queue.length > distinctCharacters) {
                queue.shift();
            }
        }

        return index;
    }

    override solveCore(_: string[]) {
        const datastream = this.readResourceAsString();

        this.indexOfFirstStartOfPacketMarker = Day06.findFirstPacket(datastream, 4);
        this.indexOfFirstStartOfMessageMarker = Day06.findFirstPacket(datastream, 14);

        console.info(
            `${this.indexOfFirstStartOfPacketMarker} characters need to be processed before the first start-of-packet marker is detected.`
        );
        console.info(
            `${this.indexOfFirstStartOfMessageMarker} characters need to be processed before the first start-of-message marker is detected.`
        );

        return this.createResult([this.indexOfFirstStartOfPacketMarker, this.indexOfFirstStartOfMessageMarker]);
    }
}
