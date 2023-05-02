import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, Observable, of, tap } from 'rxjs';
import { Timelogs } from '../models/timelog.model';
import { CustomErrorHandlerService } from './custom-error-handler.service';
import * as serviceVariables from './serviceVariables'

@Injectable({
  providedIn: 'root'
})
export class TimelogService {
  isLoading: boolean = false;

  ////// For code clarification look in teams.serviec.ts //////
  constructor(private http: HttpClient, private customErrorHandlerService: CustomErrorHandlerService) { }

  //Declaring error variables to dynicamally update it depending on what error you encounter
  //The components who wants to use these will then have to .subscribe to it
  private errorSubject = new BehaviorSubject<string>("");
  errorMessage = this.errorSubject.asObservable();

  getAllTimelogs(): Observable<Timelogs[]> {
    this.isLoading = true;
    return this.http.get<Timelogs[]>(serviceVariables.baseApiUrl + '/api/Timelog')
      .pipe(
        tap(timelogs => {
          this.errorSubject.next('');
        }),
        catchError(error => {
          this.errorSubject.next(this.customErrorHandlerService.handleError(error));
          this.isLoading = false;
          return of([]);
        })
      );
  }


  getTimelogsFromGamematch(gamematchId: string): Observable<Timelogs[]> {
    this.isLoading = true;
    return this.http.get<Timelogs[]>(serviceVariables.baseApiUrl + '/api/Timelog/' + gamematchId)
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

  getLastTimelogFromGamematch(gamematchId: string): Observable<Timelogs[]> {
    this.isLoading = true;
    return this.http.get<Timelogs[]>(serviceVariables.baseApiUrl + '/api/Timelog/lastestTimelog/' + gamematchId)
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
}
