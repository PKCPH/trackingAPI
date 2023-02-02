import { Match } from "./matches.model";

export interface Team {
    id: string;
    name: string;
    IsAvailable: boolean;
    matches: Match[];
    availability: string;
}

