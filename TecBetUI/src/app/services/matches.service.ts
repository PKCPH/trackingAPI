import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, Observable, of, switchMap, tap, timer } from 'rxjs';
import { Match } from '../models/matches.model';
import { CustomErrorHandlerService } from './custom-error-handler.service';
import * as serviceVariables from './serviceVariables'

@Injectable({
  providedIn: 'root'
})
export class MatchesService {
  isLoading: boolean = false;

  ////// For code clarification look in teams.serviec.ts //////

  constructor(private http: HttpClient, private customErrorHandlerService: CustomErrorHandlerService) {}

   private errorSubject = new BehaviorSubject<string>("");
   errorMessage = this.errorSubject.asObservable();

   getAllMatches(): Observable<Match[]> {
    this.isLoading = true;
        return this.http.get<Match[]>(serviceVariables.baseApiUrl + '/api/Match')
          .pipe(
            tap(matches => {
              this.errorSubject.next('');
            }),
            catchError(error => {
              console.error(error);
              // this.errorSubject.next(error.message);
              this.errorSubject.next(this.customErrorHandlerService.handleError(error));
              this.isLoading = false;
              return of([]);
            })
          );
  }

  getSchedule(): Observable<Match[]> {
    this.isLoading = true;
    return this.http.get<Match[]>(serviceVariables.baseApiUrl + '/api/Matches')
      .pipe(
        tap(schedule => {
          this.errorSubject.next('');
        }),
        catchError(error => {
          // console.error(error);
          this.errorSubject.next(this.customErrorHandlerService.handleError(error));
          this.isLoading = false;
          return of([]);
        })
      );
  }

  getFinishedMatches(): Observable<Match[]> {
    this.isLoading = true;
    return this.http.get<Match[]>(serviceVariables.baseApiUrl + '/api/MatchesFin')
      .pipe(
        tap(schedule => {
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

  addMatch(addMatchRequest: Match): Observable<Match> {
    //Adding this cos JSON doesnt like that we dont return anything to our GUID ID field, so we 
    //just return an empty guid thats gonna be overwritten by the API either way
    addMatchRequest.id = '00000000-0000-0000-0000-000000000000';
    
    return this.http.post<Match>(serviceVariables.baseApiUrl + '/api/Match/CreateOneMatch', addMatchRequest);
  }

  getMatch(id: string): Observable<Match> {
    return this.http.get<Match>(serviceVariables.baseApiUrl + '/api/Match/' + id);
  }

  updateMatch(id: string, updateMatchRequest: Match): Observable<Match> {
    // updateMatchRequest.participatingTeams = [];
    return this.http.put<Match>(serviceVariables.baseApiUrl + '/api/Match/' + id, updateMatchRequest);
  }

  deleteMatch(id: string): Observable<Match> {
    return this.http.delete<Match>(serviceVariables.baseApiUrl + '/api/Match/' + id)
  }

  getMatchDetails(id: string): Observable<Match> {
    return this.http.get<Match>(serviceVariables.baseApiUrl + '/api/MatchDetails/' + id);
  }
}
