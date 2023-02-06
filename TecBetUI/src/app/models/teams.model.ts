import { Match } from "./matches.model";

export interface Team {
    id: string;
    name: string;
    isAvailable: boolean;
    matches: Match[];
    availability: string;
}

