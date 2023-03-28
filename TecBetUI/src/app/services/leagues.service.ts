import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Leagues } from '../models/leagues.model';
import * as serviceVariables from './serviceVariables'

@Injectable({
  providedIn: 'root'
})
export class LeaguesService {

  //not needed, but followed tutorial
  baseApiUrl: string = serviceVariables.baseApiUrl

  constructor(private http: HttpClient) { }

  getAllLeagues(): Observable<Leagues[]> {
  return this.http.get<Leagues[]>(this.baseApiUrl + '/api/Leagues');
  }

  addLeague(addLeagueRequest: Leagues): Observable<Leagues> {
    addLeagueRequest.id = '00000000-0000-0000-0000-000000000000';
    return this.http.post<Leagues>(this.baseApiUrl + '/api/League', addLeagueRequest);
  }
}
