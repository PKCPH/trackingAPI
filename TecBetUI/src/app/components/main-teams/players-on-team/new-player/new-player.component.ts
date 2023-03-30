import { Component } from '@angular/core';
import { Player } from 'src/app/models/player.model';
import { PlayersService } from 'src/app/services/players.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Team } from 'src/app/models/teams.model';
import { TeamsService } from 'src/app/services/teams.service';
import { playerTeam } from 'src/app/models/playerTeam.model';

@Component({
  selector: 'app-new-player',
  templateUrl: './new-player.component.html',
  styleUrls: ['./new-player.component.css']
})
export class NewPlayerComponent {
  id: string = '';
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
    rating: 0,
    round: 0
  }
  addPlayerRequest: Player = {
    id: "",
    teams:[],
    name: "",
    nationality: "",
    age: 0,
    height_cm: 0,
    weight_kg: 0,
    overall: 0,
    player_positions: "",
    preferred_foot: "",
  }
  
  constructor(private playerService: PlayersService, private teamsService: TeamsService, private router: Router, private route: ActivatedRoute){ }
  
  ngOnInit(): void {
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('id');
        if(id){
          this.id = id
          this.getTeams(id)
        }
      }
    })
  }

  getTeams(id: string){
    this.teamsService.getAllTeams()
    .subscribe({
      next: (teams) => {
        console.log(teams);
        this.teams = teams
        var team = teams.find(t => t.id == id)
        if(team){
          this.selectedTeamArray.push(team)
          this.teams.splice(this.teams.indexOf(team),1)
          console.log(this.selectedTeamArray)
          console.log(teams)
        }
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
        this.GoBack()
      }
    })
  }

  GoBack(){
    this.router.navigate(['teams/players/' + this.id])
  }
  
  removeFromSelectedTeamArray(team: Team){
    this.teams.push(team);
    this.selectedTeamArray.splice(this.selectedTeamArray.indexOf(team),1)
    console.log(this.selectedTeamArray)
  }
}