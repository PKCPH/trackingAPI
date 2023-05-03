import { Component, ElementRef, Renderer2 } from '@angular/core';
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
  updateSubscription: Subscription;
  credentials: LoginModel = this.app.credentials

  //list of all players in the database
  players: Player[] = [];

  //list of all players that matches the filter
  searchedPlayers: Player[] = [];

  //current page number starting from 0
  pageNumber: number = 0

  //number of players shown
  numberOfPlayersShown: number = 50

  //stores if the backend has responded with the playerList
  response: boolean = false
  
  //stores the filter
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
  
  constructor(private playersService: PlayersService, private router: Router, private app: AppComponent,private route: ActivatedRoute, private renderer: Renderer2, private el: ElementRef) {
    //Calls the database every minute for a fresh list of players in case players have been added/removed
    this.updateSubscription = interval(60000).pipe(
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

  //removes the subscription created in the constructor
  ngOnDestroy() {
    this.updateSubscription.unsubscribe();
  }



  ngOnInit(): void {
    //calls the backend to get all players
    this.playersService.getAllPlayers()
    .subscribe({
      next: (players) => {
        console.log(players);
        this.Hideloader();
        this.players = players
        this.searchedPlayers = players
        this.response = true
      },
      error: (response) => {
        console.log(response);
      }
    })
  }

  GoAddPlayer() {
    this.router.navigateByUrl('/players/add')
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
    var older: boolean = false
    var taller: boolean = false
    var heavier: boolean = false
    var better: boolean = false

    if(this.model.older.toLowerCase() == "older".toLocaleLowerCase()) older = true
    else older = false

    if(this.model.taller.toLowerCase() == "taller".toLocaleLowerCase()) taller = true
    else taller = false

    if(this.model.heavier.toLowerCase() == "heavier".toLocaleLowerCase()) heavier = true
    else heavier = false

    if(this.model.better.toLowerCase() == "better".toLocaleLowerCase()) better = true
    else better = false

    //applies the filter to the player list
    this.searchedPlayers = this.players.filter(p => p.name.toLowerCase().includes(this.model.name.toLowerCase()))
    this.searchedPlayers = this.searchedPlayers.filter(p => p.nationality.toLowerCase().includes(this.model.nationality.toLowerCase()))
    this.searchedPlayers = this.searchedPlayers.filter(p => taller ? p.height_cm >= this.model.height_cm : p.height_cm <= this.model.height_cm)
    this.searchedPlayers = this.searchedPlayers.filter(p => older ? p.age >= this.model.age : p.age <= this.model.age)
    this.searchedPlayers = this.searchedPlayers.filter(p => heavier ? p.weight_kg >= this.model.weight_kg : p.weight_kg <= this.model.weight_kg)
    this.searchedPlayers = this.searchedPlayers.filter(p => better ? p.overall >= this.model.overall : p.overall <= this.model.overall)
    this.searchedPlayers = this.searchedPlayers.filter(p => p.player_positions.toLowerCase().includes(this.model.player_positions.toLowerCase()))
    this.searchedPlayers = this.searchedPlayers.filter(p => p.preferred_foot.toLowerCase().includes(this.model.preferred_foot.toLowerCase()))
    this.pageNumber = 0
  }

  Hideloader() {
    // Setting display of spinner element to none
    this.renderer.setStyle(this.el.nativeElement.querySelector('#loading'), 'display', 'none');
  }
}