import { Component } from '@angular/core';
import { Player } from 'src/app/models/player.model';
import { PlayersService } from 'src/app/services/players.service';
import { Router } from '@angular/router';
import { Team } from 'src/app/models/teams.model';
import { TeamsService } from 'src/app/services/teams.service';
import { playerTeam } from 'src/app/models/playerTeam.model';

@Component({
  selector: 'app-add-player',
  templateUrl: './add-player.component.html',
  styleUrls: ['./add-player.component.css']
})
export class AddPlayerComponent {
  selectedTeamArray: Team[] = [];
  teams: Team[] = [];
  selectedTeam: Team = {
    id: '',
    name: '',
    isAvailable: true,
    matches: [],
    availability:'',
    players:[],
    score: 0,
    result: 0
  }
  addPlayerRequest: Player = {
    id: '',
    name: '',
    age: 0,
    teams:[],
    goals: 0,
    assists: 0,
    yellow: 0,
    red: 0,
    spG: 0,
    psPercent: 0,
    motm: 0,
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

  addTeam(){
    if(this.selectedTeamArray.includes(this.selectedTeam) == true) return
    this.selectedTeamArray.push(this.selectedTeam)
    this.teams.splice(this.teams.indexOf(this.selectedTeam),1)
    console.log(this.selectedTeamArray)
  }

  addPlayer(){
    this.selectedTeamArray.forEach(element => {
      const addPlayerTeam: playerTeam = {
        id: '',
        playerId: '',
        teamId: element.id
      }
      this.addPlayerRequest.teams.push(addPlayerTeam)
    });

    console.log(this.addPlayerRequest)
    this.playerService.addPlayer(this.addPlayerRequest)
    .subscribe({
      next: (player) => {
        this.router.navigate(['players'])
      }
    })
  }

  removeFromSelectedTeamArray(team: Team){
    this.teams.push(team);
    this.selectedTeamArray.splice(this.selectedTeamArray.indexOf(team),1)
    console.log(this.selectedTeamArray)
  }
}
