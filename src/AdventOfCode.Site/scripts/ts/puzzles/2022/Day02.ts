// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Solution } from '../../models/Solution';
import { Puzzle } from '../Puzzle';

enum Move {
    rock = 1,
    paper = 2,
    scissors = 3,
}

enum Outcome {
    lose = 0,
    draw = 3,
    win = 6,
}

export class Day02 extends Puzzle {
    totalScoreForMoves: number;
    totalScoreForOutcomes: number;

    get name(): string {
        return 'Rock Paper Scissors';
    }

    get day(): number {
        return 2;
    }

    get year(): number {
        return 2022;
    }

    static getTotalScore(moves: string[], containsDesiredOutcome: boolean): number {
        let total = 0;

        const moveSelector: (value: string, opponent: Move) => Move = containsDesiredOutcome
            ? (value, opponent) => Day02.getMove(Day02.parseOutcome(value), opponent)
            : (value, _) => Day02.parseMove(value);

        for (const move of moves) {
            if (move === '') {
                break;
            }

            const split = move.split(' ');
            const opponent = split[0];
            const player = split[1];

            const opponentMove = Day02.parseMove(opponent);
            const playerMove = moveSelector(player, opponentMove);

            const outcome = Day02.getOutcome(playerMove, opponentMove);

            total += playerMove + outcome;
        }

        return total;
    }

    override solveCore(_: string[]): Promise<Solution> {
        const startTime = performance.now();

        const moves = this.resource.split('\n');

        this.totalScoreForMoves = Day02.getTotalScore(moves, false);
        this.totalScoreForOutcomes = Day02.getTotalScore(moves, true);

        const endTime = performance.now();

        const solution: Solution = {
            day: this.day,
            solutions: [this.totalScoreForMoves, this.totalScoreForOutcomes],
            timeToSolve: endTime - startTime,
            visualizations: [],
            year: this.year,
        };

        return Promise.resolve(solution);
    }

    private static getMove(outcome: Outcome, opponent: Move): Move {
        switch (outcome) {
            case Outcome.win:
                switch (opponent) {
                    case Move.rock:
                        return Move.paper;
                    case Move.paper:
                        return Move.scissors;
                    case Move.scissors:
                        return Move.rock;
                    default:
                        break;
                }
            case Outcome.lose:
                switch (opponent) {
                    case Move.rock:
                        return Move.scissors;
                    case Move.paper:
                        return Move.rock;
                    case Move.scissors:
                        return Move.paper;
                    default:
                        break;
                }
            case Outcome.draw:
                return opponent;
            default:
                break;
        }

        throw new Error('Invalid outcome and move combination.');
    }

    private static getOutcome(player: Move, opponent: Move): Outcome {
        if (player === opponent) {
            return Outcome.draw;
        }

        switch (player) {
            case Move.rock:
                switch (opponent) {
                    case Move.paper:
                        return Outcome.lose;
                    case Move.scissors:
                        return Outcome.win;
                    default:
                        break;
                }
            case Move.paper:
                switch (opponent) {
                    case Move.rock:
                        return Outcome.win;
                    case Move.scissors:
                        return Outcome.lose;
                    default:
                        break;
                }
            case Move.scissors:
                switch (opponent) {
                    case Move.rock:
                        return Outcome.lose;
                    case Move.paper:
                        return Outcome.win;
                    default:
                        break;
                }
            default:
                break;
        }

        throw new Error('Invalid move combination.');
    }

    private static parseMove(value: string): Move {
        switch (value) {
            case 'A':
            case 'X':
                return Move.rock;
            case 'B':
            case 'Y':
                return Move.paper;
            case 'C':
            case 'Z':
                return Move.scissors;
            default:
                throw new Error('Invalid move.');
        }
    }

    private static parseOutcome(value: string): Outcome {
        switch (value) {
            case 'X':
                return Outcome.lose;
            case 'Y':
                return Outcome.draw;
            case 'Z':
                return Outcome.win;
            default:
                throw new Error('Invalid outcome.');
        }
    }
}
