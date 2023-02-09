//Could not import HttpClient from library wihtout adding some stuff in tsconfig.json
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, Observable, of, switchMap, tap, timer } from 'rxjs';
import { Team } from '../models/teams.model';
import { CustomErrorHandlerService } from './custom-error-handler.service';

@Injectable({
  providedIn: 'root'
})
export class TeamsService {

  baseApiUrl: string = 'https://localhost:5001';
  isLoading: boolean = false;

  constructor(private http: HttpClient, private customErrorHandlerService: CustomErrorHandlerService) {

   }

   private errorSubject = new BehaviorSubject<string>("");
   errorMessage = this.errorSubject.asObservable();

   getAllTeams(): Observable<Team[]> {
    this.isLoading = true;
    return this.http.get<Team[]>(this.baseApiUrl + '/api/Team')
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
    
    return this.http.post<Team>(this.baseApiUrl + '/api/Team', addTeamRequest);
  }

  getTeam(id: string): Observable<Team> {
    return this.http.get<Team>(this.baseApiUrl + '/api/Team/' + id);
  }

  updateTeam(id: string, updateTeamRequest: Team): Observable<Team> {
    updateTeamRequest.matches = [];
    return this.http.put<Team>(this.baseApiUrl + '/api/Team/' + id, updateTeamRequest);
  }

  deleteTeam(id: string): Observable<Team> {
    return this.http.delete<Team>(this.baseApiUrl + '/api/Team/' + id)
  }

}
