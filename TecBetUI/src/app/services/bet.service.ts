import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Bet } from 'src/app/models/bet.model';

@Injectable({
  providedIn: 'root'
})
export class BetService {

  private readonly apiUrl = 'https://localhost:5001/api/Bet/place';

  constructor(private http: HttpClient) { }

  placeBet(bet: Bet): Observable<Bet> {
    bet.id = '00000000-0000-0000-0000-000000000000';
    return this.http.post<Bet>(this.apiUrl, bet);
  }
}