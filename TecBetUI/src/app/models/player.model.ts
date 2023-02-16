import { Team } from "./teams.model";

export interface Player{
    id: string;
    name: string;
    age: number;
    teamId: string;
    team: Team;
}