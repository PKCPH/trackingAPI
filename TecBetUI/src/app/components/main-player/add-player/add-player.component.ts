import { Component } from '@angular/core';
import { Player } from 'src/app/models/player.model';
import { PlayersService } from 'src/app/services/players.service';
import { Router } from '@angular/router';
import { Team } from 'src/app/models/teams.model';
import { TeamsService } from 'src/app/services/teams.service';

@Component({
  selector: 'app-add-player',
  templateUrl: './add-player.component.html',
  styleUrls: ['./add-player.component.css']
})
export class AddPlayerComponent {

  teams: Team[] = [];
  team: Team = {
    id: '',
    name: '',
    isAvailable: true,
    matches: [],
    availability:'',
    score: 0,
    result: 0
  }
  addPlayerRequest: Player = {
    id: '',
    name: '',
    age: 0,
    teamId: '',
    team: this.team
  }

  constructor(private playerService: PlayersService, private teamsService: TeamsService, private router: Router){ }

  ngOnInit(): void {
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

  addPlayer(){
    this.addPlayerRequest.teamId = this.addPlayerRequest.team.id
    console.log(this.addPlayerRequest)
    this.playerService.addPlayer(this.addPlayerRequest)
    .subscribe({
      next: (player) => {
        this.router.navigate(['players'])
      }
    })
  }
}
