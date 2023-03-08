import { Team } from "./teams.model";

export interface Bet {
    id: string;
    matchId: string;
    loginId: string;
    team: string;
    amount: number;
    payoutAmount: number;
    betTime: Date;
    betResult: string | null;
    betState: string | null;
    participatingTeams: Team[];
  }


