import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { interval, Subscription, switchMap } from 'rxjs';
import { AppComponent } from 'src/app/app.component';
import { LoginModel } from 'src/app/models/login.model';
import { Player } from 'src/app/models/player.model';
import { Team } from 'src/app/models/teams.model';
import { PlayersService } from 'src/app/services/players.service';
import { TeamsService } from 'src/app/services/teams.service';

@Component({
  selector: 'app-players-on-team',
  templateUrl: './players-on-team.component.html',
  styleUrls: ['./players-on-team.component.css']
})
export class PlayersOnTeamComponent {
  
  players: Player[] = [];
  searchedPlayers: Player[] = [];
  id: string = '';
  updateSubscription: Subscription;
  credentials: LoginModel = this.app.credentials
  teamRating: number = 0

  model = {
    name: "",
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
  
  selectedTeam: Team = {
    id: '',
    name: '',
    isAvailable: true,
    matches: [],
    availability:'',
    players:[],
    score: 0,
    result: 0,
    rating: 0,
  }

  ngOnDestroy() {
    this.teamScoreCalculator()
    this.updateSubscription.unsubscribe();
  }
  
  constructor(private route: ActivatedRoute, private teamService: TeamsService, private router: Router, private playerService: PlayersService, private app: AppComponent) {
    this.updateSubscription = interval(3000).pipe(
      switchMap(() => this.teamService.getPlayers(this.id))
    )
    .subscribe({
      next: (players) => {
        console.log(players);
        this.players = players
        this.searchPlayers()
        this.teamScoreCalculator()
      },
      error: (response) => {
        console.log(response);
      }
    })
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('id');
        if(id){
          this.id = id
          this.teamService.getTeam(id)
          .subscribe({
            next: (team) => {
              console.log(team);
              this.selectedTeam = team
            }
          })
          this.teamService.getPlayers(id)
          .subscribe({
            next: (players) => {
              console.log(players);
              this.players = players
              this.searchPlayers()
              this.teamScoreCalculator()
            },
            error: (response) => {
              console.log(response);
            }
          })
        }
      }
    })
  }
  GoAddPlayers() {
    this.router.navigateByUrl('/teams/players/' + this.id + '/add')
  }
  NewPlayer(){
    this.router.navigateByUrl('/teams/players/' + this.id + '/new')
  }

  ChangePlayer(playerId: string){
    this.router.navigateByUrl('/teams/players/' + this.id + '/change/' + playerId)
  }

  deletePlayer(id: string){
    this.playerService.deletePlayer(id)
    .subscribe({
      next: (response) => {
      }
    })
    this.players.splice(this.players.findIndex(p => p.id == id),1)
    this.searchPlayers()
    this.teamScoreCalculator()
  }

  isUserAuthenticated = (): boolean => {
    return this.app.isUserAuthenticated()
  }

  searchPlayers(){
    this.searchedPlayers = this.players.filter(p => 
      p.name.toLowerCase().includes(this.model.name.toLocaleLowerCase()) &&
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

  teamScoreCalculator(){
    this.teamRating = 0
    this.players.forEach(player => {
      this.teamRating += player.overall
    });
    if(this.players.length > 0){
      this.teamRating = this.teamRating/this.players.length
    } else {
      this.teamRating = 0
    }
    this.teamRating = Number(this.teamRating.toPrecision(4))
    this.selectedTeam.rating = this.teamRating
    this.teamService.updateTeam(this.selectedTeam.id, this.selectedTeam)
    .subscribe({
      next: (team) => {
      }
    })
  }
}
