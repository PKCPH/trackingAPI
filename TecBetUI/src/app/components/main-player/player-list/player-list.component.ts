import { Component } from '@angular/core';
import { Player } from 'src/app/models/player.model';
import { PlayersService } from 'src/app/services/players.service';
import { Router } from '@angular/router';
import { AppComponent } from 'src/app/app.component';
import { LoginModel } from 'src/app/models/login.model';

@Component({
  selector: 'app-player-list',
  templateUrl: './player-list.component.html',
  styleUrls: ['./player-list.component.css']
})
export class PlayerListComponent {
  players: Player[] = [];
  credentials: LoginModel = this.app.credentials
  constructor(private playersService: PlayersService, private router: Router, private app: AppComponent) {
  }

  ngOnInit(): void {
    this.playersService.getAllPlayers()
    .subscribe({
      next: (players) => {
        console.log(players);
        this.players = players
      },
      error: (response) => {
        console.log(response);
      }
    })
  }
  GoAddPlayer() {
    this.router.navigateByUrl('/players/add')
  }
  FilterPlayers(search: string){

  }
  isUserAuthenticated = (): boolean => {
    return this.app.isUserAuthenticated()
  }

  deletePlayer(id: string){
    this.playersService.deletePlayer(id)
    .subscribe({
      next: (response) => {
      }
    })
    this.players.splice(this.players.findIndex(p => p.id == id),1)
  }
}