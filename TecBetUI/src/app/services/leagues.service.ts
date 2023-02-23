import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { baseApiUrl } from 'src/app/services/serviceVariables';
import { Leagues } from '../models/leagues.model';

@Injectable({
  providedIn: 'root'
})
export class LeaguesService {

  baseApiUrl: string = baseApiUrl;
  constructor(private http: HttpClient) { }

  getAllLeagues(): Observable<Leagues[]>{
    return this.http.get<Leagues[]>(this.baseApiUrl + '/api/League');
  }
}
