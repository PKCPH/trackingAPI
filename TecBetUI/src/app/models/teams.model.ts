import { Match } from "./matches.model";
import { playerTeam } from "./playerTeam.model";

export interface Team {
    id: string;
    name: string;
    isAvailable: boolean;
    matches: Match[];
    availability: string;
    score: number;
    result: number;
    players: playerTeam[];
}

