import { playerTeam } from "./playerTeam.model";

export interface Player{
    id: string;
    name: string;
    age: number;
    teams: playerTeam[];
    overall: number;
    potential: number;
    pace: number;
    shooting: number;
    passing: number;
    dribbling: number;
    defense: number;
    physical: number;
}