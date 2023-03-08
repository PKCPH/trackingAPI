//Could not import HttpClient from library wihtout adding some stuff in tsconfig.json
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, Observable, of, switchMap, tap, timer } from 'rxjs';
import { Player } from '../models/player.model';
import { playerTeam } from '../models/playerTeam.model';
import { Team } from '../models/teams.model';
import { CustomErrorHandlerService } from './custom-error-handler.service';
import * as serviceVariables from './serviceVariables'

@Injectable({
  providedIn: 'root'
})
export class TeamsService {
  isLoading: boolean = false;

  constructor(private http: HttpClient, private customErrorHandlerService: CustomErrorHandlerService) {}

   private errorSubject = new BehaviorSubject<string>("");
   errorMessage = this.errorSubject.asObservable();

   getAllTeams(): Observable<Team[]> {
    this.isLoading = true;
    return this.http.get<Team[]>(serviceVariables.baseApiUrl + '/api/Team')
          .pipe(
            tap(teams => {
              this.errorSubject.next('');
            }),
            catchError(error => {
              this.errorSubject.next(this.customErrorHandlerService.handleError(error));
              this.isLoading = false;
              return of([]);
            })
          );
  }

  addTeam(addTeamRequest: Team): Observable<Team> {
    //Adding this cos JSON doesnt like that we dont return anything to our GUID ID field, so we 
    //just return an empty guid thats gonna be overwritten by the API either way
    addTeamRequest.id = '00000000-0000-0000-0000-000000000000';
    
    return this.http.post<Team>(serviceVariables.baseApiUrl + '/api/Team', addTeamRequest);
  }

  getTeam(id: string): Observable<Team> {
    return this.http.get<Team>(serviceVariables.baseApiUrl + '/api/Team/' + id);
  }

  updateTeam(id: string, updateTeamRequest: Team): Observable<Team> {
    // updateTeamRequest.matches = [];
    return this.http.put<Team>(serviceVariables.baseApiUrl + '/api/Team/' + id, updateTeamRequest);
  }

  deleteTeam(id: string): Observable<Team> {
    return this.http.delete<Team>(serviceVariables.baseApiUrl + '/api/Team/' + id)
  }

  getPlayers(id: string): Observable<Player[]>{
    return this.http.get<Player[]>(serviceVariables.baseApiUrl + '/api/Team/players/' + id)
  }

  changePlayers(playerTeams: playerTeam[][]): Observable<playerTeam[][]>{
    console.log(playerTeams);
    return this.http.post<playerTeam[][]>(serviceVariables.baseApiUrl + '/api/Team/players/add', playerTeams)
  }
}
