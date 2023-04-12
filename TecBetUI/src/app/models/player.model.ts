import { playerTeam } from "./playerTeam.model";

export interface Player{
    id: string;
    teams: playerTeam[];
    name: string;
    nationality: string;
    age: number;
    height_cm: number;
    weight_kg: number;
    overall: number;
    player_positions: string;
    preferred_foot: string;
}