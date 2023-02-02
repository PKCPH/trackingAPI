import { Team } from "./teams.model";

export interface Match {
    id: number;
    TeamAScore: number;
    TeamBScore: number;
    MatchState: number;
    DateOfMatch: Date;
    participatingTeams: Team[];
}