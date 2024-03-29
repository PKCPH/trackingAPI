import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Player } from '../models/player.model';
import { Observable } from 'rxjs';
import * as serviceVariables from './serviceVariables'

@Injectable({
  providedIn: 'root'
})
export class PlayersService {

  constructor(private http: HttpClient) { }

  getAllPlayers(): Observable<Player[]>{
    return this.http.get<Player[]>(serviceVariables.baseApiUrl + '/api/Player')
  }
  addPlayer(addPlayerRequest: Player): Observable<Player>{
    addPlayerRequest.id = '00000000-0000-0000-0000-000000000000'
    addPlayerRequest.teams.forEach(element => {
      element.id = '00000000-0000-0000-0000-000000000000';
      element.playerId = '00000000-0000-0000-0000-000000000000'
    });
    if(addPlayerRequest.overall == 0) addPlayerRequest.overall = 1
    return this.http.post<Player>(serviceVariables.baseApiUrl + '/api/Player',addPlayerRequest);
  }
  getPlayer(id:string): Observable<Player>{
    return this.http.get<Player>(serviceVariables.baseApiUrl + '/api/Player/' + id)
  }
  updatePlayer(id:string, updatePlayerRequest: Player): Observable<Player>{
    if(updatePlayerRequest.teams != null){
      updatePlayerRequest.teams.forEach(element => {
        element.id = '00000000-0000-0000-0000-000000000000'
      });
    }
    
    console.log(updatePlayerRequest)

    return this.http.put<Player>(serviceVariables.baseApiUrl + '/api/Player/' + id, updatePlayerRequest);
  }
  deletePlayer(id:string):Observable<Player>{
    return this.http.delete<Player>(serviceVariables.baseApiUrl + '/api/Player/' + id);
  }
}