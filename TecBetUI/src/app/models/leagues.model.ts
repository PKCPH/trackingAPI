import { Time } from "@angular/common";
import { DeclarationListEmitMode } from "@angular/compiler";
import { Team } from "./teams.model";
import { Match } from "./matches.model";

export interface Leagues {
    id: string;
    name: string;
    startDate: string;
    match: Match[];
    team: Team[];
}

// enum LeagueState{
//     NotStarted, InProgress, Finished
//}