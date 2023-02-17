import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Player } from 'src/app/models/player.model';
import { Team } from 'src/app/models/teams.model';
import { PlayersService } from 'src/app/services/players.service';
import { TeamsService } from 'src/app/services/teams.service';

@Component({
  selector: 'app-edit-player',
  templateUrl: './edit-player.component.html',
  styleUrls: ['./edit-player.component.css']
})
export class EditPlayerComponent {

  teams: Team[] = [];
  team: Team = {
    id: '',
    name: '',
    isAvailable: true,
    matches: [],
    availability:''
  }
  playerDetails: Player = {
    id: '',
    name: '',
    age: 0,
    teamId: '',
    team: this.team
  }

  constructor(private route: ActivatedRoute, private teamsService: TeamsService, private playerService: PlayersService, private router: Router){ }

  ngOnInit(): void {
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('id');

        if(id){
          this.playerService.getPlayer(id)
          .subscribe({
            next: (response) => {
              this.playerDetails = response;
            }
          })
        }
      }
    })
    this.teamsService.getAllTeams()
    .subscribe({
      next: (teams) => {
        console.log(teams);
        this.teams = teams
      },
      error: (response) => {
        console.log(response);
      }
    })
  }

  updatePlayer(){
    this.playerDetails.teamId = this.playerDetails.team.id
    this.playerService.updatePlayer(this.playerDetails.id, this.playerDetails)
    .subscribe({
      next: (response) => {
        this.router.navigate(['players'])
      }
    })
  }

  deletePlayer(id: string){
    this.playerService.deletePlayer(id)
    .subscribe({
      next: (response) => {
        this.router.navigate(['players'])
      }
    })
  }
}
