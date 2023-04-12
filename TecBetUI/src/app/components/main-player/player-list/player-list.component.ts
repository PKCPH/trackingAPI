import { Component } from '@angular/core';
import { Player } from 'src/app/models/player.model';
import { PlayersService } from 'src/app/services/players.service';
import { ActivatedRoute, Router } from '@angular/router';
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
  shownPlayers: Player[] = [];
  credentials: LoginModel = this.app.credentials
  updateSubscription: Subscription;
  iValue: number = 0
  pageNumber: number = 0

  model = {
    id: "",
    teams:[],
    name: "",
    nationality: "",
    older: "Older",
    age: 0,
    taller: "Taller",
    height_cm: 0,
    heavier: "Heavier",
    weight_kg: 0,
    better: "Better",
    overall: 0,
    player_positions: "",
    preferred_foot: "",
  }
  
  ngOnDestroy() {
    this.updateSubscription.unsubscribe();
  }

  constructor(private playersService: PlayersService, private router: Router, private app: AppComponent,private route: ActivatedRoute) {
    this.updateSubscription = interval(60000).pipe(
      switchMap(() => this.playersService.getAllPlayers())
    )
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


  ngOnInit(): void {
    this.playersService.getAllPlayers()
    .subscribe({
      next: (players) => {
        console.log(players);
        this.players = players
        this.searchedPlayers = players
        this.loadPlayers(0)
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

  loadPlayers(pageNumber: number){
    this.shownPlayers = []
    this.pageNumber = pageNumber
    for(var i = pageNumber * 50; i < (pageNumber * 50) + 50; i++){
      if(i == this.searchedPlayers.length){
        break
      }
      this.iValue = i
      this.shownPlayers.push(this.searchedPlayers[i])
    }
  }

  searchPlayers(){
    var older: boolean
    var taller: boolean
    var heavier: boolean
    var better: boolean
    if(this.model.older.toLowerCase() == "older".toLocaleLowerCase()) older = true
    if(this.model.taller.toLowerCase() == "taller".toLocaleLowerCase()) taller = true
    if(this.model.heavier.toLowerCase() == "heavier".toLocaleLowerCase()) heavier = true
    if(this.model.better.toLowerCase() == "better".toLocaleLowerCase()) better = true
    this.searchedPlayers = this.players.filter(p => 
      p.name.toLowerCase().includes(this.model.name.toLowerCase()) &&
      p.nationality.toLowerCase().includes(this.model.nationality.toLowerCase()) &&
      older ? p.age >= this.model.age : p.age <= this.model.age &&
      taller ? p.height_cm >= this.model.height_cm : p.height_cm <= this.model.height_cm &&
      heavier ? p.weight_kg >= this.model.weight_kg : p.weight_kg <= this.model.weight_kg &&
      better ? p.overall >= this.model.overall : p.overall <= this.model.overall &&
      p.player_positions.toLowerCase().includes(this.model.player_positions.toLowerCase()) &&
      p.preferred_foot.toLowerCase().includes(this.model.preferred_foot.toLowerCase())
    )
    this.loadPlayers(0)
    }
}