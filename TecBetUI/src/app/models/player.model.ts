import { playerTeam } from "./playerTeam.model";
import { Team } from "./teams.model";

export interface Player{
    id: string;
    name: string;
    age: number;
    teams: playerTeam[];
}