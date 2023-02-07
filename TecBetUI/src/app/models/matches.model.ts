import { Team } from "./teams.model";
import { ParticipatingTeam } from "./schedule.model"

export interface Match {
    id: string;
    teamAScore: number;
    teamBScore: number;
    matchState: number;
    dateOfMatch: Date;
    participatingTeams: ParticipatingTeam[];
    state: string;
}