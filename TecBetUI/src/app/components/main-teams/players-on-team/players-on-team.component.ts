import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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
  id: string = '';
  selectedTeam: Team = {
    id: '',
    name: '',
    isAvailable: true,
    matches: [],
    availability:'',
    players:[],
    score: 0,
    result: 0,
  }
  credentials: LoginModel = this.app.credentials
  
  constructor(private route: ActivatedRoute, private teamService: TeamsService, private router: Router, private playerService: PlayersService, private app: AppComponent) {}

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
  }

  isUserAuthenticated = (): boolean => {
    return this.app.isUserAuthenticated()
  }
}
