import { Team } from "./teams.model";
import { TimeLog } from "./timelog.model";

export interface Match {
    id: string;
    matchState: number;
    dateOfMatch: Date;
    participatingTeams: Team[];
    state: string;
    displayId: string;
    league: string;
    roundTerm: string;
    duration: number;
    timeLogs: TimeLog[];
}