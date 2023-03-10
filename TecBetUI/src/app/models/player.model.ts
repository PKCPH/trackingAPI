import { playerTeam } from "./playerTeam.model";
import { Team } from "./teams.model";

export interface Player{
    id: string;
    name: string;
    age: number;
    teams: playerTeam[];
    goals: number;
    assists: number;
    yellow: number;
    red: number;
    spG: number;
    psPercent: number;
    motm: number;
}