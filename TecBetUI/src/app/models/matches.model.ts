import { ParticipatingTeam } from "./schedule.model"
import { Team } from "./teams.model";

export interface Match {
    id: string;
    teamAScore: number;
    teamBScore: number;
    matchState: number;
    dateOfMatch: Date;
    participatingTeams: Team[];
    state: string;
}