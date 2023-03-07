import { Team } from "./teams.model";

export interface Bet {
    id: string;
    gameMatchId: string;
    loginId: string;
    team: string;
    amount: number;
    payoutAmount: number;
    betTime: Date;
    result: BetResult | null;
  }

  export enum BetResult {
    Win = 'win',
    Loss = 'loss'
  }


