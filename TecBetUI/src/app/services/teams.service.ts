//Could not import HttpClient from library wihtout adding some stuff in tsconfig.json
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, Observable, of, switchMap, tap, timer } from 'rxjs';
import { Player } from '../models/player.model';
import { Team } from '../models/teams.model';
import { CustomErrorHandlerService } from './custom-error-handler.service';
import * as serviceVariables from './serviceVariables'

@Injectable({
  providedIn: 'root'
})
export class TeamsService {
  isLoading: boolean = false;

  constructor(private http: HttpClient, private customErrorHandlerService: CustomErrorHandlerService) {}

  //Declaring error variables to dynicamally update it depending on what error you encounter
  //The components who wants to use these will then have to .subscribe to it
  private errorSubject = new BehaviorSubject<string>("");
  errorMessage = this.errorSubject.asObservable();

  //Observable is a class in the RxJS library that represents a data stream that can emit zero or more values over time. 
  //Observables are often used to represent asynchronous data sources, such as HTTP requests or user input events.
   //pipe() is a method that allows you to chain together multiple operators to transform or filter the data emitted by an Observable. 
   //An operator is a function that takes an Observable as input and returns a new Observable with modified data. (Below we use .tap() and catchError())
   //tap() is a method that allows you to inspect the data stream without modifying it. It's useful for debugging or logging purposes. 
   getAllTeams(): Observable<Team[]> {
    this.isLoading = true;
    return this.http.get<Team[]>(serviceVariables.baseApiUrl + '/api/Team')
          .pipe(
            tap(teams => {
              this.errorSubject.next('');
            }),
            catchError(error => {
              console.error(error);
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
    return this.http.delete<Team>(serviceVariables.baseApiUrl + '/api/Team/' + id);
  }

  getPlayers(id: string): Observable<Player[]>{
    return this.http.get<Player[]>(serviceVariables.baseApiUrl + '/players/' + id);
  }

}
