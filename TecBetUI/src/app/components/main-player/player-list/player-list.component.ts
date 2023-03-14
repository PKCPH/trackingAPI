import { Component } from '@angular/core';
import { Player } from 'src/app/models/player.model';
import { PlayersService } from 'src/app/services/players.service';
import { Router } from '@angular/router';
import { AppComponent } from 'src/app/app.component';
import { LoginModel } from 'src/app/models/login.model';
import { interval, Subscription, switchMap } from 'rxjs';

@Component({
  selector: 'app-player-list',
  templateUrl: './player-list.component.html',
  styleUrls: ['./player-list.component.css']
})
export class PlayerListComponent {
  players: Player[] = [];
  searchedPlayers: Player[] = [];
  credentials: LoginModel = this.app.credentials
  updateSubscription: Subscription;

  model = {name: "",
  age: 0,
  overall: 0,
  potential: 0,
  pace: 0,
  shooting: 0,
  passing: 0,
  dribbling: 0,
  defense: 0,
  physical: 0,
  }
  
  ngOnDestroy() {
    this.updateSubscription.unsubscribe();
  }

  constructor(private playersService: PlayersService, private router: Router, private app: AppComponent) {
    this.updateSubscription = interval(3000).pipe(
      switchMap(() => this.playersService.getAllPlayers())
    )
    .subscribe({
      next: (players) => {
        console.log(players);
        this.players = players
        this.searchPlayers()
      },
      error: (response) => {
        console.log(response);
      }
    })
  }


  ngOnInit(): void {
    this.playersService.getAllPlayers()
    .subscribe({
      next: (players) => {
        console.log(players);
        this.players = players
        this.searchedPlayers = players
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
    this.searchPlayers()
  }

  searchPlayers(){
    this.searchedPlayers = this.players.filter(p => 
      p.name.includes(this.model.name) &&
      p.overall >= this.model.overall &&
      p.potential >= this.model.potential &&
      p.pace >= this.model.pace &&
      p.shooting >= this.model.pace &&
      p.passing >= this.model.passing &&
      p.dribbling >= this.model.dribbling &&
      p.defense >= this.model.defense &&
      p.physical >= this.model.physical
    )
  }
}