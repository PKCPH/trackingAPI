import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, Observable, of, switchMap, tap, timer } from 'rxjs';
import { Match } from '../models/matches.model';
import { ParticipatingTeam } from '../models/schedule.model';
import { CustomErrorHandlerService } from './custom-error-handler.service';

@Injectable({
  providedIn: 'root'
})
export class MatchesService {

  baseApiUrl: string = 'https://localhost:5001';
  isLoading: boolean = false;

  constructor(private http: HttpClient, private customErrorHandlerService: CustomErrorHandlerService) {

   }

   private errorSubject = new BehaviorSubject<string>("");
   errorMessage = this.errorSubject.asObservable();

   getAllMatches(): Observable<Match[]> {
    this.isLoading = true;
        return this.http.get<Match[]>(this.baseApiUrl + '/api/Match')
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
    return this.http.get<Match[]>(this.baseApiUrl + '/api/Matches')
      .pipe(
        tap(schedule => {
          this.errorSubject.next('');
        }),
        catchError(error => {
          console.error(error);
          this.errorSubject.next(this.customErrorHandlerService.handleError(error));
          this.isLoading = false;
          this.customErrorHandlerService.handleError(error);
          return of([]);
        })
      );
  }

  addMatch(addMatchRequest: Match): Observable<Match> {
    //Adding this cos JSON doesnt like that we dont return anything to our GUID ID field, so we 
    //just return an empty guid thats gonna be overwritten by the API either way
    addMatchRequest.id = '00000000-0000-0000-0000-000000000000';
    
    return this.http.post<Match>(this.baseApiUrl + '/api/Match/CreateOneMatch', addMatchRequest);
  }

  getMatch(id: string): Observable<Match> {
    return this.http.get<Match>(this.baseApiUrl + '/api/Match/' + id);
  }

  updateMatch(id: string, updateMatchRequest: Match): Observable<Match> {
    updateMatchRequest.participatingTeams = [];
    return this.http.put<Match>(this.baseApiUrl + '/api/Match/' + id, updateMatchRequest);
  }

  deleteMatch(id: string): Observable<Match> {
    return this.http.delete<Match>(this.baseApiUrl + '/api/Match/' + id)
  }
}
