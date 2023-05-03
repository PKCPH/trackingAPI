import { Time } from "@angular/common";
import { DeclarationListEmitMode } from "@angular/compiler";
import { Team } from "./teams.model";
import { Match } from "./matches.model";

export interface Leagues {
    id: string;
    name: string;
    startDate: string;
    match: Match[];
    amountOfTeams: string;
    leagueState: number;
}

// enum LeagueState{
//     NotStarted, InProgress, Finished
//}