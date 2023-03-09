import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, catchError, of, tap } from 'rxjs';
import { Bet } from 'src/app/models/bet.model';
import { baseApiUrl } from './serviceVariables'
import { CustomErrorHandlerService } from './custom-error-handler.service';

@Injectable({
  providedIn: 'root'
})
export class BetService {

  private errorSubject = new BehaviorSubject<string>("");
  errorMessage = this.errorSubject.asObservable();
  isLoading: boolean = false;

  constructor(private http: HttpClient, private customErrorHandler: CustomErrorHandlerService) { }

  getUserBets(userid: string): Observable<Bet[]> {
    this.isLoading = true;
    return this.http.get<Bet[]>(baseApiUrl + '/api/Bet/mybets/' + userid)
          .pipe(
            tap(bets => {
              this.errorSubject.next('');
            }),
            catchError(error => {
              this.errorSubject.next(this.customErrorHandler.handleError(error));
              this.isLoading = false;
              return of([]);
            })
          );
  }

  getBets(): Observable<Bet[]> {
    this.isLoading = true;
    return this.http.get<Bet[]>(baseApiUrl + '/api/Bet')
          .pipe(
            tap(bets => {
              this.errorSubject.next('');
            }),
            catchError(error => {
              this.errorSubject.next(this.customErrorHandler.handleError(error));
              this.isLoading = false;
              return of([]);
            })
          );
  }

  placeBet(bet: Bet): Observable<Bet> {
    bet.id = '00000000-0000-0000-0000-000000000000';
    return this.http.post<Bet>(baseApiUrl + '/api/Bet/place', bet);
  }
}