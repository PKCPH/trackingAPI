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
    result: 0,
    rating: 0
  }
  addPlayerRequest: Player = {
    id: '',
    name: '',
    age: 0,
    teams:[],
    overall: 0,
    potential: 0,
    pace: 0,
    shooting: 0,
    passing: 0,
    dribbling: 0,
    defense: 0,
    physical: 0,
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
    this.selectedTeamArray.forEach(team => {
      const addPlayerTeam: playerTeam = {
        id: '',
        playerId: '',
        teamId: team.id
      }
      this.addPlayerRequest.teams.push(addPlayerTeam)

      this.teamsService.getTeam(team.id)
      .subscribe({
        next: (team) => {
          team.rating = team.rating * team.players.length
          team.rating = team.rating + this.addPlayerRequest.overall
          team.rating = team.rating / (team.players.length + 1)
          team.rating = Number(team.rating.toPrecision(4))
          this.teamsService.updateTeam(team.id, team)
          .subscribe({
            next: (response) => {
            }
          })
        }
      })
    });

    this.teams.forEach(team => {
      if(team.players.some(p => p.playerId == this.addPlayerRequest.id)){
        team.rating = team.rating * (team.players.length - 1)
        this.teamsService.updateTeam(team.id, team)
        .subscribe({
          next: (response) => {
            console.log(team.rating)
          }
        })
      }
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
